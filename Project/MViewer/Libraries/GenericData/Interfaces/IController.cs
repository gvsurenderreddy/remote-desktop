using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace GenericDataLayer
{
    public interface IController
    {
        void StartApplication();
        void StopApplication();

        void StartVideoChat(object sender, RoomActionEventArgs args);
        void StopVideChat(object sender, RoomActionEventArgs args);
        void PauseVideoChat(object sender, RoomActionEventArgs args);
        void ResumeVideoChat(object sender, RoomActionEventArgs args);
        void SendFileHandler(object sender, RoomActionEventArgs args);
        void StopRemotingChat(object sender, RoomActionEventArgs args);
        void StartRemotingChat(object sender, RoomActionEventArgs args);
        void PauseRemotingChat(object sender, RoomActionEventArgs args);
        void ResumeRemotingChat(object sender, RoomActionEventArgs args);

        void SendRemotingCommand(object sender, RemotingCommandEventArgs args);
        void ExecuteRemotingCommand(object sender, EventArgs e);

        void IdentityObserver(object sender, IdentityEventArgs e);
        void RoomButtonAction(object sender, EventArgs e);
        Contact PerformContactsOperation(object sender, ContactsEventArgs ee);
        void StartVideoChat(WebcamCapture webcamControl);
        void ActiveRoomChanged(string newIdentity, GenericEnums.RoomType roomType);

        string MyIdentity();
        void VideoImageCaptured(object source, EventArgs e);

        void RemotingImageCaptured(object source, EventArgs e);

        void InitializeSettings();

        void KeyDown(object sender, RemotingCommandEventArgs args);
        void KeyPress(object sender, RemotingCommandEventArgs args);
        void KeyUp(object sender, RemotingCommandEventArgs args);
        void LeftClickCommand(object sender, RemotingCommandEventArgs args);
        void RightClickCommand(object sender, RemotingCommandEventArgs args);
        void DoubleRightClickCommand(object sender, RemotingCommandEventArgs args);
        void DoubleLeftClickCommand(object sender, RemotingCommandEventArgs args);
        void MiddleClickCommand(object sender, RemotingCommandEventArgs args);
        void DoubleMiddleClickCommand(object sender, RemotingCommandEventArgs args);
        void WheelCommand(object sender, RemotingCommandEventArgs args);
        void MoveCommand(object sender, RemotingCommandEventArgs args);
        void LeftMouseDownCommand(object sender, RemotingCommandEventArgs args);
        void LeftMouseUpCommand(object sender, RemotingCommandEventArgs args);
        void RightMouseDownCommand(object sender, RemotingCommandEventArgs args);
        void RightMouseUpCommand(object sender, RemotingCommandEventArgs args);
        void MiddleMouseDownCommand(object sender, RemotingCommandEventArgs args);
        void MiddleMouseUpCommand(object sender, RemotingCommandEventArgs args);
    }
}
