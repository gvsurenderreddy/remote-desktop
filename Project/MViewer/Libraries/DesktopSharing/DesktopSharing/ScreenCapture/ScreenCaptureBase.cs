using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using Utils;
using GenericObjects;

namespace DesktopSharing
{
    public class ScreenCaptureBase
    {
        #region methods

        /// <summary>
        /// method used to get the desktop screen capture
        /// </summary>
        Bitmap GetDesktopCapture()
        {
            IntPtr desktopContextHeight = IntPtr.Zero;
            Bitmap screenImage = null;
            try
            {
                desktopContextHeight = WebcamWin32APIMethods.GetDesktopContext(WebcamWin32APIMethods.GetDesktopWindow());
                IntPtr gdiDesktopContext = GraphicDeviceInterfaceImports.CreateCompatibleDesktopContext(desktopContextHeight);
                DescriptorUtils.Structures.ScreenSize screenSize;
                screenSize.Width = WebcamWin32APIMethods.GetSystemMetrics(WebcamWin32APIMethods.Width);
                screenSize.Height = WebcamWin32APIMethods.GetSystemMetrics(WebcamWin32APIMethods.Height);
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
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                if (desktopContextHeight != IntPtr.Zero)
                {
                    WebcamWin32APIMethods.ReleaseDesktopContext(WebcamWin32APIMethods.GetDesktopWindow(), desktopContextHeight);
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
        Bitmap GetCursorCapture(ref int newX, ref int newY)
        {
            Bitmap cursorCapture = null;
            try
            {
                DescriptorUtils.Structures.CursorInfo cursorInfo = new DescriptorUtils.Structures.CursorInfo();
                DescriptorUtils.Structures.IconInfo iconInfo = new DescriptorUtils.Structures.IconInfo();
                cursorInfo.Size = Marshal.SizeOf(cursorInfo);
                if (WebcamWin32APIMethods.GetCursorInfo(out cursorInfo))
                {
                    if (cursorInfo.State == WebcamWin32APIMethods.CURSOR_SHOWING)
                    {
                        IntPtr iconHandle = WebcamWin32APIMethods.CopyIcon(cursorInfo.Handle);
                        if (WebcamWin32APIMethods.GetIconInfo(iconHandle, out iconInfo))
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
                            WebcamWin32APIMethods.DestroyIcon(iconHandle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
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
