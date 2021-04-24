using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Drawing.Drawing2D;

/// <summary>
/// Summary description for drArchivos
/// </summary>
namespace drualcman
{
    public class imagenes
    {
        #region utilidades

        /// <summary>
        /// Convert image to bytes[]
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public byte[] imageToByteArray(Image imageIn)
        {
            utilidades a = new utilidades();
            byte[] retorno = a.imageToByteArray(imageIn);
            a = null;
            return retorno;
        }

        /// <summary>
        /// Convert bytes[] to image
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            utilidades a = new utilidades();
            Image retorno = a.byteArrayToImage(byteArrayIn);
            a = null;
            return retorno;
        }
        #endregion utilidades

        #region Insertar Texto
        /// <summary>
        /// Insertar texto dentro de una imagen
        /// </summary>
        /// <param name="file">Nombre imagen original</param>
        /// <param name="rutaImg">Carpeta donde se encuentra y guardara la nueva imagen</param>
        /// <param name="Str_TextOnImage">Texto a incluir</param>
        /// <param name="StringColor">Color del texto</param>
        /// <param name="fuente">tipo de fuente a utilizar</param>
        /// <param name="posicion">posicion dentro de la imagen</param>
        /// <param name="horizontal">alineacion horizontal</param>
        /// <param name="vertical">alineacion vertial</param>
        /// <returns>Devuelve el nombre de la nueva imagen</returns>
        public string InsertaTexto(string file, string folder, string Str_TextOnImage, Color StringColor,
            Font fuente, Point posicion, StringAlignment horizontal, StringAlignment vertical)
        {
                
            return imagenes.InsertarTexto(file, folder, Str_TextOnImage, StringColor, fuente, posicion, horizontal, vertical);
        }

        /// <summary>
        /// Insertar texto dentro de una imagen
        /// </summary>
        /// <param name="bitmap">Imagen a modificar</param>
        /// <param name="rutaImg">Carpeta donde se encuentra y guardara la nueva imagen</param>
        /// <param name="Str_TextOnImage">Texto a incluir</param>
        /// <param name="StringColor">Color del texto</param>
        /// <param name="fuente">tipo de fuente a utilizar</param>
        /// <param name="posicion">posicion dentro de la imagen</param>
        /// <param name="horizontal">alineacion horizontal</param>
        /// <param name="vertical">alineacion vertial</param>
        /// <returns>Devuelve el nombre de la nueva imagen</returns>
        public static string InsertaTexto(Image bitmap, string folder, string Str_TextOnImage, Color StringColor,
            Font fuente, Point posicion, StringAlignment horizontal, StringAlignment vertical)
        {
            return imagenes.InsertarTexto(bitmap, folder, Str_TextOnImage, StringColor, fuente, posicion, horizontal, vertical);
        }

        /// <summary>
        /// Insertar texto dentro de una imagen
        /// </summary>
        /// <param name="file">Nombre imagen original</param>
        /// <param name="rutaImg">Carpeta donde se encuentra y guardara la nueva imagen</param>
        /// <param name="Str_TextOnImage">Texto a incluir</param>
        /// <param name="StringColor">Color del texto</param>
        /// <param name="fuente">tipo de fuente a utilizar</param>
        /// <param name="posicion">posicion dentro de la imagen</param>
        /// <param name="horizontal">alineacion horizontal</param>
        /// <param name="vertical">alineacion vertial</param>
        /// <returns>Devuelve el nombre de la nueva imagen</returns>
        public Image InsertaTextoImagen(string file, string folder, string Str_TextOnImage, Color StringColor,
            Font fuente, Point posicion, StringAlignment horizontal, StringAlignment vertical)
        {
            return imagenes.InsertarTextoImagen(file, folder, Str_TextOnImage, StringColor, fuente, posicion, horizontal, vertical);
        }

