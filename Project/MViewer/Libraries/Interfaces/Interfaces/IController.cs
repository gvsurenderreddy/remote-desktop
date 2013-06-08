using Abstraction;
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

        void StartAudio(object sender, EventArgs args);
        void StopAudio(object sender, EventArgs args);
        void PauseAudio(object sender, EventArgs args);
        void ResumeAudio(object sender, EventArgs args);

        void StartVideo(object sender, EventArgs args);
        void StopVideo(object sender, EventArgs args);
        void PauseVideo(object sender, EventArgs args);
        void ResumeVideo(object sender, EventArgs args);
        void SendFileHandler(object sender, EventArgs args);
        
        void StopRemoting(object sender, EventArgs args);
        void StartRemoting(object sender, EventArgs args);
        void PauseRemoting(object sender, EventArgs args);
        void ResumeRemoting(object sender, EventArgs args);

        void SendRemotingCommand(object sender, EventArgs args);
        void ExecuteRemotingCommand(object sender, EventArgs e);

        void ChangeLanguage(string language);
        void FriendlyNameObserver(object sender, EventArgs e);
        void OnRoomButtonActionTriggered(object sender, EventArgs e);
        ContactBase PerformContactOperation(object sender, EventArgs ee);
        void StartVideo(IWebcamCapture webcamControl);
        void OnActiveRoomChanged(string newIdentity, GenericEnums.RoomType roomType);
  
        void OnVideoImageCaptured(object source, EventArgs e);
        void OnRemotingImageCaptured(object source, EventArgs e);
        void InitializeSettings();

        void KeyDown(object sender, EventArgs args);
        void KeyPress(object sender, EventArgs args);
        void KeyUp(object sender, EventArgs args);
        void DoubleRightClickCommand(object sender, EventArgs args);
        void DoubleLeftClickCommand(object sender, EventArgs args);
        void MiddleClickCommand(object sender, EventArgs args);
        void DoubleMiddleClickCommand(object sender, EventArgs args);
        void MouseWheelCommand(object sender, EventArgs args);
        void MouseMoveCommand(object sender, EventArgs args);
        void LeftMouseDownCommand(object sender, EventArgs args);
        void LeftMouseUpCommand(object sender, EventArgs args);
        void RightMouseDownCommand(object sender, EventArgs args);
        void RightMouseUpCommand(object sender, EventArgs args);
        void MiddleMouseDownCommand(object sender, EventArgs args);
        void MiddleMouseUpCommand(object sender, EventArgs args);

        void OnAudioCaptured(object sender, EventArgs e);
    }
}
