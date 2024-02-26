using System;
using System.Collections.Generic;
using System.Text;
using WLQuickApps.SocialNetwork.Data;

namespace WLQuickApps.SocialNetwork.Business
{
    /// <summary>
    /// Represents a user created event.
    /// </summary>
    public class Event : Group, IComparable<Event>
    {
        /// <summary>
        /// The event's start date and time
        /// </summary>
        public DateTime StartDateTime
        {
            get { return this._startDateTime; }
        }
        private DateTime _startDateTime;

        /// <summary>
        /// The event's end date and time.
        /// </summary>
        public DateTime EndDateTime
        {
            get { return this._endDateTime; }
        }
        private DateTime _endDateTime;

        internal Event(int baseItemID, DateTime startDateTime, DateTime endDateTime)
            : base(baseItemID)
        {
            this._startDateTime = startDateTime;
            this._endDateTime = endDateTime;
        }

        /// <summary>
        /// Constructor for event based on an EventRow in the EventDataTable. 
        /// </summary>
        /// <param name="row"></param>
        internal Event(EventDataSet.EventRow row)
            : this(row.BaseItemID, row.StartTime, row.EndTime)
        {
        }

        public override void Delete() { EventManager.DeleteEvent(this); }

        public void ChangeEventTime(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime >= endDateTime) { throw new ArgumentException("Event cannot start after it ends"); }

            this._startDateTime = startDateTime;
            this._endDateTime = endDateTime;
        }



        #region IComparable<Event> Members

        public int CompareTo(Event other)
        {
            if (other == null) { return 1; }

            return this.StartDateTime.CompareTo(other.StartDateTime);
        }

        #endregion
    }
}