        /// <summary>
        /// Insertar texto dentro de una imagen
        /// </summary>
        /// <param name="imagen">Imagen a modificar</param>
        /// <param name="Str_TextOnImage">Texto a incluir</param>
        /// <param name="StringColor">Color del texto</param>
        /// <param name="fuente">tipo de fuente a utilizar</param>
        /// <param name="posicion">posicion dentro de la imagen</param>
        /// <param name="horizontal">alineacion horizontal</param>
        /// <param name="vertical">alineacion vertial</param>
        /// <returns>Devuelve el nombre de la nueva imagen</returns>
        public static Image InsertaTextoImagen(Image bitmap, string Str_TextOnImage, Color StringColor,
            Font fuente, Point posicion, StringAlignment horizontal, StringAlignment vertical)
        {
            return imagenes.InsertarTextoImagen(bitmap, Str_TextOnImage, StringColor, fuente, posicion, horizontal, vertical);
        }

        /// <summary>
        /// Insertar texto dentro de una imagen en la base por defecto
        /// </summary>
        /// <param name="file">Nombre imagen original</param>
        /// <param name="rutaImg">Carpeta donde se encuentra y guardara la nueva imagen</param>
        /// <param name="Str_TextOnImage">Texto a incluir</param>
        /// <param name="StringColor">Color del texto</param>
        /// <param name="fuente">tipo de fuente a utilizar</param>
        /// <param name="posicion">posicion dentro de la imagen</param>
        /// <param name="horizontal">alineacion horizontal</param>
        /// <param name="vertical">alineacion vertial</param>
        /// <returns>Devuelve el nombre de la nueva imagen</returns>
        public static string InsertarTexto(string file, string folder, string Str_TextOnImage, Color StringColor,  
            Font fuente, Point posicion, StringAlignment horizontal, StringAlignment vertical)
        {
                //creando el objeto de la imagen
                archivos a = new archivos();
                string rutaImg = a.checkCarpeta(folder) + file;
                string newFile = a.creaNombreFile(folder, "jpg", "txt");
                a = null;
                Image bitmap = (Image)Bitmap.FromFile(rutaImg);         // set image
                //Dibujo la imagen
                Graphics graphicsImage = Graphics.FromImage(bitmap);
                //Establezco la orientación mediante coordenadas   
                StringFormat stringformat = new StringFormat();
                stringformat.Alignment = horizontal;
                stringformat.LineAlignment = vertical;
                //si la posicion es 0,0 calcular donde esta el fina del archivo para poner el texto justo al final
                Point MiPOS = new Point();
                if (posicion.Y == 0 && posicion.X == 0)
                {
                    MiPOS.Y = bitmap.Height - 25;
                    MiPOS.X = 175;
                }
                else MiPOS = posicion;
                //modificar la imagen
                graphicsImage.DrawString(Str_TextOnImage, fuente, new SolidBrush(StringColor), MiPOS, stringformat);
                graphicsImage.Save();
                //guardar la nueva imagen
                bitmap.Save(newFile);
                return newFile;
        }

        /// <summary>
        /// Insertar texto dentro de una imagen
        /// </summary>
        /// <param name="bitmap">Imagen a modificar</param>
        /// <param name="rutaImg">Carpeta donde se encuentra y guardara la nueva imagen</param>
        /// <param name="Str_TextOnImage">Texto a incluir</param>
        /// <param name="StringColor">Color del texto</param>
        /// <param name="fuente">tipo de fuente a utilizar</param>
        /// <param name="posicion">posicion dentro de la imagen</param>
        /// <param name="horizontal">alineacion horizontal</param>
        /// <param name="vertical">alineacion vertial</param>
        /// <returns>Devuelve el nombre de la nueva imagen</returns>
        public static string InsertarTexto(Image bitmap, string folder, string Str_TextOnImage, Color StringColor,
            Font fuente, Point posicion, StringAlignment horizontal, StringAlignment vertical)
        {
                //creando el objeto de la imagen
                archivos a = new archivos();
                string newFile = a.creaNombreFile(a.checkCarpeta(folder), "jpg", "txt");
                a = null;
                //Dibujo la imagen
                Graphics graphicsImage = Graphics.FromImage(bitmap);
                //Establezco la orientación mediante coordenadas   
                StringFormat stringformat = new StringFormat();
                stringformat.Alignment = horizontal;
                stringformat.LineAlignment = vertical;
                Point MiPOS = new Point();
                if (posicion.Y == 0 && posicion.X == 0)
                {
                    MiPOS.Y = bitmap.Height - 25;
                    MiPOS.X = 175;
                }
                else MiPOS = posicion;
                //modificar la imagen
                graphicsImage.DrawString(Str_TextOnImage, fuente, new SolidBrush(StringColor), MiPOS, stringformat);
                graphicsImage.Save();
                //guardar la nueva imagen
                bitmap.Save(newFile);
                return newFile;
          
        }

