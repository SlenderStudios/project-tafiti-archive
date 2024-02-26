/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
using System;
namespace Contoso.Common.Entity
{
    public class LiveContact
    {
        private Guid liveId;
        private string userHandle;
        private string domainAuthentication;
        
        public Guid LiveId
        {
            get { return liveId; }
            set { liveId = value; }
        }

        public string UserHandle
        {
            get { return userHandle; }
            set { userHandle = value; }
        }

        public string DomainAuthentication
        {
            get { return domainAuthentication; }
            set { domainAuthentication = value; }
        }

    }
}
