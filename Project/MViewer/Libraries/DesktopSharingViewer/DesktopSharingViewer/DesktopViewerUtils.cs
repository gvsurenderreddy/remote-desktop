using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace DesktopSharingViewer
{
    public static class DesktopViewerUtils
    {
        #region static methods

        public static void UpdateScreen(ref Image screen, Image newPartialScreen, Rectangle boundingBox)
        {
            // Create the first screen if one does not exist.
            //
            if (screen == null)
            {
                screen = new Bitmap(boundingBox.Width, boundingBox.Height);
            }

            // Draw the partial image into the current
            //	screen. This replaces the changed pixels.
            //
            Graphics g = null;
            try
            {
                lock (screen)
                {
                    g = Graphics.FromImage(screen);
                    g.DrawImage(newPartialScreen, boundingBox);
                    g.Flush();
                }
            }
            catch(Exception ex)
            {
                // Do something with this info.
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (g != null) g.Dispose();
            }
        }

        public static Image AppendMouseToDesktop(Image screen, Image cursor, int cursorX, int cursorY)
        {
            Image mergedImage = null;
            Graphics g = null;
            try
            {
                lock (screen)
                {
                    mergedImage = (Image)screen.Clone();
                }
                Rectangle r;
                lock (cursor)
                {
                    r = new Rectangle(cursorX, cursorY, cursor.Width, cursor.Height);
                }
                g = Graphics.FromImage(mergedImage);
                g.DrawImage(cursor, r);
                g.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }

            return mergedImage;
        }

        public static void Deserialize(byte[] data, out Image image, out int cursorX, out int cursorY, out Guid id)
        {
            // Unpack the data that is transferred over the wire.
            //

            // Create byte arrays to hold the unpacked parts.
            //
            const int numBytesInInt = sizeof(int);
            int idLength = Guid.NewGuid().ToByteArray().Length;
            int imgLength = data.Length - 2 * numBytesInInt - idLength;
            byte[] xPosData = new byte[numBytesInInt];
            byte[] yPosData = new byte[numBytesInInt];
            byte[] imgData = new byte[imgLength];
            byte[] idData = new byte[idLength];

            // Fill the byte arrays.
            //
            Array.Copy(data, 0, xPosData, 0, numBytesInInt);
            Array.Copy(data, numBytesInInt, yPosData, 0, numBytesInInt);
            Array.Copy(data, 2 * numBytesInInt, imgData, 0, imgLength);
            Array.Copy(data, 2 * numBytesInInt + imgLength, idData, 0, idLength);

            // Create the cursor position x,y values.
            //
            cursorX = BitConverter.ToInt32(xPosData, 0);
            cursorY = BitConverter.ToInt32(yPosData, 0);

            // Create the bitmap from the byte array.
            //
            MemoryStream ms = new MemoryStream(imgData, 0, imgData.Length);
            ms.Write(imgData, 0, imgData.Length);
            image = Image.FromStream(ms, true);

            // Create a Guid
            //
            id = new Guid(idData);
        }

        public static void Deserialize(byte[] data, out Image image, out Rectangle bounds, out Guid id)
        {
            //if (data != null)
            {
                // Unpack the data that is transferred over the wire.
                //

                // Create byte arrays to hold the unpacked parts.
                //
                const int numBytesInInt = sizeof(int);
                int idLength = Guid.NewGuid().ToByteArray().Length;
                int imgLength = data.Length - 4 * numBytesInInt - idLength;
                byte[] topPosData = new byte[numBytesInInt];
                byte[] botPosData = new byte[numBytesInInt];
                byte[] leftPosData = new byte[numBytesInInt];
                byte[] rightPosData = new byte[numBytesInInt];
                byte[] imgData = new byte[imgLength];
                byte[] idData = new byte[idLength];

                // Fill the byte arrays.
                //
                Array.Copy(data, 0, topPosData, 0, numBytesInInt);
                Array.Copy(data, numBytesInInt, botPosData, 0, numBytesInInt);
                Array.Copy(data, 2 * numBytesInInt, leftPosData, 0, numBytesInInt);
                Array.Copy(data, 3 * numBytesInInt, rightPosData, 0, numBytesInInt);
                Array.Copy(data, 4 * numBytesInInt, imgData, 0, imgLength);
                Array.Copy(data, 4 * numBytesInInt + imgLength, idData, 0, idLength);

                // Create the bitmap from the byte array.
                //
                MemoryStream memoryStream = new MemoryStream(imgData, 0, imgData.Length);
                memoryStream.Write(imgData, 0, imgData.Length);
                image = Image.FromStream(memoryStream, true);

                // Create the bound rectangle.
                //
                int top = BitConverter.ToInt32(topPosData, 0);
                int bot = BitConverter.ToInt32(botPosData, 0);
                int left = BitConverter.ToInt32(leftPosData, 0);
                int right = BitConverter.ToInt32(rightPosData, 0);
                int width = right - left + 1;
                int height = bot - top + 1;
                bounds = new Rectangle(left, top, width, height);

                // Create a Guid
                //
                id = new Guid(idData);
            }
            //else
            //{
            //    image = null;
            //    id = new Guid(new byte[0]);
            //}
        }

        #endregion
    }
}
