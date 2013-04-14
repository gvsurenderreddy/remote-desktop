using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopSharing
{
    public interface IScreenCaptureTool
    {
        void TogglerTimer(bool start);
        void WaitRoomButtonAction(bool wait);
        bool RemotingCaptureClosed { get; }
    }
}
