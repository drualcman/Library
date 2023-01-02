using System;
using System.IO;
using System.Net.Http;

/// <summary>
/// Summary description for drAspFicheros
/// </summary>
namespace drualcman
{
    /// <summary>
    /// Name Space for ASP.NET c#
    /// </summary>
    namespace basicASP
    {
        #region SEO Tools
        public static class SEO
        {
            public enum FileTipe
            {
                css,
                javascript
            }

            /// <summary>
            /// Recoge un archivo css y lo devuelve sin lineas en blanco
            /// </summary>
            /// <param name="fileName">Nombre del archivo</param>
            /// <param name="folder">Carpeta contenedora en el servidor desde la raiz</param>
            /// <param name="min">Nombre del archivo a minificar sin extension</param>
            /// <returns></returns>
            public static string InlineFile(FileTipe tipo, string fileName, string folder, string min)
            {
                string retorno = string.Empty;

                switch(tipo)
                {
                    case FileTipe.css:
                        retorno = "<style type=\"text/css\">";
                        break;
                    case FileTipe.javascript:
                        retorno = "<script type=\"text/javascript\">";
                        break;
                }

                try
                {
                    ficheros f = new ficheros();
                    string file = f.RutaCompleta(folder) + fileName;
                    if(f.existeFichero(fileName, folder) == true)
                    {
                        StreamReader read = new StreamReader(file);
                        string content = read.ReadToEnd();
                        if(tipo == FileTipe.css)
                        {
                            content = content.Replace(Environment.NewLine, " ");
                            content = content.Replace((char)10, ' ');
                            content = content.Replace((char)11, ' ');
                            content = content.Replace((char)12, ' ');
                            content = content.Replace((char)13, ' ');
                            content = content.Replace("\t", "");
                            content = content.Replace("  ", "");
                            content = content.Replace("; ", ";");
                            content = content.Replace(": ", ":");
                            content = content.Replace(" {", "{");
                            content = content.Replace("{ ", "{");
                            content = content.Replace(" }", "}");
                            content = content.Replace("} ", "}");
                        }
                        retorno += content;

                        read.Close();
                    }
                    f = null;
                }
                catch
                {
                    retorno += "";
                }

                switch(tipo)
                {
                    case FileTipe.css:
                        retorno += "</style>";
                        break;
                    case FileTipe.javascript:
                        retorno += "</script>";
                        break;
                }

                return retorno;
            }

            /// <summary>
            /// Recoge un archivo css y lo devuelve sin lineas en blanco
            /// </summary>
            /// <param name="fileName">Nombres de los archivos</param>
            /// <param name="folder">Carpeta contenedora en el servidor desde la raiz</param>
            /// <param name="min">Nombre del archivo a minificar sin extension</param>
            /// <returns></returns>
            public static string InlineFile(FileTipe tipo, string[] fileName, string folder, string min)
            {
                string retorno = string.Empty;

                switch(tipo)
                {
                    case FileTipe.css:
                        retorno = "<style type=\"text/css\">";
                        break;
                    case FileTipe.javascript:
                        retorno = "<script type=\"text/javascript\">";
                        break;
                }

                try
                {
                    ficheros f = new ficheros();
                    string dir = f.RutaCompleta(folder);

                    foreach(string item in fileName)
                    {
                        string file = f.RutaCompleta(folder) + item;
                        if(f.existeFichero(item, folder) == true)
                        {
                            StreamReader read = new StreamReader(file);
                            string content = read.ReadToEnd();
                            if(tipo == FileTipe.css)
                            {
                                content = content.Replace(Environment.NewLine, " ");
                                content = content.Replace((char)10, ' ');
                                content = content.Replace((char)11, ' ');
                                content = content.Replace((char)12, ' ');
                                content = content.Replace((char)13, ' ');
                                content = content.Replace("\t", "");
                                content = content.Replace("  ", "");
                                content = content.Replace("; ", ";");
                                content = content.Replace(": ", ":");
                                content = content.Replace(" {", "{");
                                content = content.Replace("{ ", "{");
                                content = content.Replace(" }", "}");
                                content = content.Replace("} ", "}");
                            }
                            retorno += content;

                            read.Close();
                        }
                    }
                    f = null;
                }
                catch
                {
                    retorno += "";
                }

                switch(tipo)
                {
                    case FileTipe.css:
                        retorno += "</style>";
                        break;
                    case FileTipe.javascript:
                        retorno += "</script>";
                        break;
                }

                return retorno;
            }

