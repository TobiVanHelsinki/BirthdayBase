using System.Threading.Tasks;

namespace BirthdayBaseX
{
    public interface IDeviceCalendar
    {
        Task<(int,int)> DeleteAllEntrysAsync();
        Task<bool> DeleteAllCreatedCalendarsAsync();
        Task<bool> SaveAppointmentAsync(CustomAppointment a);
        Task<bool> SaveAsync();
    }
}
