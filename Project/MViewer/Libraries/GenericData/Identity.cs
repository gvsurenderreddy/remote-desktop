using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using Utils;
using Abstraction;

namespace GenericObjects
{
    public class Identity : IdentityBase
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

        public override void UpdateFriendlyName(string newFriendlyName)
        {
            try
            {
                _friendlyName = newFriendlyName;

                Configuration config = ConfigurationManager.OpenExeConfiguration(
                               Assembly.GetEntryAssembly().Location);
                config.AppSettings.Settings["FriendlyName"].Value = _friendlyName;
                config.Save();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override string GenerateIdentity(string newAddress, int newPort, string newPath)
        {
            string toEncrypt = "http://" + newAddress + ":" + (newPort - 1).ToString() + "/" + newPath;
            string encrypted = Tools.Instance.Cryptography.TrippleDESEncrypt(toEncrypt, true);
            _myIdentity = encrypted;
            return MyIdentity;
        }

        #endregion

        #region proprieties

        public override string MyIdentity
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
