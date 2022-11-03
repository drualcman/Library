using System.Text;

namespace drualcman
{
    public class archivos
    {
        /// <summary>
        /// Crear un nombre de archivo
        /// </summary>
        /// <param name="carpeta">Ruta completa de la carpeta de destino</param>
        /// <param name="extension">extension del archivo</param>
        /// <returns></returns>
        public string creaNombreFile(string carpeta, string extension)
        {
            string strRuta = checkCarpeta(carpeta);

            string nombreArchivo = "";

            //comprobar que el primer caracter es . si no agregarlo
            if(extension.Substring(0, 1) != ".") extension = "." + extension;

            do
            {
                nombreArchivo = strRuta + nombreFile() + extension;
            } while(existeFichero(nombreArchivo));

            strRuta = nombreArchivo;

            return strRuta;
        }

        /// <summary>
        /// Crear un nombre de archivo
        /// </summary>
        /// <param name="carpeta">Ruta completa de la carpeta de destino</param>
        /// <param name="extension">extension del archivo</param>
        /// <param name="pre">Prefijo para el nombre</param>
        /// <returns></returns>
        public string creaNombreFile(string carpeta, string extension, string pre)
        {
            string strRuta = checkCarpeta(carpeta);

            string nombreArchivo = "";

            //comprobar que el primer caracter es . si no agregarlo
            if(extension.Substring(0, 1) != ".") extension = "." + extension;

            do
            {
                nombreArchivo = strRuta + pre + nombreFile(2) + extension;
            } while(existeFichero(nombreArchivo));

            strRuta = nombreArchivo;

            return strRuta;
        }

        /// <summary>
        /// comprobar que el archivo no existe en la ruta seleccionada
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        public bool existeFichero(string nombreArchivo)
        {
            bool bExsiste = false;

            //comprobar que el archivo no existe en la ruta seleccionada
            try
            {
                bExsiste = File.Exists(nombreArchivo);
            }
            catch
            {
                bExsiste = false;
            }

            return bExsiste;
        }