            /// <summary>
            /// Recoge un archivo css y lo devuelve sin lineas en blanco
            /// </summary>
            /// <param name="fileName">Nombres de los archivos</param>
            /// <param name="folder">Carpetas contenedoras en el servidor desde la raiz en el mismo orden que los archivos</param>
            /// <param name="min">Nombre del archivo a minificar sin extension</param>
            /// <returns></returns>
            public static string InlineFile(FileTipe tipo, string[] fileName, string[] folder, string min, bool inline = false)
            {
                string retorno = string.Empty;

                string fichero = min;

                ficheros f = new ficheros();
                switch(tipo)
                {
                    case FileTipe.css:
                        fichero += ".css";
                        retorno = "<style>";
                        break;
                    case FileTipe.javascript:
                        fichero += ".js";
                        retorno = "<script>";
                        break;
                }

                string ok = string.Empty;
                try
                {
                    using HttpClient client = new HttpClient();
                    for(int i = 0; i < fileName.Length; i++)
                    {
                        ok += "/* " + folder[i] + "/" + fileName[i];
                        if(string.IsNullOrEmpty(folder[i]))
                        {
                            ok += " (fichero remoto) */";
                            string content = client.GetStringAsync(fileName[i]).Result;
                            if(tipo == FileTipe.css)
                            {
                                content = content.Replace(Environment.NewLine, " ");
                                content = content.Replace((char)10, ' ');
                                content = content.Replace((char)11, ' ');
                                content = content.Replace((char)12, ' ');
                                content = content.Replace((char)13, ' ');
                                content = content.Replace("\t", "");
                                content = content.Replace("  ", " ");
                                content = content.Replace("; ", ";");
                                content = content.Replace(": ", ":");
                                content = content.Replace(" {", "{");
                                content = content.Replace("{ ", "{");
                                content = content.Replace(" }", "}");
                                content = content.Replace("} ", "}");
                            }
                            ok += content;
                        }
                        else
                        {
                            if(f.existeFichero(fileName[i], folder[i]))
                            {
                                ok += " exist */";
                                string file = f.RutaCompleta(folder[i]) + fileName[i];
                                StreamReader read = new StreamReader(file);
                                string content = read.ReadToEnd();
                                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(@"(?=\/\*)(.*)(\*\/)");
                                content = rgx.Replace(content, "");
                                if(tipo == FileTipe.css)
                                {
                                    content = content.Replace(Environment.NewLine, " ");
                                    content = content.Replace((char)10, ' ');
                                    content = content.Replace((char)11, ' ');
                                    content = content.Replace((char)12, ' ');
                                    content = content.Replace((char)13, ' ');
                                    content = content.Replace("\t", "");
                                    content = content.Replace("  ", "");
                                    content = content.Replace("; ", ";");
                                    content = content.Replace(": ", ":");
                                    content = content.Replace(" {", "{");
                                    content = content.Replace("{ ", "{");
                                    content = content.Replace(" }", "}");
                                    content = content.Replace("} ", "}");
                                }
                                ok += content;

                                //retorno += content;
                                read.Close();
                            }
                            else
                                ok += " not exist */";
                        }
                    }
                }
                catch(Exception ex)
                {
                    retorno = "/* exception " + ex.ToString() + "*/";
                }

                if(inline == true)
                {
                    switch(tipo)
                    {
                        case FileTipe.css:
                            retorno += ok + "</style>";
                            break;
                        case FileTipe.javascript:
                            retorno += ok + "</script>";
                            break;
                    }
                }
                else
                {
                    f.guardaDato(fichero, ok, "dat");
                    switch(tipo)
                    {
                        case FileTipe.css:
                            retorno = "<link rel=\"stylesheet\" href=\"" + string.Format("/dat/" + fichero + "?v={0}", CacheExpire(0)) + "\" type=\"text/css\" />";
                            break;
                        case FileTipe.javascript:
                            retorno = "<script src=\"" + string.Format("/dat/" + fichero + "?v={0}", CacheExpire(0)) + "\"></script>";
                            break;
                    }
                }

                f = null;

                return retorno;
            }

