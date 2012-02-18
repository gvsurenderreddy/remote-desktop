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
            _display = new Bitmap(100, 100);
        }

        #endregion

        #region proprieties

        public Image Mouse
        {
            get { return _mouse; }
            set { _mouse = value; }
        }

        public int CursorX
        {
            get { return _cursorX; }
            set { _cursorX = value; }
        }

        public int CursorY
        {
            get { return _cursorY; }
            set { _cursorY = value; }
        }

        public Image Desktop
        {
            get { return _desktop; }
            set { _desktop = value; }
        }
       
        public Image Display
        {
            get { return _display; }
            set { _display = value; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        #endregion
    }
}
