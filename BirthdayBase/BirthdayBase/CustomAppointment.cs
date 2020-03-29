using System;

namespace BirthdayBase
{
    public class CustomAppointment
    {
        //A.BusyStatus = AppointmentBusyStatus.Free;
        public string Details { get; set; } = "";
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.Now;
        public TimeSpan Reminder { get; set; } = TimeSpan.Zero;
        public bool IsReminderOn { get; set; } = true;
        public string RoamingId { get; set; } = "";
        public string Subject { get; set; } = "";
        public string LocalId { get; set; } = "";
        public bool AllDay { get; set; } = true;
    }
}
