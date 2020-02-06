using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BirthdayBase.Droid
{
    internal class AndroidCalendar : IDeviceCalendar
    {
        //IReadOnlyList<Appointment> AllAppointments;
        //AppointmentStore appointmentStore = null;
        //AppointmentCalendar CalendarToUse = null;

        //async Task GetStoreAsync()
        //{
        //    try
        //    {
        //        IReadOnlyList<User> users = await User.FindAllAsync();
        //        User ActiveUser = users[0];
        //        try
        //        {
        //            AppointmentManagerForUser appointmentManager = AppointmentManager.GetForUser(ActiveUser);
        //            appointmentStore = await appointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);
        //        }
        //        catch (Exception ex)
        //        {
        //            appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);
        //        }

        //        if (appointmentStore == null)
        //        {
        //            throw new Exception();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public async Task GetCalendarAsync()
        //{
        //    if (appointmentStore == null)
        //    {
        //        await GetStoreAsync();
        //    }
        //    try
        //    {
        //        var Cals = (await appointmentStore.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden)).Where(x => x.DisplayName == Const.CalendarName);
        //        if (Cals.Count() == 0)
        //        {
        //            CalendarToUse = await appointmentStore.CreateAppointmentCalendarAsync(Const.CalendarName);
        //        }
        //        else if (Cals.Count() > 0)
        //        {
        //            foreach (var item in Cals.TakeLast(Cals.Count() - 1))
        //            {
        //                await item.DeleteAsync();
        //            }
        //            CalendarToUse = Cals.FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    if (CalendarToUse != null)
        //    {
        //        CalendarToUse.CanCreateOrUpdateAppointments = true;
        //        CalendarToUse.DisplayColor = Windows.UI.Color.FromArgb(0xFF, 0xB3, 0xE5, 0xB8); //{#FFB3E5B8}
        //        GotCalendar = true;
        //    }
        //}
        //async Task<int> FindAllAsync()
        //{
        //    if (CalendarToUse == null)
        //    {
        //        await GetCalendarAsync();
        //    }
        //    if (CalendarToUse == null)
        //    {
        //        return 0;
        //    }
        //    var Start = DateTimeOffset.MinValue;
        //    var Range = TimeSpan.MaxValue;

        //    FindAppointmentsOptions op = new FindAppointmentsOptions()
        //    {
        //        IncludeHidden = true
        //    };
        //    //NewNot("Searching all Appointments from {0} over {1}", Start, Range);
        //    AllAppointments = await CalendarToUse.FindAppointmentsAsync(Start, Range, op);
        //    foreach (var item in AllAppointments)
        //    {
        //        //NewNot("Found Element \"{0}\" lID: {1} rID: {2}", item.Subject, item.LocalId, item.RoamingId);
        //    }
        //    return AllAppointments.Count;
        //    //NewNot("Found {0} Appointments", AllAppointments.Count);
        //}

        //#region IDeviceCalendar
        //public async Task<(int, int)> DeleteAllEntrysAsync()
        //{
        //    if (CalendarToUse == null)
        //    {
        //        await GetCalendarAsync();
        //    }
        //    if (AllAppointments == null)
        //    {
        //        try
        //        {
        //            await FindAllAsync();
        //            if (AllAppointments == null)
        //            {
        //                return (0, 0);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            return (0, 0);
        //        }
        //    }
        //    int DeleteCounter = 0;
        //    int ToDelete = AllAppointments.Count;
        //    foreach (var item in AllAppointments)
        //    {
        //        try
        //        {
        //            await CalendarToUse.DeleteAppointmentAsync(item.LocalId);
        //            //NewNot("Deleting Element {0} now lID: {1} rID: {2}", item.Subject, item.LocalId, item.RoamingId);
        //            DeleteCounter++;
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    await SaveAsync();
        //    AllAppointments = null;
        //    return (DeleteCounter, ToDelete);
        //}

        //public async Task<bool> SaveAppointmentAsync(CustomAppointment a)
        //{
        //    if (CalendarToUse == null)
        //    {
        //        await GetCalendarAsync();
        //    }
        //    var A = new Appointment
        //    {
        //        AllDay = a.AllDay,
        //        Details = a.Details,
        //        Reminder = a.Reminder,
        //        RoamingId = a.RoamingId,
        //        StartTime = a.StartTime,
        //        Subject = a.Subject,
        //        BusyStatus = AppointmentBusyStatus.Free
        //    };
        //    try
        //    {
        //        await CalendarToUse.SaveAppointmentAsync(A);
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public async Task<bool> SaveAsync()
        //{
        //    try
        //    {
        //        await CalendarToUse.SaveAsync();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public async Task<bool> DeleteAllCreatedCalendarsAsync()
        //{
        //    try
        //    {
        //        if (appointmentStore == null)
        //        {
        //            await GetStoreAsync();
        //        }
        //        var Cals = (await appointmentStore.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden)).Where(x => x.DisplayName == Const.CalendarName);
        //        foreach (var item in Cals)
        //        {
        //            try
        //            {
        //                await item.DeleteAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //#endregion
        public Task<bool> DeleteAllCreatedCalendarsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(int, int)> DeleteAllEntrysAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAppointmentAsync(CustomAppointment a)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}