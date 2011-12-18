using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DesktopSharing;

namespace WpfRemotingServer
{
    public class RemoteService : IRemoteService
    {
        #region members

        ScreenCapture _capture;

        #endregion

        #region c-tor

        public RemoteService()
        {
            _capture = new ScreenCapture();
        }

        #endregion

        #region methods

        /// <summary>
        /// method used to capture and serialize the desktop image
        /// </summary>
        /// <returns>serialized desktop image</returns>
        public byte[] CaptureDekstopImage()
        {
            byte[] serialized = null;
            Rectangle rect = new Rectangle();
            Bitmap screenCapture = _capture.CaptureScreen(ref rect);
            if (screenCapture != null)
            {
                // something has changed on the screen
                serialized = RemoteServiceUtils.SerializeCapture(screenCapture, rect);

                // todo: display the trafic

            }
            else
            {
                // nothing has changed
                // todo: display the trafic
            }
            return serialized;
        }

        /// <summary>
        /// method used to capture and serialize the mouse cursor image
        /// </summary>
        /// <returns>serialized mouse cursor image</returns>
        public byte[] CaptureMouseImage()
        {
            byte[] serialized = null;
            int x = 0, y = 0;
            Image cursorCapture = _capture.GetCursorCapture(ref x, ref y);
            if (cursorCapture != null)
            {
                // something has changed
                serialized = RemoteServiceUtils.SerializeCapture(cursorCapture, x, y);
                // todo: display the trafic
            }
            else
            {
                // nothing has changed to the cursor
                // todo: display the trafic
            }
            return serialized;
        }

        #endregion
    }
}
