using BusinessLogicLayer;
using GenericObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MViewer
{
    public partial class Controller : IController
    {
        public void OnAudioCaptured(object sender, EventArgs e)
        {
            // todo: implement OnAudioCaptured
        }

        public void PauseAudio(object sender, RoomActionEventArgs args)
        {
            // todo: implement PauseAudio
        }

        public void ResumeAudio(object sender, RoomActionEventArgs args)
        {
            // todo: implement ResumeAudio
        }

        public void StartAudio(object sender, RoomActionEventArgs args)
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartAudioPresentation();
        }

        public void StopAudio(object sender, RoomActionEventArgs args)
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartAudioPresentation();
        }
    }
}
