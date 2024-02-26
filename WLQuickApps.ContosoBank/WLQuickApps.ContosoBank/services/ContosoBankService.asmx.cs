/*****************************************************************************
 * ContosoBankService.asmx
 * Notes: Service to expose key business functions to AJAX or Web
 * **************************************************************************/
using System;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using WLQuickApps.ContosoBank.Common;
using WLQuickApps.ContosoBank.Entity;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.Services
{
    /// <summary>
    /// Summary description for ContosoBankService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ContosoBankService : WebService
    {
        [ScriptMethod]
        [WebMethod]
        public void AddAppointment(int advisorId, DateTime appointmentDate, int appointmentSlot)
        {
            try
            {
                // check we have supplied valid advisorid and appointment slots
                if (AdvisorLogic.GetAdvisorByID(advisorId) == null)
                {
                    throw new Exception("Invalid advisor supplied;");
                }

                if (!Enum.IsDefined(typeof(AppoinmentSlotEnum), appointmentSlot))
                {
                    throw new Exception("Invalid appointment slot supplied.");
                }

                Appointment newAppointment = new Appointment
                                                 {
                                                     AdvisorID = advisorId,
                                                     AppointmentSlot = appointmentSlot,
                                                     AppointmentDate = appointmentDate.ToLocalTime()
                                                 };
                AppointmentLogic.AddAppointment(newAppointment);
                
            }
            catch
            {
                // This is where you would add your logging and show a generalised message to the end user.
                throw new Exception("An error has occured.");
            }
        }

        [ScriptMethod]
        [WebMethod]
        public string GetClusteredMapData(string encodedBounds, int zoomLevel, bool users)
        {
            try
            {
                // check we have supplied valid zoomlevel and bounds
                if (zoomLevel < 1 || zoomLevel > 19)
                {
                    throw new Exception("Invalid zoom level supplied.");
                }
                if (Utilities.DecodeBounds(encodedBounds) == null)
                {
                    throw new Exception("Invalid bounds supplied.");
                }

                return ClusterBusinessLogic.GetClusteredMapData(encodedBounds, zoomLevel, users);
            }
            catch
            {
                // This is where you would add your logging and show a generalised message to the end user.
                throw new Exception("An error has occured.");
            }
        }

        [ScriptMethod]
        [WebMethod]
        public LocalEvent GetEventByID(int id)
        {
            try
            {
                return EventLogic.GetEventByID(id);
            }
            catch
            {
                // This is where you would add your logging and show a generalised message to the end user.
                throw new Exception("An error has occured.");
            }
        }

        [ScriptMethod]
        [WebMethod]
        public Background GetBackgroundByName(string backgroundName)
        {
            try
            {
                return BackgroundLogic.GetBackgroundByName(backgroundName);
            }
            catch
            {
                // This is where you would add your logging and show a generalised message to the end user.
                throw new Exception("An error has occured.");
            }
        }


        [WebMethod]
        public ContosoCup GetContosoCupLeaderBoard()
        {
            try
            {
                return ContosoCupLogic.GetLeaderBoard();
            }
            catch
            {
                // This is where you would add your logging and show a generalised message to the end user.
                throw new Exception("An error has occured.");
            }
        }
    }
}