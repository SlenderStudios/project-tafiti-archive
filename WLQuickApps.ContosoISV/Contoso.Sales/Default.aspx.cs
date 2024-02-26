 
/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com
 

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Contoso.Common.Entity;
using Contoso.Common.Logic;

namespace Contoso.Sales
{
    public partial class _Default : Page
    {
        private AppointmentBL appointments;
        private int appointmentCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            appointments = new AppointmentBL();
            myCalendar.SelectedDate = DateTime.Now;
            updatedSelectDate();
        }

        protected void myCalendar_SelectionChanged(object sender, EventArgs e)
        {
            updatedSelectDate();
        }

        private void updatedSelectDate()
        {
            CalendarSelectedDate.Value = myCalendar.SelectedDate.ToLongDateString();
        }

        protected void myCalendar_DayRender(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
        {
            if (appointments.DayHasAppointment(e.Day.Date))
            {
                // change colour
                List<Appointment> appointmentList =  appointments.GetAppointmentsForDate(e.Day.Date);
                appointmentCount += appointmentList.Count;
                AppointmentCountLabel.Text = appointmentCount + " Appointments";
                StringBuilder str = new StringBuilder();
                foreach (Appointment appointment in appointmentList)
                {
                    str.Append(appointment.ClientCompany);
                    str.Append(": ");
                    str.Append(appointment.AptTime);
                    str.AppendLine();
                }
                e.Cell.CssClass = "AppointmentDayStyle";
                e.Cell.ToolTip = str.ToString();
            }
        }
    }
}