using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Utils
{
    public class RemotingUtils
    {
        #region private members

        Guid _id = Guid.NewGuid();

        #endregion

        #region public methods

        public double ConvertXToRemote(double localX, double localWidth)
        {
            //local_x * remote_width / local_width

            double remote_x = localX / localWidth;
            return remote_x;
        }

        public double ConvertYToRemote(double localY, double localHeight)
        {
            //local_y * remote_height / local_height
            double remote_y = localY / localHeight;
            return remote_y;
        }

        public double ConvertXToAbsolute(double remoteX)
        {
            //local_x * remote_width / local_width

            double absolute_x = remoteX * Screen.PrimaryScreen.Bounds.Size.Width;
            return absolute_x;
        }

        public double ConvertYToAbsolute(double remoteY)
        {
            //local_y * remote_height / local_height
            double absolute_y = remoteY * Screen.PrimaryScreen.Bounds.Size.Height;
            return absolute_y;
        }

        public byte[] SerializeDesktopCapture(Image capture, Rectangle rectBounds)
        {
            byte[] data = _id.ToByteArray();
            byte[] Buffer;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                capture.Save(memoryStream, ImageFormat.Jpeg);
                Buffer = memoryStream.ToArray();
            }
            // get the bounds
            byte[] rectTopBound = BitConverter.GetBytes(rectBounds.Top);
            byte[] rectBottomBound = BitConverter.GetBytes(rectBounds.Bottom);
            byte[] rectLeftBound = BitConverter.GetBytes(rectBounds.Left);
            byte[] rectRightBound = BitConverter.GetBytes(rectBounds.Right);

            // create final bytes
            int size = rectTopBound.Length;
            byte[] serializedScreen = new byte[Buffer.Length + 4 * size + data.Length];
            Array.Copy(rectTopBound, 0, serializedScreen, 0, rectTopBound.Length);
            Array.Copy(rectBottomBound, 0, serializedScreen, size, rectBottomBound.Length);
            Array.Copy(rectLeftBound, 0, serializedScreen, 2 * size, rectLeftBound.Length);
            Array.Copy(rectRightBound, 0, serializedScreen, 3 * size, rectRightBound.Length);
            Array.Copy(Buffer, 0, serializedScreen, 4 * size, Buffer.Length);
            Array.Copy(data, 0, serializedScreen, 4 * size + Buffer.Length, data.Length);
            return serializedScreen;
        }

        public byte[] SerializeMouseCapture(Image capture, int x, int y)
        {
            byte[] data = _id.ToByteArray();
            byte[] tempBuffer;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                capture.Save(memoryStream, ImageFormat.Png);
                tempBuffer = memoryStream.ToArray();
            }
            byte[] x1 = BitConverter.GetBytes(x);
            byte[] y1 = BitConverter.GetBytes(y);
            byte[] serializedCapture = new byte[tempBuffer.Length + x1.Length + y1.Length + data.Length];
            Array.Copy(x1, 0, serializedCapture, 0, x1.Length);
            Array.Copy(y1, 0, serializedCapture, x1.Length, y1.Length);
            Array.Copy(tempBuffer, 0, serializedCapture, x1.Length + y1.Length, tempBuffer.Length);
            Array.Copy(data, 0, serializedCapture, x1.Length + y1.Length + tempBuffer.Length, data.Length);
            return serializedCapture;
        }

        public Image AppendMouseToDesktop(Image screenCapture, Image cursor, int cursorX, int cursorY)
        {
            Image mergedCapture = null;
            Graphics graphics = null;
            try
            {
                lock (screenCapture)
                {
                    mergedCapture = (Image)screenCapture.Clone();
                }
                Rectangle bounds;
                lock (cursor)
                {
                    bounds = new Rectangle(cursorX, cursorY, cursor.Width, cursor.Height);
                }
                graphics = Graphics.FromImage(mergedCapture);
                graphics.DrawImage(cursor, bounds);
                graphics.Flush();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
            }
            return mergedCapture;
        }

        public void DeserializeMouseCapture(byte[] serializedCapture, out Image image, out int cursorX, out int cursorY, out Guid id)
        {
            // unpack the received data 
            // create buffers to hold the unpacked parts
            const int numBytesInInt = sizeof(int);
            int idLength = Guid.NewGuid().ToByteArray().Length;
            int imgLength = serializedCapture.Length - 2 * numBytesInInt - idLength;
            byte[] xPosData = new byte[numBytesInInt];
            byte[] yPosData = new byte[numBytesInInt];
            byte[] imgData = new byte[imgLength];
            byte[] idData = new byte[idLength];

            // fill the buffers
            Array.Copy(serializedCapture, 0, xPosData, 0, numBytesInInt);
            Array.Copy(serializedCapture, numBytesInInt, yPosData, 0, numBytesInInt);
            Array.Copy(serializedCapture, 2 * numBytesInInt, imgData, 0, imgLength);
            Array.Copy(serializedCapture, 2 * numBytesInInt + imgLength, idData, 0, idLength);

            // create the cursor coordinates x,y 
            cursorX = BitConverter.ToInt32(xPosData, 0);
            cursorY = BitConverter.ToInt32(yPosData, 0);

            // obtain the bitmap from the buffer
            MemoryStream memoryStream = new MemoryStream(imgData, 0, imgData.Length);
            memoryStream.Write(imgData, 0, imgData.Length);
            image = Image.FromStream(memoryStream, true);

            // create a Guid
            id = new Guid(idData);
        }

        public void DeserializeDesktopCapture(byte[] data, out Image image, out Rectangle bounds, out Guid id)
        {
            // unpack the received data 
            // create buffers to hold the unpacked parts
            const int numBytesInInt = sizeof(int);
            int idLength = Guid.NewGuid().ToByteArray().Length;
            int imgLength = data.Length - 4 * numBytesInInt - idLength;
            byte[] topPosData = new byte[numBytesInInt];
            byte[] botPosData = new byte[numBytesInInt];
            byte[] leftPosData = new byte[numBytesInInt];
            byte[] rightPosData = new byte[numBytesInInt];
            byte[] imgData = new byte[imgLength];
            byte[] idData = new byte[idLength];

            // fill the buffers
            Array.Copy(data, 0, topPosData, 0, numBytesInInt);
            Array.Copy(data, numBytesInInt, botPosData, 0, numBytesInInt);
            Array.Copy(data, 2 * numBytesInInt, leftPosData, 0, numBytesInInt);
            Array.Copy(data, 3 * numBytesInInt, rightPosData, 0, numBytesInInt);
            Array.Copy(data, 4 * numBytesInInt, imgData, 0, imgLength);
            Array.Copy(data, 4 * numBytesInInt + imgLength, idData, 0, idLength);

            // obtain the bitmap from the buffer
            MemoryStream memoryStream = new MemoryStream(imgData, 0, imgData.Length);
            memoryStream.Write(imgData, 0, imgData.Length);
            image = Image.FromStream(memoryStream, true);

            // create the rectangle bounds
            int top = BitConverter.ToInt32(topPosData, 0);
            int bot = BitConverter.ToInt32(botPosData, 0);
            int left = BitConverter.ToInt32(leftPosData, 0);
            int right = BitConverter.ToInt32(rightPosData, 0);
            int width = right - left + 1;
            int height = bot - top + 1;
            bounds = new Rectangle(left, top, width, height);

            // create Guid
            id = new Guid(idData);
        }

        #endregion
    }
}
