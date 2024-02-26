using System;
using System.Collections.Generic;

namespace WLQuickApps.ContosoBank.Entity
{
    public class Appointments
    {
        public List<Appointment> Appointment { get; set; }
    }

    public class Appointment
    {
        public int AdvisorID { get; set; }
        public int AppointmentSlot { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

    public enum AppoinmentSlotEnum
    {
        Morning = 1,
        Afternoon = 2
    }
}