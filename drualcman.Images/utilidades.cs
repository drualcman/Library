using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace drualcman.Images
{
    public partial class utilidades
    {

        /// <summary>
        /// Convert image to bytes[]
        /// </summary>
        /// <param name="imageIn"></param>
        /// <param name="formato">formato de la imagen</param>
        /// <returns></returns>
        public byte[] imageToByteArray(Image imageIn, ImageFormat formato)
        {
            using MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, formato);
            return ms.ToArray();
        }

        /// <summary>
        /// Convert image to bytes[]
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public byte[] imageToByteArray(Image imageIn)
        {
            return imageToByteArray(imageIn, imageIn.RawFormat);
        }

        /// <summary>
        /// Convert image to bytes[]
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public byte[] imageToByteArray(Bitmap imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
             //   return imageToByteArray(imageIn, new ImageFormat(System.Guid.NewGuid()));
            }
            }

        /// <summary>
        /// Convert image to bytes[]
        /// </summary>
        /// <param name="imageIn"></param>
        /// <param name="formato">formato de la imagen</param>
        /// <returns></returns>
        public byte[] imageToByteArray(Bitmap imageIn, ImageFormat formato)
        {

            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, formato);
            return ms.ToArray();
        }

        /// <summary>
        /// Convert bytes[] to image
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
