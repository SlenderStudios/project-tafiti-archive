/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Contoso.Common.Entity;
namespace Contoso.Common.Logic
{
    public class AppointmentBL
    {
        private XmlDocument doc = null;
        private string fileName = null;
        public AppointmentBL()
        {
            loadXMLDocument();
        }

        private void loadXMLDocument()
        {
            string cacheKey = "appointments.xml";
            doc = (XmlDocument)HttpContext.Current.Cache.Get(cacheKey);
            if (doc == null)
            {
                doc = new XmlDocument();
                fileName = Path.Combine(HttpRuntime.AppDomainAppPath, @"appointments.XML");
                doc.Load(fileName);
                HttpContext.Current.Cache.Add(cacheKey, doc, new CacheDependency(fileName), DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }

        public bool DayHasAppointment(DateTime suppliedDate)
        {
            bool result = false;
            XmlNodeList appointments = getAppointmentsForDate(suppliedDate);
            if (appointments.Count > 0)
            {
                result = true;
            }

            return result;
        }

        private XmlNodeList getAppointmentsForDate(DateTime suppliedDate)
        {
            string dateToCheck = suppliedDate.ToString("yyyy-MM-dd");
            return doc.SelectNodes("appointments/appointment[aptDate='" + dateToCheck + "']");
        }

        public List<Appointment>GetAppointmentsForDate(DateTime suppliedDate)
        {
            List<Appointment> appointments = new List<Appointment>();
            XmlNodeList items = getAppointmentsForDate(suppliedDate);
            for (int i = 0; i < items.Count; i++ )
            {
                Appointment appointment = new Appointment();
                appointment.ClientCompany = items[i].SelectSingleNode("clientCompany").InnerText.ToString();
                appointment.ClientName = items[i].SelectSingleNode("clientName").InnerText.ToString();
                appointment.ClientPhone = items[i].SelectSingleNode("clientPhone").InnerText.ToString();
                appointment.ClientAddress = items[i].SelectSingleNode("clientAddress").InnerText.ToString();
                appointment.AptDate = items[i].SelectSingleNode("aptDate").InnerText.ToString();
                appointment.AptTime = items[i].SelectSingleNode("aptTime").InnerText.ToString();
                appointments.Add(appointment);
            }
            return appointments;
        }
    }
}
