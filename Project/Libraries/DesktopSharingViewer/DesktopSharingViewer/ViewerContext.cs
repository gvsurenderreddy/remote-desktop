using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DesktopSharingViewer
{
    public class ViewerContext
    {
        #region members

        private Guid _id;
        private Image _desktop;
        private Image _mouse;
        private int _cursorX;
        private int _cursorY;
        private Image _display;

        #endregion

        #region c-tor

        public ViewerContext(Guid id)
        {
            _id = id;
        }

        #endregion

        #region proprieties

        #endregion
    }
}
