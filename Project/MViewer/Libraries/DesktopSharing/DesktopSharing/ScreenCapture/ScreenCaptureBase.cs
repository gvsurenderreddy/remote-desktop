using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using Utils;

namespace DesktopSharing
{
    public class ScreenCaptureBase //: ScreenSizeStructures
    {
        #region methods

        /// <summary>
        /// method used to get the desktop screen capture
        /// </summary>
        public Bitmap GetDesktopCapture()
        {
            IntPtr desktopContextHeight = IntPtr.Zero;
            Bitmap screenImage = null;
            try
            {
                desktopContextHeight = Win32Imports.GetDesktopContext(Win32Imports.GetDesktopWindow());
                IntPtr gdiDesktopContext = GraphicDeviceInterfaceImports.CreateCompatibleDesktopContext(desktopContextHeight);
                Structures.ScreenSize screenSize;
                screenSize.Width = Win32Imports.GetSystemMetrics(Win32Imports.Width);
                screenSize.Height = Win32Imports.GetSystemMetrics(Win32Imports.Height);
                IntPtr gdiBitmap = GraphicDeviceInterfaceImports.CreateCompatibleBitmap(desktopContextHeight, screenSize.Width, screenSize.Height);
                if (gdiBitmap != IntPtr.Zero)
                {
                    IntPtr oldGDI = GraphicDeviceInterfaceImports.SelectObject(gdiDesktopContext, gdiBitmap);
                    GraphicDeviceInterfaceImports.BitBlt(gdiDesktopContext, 0, 0, screenSize.Width, screenSize.Height, desktopContextHeight,
                        0, 0, GraphicDeviceInterfaceImports.SRCCOPY);
                    GraphicDeviceInterfaceImports.SelectObject(gdiDesktopContext, oldGDI);
                    screenImage = Image.FromHbitmap(gdiBitmap);
                    GraphicDeviceInterfaceImports.DeleteObject(gdiBitmap);
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (desktopContextHeight != IntPtr.Zero)
                {
                    Win32Imports.ReleaseDesktopContext(Win32Imports.GetDesktopWindow(), desktopContextHeight);
                }
            }
            return screenImage;
        }

        /// <summary>
        /// method used to get the cursor capture
        /// </summary>
        /// <param name="newX">new cursor X coord</param>
        /// <param name="y">new cursor Y coord</param>
        /// <returns></returns>
        public Bitmap GetCursorCapture(ref int newX, ref int newY)
        {
            Bitmap cursorCapture = null;
            try
            {
                Structures.CursorInfo cursorInfo = new Structures.CursorInfo();
                Structures.IconInfo iconInfo = new Structures.IconInfo();
                cursorInfo.Size = Marshal.SizeOf(cursorInfo);
                if (Win32Imports.GetCursorInfo(out cursorInfo))
                {
                    if (cursorInfo.State == Win32Imports.CURSOR_SHOWING)
                    {
                        IntPtr iconHandle = Win32Imports.CopyIcon(cursorInfo.Handle);
                        if (Win32Imports.GetIconInfo(iconHandle, out iconInfo))
                        {
                            if (iconInfo.Bitmask != IntPtr.Zero)
                            {
                                GraphicDeviceInterfaceImports.DeleteObject(iconInfo.Bitmask);
                            }
                            if (iconInfo.Color != IntPtr.Zero)
                            {
                                GraphicDeviceInterfaceImports.DeleteObject(iconInfo.Color);
                            }
                            newX = cursorInfo.Coordinates.X - iconInfo.Xcoord;
                            newY = cursorInfo.Coordinates.Y - iconInfo.Ycoord;
                            Icon newIcon = Icon.FromHandle(iconHandle);
                            if (newIcon.Width > 0 && newIcon.Height > 0)
                            {
                                cursorCapture = newIcon.ToBitmap();
                            }
                            Win32Imports.DestroyIcon(iconHandle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return cursorCapture;
        }

        /// <summary>
        /// method used to retrieve both desktop and cursor captures
        /// </summary>
        /// <returns>bitmap image</returns>
        public Bitmap GetDesktopAndCursorCapture()
        {
            Bitmap desktopCapture = GetDesktopCapture();
            int cursorXcoord = 0, cursorYcoord = 0;
            Bitmap cursorCapture = GetCursorCapture(ref cursorXcoord, ref cursorYcoord);
            if (desktopCapture != null)
            {
                if (cursorCapture != null)
                {
                    Rectangle rect = new Rectangle(cursorXcoord, cursorYcoord, cursorCapture.Width, cursorCapture.Height);
                    Graphics graphics = Graphics.FromImage(desktopCapture);
                    // append the cursor capture to the desktop capture
                    graphics.DrawImage(cursorCapture, rect);
                    graphics.Flush();
                }
                //return only the desktop capture if no mouse cursor capture is available
            }
            return desktopCapture;
        }

        #endregion
    }
}
