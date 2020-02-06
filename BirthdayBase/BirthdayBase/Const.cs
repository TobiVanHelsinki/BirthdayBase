namespace BirthdayBase
{
    public static class Const
    {
        
        public const string FileName = "Anniversaries.csv";
        public const string CalendarName = "Anniversaries";
        public const string CalendarNameApp = "BirthdayBase";
        public const string STD_FileContent = "Name;Type;Date\nHans;Birthday;30.02.2000\n";

        public const string Type_Birthday = "Birthday";
        public const string Type_Anniversary = "Anniversary";
        public const string Type_WeddingDay = "Wedding";
        public const string Type_Event = "Event";
        public const string Type_nBirthday = "1";
        public const string Type_nAnniversery = "2";
        public const string Type_nWeddingDay = "3";
        public const string Type_nEvent = "4";

        public const string Help =
@"* Type you Data into the big textfeld in this form: Name;Type;Date
* For Type you can use: Birthday, Anniversery, Wedding or just Event. 
* For Type you can also use the Numbers 1-4
* If you don't know the Startyear, you can use '0001' as Year.
* If you have complete the text, press the create / update Button.
* The App takes care of your text, the border indicates if the text is saved at the apps internal memory.
** Note: If you deinstall the app, you Data will be lost.
";
    }
}