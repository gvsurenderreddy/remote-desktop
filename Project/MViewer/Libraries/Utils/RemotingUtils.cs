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
        #region members

        Guid _id = Guid.NewGuid();

        #endregion

        #region public methods

        #region static methods

        //todo: use the below methods if necessary
        public double convertXToAbsolute(int x)
        {
            return ((double)65535 * x) / (double)Screen.PrimaryScreen.Bounds.Width;
        }

        public double convertYToAbsolute(int y)
        {
            return ((double)65535 * y) / (double)Screen.PrimaryScreen.Bounds.Height;
        }

        public byte[] SerializeDesktopCapture(Image capture, Rectangle rect)
        {
            byte[] data = _id.ToByteArray();
            byte[] temp;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                capture.Save(memoryStream, ImageFormat.Jpeg);
                temp = memoryStream.ToArray();
            }
            // get the bounds
            byte[] rectTop = BitConverter.GetBytes(rect.Top);
            byte[] rectBottom = BitConverter.GetBytes(rect.Bottom);
            byte[] rectLeft = BitConverter.GetBytes(rect.Left);
            byte[] rectRight = BitConverter.GetBytes(rect.Right);

            // create final bytes
            int size = rectTop.Length;
            byte[] serialized = new byte[temp.Length + 4 * size + data.Length];
            Array.Copy(rectTop, 0, serialized, 0, rectTop.Length);
            Array.Copy(rectBottom, 0, serialized, size, rectBottom.Length);
            Array.Copy(rectLeft, 0, serialized, 2 * size, rectLeft.Length);
            Array.Copy(rectRight, 0, serialized, 3 * size, rectRight.Length);
            Array.Copy(temp, 0, serialized, 4 * size, temp.Length);
            Array.Copy(data, 0, serialized, 4 * size + temp.Length, data.Length);
            return serialized;
        }

        public byte[] SerializeMouseCapture(Image capture, int x, int y)
        {
            byte[] data = _id.ToByteArray();
            byte[] temp;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                capture.Save(memoryStream, ImageFormat.Png);
                temp = memoryStream.ToArray();
            }
            byte[] x1 = BitConverter.GetBytes(x);
            byte[] y1 = BitConverter.GetBytes(y);
            byte[] serialized = new byte[temp.Length + x1.Length + y1.Length + data.Length];
            Array.Copy(x1, 0, serialized, 0, x1.Length);
            Array.Copy(y1, 0, serialized, x1.Length, y1.Length);
            Array.Copy(temp, 0, serialized, x1.Length + y1.Length, temp.Length);
            Array.Copy(data, 0, serialized, x1.Length + y1.Length + temp.Length, data.Length);
            return serialized;
        }

        #endregion

        public Image AppendMouseToDesktop(Image screen, Image cursor, int cursorX, int cursorY)
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

        public void Deserialize(byte[] data, out Image image, out int cursorX, out int cursorY, out Guid id)
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

        public void Deserialize(byte[] data, out Image image, out Rectangle bounds, out Guid id)
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

        public Size GetResolution()
        {
            Size size = Screen.PrimaryScreen.Bounds.Size;
            return size;
        }

        #endregion
    }
}
