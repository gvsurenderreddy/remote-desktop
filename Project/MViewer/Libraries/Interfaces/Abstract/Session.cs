using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace GenericObjects
{
    public abstract class Session
    {
        #region protected members

        protected string _identity;
        protected PeerStates _peers;
        protected PendingTransfer _pendingTransfer;
        protected ConferenceStatus _transferUpdating;

        #endregion
    }
}
