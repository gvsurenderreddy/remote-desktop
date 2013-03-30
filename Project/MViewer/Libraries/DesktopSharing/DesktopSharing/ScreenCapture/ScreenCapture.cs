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
                     //if we do not have a previous screen we have to capture the whole screen
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
            // todo: implement GetRectangle
            //throw new NotImplementedException();

            // The search algorithm starts by looking
            //	for the top and left bounds. The search
            //	starts in the upper-left corner and scans
            //	left to right and then top to bottom. It uses
            //	an adaptive approach on the pixels it
            //	searches. Another pass is looks for the
            //	lower and right bounds. The search starts
            //	in the lower-right corner and scans right
            //	to left and then bottom to top. Again, an
            //	adaptive approach on the search area is used.
            //
            
            // Notice: The GetPixel member of the Bitmap class
            //	is too slow for this purpose. This is a good
            //	case of using unsafe code to access pointers
            //	to increase the speed.
            //
            
            // Validate the images are the same shape and type.
            //
            if (_oldCapture.Width != _newCapture.Width ||
                _oldCapture.Height != _newCapture.Height ||
                _oldCapture.PixelFormat != _newCapture.PixelFormat)
            {
                // Not the same shape...can't do the search.
                //
                return Rectangle.Empty;
            }

            // Init the search parameters.
            //
            int width = _newCapture.Width;
            int height = _newCapture.Height;
            int left = width;
            int right = 0;
            int top = height;
            int bottom = 0;

            BitmapData bmNewData = null;
            BitmapData bmPrevData = null;
            try
            {
                // Lock the bits into memory.
                //
                bmNewData = _newCapture.LockBits(
                    new Rectangle(0, 0, _newCapture.Width, _newCapture.Height),
                    ImageLockMode.ReadOnly, _newCapture.PixelFormat);
                bmPrevData = _oldCapture.LockBits(
                    new Rectangle(0, 0, _oldCapture.Width, _oldCapture.Height),
                    ImageLockMode.ReadOnly, _oldCapture.PixelFormat);

                // The images are ARGB (4 bytes)
                //
                const int numBytesPerPixel = 4;

                // Get the number of integers (4 bytes) in each row
                //	of the image.
                //
                int strideNew = bmNewData.Stride / numBytesPerPixel;
                int stridePrev = bmPrevData.Stride / numBytesPerPixel;

                // Get a pointer to the first pixel.
                //
                // Notice: Another speed up implemented is that I don't
                //	need the ARGB elements. I am only trying to detect
                //	change. So this algorithm reads the 4 bytes as an
                //	integer and compares the two numbers.
                //
                IntPtr scanNew0 = bmNewData.Scan0;
                IntPtr scanPrev0 = bmPrevData.Scan0;

                // Enter the unsafe code.
                //
                unsafe
                {
                    // Cast the safe pointers into unsafe pointers.
                    //
                    int* pNew = (int*)(void*)scanNew0;
                    int* pPrev = (int*)(void*)scanPrev0;

                    // First Pass - Find the left and top bounds
                    //	of the minimum bounding rectangle. Adapt the
                    //	number of pixels scanned from left to right so
                    //	we only scan up to the current bound. We also
                    //	initialize the bottom & right. This helps optimize
                    //	the second pass.
                    //
                    // For all rows of pixels (top to bottom)
                    //
                    for (int y = 0; y < _newCapture.Height; ++y)
                    {
                        // For pixels up to the current bound (left to right)
                        //
                        for (int x = 0; x < left; ++x)
                        {
                            // Use pointer arithmetic to index the
                            //	next pixel in this row.
                            //
                            if ((pNew + x)[0] != (pPrev + x)[0])
                            {
                                // Found a change.
                                //
                                if (x < left)
                                {
                                    left = x;
                                }
                                if (x > right)
                                {
                                    right = x;
                                }
                                if (y < top)
                                {
                                    top = y;
                                }
                                if (y > bottom)
                                {
                                    bottom = y;
                                }
                            }
                        }

                        // Move the pointers to the next row.
                        //
                        pNew += strideNew;
                        pPrev += stridePrev;
                    }

                    // If we did not find any changed pixels
                    //	then no need to do a second pass.
                    //
                    if (left != width)
                    {
                        // Second Pass - The first pass found at
                        //	least one different pixel and has set
                        //	the left & top bounds. In addition, the
                        //	right & bottom bounds have been initialized.
                        //	Adapt the number of pixels scanned from right
                        //	to left so we only scan up to the current bound.
                        //	In addition, there is no need to scan past
                        //	the top bound.
                        //

                        // Set the pointers to the first element of the
                        //	bottom row.
                        //
                        pNew = (int*)(void*)scanNew0;
                        pPrev = (int*)(void*)scanPrev0;
                        pNew += (_newCapture.Height - 1) * strideNew;
                        pPrev += (_oldCapture.Height - 1) * stridePrev;

                        // For each row (bottom to top)
                        //
                        for (int y = _newCapture.Height - 1; y > top; y--)
                        {
                            // For each column (right to left)
                            //
                            for (int x = _newCapture.Width - 1; x > right; x--)
                            {
                                // Use pointer arithmetic to index the
                                //	next pixel in this row.
                                //
                                if ((pNew + x)[0] != (pPrev + x)[0])
                                {
                                    // Found a change.
                                    //
                                    if (x > right)
                                    {
                                        right = x;
                                    }
                                    if (y > bottom)
                                    {
                                        bottom = y;
                                    }
                                }
                            }

                            // Move up one row.
                            //
                            pNew -= strideNew;
                            pPrev -= stridePrev;
                        }
                    }
                }
            }
            catch 
            {
                System.Diagnostics.Debugger.Break();
            }
            finally
            {
                // Unlock the bits of the image.
                //
                if (bmNewData != null)
                {
                    _newCapture.UnlockBits(bmNewData);
                }
                if (bmPrevData != null)
                {
                    _oldCapture.UnlockBits(bmPrevData);
                }
            }

            // Validate we found a bounding box. If not
            //	return an empty rectangle.
            //
            int diffImgWidth = right - left + 1;
            int diffImgHeight = bottom - top + 1;
            if (diffImgHeight < 0 || diffImgWidth < 0)
            {
                // Nothing changed
                return Rectangle.Empty;
            }

            // Return the bounding box.
            //
            return new Rectangle(left, top, diffImgWidth, diffImgHeight);
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
