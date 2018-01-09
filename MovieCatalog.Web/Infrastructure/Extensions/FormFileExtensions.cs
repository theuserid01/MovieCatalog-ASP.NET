namespace MovieCatalog.Web.Infrastructure.Extensions
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using MovieCatalog.Common;

    public static class FormFileExtensions
    {
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile formFile)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public static Image ToImage(this IFormFile file)
        {
            return Image.FromStream(file.OpenReadStream(), true, true);
        }

        public static bool ValidateImage(this IFormFile file, ModelStateDictionary ModelState)
        {
            // Allowed image formats
            ImageFormat[] allowedImageFormats = new ImageFormat[]
            {
                    ImageFormat.Bmp,
                    ImageFormat.Gif,
                    ImageFormat.Jpeg,
                    ImageFormat.Png,
                    ImageFormat.Tiff
            };

            string ext = Path.GetExtension(file.FileName).ToLower();
            if (!DataConstants.AllowedImageExtensions.Contains(ext))
            {
                string allowedImageExtensions = string.Join(", ", DataConstants.AllowedImageExtensions);
                ModelState.AddModelError(string.Empty, $"Allowed image extensions {allowedImageExtensions}");

                return false;
            }

            if (file.Length > DataConstants.PosterMaxLength)
            {
                string mb = $"{DataConstants.ThumbnailMaxLength / 1024 / 1024} MB";
                ModelState.AddModelError(string.Empty, $"Allowed max file size {mb}");

                return false;
            }

            try
            {
                // Create an Image from the specified data stream
                using (Image img = Image.FromStream(file.OpenReadStream()))
                {
                    // Return true if the image format is allowed
                    if (!allowedImageFormats.Contains(img.RawFormat))
                    {
                        ModelState.AddModelError(string.Empty, $"{file.FileName} has no valid image format.");

                        return false;
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"{file.FileName} is not a valid image.");

                return false;
            }

            return true;
        }
    }
}
