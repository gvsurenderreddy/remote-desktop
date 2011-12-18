using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace DesktopSharing
{
    public class ScreenCapture : ScreenCaptureBase
    {
        #region members

        Bitmap _oldCapture;
        Bitmap _newCapture;
        Graphics _graphics;
        double _percentDisplayed;
        // ARGB images - 4 bytes
        const int _numBytesPerPixel = 4;

        #endregion

        #region c-tor

        public ScreenCapture()
        {
            _newCapture = new Bitmap(1, 1);
            _graphics = Graphics.FromImage(new Bitmap(10, 10));
        }

        #endregion

        #region methods

        public Bitmap CaptureScreen(ref Rectangle rect)
        {
            Bitmap screenCapture = null;
            lock (_newCapture)
            {
                _newCapture = GetDesktopCapture();
                if (_oldCapture != null)
                {
                    // if we have a previous screen we have just to update it
                    rect = GetRectangle();
                    if (rect == Rectangle.Empty)
                    {
                        // nothing has changed to the screen
                        _percentDisplayed = 0.0;
                    }
                    else
                    {
                        screenCapture = new Bitmap(rect.Width, rect.Height);
                        _graphics = Graphics.FromImage(screenCapture); 
                        _graphics.DrawImage(_newCapture, 0, 0, rect, GraphicsUnit.Pixel);
                        _oldCapture = _newCapture;
                        lock (_newCapture)
                        {
                            _percentDisplayed = 100.0 * (screenCapture.Height * screenCapture.Width) /
                                (_newCapture.Height * _newCapture.Width);
                        }
                    }
                }
                else
                {
                    // if we do not have a previous screen we have to capture the whole screen
                    _oldCapture = _newCapture;
                    screenCapture = _newCapture;
                    rect = new Rectangle(0, 0, _newCapture.Width, _newCapture.Height);
                    _percentDisplayed = 100.0;
                }
            }
            return screenCapture;
        }

        private Rectangle GetRectangle()
        {
            throw new NotImplementedException();
        }

        public Bitmap CaptureMouse(ref int x, ref int y)
        {
            int desktopWidth = 1, desktopHeight = 1;
            lock (_newCapture)
            {
                try
                {
                    desktopWidth = _newCapture.Width;
                    desktopHeight = _newCapture.Height;
                }
                catch (Exception ex)
                {
                    // this exception must not occur
                    throw new Exception(ex.Message, ex);
                }
            }
            if (desktopWidth == 1 && desktopHeight == 1)
            {
                return null;
            }
            Bitmap cursorCapture = GetCursorCapture(ref x, ref y);
            if (cursorCapture != null && x < desktopWidth && y < desktopHeight)
            {
                int width = cursorCapture.Width;
                int height = cursorCapture.Height;
                BitmapData bmpData = cursorCapture.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, cursorCapture.PixelFormat);
                int stride = bmpData.Stride;
                IntPtr bmpScan0 = bmpData.Scan0;
                unsafe
                {
                    byte* b = (byte*)(void*)bmpScan0;
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            int offset = i * stride + j * _numBytesPerPixel + 3;
                            if(*(b + offset) == 0)
                            {
                                *(b + offset) = 60;
                            }
                        }
                    }
                }
                cursorCapture.UnlockBits(bmpData);
                return cursorCapture;
            }
            return null;
        }

        public void ResetCaptures()
        {
            _oldCapture = null;
            _newCapture = new Bitmap(1, 1);
        }

        #endregion

        #region proprieties



        #endregion
    }
}
