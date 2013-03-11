using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public struct PresenterSettings
    {
        public WebcamCapture captureControl { get; set; }
        public string identity { get; set; }
        public int timerInterval { get; set; }
        public Structures.ScreenSize videoSize { get; set; }
        public EventHandler webCamImageCaptured { get; set; }
    }
}
