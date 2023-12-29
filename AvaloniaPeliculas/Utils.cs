using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AvaloniaPeliculas
{
    internal class Utils
    {
        /// <summary>
        /// Convierte una imagen en un array de bytes.
        /// </summary>
        /// <param name="filePath">La ruta del archivo de imagen.</param>
        /// <returns>Un array de bytes que representa la imagen.</returns>
        public static byte[] imageToByteArray(string filePath)
        {
            Image imageIn = Image.FromFile(filePath);
            string extension = Path.GetExtension(filePath);

            // Determine the image format based on the extension
            ImageFormat imageFormat = ImageFormat.Jpeg;
            switch (extension)
            {
                case ".jpg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".tiff":
                    imageFormat = ImageFormat.Tiff;
                    break;
                default:
                    // Handle unsupported image formats
                    throw new Exception("Unsupported image format: " + extension);
            }
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imageFormat);
            return ms.ToArray();
        }
        /// <summary>
        /// Convierte un array de bytes en una imagen.
        /// </summary>
        /// <param name="byteArrayIn">El array de bytes que representa la imagen.</param>
        /// <returns>Una imagen creada a partir del array de bytes.</returns>
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}