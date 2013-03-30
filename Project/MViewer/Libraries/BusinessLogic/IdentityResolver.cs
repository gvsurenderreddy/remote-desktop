using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using Utils;

namespace BusinessLogicLayer
{
    public static class IdentityResolver
    {
        #region public static methods

        public static ContactEndpoint ResolveIdentity(string identity)
        {
            // pattern: "https://" + Address + ":" + Port.ToString() + "/" + Path;

            string decrypted = Tools.Instance.Cryptography.TrippleDESDecrypt(identity, true);
            Uri uri = new Uri(decrypted, UriKind.Absolute);

            string host = uri.Host;
            int port = uri.Port;
            string path = uri.AbsolutePath;
            
            ContactEndpoint endpoint = new ContactEndpoint(host, port, path);

            return endpoint;
        }

        #endregion
    }
}