        /// <summary>
        /// Insertar texto dentro de una imagen
        /// </summary>
        /// <param name="file">Nombre imagen original</param>
        /// <param name="rutaImg">Carpeta donde se encuentra y guardara la nueva imagen</param>
        /// <param name="Str_TextOnImage">Texto a incluir</param>
        /// <param name="StringColor">Color del texto</param>
        /// <param name="fuente">tipo de fuente a utilizar</param>
        /// <param name="posicion">posicion dentro de la imagen</param>
        /// <param name="horizontal">alineacion horizontal</param>
        /// <param name="vertical">alineacion vertial</param>
        /// <returns>Devuelve el nombre de la nueva imagen</returns>
        public static Image InsertarTextoImagen(string file, string folder, string Str_TextOnImage, Color StringColor,
            Font fuente, Point posicion, StringAlignment horizontal, StringAlignment vertical)
        {
                //creando el objeto de la imagen
                archivos a = new archivos();
                string rutaImg = a.checkCarpeta(folder) + file;
                a = null;
                Image bitmap = (Image)Bitmap.FromFile(rutaImg);         // set image
                //Dibujo la imagen
                Graphics graphicsImage = Graphics.FromImage(bitmap);
                //Establezco la orientación mediante coordenadas   
                StringFormat stringformat = new StringFormat();
                stringformat.Alignment = horizontal;
                stringformat.LineAlignment = vertical;
                Point MiPOS = new Point();
                if (posicion.Y == 0 && posicion.X == 0)
                {
                    MiPOS.Y = bitmap.Height - 25;
                    MiPOS.X = 175;
                }
                else MiPOS = posicion;
                //modificar la imagen
                graphicsImage.DrawString(Str_TextOnImage, fuente, new SolidBrush(StringColor), MiPOS, stringformat);
                graphicsImage.Save();
                return bitmap;
        }

        /// <summary>
        /// Insertar texto dentro de una imagen
        /// </summary>
        /// <param name="bitmap">Imagen a modificar</param>
        /// <param name="Str_TextOnImage">Texto a incluir</param>
        /// <param name="StringColor">Color del texto</param>
        /// <param name="fuente">tipo de fuente a utilizar</param>
        /// <param name="posicion">posicion dentro de la imagen</param>
        /// <param name="horizontal">alineacion horizontal</param>
        /// <param name="vertical">alineacion vertial</param>
        /// <returns>Devuelve el nombre de la nueva imagen</returns>
        public static Image InsertarTextoImagen(Image bitmap, string Str_TextOnImage, Color StringColor,
            Font fuente, Point posicion, StringAlignment horizontal, StringAlignment vertical)
        {
                //Dibujo la imagen
                Graphics graphicsImage = Graphics.FromImage(bitmap);
                //Establezco la orientación mediante coordenadas   
                StringFormat stringformat = new StringFormat();
                stringformat.Alignment = horizontal;
                stringformat.LineAlignment = vertical;
                Point MiPOS = new Point();
                if (posicion.Y == 0 && posicion.X == 0)
                {
                    MiPOS.Y = bitmap.Height - 25;
                    MiPOS.X = 175;
                }
                else MiPOS = posicion;
                //modificar la imagen
                graphicsImage.DrawString(Str_TextOnImage, fuente, new SolidBrush(StringColor), MiPOS, stringformat);
                graphicsImage.Save();
                return bitmap;
        }
        #endregion
    }
}