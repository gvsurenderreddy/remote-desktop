using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopSharingViewer
{
    public interface IDesktopViewer
    {
        void UpdateDesktop(byte[] desktop);
        string UpdateMouse(byte[] mouse);
    }
}
