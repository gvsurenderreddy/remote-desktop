using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericData;
using UIControls;

namespace MViewer
{
    public class Model
    {
        #region private members

        Identity _identity;
        SystemConfiguration _systemConfiguration;

        #endregion

        #region public event handlers

        //event EventHandlers.IdentityEventHandler IdentityUpdatedEvent;
        public void IdentityUpdated(object sender, IdentityEventArgs e)
        {
            _identity.UpdateFriendlyName(e.FriendlyName);
            //_identity.GenerateIdentity(e.IP, e.Port);
        }

        #endregion

        #region c-tor

        public Model()
        {
            ReloadSystemConfiguration();
            //IdentityUpdatedEvent += new EventHandlers.IdentityEventHandler(IdentityUpdated);
            _identity = new Identity(_systemConfiguration.FriendlyName);
            _identity.GenerateIdentity(_systemConfiguration.MyIP, _systemConfiguration.Port);
        }

        #endregion

        #region public methods

        public void ReloadSystemConfiguration()
        {
            _systemConfiguration = new SystemConfiguration();
        }

        #endregion

        #region proprieties

        public string FriendlyName
        {
            get { return _identity.FriendlyName; }
        }

        public string MyIdentity
        {
            get { return _identity.MyIdentity; }
        }

        #endregion
    }
}
