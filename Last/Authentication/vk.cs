using DotNetOpenAuth.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Last.Authentication
{
    public class vk : IAuthenticationClient
    {
        public string appId;
        public string appSecret;

        public vk(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        string IAuthenticationClient.ProviderName
        {
            get { return "vkontakte"; }
        }

        void IAuthenticationClient.RequestAuthentication(
                                HttpContextBase context, Uri returnUrl)
        {
            throw new NotImplementedException();
        }

        AuthenticationResult IAuthenticationClient.VerifyAuthentication(
                               HttpContextBase context)
        {
            throw new NotImplementedException();
        }
    }
}