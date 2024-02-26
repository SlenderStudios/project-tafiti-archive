function createAppointment(advisorid, appointmentdate, appointmentslot)
{
    /// <summary>
    ///     Call AJAX webservice to create the appointment.
    /// </summary>  
    /// <param name="advisorid">The Advisor ID</param>       
    /// <param name="appointmentdate">The appointment's Date</param>       
    /// <param name="appointmentslot">The appointment slot - morning/afternoon </param>       

    WLQuickApps.ContosoBank.Services.ContosoBankService.AddAppointment(advisorid, appointmentdate, appointmentslot, onAppointmentSuccess, Utility.OnFailed);
}

function onAppointmentSuccess() {
    alert('Appointment sent to advisor.');
}