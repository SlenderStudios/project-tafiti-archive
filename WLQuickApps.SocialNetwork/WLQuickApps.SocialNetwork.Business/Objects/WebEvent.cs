using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;

namespace WLQuickApps.SocialNetwork.Business
{
    public class WebEvent
    {
        public string EventID
        {
            get { return this._eventID; }
        }
        private string _eventID;

        public DateTime EventTime
        {
            get { return this._eventTime; }
        }
        private DateTime _eventTime;

        public string EventType
        {
            get { return this._eventType; }
        }
        private string _eventType;

        public string Message
        {
            get { return this._message; }
        }
        private string _message;

        public string ApplicationPath
        {
            get { return this._applicationPath; }
        }
        private string _applicationPath;

        public string ApplicationVirtualPath
        {
            get { return this._applicationVirtualPath; }
        }
        private string _applicationVirtualPath;

        public string MachineName
        {
            get { return this._machineName; }
        }
        private string _machineName;

        public string RequestURL
        {
            get { return this._requestURL; }
        }
        private string _requestURL;

        public string ExceptionType
        {
            get { return this._exceptionType; }
        }
        private string _exceptionType;

        public string Details
        {
            get { return this._details; }
        }
        private string _details;

        internal WebEvent(WebEventDataSet.aspnet_WebEvent_EventsRow row)
        {
            this._eventID = row.EventId;
            this._eventTime = row.EventTime;
            this._eventType = row.EventType;
            this._message = row.Message;
            this._applicationPath = row.ApplicationPath;
            this._applicationVirtualPath = row.ApplicationVirtualPath;
            this._machineName = row.MachineName;
            this._requestURL = row.RequestUrl;
            this._exceptionType = row.ExceptionType;
            this._details = row.Details;
        }
    }
}
