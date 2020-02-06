using BirthdayBaseX.Resources;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BirthdayBaseX
{
    class MainPageModel : INotifyPropertyChanged
	{
        #region NotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            Device.BeginInvokeOnMainThread(() =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))
            );
        }
        #endregion
        string _FileContent;
        public string FileContent
        {
            get { return _FileContent; }
            set { if (_FileContent != value) { _FileContent = value; NotifyPropertyChanged(); } }
        }

        public ObservableCollection<string> Notifications { get; set; } = new ObservableCollection<string>();
        bool _TextIsSave;
        public bool TextIsSave
        {
            get { return _TextIsSave; }
            set { if (_TextIsSave != value) { _TextIsSave = value; NotifyPropertyChanged(); } }
        }

    }
    public partial class MainPage : ContentPage
    {
        readonly IDeviceCalendar CalendarTouse;
        readonly IDeviceCalendar LocalCalendar;
        readonly IDeviceCalendar MicrosoftCalendar;
        MainPageModel Model { get; set; }
        Timer SaveTimer;
        public string FullPath { get; set; }
        public void NewNot(string t, params object[] args)
        {
            Device.BeginInvokeOnMainThread(()=>
                Model.Notifications.Insert(0, string.Format(t, args))
            );
        }
        public MainPage(IDeviceCalendar Cal)
        {
            MicrosoftCalendar = new MicrosoftCalendar();
            LocalCalendar = Cal;
            CalendarTouse = MicrosoftCalendar;
            InitializeComponent();
            Model = BindingContext as MainPageModel;
            Model.PropertyChanged += Model_PropertyChanged;
            SaveTimer = new Timer((state) => WriteStorage(), null, Timeout.Infinite, Timeout.Infinite);

            var docpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            FullPath = Path.Combine(docpath, Const.FileName);
            ReadStorage();
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Model.TextIsSave):
                    if (Model.TextIsSave)
                    {
                        CSVInputBorder.BackgroundColor = Color.ForestGreen;
                    }
                    else
                    {
                        CSVInputBorder.BackgroundColor = Color.IndianRed;
                    }
                    break;
                default:
                    break;
            }
        }
        #region IO
        void ReadStorage()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    string Text = File.ReadAllText(FullPath);
                    Model.FileContent = Text;
                }
                else
                {
                    Model.FileContent = Const.STD_FileContent;
                    WriteStorage();
                }
                //NewNot("Got Filecontent, read {0} chars", Model.FileContent.Length);
            }
            catch (Exception ex)
            {
                NewNot("Error getting Filecontent V2: {0}", ex.Message);
            }
        }
        void CSVInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.TextIsSave = false;
            SaveTimer.Change(1000, 0);
        }
        void WriteStorage()
        {
            try
            {
                using (var streamWriter = File.CreateText(FullPath))
                {
                    streamWriter.Write(Model.FileContent);
                }
                Model.TextIsSave = true;
            }
            catch (Exception ex)
            {
                NewNot("Error Saving Filecontent: {0}", ex.Message);
            }
        }
        #endregion
        #region Cal Stuff
        async void UpdateCalendar(object sender, EventArgs e)
        {
            NewNot("Now deleting all appointments, this might take a little while");
            (int, int) res;
            try
            {
                res = await CalendarTouse.DeleteAllEntrysAsync();
                if (res.Item1 != res.Item2)
                {
                    NewNot("Could just delete" + res.Item1 + " of " + res.Item2 + " Appointments; you may now have some duplicats");
                }
                else
                {
                    NewNot("Deleted all " + res.Item1 + " Appointments");
                    CreateNewAsync();
                }
            }
            catch (Exception ex)
            {
                NewNot("There was this Exception during deletion: " + ex.Message);
            }
           
        }
        async void DeleteCalendar(object sender, EventArgs e)
        {
            if (await CalendarTouse.DeleteAllCreatedCalendarsAsync())
            {
                NewNot("Deleted all created calendars");
            }
            else
            {
                NewNot("Error deleting all created calendars, maybe some left");
            }
        }
        public async Task CreateNewAsync(bool DryRun = false)
        {
            if (DryRun) NewNot("This is a dry run");
            if (Model.FileContent == null)
            {
                return;
            }
            string FileContent = "";
            FileContent = Model.FileContent.Replace("\r", "\n");
            FileContent = FileContent.Replace("\n\n", "\n");
            int Counter = 0;
            foreach (var item in FileContent.Split('\n'))
            {
                var fields = item.Split(';');
                if (fields.Length < 3 || fields[0] == "" || fields[1] == "" || fields[2] == "")
                {
                    continue;
                }
                DateTimeOffset Date;
                var x = DateTime.MinValue;
                try
                {
                    Date = DateTimeOffset.Parse(fields[2].TrimEnd(' '), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal);
                }
                catch (Exception)
                {
                    NewNot("Error Getting Date \"{2}\" from {0} {1}", fields[0], fields[1], fields[2]);
                    continue;
                }
                CustomAppointment A = new CustomAppointment();
                A.AllDay = fields[2].Length <= 11; // wenn also keine Uhrzeit angegeben worden ist
                if (Date.Year != 1)
                {
                    A.Details = AppResource.Started + ": " + Date.Year;
                }
                A.StartTime = Date.AddYears(DateTimeOffset.UtcNow.Year - Date.Year);
                A.Reminder = TimeSpan.FromDays(0);
                A.RoamingId = Guid.NewGuid().ToString();
                string Subject = "NA";
                switch (fields[1])
                {
                    case Const.Type_Birthday:
                    case Const.Type_nBirthday:
                        if (Date.Year == 1)
                        {
                            Subject = String.Format(AppResource.BirthdayStringGeneral, fields[0]);
                        }
                        else
                        {
                            Subject = String.Format(AppResource.BirthdayStringConcrete, fields[0], DateTime.Now.Year - Date.Year);
                        }
                        break;
                    case Const.Type_Event:
                    case Const.Type_nEvent:
                        if (Date.Year == 1)
                        {
                            Subject = String.Format(AppResource.EventStringGeneral, fields[0]);
                        }
                        else
                        {
                            Subject = String.Format(AppResource.EventStringConcrete, fields[0], DateTime.Now.Year - Date.Year);
                        }
                        break;
                    case Const.Type_Anniversary:
                    case Const.Type_nAnniversery:
                        if (Date.Year == 1)
                        {
                            Subject = String.Format(AppResource.AnniversaryStringGeneral, fields[0]);
                        }
                        else
                        {
                            Subject = String.Format(AppResource.AnniversaryStringConcrete, fields[0], DateTime.Now.Year - Date.Year);
                        }
                        break;
                    case Const.Type_WeddingDay:
                    case Const.Type_nWeddingDay:
                        if (Date.Year == 1)
                        {
                            Subject = String.Format(AppResource.WeddingStringGeneral, fields[0]);
                        }
                        else
                        {
                            Subject = String.Format(AppResource.WeddingStringConcrete, fields[0], DateTime.Now.Year - Date.Year);
                        }
                        break;
                    default:
                        break;
                }
                A.Subject = Subject;
                try
                {
                    bool complete = false;
                    if (!DryRun) complete = await CalendarTouse.SaveAppointmentAsync(A);
                    if (!complete) throw new Exception();
                    NewNot("Saved Element: \"{0}\"", A.Subject, A.RoamingId, A.LocalId);
                    Counter++;
                }
                catch (Exception ex)
                {
                    NewNot("Error Saving Element \"{0}\" m: {1}", A.Subject, ex.Message);
                }
            }
            if (!DryRun) await CalendarTouse.SaveAsync();
            NewNot("Saved {0} Elements ", Counter);
        }

        #endregion

        void Help(object sender, EventArgs e)
        {
            //TODO Add help Text
        }
    }
}
