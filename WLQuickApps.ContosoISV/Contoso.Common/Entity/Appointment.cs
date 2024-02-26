namespace Contoso.Common.Entity
{
    public class Appointment
    {
        private string clientName;
        private string clientCompany;
        private string aptTime;
        private string aptDate;
        private string clientAddress;
        private string clientPhone;

        public string ClientName
        {
            get
            {
                return clientName;
            }
            set
            {
                clientName = value;
            }
        }
        public string ClientCompany
        {
            get
            {
                return clientCompany;
            }
            set
            {
                clientCompany = value;
            }
        }
        public string AptTime
        {
            get
            {
                return aptTime;
            }
            set
            {
                aptTime = value;
            }
        }
        public string AptDate
        {
            get
            {
                return aptDate;
            }
            set
            {
                aptDate = value;
            }
        }
        public string ClientAddress
        {
            get
            {
                return clientAddress;
            }
            set
            {
                clientAddress = value;
            }
        }
        public string ClientPhone
        {
            get
            {
                return clientPhone;
            }
            set
            {
                clientPhone = value;
            }
        }
    }
}