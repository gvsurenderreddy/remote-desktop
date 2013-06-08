using System;
using System.Drawing;
using System.IO;
using System.Collections;
using Utils;

public class ImageConverter
{
    #region public methods

    public Bitmap ResizeImage(Image imageToResize, int width, int height)
    {
        Bitmap resizedImage = null;
        //try
        //{
            //a holder for the result
            resizedImage = new Bitmap(width, height);
            // set the resolutions the same to avoid cropping due to resolution differences
            resizedImage.SetResolution(imageToResize.HorizontalResolution, imageToResize.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(imageToResize, 0, 0, resizedImage.Width, resizedImage.Height);
            }
        //}
        //catch (Exception ex)
        //{
        //    Tools.Instance.Logger.LogError(ex.ToString());
        //}
        //return the resulting bitmap
        return resizedImage;
    }

    #endregion
}