            /// <summary>
            /// Indicar la caducidad del contenido CSS o JS
            /// </summary>
            /// <param name="days">Numero de dias</param>
            /// <returns></returns>
            public static string CacheExpire(int days = 0)
            {
                string retorno = string.Empty;

                System.DateTime hoy = System.DateTime.Today;

                if(days > 0) hoy = hoy.AddDays(days);

                retorno = hoy.ToString("d").Replace("/", "");

                return retorno;
            }
        }
        #endregion

        /// <summary>
        /// Utilidades para manejo de archivos (ficheros)
        /// </summary>
        public class ficheros
        {
            /// <summary>
            /// Crear un nombre de archivo
            /// </summary>
            /// <param name="carpeta"></param>
            /// <param name="extension">extension del archivo</param>
            /// <returns></returns>
            public string creaNombreFile(string carpeta, string extension)
            {
                archivos a = new archivos();

                carpeta = ApplicationPath + carpeta;
                string strRuta = a.creaNombreFile(carpeta, extension);
                a = null;

                return strRuta;
            }

            /// <summary>
            /// Crear un nombre de archivo
            /// </summary>
            /// <param name="removeFolder">Folder to remove inclusive the last "\"</param>
            /// <param name="carpeta"></param>
            /// <param name="extension">extension del archivo</param>
            /// <returns></returns>
            public string creaNombreFile(string removeFolder, string carpeta, string extension)
            {
                archivos a = new archivos();

                carpeta = ApplicationPath.Replace(removeFolder, "") + carpeta;
                string strRuta = a.creaNombreFile(carpeta, extension);
                a = null;

                return strRuta;
            }

