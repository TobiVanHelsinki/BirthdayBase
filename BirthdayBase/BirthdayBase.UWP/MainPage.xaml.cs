namespace BirthdayBase.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new BirthdayBase.App(new WindowsCalendar()));
        }
    }
}
