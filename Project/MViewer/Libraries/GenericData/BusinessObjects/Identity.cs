using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace GenericDataLayer
{
    public class Identity
    {
        #region private members

        string _myIdentity;
        string _friendlyName;

        #endregion

        #region c-tor

        public Identity(string friendlyName)
        {
            _friendlyName = friendlyName;
        }

        #endregion

        #region public methods

        public void UpdateFriendlyName(string newFriendlyName)
        {
            _friendlyName = newFriendlyName;

            Configuration config = ConfigurationManager.OpenExeConfiguration(
                           Assembly.GetEntryAssembly().Location);
            config.AppSettings.Settings["FriendlyName"].Value = _friendlyName;
            config.Save();
        }

        public string GenerateIdentity(string newAddress, int newPort, string newPath)
        {
            string toEncrypt = "https://" + newAddress + ":" + newPort.ToString() + "/" + newPath;
            string encrypted = Utils.Cryptography.TrippleDESEncrypt(toEncrypt, true);
            _myIdentity = encrypted;
            return MyIdentity;
        }

        #endregion

        #region proprieties

        public string MyIdentity
        {
            get { return _myIdentity; }
        }

        public string FriendlyName
        {
            get { return _friendlyName; }
        }

        #endregion
    }
}
