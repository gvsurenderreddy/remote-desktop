using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Utils
{
    public static class ScreenCaptureUtils
    {
        #region static members

        static Guid _id = Guid.NewGuid();

        #endregion

        #region static methods

        public static byte[] SerializeCapture(Image capture, Rectangle rect)
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

        public static byte[] SerializeCapture(Image capture, int x, int y)
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
    }
}
