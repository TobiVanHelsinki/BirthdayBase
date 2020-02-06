namespace BirthdayBaseX.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new BirthdayBaseX.App(new WindowsCalendar()));
        }
    }
}
