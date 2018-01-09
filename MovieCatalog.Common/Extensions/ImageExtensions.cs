namespace MovieCatalog.Common.Extensions
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;

    public static class ImageExtensions
    {
        // ImageConverter object used to convert byte arrays containing JPEG or PNG file images into
        // Bitmap objects. This is static and only gets instantiated once.
        private static readonly ImageConverter ImageConverter = new ImageConverter();

        public static async Task<byte[]> CreateThumbnail(this byte[] byteArray, int width, int height)
        {
            using (MemoryStream startMemoryStream = new MemoryStream(),
                                newMemoryStream = new MemoryStream())
            {
                // Write the string to the stream
                await startMemoryStream.WriteAsync(byteArray, 0, byteArray.Length);

                // Create the start Bitmap from the MemoryStream that contains the image
                Bitmap startBitmap = new Bitmap(startMemoryStream);

                // Create a new Bitmap with dimensions for the thumbnail.
                Bitmap newBitmap = new Bitmap(width, height);

                // Copy the image from the START Bitmap into the NEW Bitmap.
                // This will create a thumnail size of the same image.
                newBitmap = await Task.Run(() => ResizeBitmapImage(startBitmap, width, height));

                // Save this image to the specified stream in the specified format.
                newBitmap.Save(newMemoryStream, ImageFormat.Png);

                // Fill the byte[] for the thumbnail from the new MemoryStream.
                return newMemoryStream.ToArray();
            }
        }

        // Resize Bitmap
        public static Bitmap ResizeBitmapImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.CompositingMode = CompositingMode.SourceCopy;
                gfx.CompositingQuality = CompositingQuality.HighQuality;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;

                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }

            return resizedImage;
        }

        /// <summary>
        /// Method that uses the ImageConverter object in .Net Framework to convert a byte array,
        /// presumably containing a JPEG or PNG file image, into a Bitmap object, which can also be
        /// used as an Image object.
        /// </summary>
        /// <param name="byteArray">byte array containing JPEG or PNG file image or similar</param>
        /// <returns>Bitmap object if it works, else exception is thrown</returns>
        public static Bitmap ToBimapImage(this byte[] byteArray)
        {
            Bitmap bm = (Bitmap)ImageConverter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
                               bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                // Correct a strange glitch that has been observed in the test program when converting
                //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts"
                //  slightly away from the nominal integer value
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
                                 (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }

        /// <summary>
        /// Method to "convert" an Image object into a byte array, formatted in PNG file format, which
        /// provides lossless compression. This can be used together with the GetImageFromByteArray()
        /// method to provide a kind of serialization / deserialization.
        /// </summary>
        /// <param name="theImage">Image object, must be convertable to PNG format</param>
        /// <returns>byte array image of a PNG file containing the image</returns>
        public static byte[] ToByteArray(Image theImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                theImage.Save(memoryStream, ImageFormat.Png);

                return memoryStream.ToArray();
            }
        }

        public static Image ToImage(this byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
