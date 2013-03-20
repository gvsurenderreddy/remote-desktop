using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public struct ControllerEventHandlers
    {
        public EventHandler ClientConnectedObserver
        {
            get;
            set;
        }

        public EventHandler RoomButtonObserver
        {
            get;
            set;
        }

        public EventHandler VideoCaptureObserver
        {
            get;
            set;
        }

        public EventHandler AudioCaptureObserver
        {
            get;
            set;
        }

        public EventHandler RemotingCaptureObserver
        {
            get;
            set;
        }

        public EventHandler MouseCaptureObserver
        {
            get;
            set;
        }

        public EventHandler ContactsObserver
        {
            get;
            set;
        }

        public EventHandler WaitRoomActionObserver
        {
            get;
            set;
        }

        public EventHandler FileTransferObserver
        {
            get;
            set;
        }

        public EventHandler FilePermissionObserver
        {
            get;
            set;
        }

        public EventHandler RemotingCommandHandler
        {
            get;
            set;
        }
    }
}
