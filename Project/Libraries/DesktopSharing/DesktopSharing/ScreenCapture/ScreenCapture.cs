using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DesktopSharing
{
    public class ScreenCapture
    {
        #region methods

        public void UpdateDesktop()
        {
            try
            {
                Bitmap screenImage = null;
                IntPtr height = IntPtr.Zero;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
    }
}