            public string ApplicationPath
            {
                get
                {
                    string dir;
                    if(String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
                    {
                        dir = AppDomain.CurrentDomain.BaseDirectory; //exe folder for WinForms, Consoles, Windows Services
                    }
                    else
                    {
                        dir = AppDomain.CurrentDomain.RelativeSearchPath; //bin folder for Web Apps 
                    }
                    return dir.Replace("bin\\", "")
                        .Replace("Debug\\", "")
                        .Replace("Release\\", "")
                        .Replace("netcoreapp2.1\\", "")
                        .Replace("netcoreapp2.2\\", "")
                        .Replace("netcoreapp3.0\\", "")
                        .Replace("netcoreapp3.1\\", "")
                        .Replace("net5.0\\", "")
                        .Replace("net6.0\\", "")
                        .Replace("net7.0\\", "")
                        .Replace("net8.0\\", "")
                        .Replace("net9.0\\", "")
                        .Replace("x64\\", "")
                        .Replace("x86\\", "");
                }
            }

            /// <summary>
            /// comprobar que el archivo no existe en la ruta seleccionada
            /// </summary>
            /// <param name="nombreArchivo"></param>
            /// <param name="carpeta"></param>
            /// <returns></returns>
            public bool existeFichero(string nombreArchivo, string carpeta)
            {
                bool bExsiste = false;

                archivos a = new archivos();

                string strRuta = ApplicationPath + a.checkCarpeta(carpeta) + nombreArchivo;

                bExsiste = a.existeFichero(strRuta);
                a = null;
                return bExsiste;
            }

            /// <summary>
            /// comprobar que el archivo no existe en la ruta seleccionada
            /// </summary>
            /// <param name="removeFolder">Folder to remove inclusive the last "\"</param>
            /// <param name="nombreArchivo"></param>
            /// <param name="carpeta"></param>
            /// <returns></returns>
            public bool existeFichero(string removeFolder, string nombreArchivo, string carpeta)
            {
                bool bExsiste = false;

                archivos a = new archivos();

                removeFolder = a.checkCarpeta(removeFolder);
                string strRuta = ApplicationPath +
                                    a.checkCarpeta(carpeta).Replace(removeFolder, "") + nombreArchivo;

                bExsiste = a.existeFichero(strRuta);
                a = null;
                return bExsiste;
            }

            /// <summary>
            /// Borrar el archivo en la ruta seleccionada
            /// </summary>
            /// <param name="removeFolder">Folder to remove inclusive the last "\"</param>
            /// <param name="nombreArchivo"></param>
            /// <param name="carpeta"></param>
            /// <returns></returns>
            public bool borrarArchivo(string removeFolder, string nombreArchivo, string carpeta)
            {
                archivos a = new archivos();
                removeFolder = a.checkCarpeta(removeFolder);
                string strRuta = ApplicationPath +
                                    a.checkCarpeta(carpeta).Replace(removeFolder, "");
                a = null;
                strRuta += nombreArchivo;

                return borrarArchivo(strRuta);
            }

            /// <summary>
            /// Borrar el archivo en la ruta seleccionada
            /// </summary>
            /// <param name="nombreArchivo"></param>
            /// <param name="carpeta"></param>
            /// <returns></returns>
            public bool borrarArchivo(string nombreArchivo, string carpeta)
            {
                archivos a = new archivos();
                string strRuta = ApplicationPath +
                                    a.checkCarpeta(carpeta);
                a = null;
                strRuta += nombreArchivo;

                return borrarArchivo(strRuta);
            }

            /// <summary>
            /// Borrar el archivo en la ruta seleccionada
            /// </summary>
            /// <param name="strRuta"></param>
            /// <returns></returns>
            public bool borrarArchivo(string strRuta)
            {
                bool bExsiste = false;

                //comprobar que el archivo no existe en la ruta seleccionada
                try
                {
                    File.Delete(strRuta);
                    bExsiste = true;
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
            /// <returns></returns>
            public string guardaDato(string NombreArchivo, string Archivo, string Carpeta = "", bool NombreDinamico = false, string pre = "")
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] file = encoding.GetBytes(Archivo);
                return guardaDato(NombreArchivo, file, Carpeta, NombreDinamico, pre);
            }

            /// <summary>
            /// Save file in server. Return the name of file.
            /// </summary>
            /// <param name="NombreArchivo">Name of file</param>
            /// <param name="Archivo">File</param>
            /// <param name="Carpeta">Folder to save</param>
            /// <param name="NombreDinamico">Generate dinamic name</param>
            /// <param name="HEX">Indica si el archivo esta en formato Hexadecimal</param>
            /// <returns></returns>
            public string guardaDato(string NombreArchivo, byte[] Archivo, string Carpeta = "", bool NombreDinamico = false, string pre = "")
            {
                //string strRuta;
                string guardar = "";
                Carpeta = ApplicationPath + Carpeta.Replace("adminweb\\", "");
                archivos a = new archivos();
                guardar = a.guardaDato(NombreArchivo, Archivo, Carpeta, NombreDinamico, pre);
                a = null;
                return guardar;
            }

            /// <summary>
            /// Save file in server. Return the name of file.
            /// </summary>
            /// <param name="removeFolder">Folder to remove inclusive the last "\"</param>
            /// <param name="NombreArchivo">Name of file</param>
            /// <param name="Archivo">File</param>
            /// <param name="Carpeta">Folder to save</param>
            /// <param name="NombreDinamico">Generate dinamic name</param>
            /// <param name="HEX">Indica si el archivo esta en formato Hexadecimal</param>
            /// <returns></returns>
            public string guardaDato(string removeFolder, string NombreArchivo, byte[] Archivo, string Carpeta = "", bool NombreDinamico = false, string pre = "")
            {
                //string strRuta;
                string guardar = "";
                archivos a = new archivos();
                removeFolder = a.checkCarpeta(removeFolder);
                Carpeta = ApplicationPath + Carpeta.Replace(removeFolder, "");
                guardar = a.guardaDato(NombreArchivo, Archivo, Carpeta, NombreDinamico, pre);
                a = null;
                return guardar;
            }

            /// <summary>
            /// Save file in server. Return the name of file.
            /// </summary>
            /// <param name="NombreArchivo">Name of file</param>
            /// <param name="Archivo">File</param>
            /// <param name="Carpeta">Folder to save</param>
            /// <param name="NombreDinamico">Generate dinamic name</param>
            /// <param name="HEX">Indica si el archivo esta en formato Hexadecimal</param>
            /// <returns></returns>
            public string guardaDato(string NombreArchivo, Stream Archivo, string Carpeta = "", bool NombreDinamico = false, string pre = "")
            {
                //string strRuta;
                string guardar = "";
                Carpeta = ApplicationPath + Carpeta;
                archivos a = new archivos();
                guardar = a.guardaDato(NombreArchivo, Archivo, Carpeta, NombreDinamico, pre);
                a = null;
                return guardar;
            }

            /// <summary>
            /// Save file in server. Return the name of file.
            /// </summary>
            /// <param name="removeFolder">Folder to remove inclusive the last "\"</param>
            /// <param name="NombreArchivo">Name of file</param>
            /// <param name="Archivo">File</param>
            /// <param name="Carpeta">Folder to save</param>
            /// <param name="NombreDinamico">Generate dinamic name</param>
            /// <param name="HEX">Indica si el archivo esta en formato Hexadecimal</param>
            /// <returns></returns>
            public string guardaDato(string removeFolder, string NombreArchivo, Stream Archivo, string Carpeta = "", bool NombreDinamico = false, string pre = "")
            {
                //string strRuta;
                string guardar = "";
                archivos a = new archivos();
                removeFolder = a.checkCarpeta(removeFolder);
                Carpeta = ApplicationPath + Carpeta.Replace(removeFolder, "");
                guardar = a.guardaDato(NombreArchivo, Archivo, Carpeta, NombreDinamico, pre);
                a = null;
                return guardar;
            }

            /// <summary>
            /// Save file in server. Return the name of file.
            /// </summary>
            /// <param name="NombreArchivo">Name of file</param>
            /// <param name="Archivo">File</param>
            /// <param name="HEX">Indica si el archivo esta en formato Hexadecimal</param>
            /// <param name="Carpeta">Folder to save</param>
            /// <param name="NombreDinamico">Generate dinamic name</param>            
            /// <returns></returns>
            public string guardaDato(string NombreArchivo, string Archivo, bool HEX, string Carpeta = "", bool NombreDinamico = false, string pre = "")
            {
                //string strRuta;
                string guardar = "";
                Carpeta = ApplicationPath + Carpeta;
                archivos a = new archivos();
                guardar = a.guardaDato(NombreArchivo, Archivo, HEX, Carpeta, NombreDinamico, pre);
                a = null;
                return guardar;
            }

            /// <summary>
            /// Save file in server. Return the name of file.
            /// </summary>
            /// <param name="removeFolder">Folder to remove inclusive the last "\"</param>
            /// <param name="NombreArchivo">Name of file</param>
            /// <param name="Archivo">File</param>
            /// <param name="HEX">Indica si el archivo esta en formato Hexadecimal</param>
            /// <param name="Carpeta">Folder to save</param>
            /// <param name="NombreDinamico">Generate dinamic name</param>            
            /// <returns></returns>
            public string guardaDato(string removeFolder, string NombreArchivo, string Archivo, bool HEX, string Carpeta, bool NombreDinamico = false, string pre = "")
            {
                //string strRuta;
                string guardar = "";
                archivos a = new archivos();
                removeFolder = a.checkCarpeta(removeFolder);
                Carpeta = ApplicationPath + Carpeta.Replace(removeFolder, "");
                guardar = a.guardaDato(NombreArchivo, Archivo, HEX, Carpeta, NombreDinamico, pre);
                a = null;
                return guardar;
            }

            /// <summary>
            /// Recoger la ruta donde se almacena la aplicacion
            /// </summary>
            /// <returns></returns>
            public string RutaCompleta()
            {
                return ApplicationPath;
            }

            /// <summary>
            /// Recoger la ruta donde se almacena la aplicacion
            /// </summary>
            /// <param name="Carpeta">Carpeta deseada dentro del servidor</param>
            /// <returns></returns>
            public string RutaCompleta(string Carpeta)
            {
                archivos a = new archivos();
                Carpeta = a.checkCarpeta(Carpeta);
                a = null;
                return ApplicationPath + Carpeta;
            }

            /// <summary>
            /// Recoger la ruta donde se almacena la aplicacion
            /// </summary>
            /// <param name="removeFolder">Folder to remove inclusive the last "\"</param>
            /// <param name="Carpeta">Carpeta deseada dentro del servidor</param>
            /// <returns></returns>
            public string RutaCompleta(string removeFolder, string Carpeta)
            {
                archivos a = new archivos();
                Carpeta = a.checkCarpeta(Carpeta);
                removeFolder = a.checkCarpeta(removeFolder);
                a = null;
                return ApplicationPath + Carpeta.Replace(removeFolder, "");
            }
        }
    }
}