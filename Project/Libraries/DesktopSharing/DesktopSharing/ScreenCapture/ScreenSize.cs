using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopSharing
{
    public class ScreenSize
    {
        #region members

        int _width;
        int _height;

        #endregion

        #region c-tor

        public ScreenSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        #endregion

        #region proprieties

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        #endregion
    }
}
