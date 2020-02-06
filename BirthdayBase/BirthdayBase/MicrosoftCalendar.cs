using Microsoft.Graph;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayBaseX
{
    internal class MicrosoftCalendar : IDeviceCalendar
    {
        ICalendarEventsCollectionPage AllAppointments;
        GraphServiceClient graphClient = null;
        string CalendarIDToUse = null;

        async Task GetCalendarAsync()
        {
            if (graphClient == null)
            {
                graphClient = AuthenticationHelper.GetAuthenticatedClient();
            }
            try
            {
                var Cals = (await graphClient.Me.Calendars.Request().GetAsync()).Where(x => x.Name == Const.CalendarName);
                if (Cals.Count() == 0)
                {
                    Calendar newcal = new Calendar() {
                        Name = Const.CalendarName,
                        Color = CalendarColor.LightGreen,
                        CanEdit = true,
                        CanShare = true
                    };
                    var newcalback = (await graphClient.Me.Calendars.Request().AddAsync(newcal));
                    CalendarIDToUse = newcalback.Id;
                }
                else if (Cals.Count() > 0)
                {
                    foreach (var item in Cals.Take(Cals.Count() - 1))
                    {
                        //await item.DeleteAsync();
                    }
                    CalendarIDToUse = Cals.FirstOrDefault().Id;
                }
            }
            catch (Exception ex)
            {
            }
        }
        async Task<int> FindAllAsync()
        {
            if (CalendarIDToUse == null)
            {
                await GetCalendarAsync();
            }
            if (CalendarIDToUse == null)
            {
                return 0;
            }
            var Start = DateTimeOffset.MinValue;
            var End = DateTimeOffset.MaxValue;
            var ToFind = int.MaxValue;
            string Filter = "Start/DateTime+ge+'"+Start.ToString("u")+"'+and+End/DateTime+lt+'"+ End.ToString("u") + "'";

            AllAppointments = await graphClient.Me.Calendars[CalendarIDToUse].Events.Request().Filter(Filter).Top(ToFind).GetAsync();

            return AllAppointments.Count;
        }

        #region IDeviceCalendar
        public async Task<(int, int)> DeleteAllEntrysAsync()
        {
            if (CalendarIDToUse == null)
            {
                await GetCalendarAsync();
            }
            if (AllAppointments == null)
            {
                try
                {
                    await FindAllAsync();
                    if (AllAppointments == null)
                    {
                        return (0, 0);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return (0, int.MaxValue);
                }
            }
            int DeleteCounter = 0;
            int ToDelete = AllAppointments.Count;
            foreach (var item in AllAppointments)
            {
                try
                {
                    await graphClient.Me.Events[item.Id].Request().DeleteAsync();
                    DeleteCounter++;
                }
                catch (Exception ex)
                {
                }
            }
            await SaveAsync();
            AllAppointments = null;
            return (DeleteCounter, ToDelete);
        }

        public async Task<bool> SaveAppointmentAsync(CustomAppointment a)
        {
            if (CalendarIDToUse == null)
            {
                await GetCalendarAsync();
            }

            DateTimeOffset tmptime;
            if (a.AllDay)
            {
                tmptime = a.StartTime.Subtract(a.StartTime.TimeOfDay);
            }
            else
            {
                tmptime = a.StartTime;
            }
            var A = new Event
            {
                IsAllDay = a.AllDay,
                Body = new ItemBody
                {
                    Content = a.Details,
                    ContentType = BodyType.Text,
                },
                IsReminderOn = a.Reminder != TimeSpan.Zero,
                ReminderMinutesBeforeStart = a.Reminder.Days * 24 * 60 + a.Reminder.Hours * 60 + a.Reminder.Minutes,
                OriginalStart = a.StartTime,
                Start = new DateTimeTimeZone() { DateTime = tmptime.ToString("s"), TimeZone = "UTC" },
                Subject = a.Subject, 
                ShowAs = FreeBusyStatus.Free,
                End = new DateTimeTimeZone() { DateTime = tmptime.AddDays(1).ToString("s"), TimeZone = "UTC" },
            };
            try
            {
                var createdEvent = await graphClient.Me.Calendars[CalendarIDToUse].Events.Request().AddAsync(A);
                if (createdEvent.Id == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            return true;
        }

        public async Task<bool> DeleteAllCreatedCalendarsAsync()
        {
            //https://docs.microsoft.com/en-us/previous-versions/office/office-365-api/api/version-2.0/calendar-rest-operations#delete-calendars
            return true;
        }
        #endregion
    }
}