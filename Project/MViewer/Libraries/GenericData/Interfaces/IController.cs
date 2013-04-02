using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace GenericObjects
{
    public interface IController
    {
        void StartApplication();
        void StopApplication();

        void StartAudio(object sender, RoomActionEventArgs args);
        void StopAudio(object sender, RoomActionEventArgs args);
        void PauseAudio(object sender, RoomActionEventArgs args);
        void ResumeAudio(object sender, RoomActionEventArgs args);

        void StartVideo(object sender, RoomActionEventArgs args);
        void StopVideo(object sender, RoomActionEventArgs args);
        void PauseVideo(object sender, RoomActionEventArgs args);
        void ResumeVideo(object sender, RoomActionEventArgs args);
        void SendFileHandler(object sender, RoomActionEventArgs args);
        void StopRemoting(object sender, RoomActionEventArgs args);
        void StartRemoting(object sender, RoomActionEventArgs args);
        void PauseRemoting(object sender, RoomActionEventArgs args);
        void ResumeRemoting(object sender, RoomActionEventArgs args);

        void SendRemotingCommand(object sender, RemotingCommandEventArgs args);
        void ExecuteRemotingCommand(object sender, EventArgs e);

        void ChangeLanguage(string language);
        void IdentityObserver(object sender, IdentityEventArgs e);
        void OnRoomButtonActionTriggered(object sender, EventArgs e);
        Contact PerformContactsOperation(object sender, ContactsEventArgs ee);
        void StartVideo(WebcamCapture webcamControl);
        void OnActiveRoomChanged(string newIdentity, GenericEnums.RoomType roomType);
        void FocusActionsForm();

        string MyIdentity();
        void OnVideoImageCaptured(object source, EventArgs e);

        void OnRemotingImageCaptured(object source, EventArgs e);

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
        void MouseWheelCommand(object sender, RemotingCommandEventArgs args);
        void MouseMoveCommand(object sender, RemotingCommandEventArgs args);
        void LeftMouseDownCommand(object sender, RemotingCommandEventArgs args);
        void LeftMouseUpCommand(object sender, RemotingCommandEventArgs args);
        void RightMouseDownCommand(object sender, RemotingCommandEventArgs args);
        void RightMouseUpCommand(object sender, RemotingCommandEventArgs args);
        void MiddleMouseDownCommand(object sender, RemotingCommandEventArgs args);
        void MiddleMouseUpCommand(object sender, RemotingCommandEventArgs args);

        void OnAudioCaptured(object sender, EventArgs e);
    }
}