        /// <summary>
        /// Save file in server. Return the name of file.
        /// </summary>
        /// <param name="NombreArchivo">Name of file</param>
        /// <param name="Archivo">File in string format</param>
        /// <param name="Carpeta">Folder to save</param>
        /// <param name="NombreDinamico">Generate dinamic name</param>
        /// <param name="pre">prefijo del archivo</param>
        /// <returns></returns>
        public string guardaDato(string NombreArchivo, string Archivo, string Carpeta = "", bool NombreDinamico = false, string pre = "")
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] file = encoding.GetBytes(Archivo);
            return guardaDato(NombreArchivo, file, Carpeta, NombreDinamico, pre);
        }

        /// <summary>
        /// Save file in server. Return the name of file.
        /// </summary>
        /// <param name="NombreArchivo">Name of file</param>
        /// <param name="Archivo">File in string format</param>
        /// <param name="HEX">Si el valor enviado esta escrito en hexagesimal</param>
        /// <param name="Carpeta">Folder to save</param>
        /// <param name="NombreDinamico">Generate dinamic name</param>
        /// <param name="pre">prefijo del archivo</param>
        /// <returns></returns>
        public string guardaDato(string NombreArchivo, string Archivo, bool HEX, string Carpeta = "", bool NombreDinamico = false, string pre = "")
        {
            byte[] file;
            if(HEX == true)
            {
                file = ConvertHexToBytes(Archivo);
            }
            else
            {
                UTF8Encoding encoding = new UTF8Encoding();
                file = encoding.GetBytes(Archivo);
            }

            return guardaDato(NombreArchivo, file, Carpeta, NombreDinamico, pre);
        }

        /// <summary>
        /// Save file in server
        /// </summary>
        /// <param name="NombreArchivo">Name of file</param>
        /// <param name="File">File</param>
        /// <param name="Carpeta">Folder to save with destination drive (C:\tmp, e:\tmp)</param>
        /// <param name="NombreDinamico">Generate dinamic name</param>
        /// <param name="pre">prefijo del archivo</param>
        /// <returns></returns>
        public string guardaDato(string NombreArchivo, Stream File, string Carpeta = "", bool NombreDinamico = false, string pre = "")
        {
            byte[] Archivo = ReadToEnd(File);
            return guardaDato(NombreArchivo, Archivo, Carpeta, NombreDinamico, pre);
        }

        /// <summary>
        /// Save file in server. Return the name of file.
        /// </summary>
        /// <param name="NombreArchivo">Name of file</param>
        /// <param name="Archivo">File</param>
        /// <param name="Carpeta">Folder to save with destination drive (C:\tmp, e:\tmp)</param>
        /// <param name="NombreDinamico">Generate dinamic name</param>
        /// <param name="pre">prefijo del archivo</param>
        /// <returns></returns>
        public string guardaDato(string NombreArchivo, byte[] Archivo, string Carpeta = "", bool NombreDinamico = false, string pre = "")
        {
            string strRuta;
            string guardar = "";

            Carpeta = checkCarpeta(Carpeta);

            //comprobar que el archivo no existe en la web
            if(NombreDinamico == true)
            {
                do
                {
                    //el nombre existe en el servidor, cambiar el nombre del archivo
                    //tantas veces como sea necesario para poder almacenar el archivo
                    guardar = pre + nombreFile() + Path.GetExtension(NombreArchivo);
                    strRuta = Path.Combine(Carpeta, guardar);
                } while(existeFichero(strRuta));
            }
            else
            {
                if(string.IsNullOrEmpty(pre)) guardar = NombreArchivo;
                else guardar = pre + NombreArchivo;
            }
            strRuta = Carpeta + guardar;

            try
            {
                using FileStream save = new FileStream(strRuta, FileMode.OpenOrCreate, FileAccess.Write);
                save.Write(Archivo, 0, Archivo.Length);
                save.Close();
                return guardar;
            }
            catch(Exception ex)
            {
                string info = "No se ha podido guardar: " + NombreArchivo +
                                " \r\n Ruta destino: " + strRuta +
                                " \r\n " + ex.Message + " \r\n " + ex.StackTrace;
                throw new ArgumentException(info);
            }
        }

        /// <summary>
        /// Borra el archivo enviado. Devuelve FALSE si no existe.
        /// </summary>
        /// <param name="nombreArchivo">Ruta completa del archivo a borrar</param>
        /// <returns></returns>
        public bool borrarArchivo(string nombreArchivo)
        {
            bool bResultado = true;
            if(existeFichero(nombreArchivo) == true)
                File.Delete(nombreArchivo);
            else
                bResultado = false;
            return bResultado;
        }

        /// <summary>
        /// Convertir desde Hexagesimal a bytes
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for(int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// Adjuntar archivo al un mail
        /// </summary>
        /// <param name="filePath">ruta del archivo</param>
        /// <returns></returns>
        public Stream GetStreamFile(string filePath)
        {
            using FileStream fileStream = File.OpenRead(filePath);

            using MemoryStream memStream = new MemoryStream();
            memStream.SetLength(fileStream.Length);
            fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
            
            return memStream;

        }

        public StreamReader GetStreamBytes(byte[] bytes)
        {
            try
            {
                return new StreamReader(new MemoryStream(bytes));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convertir un string en Stream
        /// </summary>
        /// <param name="str">string a convertir</param>
        /// <param name="enc">Encoding a utilizar, por defecto UTF8</param>
        /// <returns></returns>
        public Stream ToStream(string str, Encoding enc = null)
        {
            enc = enc ?? Encoding.UTF8;
            return new MemoryStream(enc.GetBytes(str ?? ""));
        }

        /// <summary>
        /// Convertir un bytes en Stream
        /// </summary>
        /// <param name="bytes">bytes a convertir</param>
        /// <returns></returns>
        public Stream ToStream(byte[] bytes)
        {
            using MemoryStream stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Leer un archivo de texto en bytes y devolver el texto como string
        /// </summary>
        /// <param name="bytes">archivo en bytes</param>
        /// <returns></returns>
        public string bytes2string(byte[] bytes)
        {
            try
            {
                using StreamReader sr = new StreamReader(GetStreamBytes(bytes).BaseStream);
                string texto;
                texto = sr.ReadToEnd();
                sr.Close();
                return texto;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Obtener la extension del archivo con el .
        /// </summary>
        /// <param name="file">Ruta completa del archivo</param>
        /// <returns></returns>
        public static string GetFileExtension(string file)
        {
            string retorno;
            try
            {
                retorno = Path.GetExtension(file);
            }
            catch
            {
                retorno = string.Empty;
            }
            return retorno;
        }

        /// <summary>
        /// Obtener la extension del archivo
        /// </summary>
        /// <param name="file">Ruta completa del archivo</param>
        /// <returns></returns>
        public string getFileExtension(string file)
        {
            return archivos.GetFileExtension(file);
        }


        /// <summary>
        /// Obtener solo el nombre del archivo y la extension
        /// </summary>
        /// <param name="file">Ruta completa del archivo</param>
        /// <param name="soloNombre">Indica si hay que devolver solo el nombre del archivo</param>
        /// <returns></returns>
        public static string GetFileName(string file, bool soloNombre)
        {
            string retorno;
            try
            {
                if(soloNombre) retorno = Path.GetFileNameWithoutExtension(file);
                else retorno = Path.GetFileName(file);
            }
            catch
            {
                retorno = string.Empty;
            }
            return retorno;
        }

        /// <summary>
        /// Obtener solo el nombre del archivo y la extension
        /// </summary>
        /// <param name="file">Ruta completa del archivo</param>
        /// <param name="soloNombre">Indica si hay que devolver solo el nombre del archivo</param>
        /// <returns></returns>
        public string getFileName(string file, bool soloNombre)
        {
            return archivos.GetFileName(file, soloNombre);
        }

        /// <summary>
        /// Devuelde un nombre con la \ al final (carpeta\)
        /// </summary>
        /// <param name="carpeta">nombre a comprobar</param>
        /// <returns></returns>
        public string checkCarpeta(string carpeta)
        {
            if(!string.IsNullOrEmpty(carpeta))
            {
                carpeta = carpeta.Trim();        //delete spaces before and end
                                                 //comprobar que la carpeta tiene la \ al final
                if(carpeta.Substring(carpeta.Length - 1, 1) != "\\") carpeta = carpeta + "\\";
                //comprobar que el primer caracter no es \
                if(carpeta.Substring(0, 1) == "\\") carpeta = carpeta.Remove(0, 1);
                return carpeta;
            }
            else return carpeta;
        }

        /// <summary>
        /// Genera un nombre de archivo unico
        /// </summary>
        /// <returns>Devuelve el nombre del archivo generado</returns>
        public string nombreFile()
        {
            string randomNum = String.Empty;
            Random autoRand = new Random();

            byte h;

            for(h = 1; h <= 5; h++)
            {
                int i_letra = Convert.ToInt32(autoRand.Next(65, 90));
                string letra = Convert.ToString(i_letra);
                randomNum += letra;
                for(int x = 0; x <= 2; x++)
                {
                    randomNum += Convert.ToInt32(autoRand.Next(0, 9)).ToString();
                }
                for(int x = 0; x <= 2; x++)
                {
                    i_letra = Convert.ToInt32(autoRand.Next(65, 90));
                    letra = Convert.ToString(i_letra);
                    randomNum += letra;
                }
                letra = Convert.ToString(Convert.ToInt32(autoRand.Next(65, 90)));
                randomNum += letra;
            }

            return randomNum;
        }

        /// <summary>
        /// Genera un nombre de archivo unico
        /// </summary>
        /// <param name="r">Numero de rondas para crear el nombre, determinara el largo del nombre del archivo cada ronda son 8 caracteres</param>
        /// <returns>Devuelve el nombre del archivo generado</returns>
        public string nombreFile(byte r)
        {
            string randomNum = String.Empty;
            Random autoRand = new Random();

            if(r == 0) r = 1;

            byte h;

            for(h = 1; h <= r; h++)
            {
                int i_letra = Convert.ToInt32(autoRand.Next(65, 90));
                string letra = Convert.ToString(i_letra);
                randomNum += letra;
                for(int x = 0; x <= 2; x++)
                {
                    randomNum += Convert.ToInt32(autoRand.Next(0, 9)).ToString();
                }
                for(int x = 0; x <= 2; x++)
                {
                    i_letra = Convert.ToInt32(autoRand.Next(65, 90));
                    letra = Convert.ToString(i_letra);
                    randomNum += letra;
                }
                letra = Convert.ToString(Convert.ToInt32(autoRand.Next(65, 90)));
                randomNum += letra;
            }

            return randomNum;
        }

        /// <summary>
        /// Convertir una variable Stream en una variable byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if(stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if(totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if(nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if(readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if(stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public Stream FileToStream(string filename, string folder)
        {
            folder = checkCarpeta(folder);
            string path = folder + filename;
            return FileToStream(path);
        }

        public Stream FileToStream(string path)
        {
            Stream data = ToStream(FileToBytes(path));
            return data;
        }

        public byte[] FileToBytes(string path)
        {
            try
            {
                byte[] bytes;
                using(FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    // Read the source file into a byte array.
                    bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while(numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached.
                        if(n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = bytes.Length;
                }
                return bytes;
            }
            catch(FileLoadException ex)
            {
                string info = "We can't read the file: " + path +
                                " \r\n " + ex.Message + " \r\n " + ex.StackTrace;
                throw new ArgumentException(info);
            }
            catch(FileNotFoundException ex)
            {
                string info = "We can't read the file: " + path +
                                " \r\n " + ex.Message + " \r\n " + ex.StackTrace;
                throw new ArgumentException(info);
            }
            catch(Exception ex)
            {
                string info = "We can't read the file: " + path +
                                " \r\n " + ex.Message + " \r\n " + ex.StackTrace;
                throw new ArgumentException(info);
            }
        }

        /// <summary>
        /// Copy files. default overwrite if exists
        /// </summary>
        /// <param name="originPath"></param>
        /// <param name="destinationPath"></param>
        /// <param name="overwrite"></param>
        public void CopyFile(string originPath, string destinationPath, bool overwrite = true)
        {
            if(existeFichero(originPath) && existeFichero(destinationPath))
                File.Copy(originPath, destinationPath, overwrite);
            else
            {
                throw new Exception("Some file are missing, can't be copied.");
            }
        }

    }

}
