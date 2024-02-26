using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class AppointmentsControl : UserControl
    {
        #region AppoinmentSlotEnum enum

        public enum AppoinmentSlotEnum
        {
            Morning = 1,
            Afternoon = 2
        }

        #endregion

        #region DayTypeEnum enum

        public enum DayTypeEnum
        {
            Past = 1,
            PastWeekend = 2,
            Current = 3,
            CurrentWeekend = 4,
            Future = 5,
            FutureWeekend = 6
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void AppointmentCalendar_DayRender(object sender, DayRenderEventArgs e)
        {
            // don't allow selection based on day cell
            e.Day.IsSelectable = false;
            if (e.Day.Date < DateTime.Now)
            {
                createPastPanels(e);
            }
            else
            {
                createCurrentPanels(e);
            }
        }

        private void createCurrentPanels(DayRenderEventArgs e)
        {
            // if the there is an appointment in the morning create a morning available panel
            // otherwise create a busy morning panel
            int advisorID = Convert.ToInt32(Session["AdvisorID"]);
            if (AppointmentLogic.IsAvailable((int)AppoinmentSlotEnum.Morning, e.Day.Date, advisorID))
            {
                createAvailablePanel(e, "morning_" + e.Day.Date.ToShortDateString(),
                                     getDayType(e.Day), e.Day.Date, AppoinmentSlotEnum.Morning);
            }
            else
            {
                createUnavailablePanel(e, getDayType(e.Day), AppoinmentSlotEnum.Morning);
            }

            // if the there is an appointment in the afternoon create a morning available panel
            // otherwise create a busy morning panel
            if (AppointmentLogic.IsAvailable((int)AppoinmentSlotEnum.Afternoon, e.Day.Date, advisorID))
            {
                createAvailablePanel(e, "afternoon_" + e.Day.Date.ToShortDateString(),
                                     getDayType(e.Day), e.Day.Date, AppoinmentSlotEnum.Afternoon);
            }
            else
            {
                createUnavailablePanel(e, getDayType(e.Day), AppoinmentSlotEnum.Afternoon);
            }
        }

        private static void createPastPanels(DayRenderEventArgs e)
        {
            if (e.Day.IsWeekend)
            {
                createUnavailablePanel(e, DayTypeEnum.PastWeekend, AppoinmentSlotEnum.Morning);
                createUnavailablePanel(e, DayTypeEnum.PastWeekend, AppoinmentSlotEnum.Afternoon);
            }
            else
            {
                createUnavailablePanel(e, DayTypeEnum.Past, AppoinmentSlotEnum.Morning);
                createUnavailablePanel(e, DayTypeEnum.Past, AppoinmentSlotEnum.Afternoon);
            }
        }

        private static DayTypeEnum getDayType(CalendarDay day)
        {
            if (day.IsWeekend && day.IsOtherMonth)
            {
                return DayTypeEnum.FutureWeekend;
            }
            if (day.IsWeekend && !day.IsOtherMonth)
            {
                return DayTypeEnum.CurrentWeekend;
            }
            if (!day.IsWeekend && day.IsOtherMonth)
            {
                return DayTypeEnum.Future;
            }
            if (!day.IsWeekend && !day.IsOtherMonth)
            {
                return DayTypeEnum.Current;
            }

            return DayTypeEnum.Current;
        }

        private void createAvailablePanel(DayRenderEventArgs e, string idName, DayTypeEnum dayType,
                                          DateTime appointmentDate,
                                          AppoinmentSlotEnum appointmentSlot)
        {
            e.Cell.Controls.Add(createBreak());

            var appointment = new ImageButton();
            appointment.ImageUrl = "../images/CalendarTiles/CalendarTileFree" + dayType + ".png";
            appointment.ID = idName;
            appointment.CssClass = "Calendar" + appointmentSlot;
            appointment.Attributes.Add("onmouseover", "this.src='../images/CalendarTiles/CalendarTileOver.png'");
            appointment.Attributes.Add("onmouseout", "this.src='" + appointment.ImageUrl + "'");

            appointment.OnClientClick = String.Format("createAppointment({0},  new Date({1}, {2}, {3}, 0,0,0,0), {4}",
                                                      Convert.ToInt32(Session["AdvisorID"]), appointmentDate.Year,
                                                      appointmentDate.Month - 1, appointmentDate.Day,
                                                      (int)appointmentSlot + ");");
            e.Cell.Controls.Add(appointment);
        }

        private static void createUnavailablePanel(DayRenderEventArgs e, DayTypeEnum dayType,
                                                   AppoinmentSlotEnum appointmentSlot)
        {
            e.Cell.Controls.Add(createBreak());

            var unavailableImage = new Image();
            unavailableImage.ImageUrl = "../images/CalendarTiles/CalendarTileBusy" + dayType + ".png";
            unavailableImage.CssClass = "Calendar" + appointmentSlot;
            e.Cell.Controls.Add(unavailableImage);
        }

        private static Control createBreak()
        {
            var br = new HtmlGenericControl("br");
            return br;
        }
    }
}