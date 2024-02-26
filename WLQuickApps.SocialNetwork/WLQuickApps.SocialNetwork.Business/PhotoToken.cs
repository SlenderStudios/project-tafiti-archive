using System;
using System.Collections.Generic;
using System.Text;

namespace WLQuickApps.SocialNetwork.Business
{
    [Serializable()] 
    public class PhotoToken
    {
        public PhotoToken()
        {

        }

        public PhotoToken(string ownerHandle, string domainAuthenticationToken)
        {
            this.OwnerHandle = ownerHandle;
            this.DomainAuthenticationToken = domainAuthenticationToken;
        }

        string _ownerHandle = "";
        string _domainAuthenticationToken = "";

        public string OwnerHandle
        {
            get { return _ownerHandle; }
            set { _ownerHandle = value; }
        }
        
        public string DomainAuthenticationToken
        {
            get { return _domainAuthenticationToken; }
            set { _domainAuthenticationToken = value; }
        }
     
    }
}
