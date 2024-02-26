using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class AppointmentLogic
    {
        public static List<Appointment> GetAppointments()
        {
            if (HttpContext.Current.Cache["Appointments"] == null)
            {
                XDocument appointmentsXML = XDocument.Load(getAppointmentLocation());
                var temp = from feed in appointmentsXML.Descendants("Appointment")
                           orderby Convert.ToDateTime(feed.Element("AppointmentDate").Value) ascending
                           where Convert.ToDateTime(feed.Element("AppointmentDate").Value) >= DateTime.Today
                           select new Appointment
                                      {
                                          AdvisorID = Convert.ToInt32(feed.Element("AdvisorID").Value),
                                          AppointmentSlot = Convert.ToInt32(feed.Element("AppointmentSlot").Value),
                                          AppointmentDate = Convert.ToDateTime(feed.Element("AppointmentDate").Value)
                                      };

                HttpContext.Current.Cache.Add("Appointments", temp.ToList(), null, Cache.NoAbsoluteExpiration,
                                              new TimeSpan(0, 20, 0), CacheItemPriority.Normal, null);
            }

            return HttpContext.Current.Cache["Appointments"] as List<Appointment>;
        }

        public static bool IsAvailable(int appointmentSlot, DateTime appointment, int advisor)
        {
            var temp = from apt in GetAppointments()
                       where (apt.AdvisorID == advisor)
                             && (apt.AppointmentDate == appointment)
                             && (apt.AppointmentSlot == appointmentSlot)
                       select apt;

            return (temp.Count() == 0);
        }

        public static void AddAppointment(Appointment newAppointment)
        {
            List<Appointment> allAppoinments = GetAppointments();
            allAppoinments.Add(newAppointment);
            HttpContext.Current.Cache.Add("Appointments", allAppoinments, null, Cache.NoAbsoluteExpiration,
                                          new TimeSpan(0, 20, 0), CacheItemPriority.Normal, null);
        }

        private static string getAppointmentLocation()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + "\\data\\Appointments.xml";
        }
    }
}