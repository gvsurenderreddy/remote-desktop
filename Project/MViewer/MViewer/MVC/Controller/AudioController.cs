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

        void StartAudio()
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartAudioPresentation();
        }

        void StopAudio()
        {
            PresenterManager.Instance(SystemConfiguration.Instance.PresenterSettings).StartAudioPresentation();
        }
    }
}
