/*
 * Created by Rajan Tawate.
 * User: Owner
 * Date: 9/3/2006
 * Time: 8:00 PM
 * 

 */

using System;
using System.Drawing;
using System.IO;
using System.Collections;

	/// <summary>
	/// Description of ImageConverter.
	/// </summary>
	public static class ImageConverter
	{
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            Bitmap result = null;
            try
            {
                //a holder for the result
                result = new Bitmap(width, height);
                // set the resolutions the same to avoid cropping due to resolution differences
                result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                //use a graphics object to draw the resized image into the bitmap
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    //set the resize quality modes to high quality
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //draw the image into the target bitmap
                    graphics.DrawImage(image, 0, 0, result.Width, result.Height);
                }
            }
            catch (Exception ex)
            {

            }
            //return the resulting bitmap
            return result;
        }

        // todo: update the below methods

        //public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imageIn.Save(ms,System.Drawing.Imaging.ImageFormat.Gif);
        //    return  ms.ToArray();
        //}

        //public static Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
	
        //}
		
	}

