using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

/// <summary>
/// Name Space with utils for develop in .NET c#
/// </summary>
namespace drualcman
{
    /// <summary>
    /// Name Space for ASP.NET c#
    /// </summary>
    namespace basicASP
    {
        /// <summary>
        /// Esencial codes for HTML
        /// </summary>
        public static class basicHTML
        {
            /// <summary>
            /// Eliminar las etiquetas HTML
            /// </summary>
            /// <param name="strTexto">texto a limpiar</param>
            /// <returns></returns>
            public static string LimpiarHTML(string strTexto)
            {
                string result = strTexto;
                Regex rgx = new Regex("<((.|\n)*?)>");
                result = rgx.Replace(strTexto, "");
                return result;
            }

            /// <summary>
            /// Eliminar las etiquetas HTML
            /// </summary>
            /// <param name="strTexto">texto a limpiar</param>
            /// <param name="longitud">longitud maxima a devolver</param>
            /// <returns></returns>
            public static string LimpiarHTML(string strTexto, int longitud)
            {
                string result = strTexto;
                Regex rgx = new Regex("<((.|\n)*?)>");
                result = rgx.Replace(strTexto, "");
                if (result.Length >= longitud) result = result.Substring(0, longitud);
                return result;
            }

            /// <summary>
            /// Crear un efecto de columnas en el texto de un parrafo
            /// </summary>
            /// <param name="Cols">Numero de columnas</param>
            /// <returns></returns>
            public static string numeroColumnas(int Cols)
            {
                string retorno = @" -webkit-column-count: " + Cols.ToString("N0") + @";
                                    -moz-column-count: " + Cols.ToString("N0") + @";
                                    column-count: " + Cols.ToString("N0") + @";    
                                    -webkit-column-gap: 20px;
                                    -moz-column-gap: 20px;
                                    column-gap: 20px;
                                    -webkit-column-rule: 5px solid rgb(75, 180, 149); 
                                    -moz-column-rule: 5px solid rgb(75, 180, 149); 
                                    column-rule: 1px solid rgb(222, 222, 222);";

                return retorno;
            }

            /// <summary>
            /// Convert HEX Color or name color String in RGB Color
            /// </summary>
            /// <param name="color"></param>
            /// <returns></returns>
            public static Color color(string color)
            {
                Color resultado = new Color();

                try
                {
                    resultado = Color.FromName(color);
                }
                catch
                {
                    try
                    {
                        if (color.IndexOf("#") > 0)
                            color = color.Substring(1, color.Length);

                        int R = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                        int G = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                        int B = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                        resultado = Color.FromArgb(R, G, B);
                    }
                    catch (Exception)
                    {
                        resultado = Color.Transparent;
                    }
                }

                return resultado;
            }

            /// <summary>
            /// Crear el html para insertar un enlace HTML con los datos proporcionados
            /// </summary>
            /// <param name="url">Destino de la pagina</param>
            /// <param name="text">Texto a mostrar</param>
            /// <returns></returns>
            public static string a(string url, string text)
            {
                return a(url, text, "");
            }

            /// <summary>
            /// Crear el html para insertar un enlace HTML con los datos proporcionados
            /// </summary>
            /// <param name="url">Destino de la pagina</param>
            /// <param name="text">Texto a mostrar</param>
            /// <param name="cssClass">clase a utilizar</param>
            /// <returns></returns>
            public static string a(string url, string text, string cssClass)
            {
                return a(url, text, "", cssClass);
            }

            /// <summary>
            /// Crear el html para insertar un enlace HTML con los datos proporcionados
            /// </summary>
            /// <param name="url">Destino de la pagina</param>
            /// <param name="text">Texto a mostrar</param>
            /// <param name="target">_self, _blank, _parent, _top</param>
            /// <param name="cssClass">clase a utilizar</param>
            /// <returns></returns>
            public static string a(string url, string text, string target, string cssClass)
            {
                return a(url, text, target, cssClass, "");
            }

            /// <summary>
            /// Crear el html para insertar un enlace HTML con los datos proporcionados
            /// </summary>
            /// <param name="url">Destino de la pagina</param>
            /// <param name="text">Texto a mostrar</param>
            /// <param name="target">_self, _blank, _parent, _top</param>
            /// <param name="cssClass">clase a utilizar</param>
            /// <param name="id">id a utilizar</param>
            /// <returns></returns>
            public static string a(string url, string text, string target, string cssClass, string id)
            {
                return a(url, text, target, cssClass, id, "");
            }

            /// <summary>
            /// Crear el html para insertar un enlace HTML con los datos proporcionados
            /// </summary>
            /// <param name="url">Destino de la pagina</param>
            /// <param name="text">Texto a mostrar</param>
            /// <param name="target">_self, _blank, _parent, _top</param>
            /// <param name="cssClass">clase a utilizar</param>
            /// <param name="id">id a utilizar</param>
            /// <param name="onclick">script a llamar</param>
            /// <returns></returns>
            public static string a(string url, string text, string target, string cssClass, string id, string onclick)
            {
                return a(url, text, target, cssClass, id, onclick, "");
            }

            /// <summary>
            /// Crear el html para insertar un enlace HTML con los datos proporcionados
            /// </summary>
            /// <param name="url">Destino de la pagina</param>
            /// <param name="text">Texto a mostrar</param>
            /// <param name="target">_self, _blank, _parent, _top</param>
            /// <param name="cssClass">clase a utilizar</param>
            /// <param name="id">id a utilizar</param>
            /// <param name="onclick">script a llamar</param>
            /// <param name="title">titulo del enlace</param>
            /// <returns></returns>
            public static string a(string url, string text, string target, string cssClass, string id, string onclick, string title)
            {
                if (string.IsNullOrEmpty(target) || string.IsNullOrWhiteSpace(target)) target = "_self";
                string link = "<a href =\"" + url + "\" target=\"" + target + "\" ";
                if (!string.IsNullOrEmpty(cssClass) || !string.IsNullOrWhiteSpace(cssClass)) link += " class=\"" + cssClass + "\" ";
                if (!string.IsNullOrEmpty(id) || !string.IsNullOrWhiteSpace(id)) link += " id=\"" + id + "\" ";
                if (!string.IsNullOrEmpty(title) || !string.IsNullOrWhiteSpace(title))
                {
                    link += " title=\"" + title + "\" data-toggle=\"tooltip\"  data-placement=\"top\"";
                }
                if (!string.IsNullOrEmpty(onclick) || !string.IsNullOrWhiteSpace(onclick)) link += " onclick=\"" + onclick + "\" ";
                link += ">" + text + "</a>";
                return link;
            }

            /// <summary>
            /// Crear el html para insertar un enlace HTML con los datos proporcionados
            /// </summary>
            /// <param name="text">Texto a mostrar</param>
            /// <param name="onclick">script a llamar</param>
            /// <param name="id">id a utilizar</param>
            /// <param name="cssClass">clase a utilizar</param>
            /// <returns></returns>
            public static string aClick(string text, string onclick, string id = "", string cssClass = "")
            {
                return aClick(text, onclick, "", id, cssClass);
            }

            /// <summary>
            /// Crear el html para insertar un enlace HTML con los datos proporcionados
            /// </summary>
            /// <param name="text">Texto a mostrar</param>
            /// <param name="onclick">script a llamar</param>
            /// <param name="title">titulo del enlace</param>
            /// <param name="id">id a utilizar</param>
            /// <param name="cssClass">clase a utilizar</param>
            /// <returns></returns>
            public static string aClick(string text, string onclick, string title, string id = "", string cssClass = "")
            {
                return a("javascript:void(0)", text, "", cssClass, id, onclick, title);
            }

            /// <summary>
            /// Crear el html para insertar una imagen
            /// </summary>
            /// <param name="descrip">Name o descrition of file</param>
            /// <param name="url">url for get the file</param>
            /// <param name="cssClass">CSS class for the format</param>
            /// <returns></returns>
            public static string img(string descrip, string url, string cssClass = "")
            {
                if (string.IsNullOrEmpty(descrip)) descrip = url;
                if (!string.IsNullOrEmpty(cssClass) || string.IsNullOrWhiteSpace(cssClass))
                    cssClass = "class=\"" + cssClass + "\"";
                return "<img " + cssClass + " alt=\"" + descrip + "\"" + "src=\"" + url +
                            "\" title =\"" + descrip + "\" />";
            }

            /// <summary>
            /// Crear el html para insertar una imagen
            /// </summary>
            /// <param name="descrip">Name o descrition of file</param>
            /// <param name="url">url for get the file</param>
            /// <param name="maxHeight">Alto maximo</param>
            /// <param name="maxWidth">Ancho de la imagen</param>
            /// <returns></returns>
            public static string img(string descrip, string url, string cssClass = "", string Align = "NotSet", int maxHeight = 0, int maxWidth = 0)
            {
                if (string.IsNullOrEmpty(descrip)) descrip = url;
                string tam = "style=\"";
                if (maxWidth > 0) tam += "max-width:" + maxWidth.ToString() + "px;";
                if (maxHeight > 0) tam += "max-height:" + maxHeight.ToString() + "px; ";
                tam += "\"";

                if (!string.IsNullOrEmpty(cssClass) || string.IsNullOrWhiteSpace(cssClass))
                    cssClass = "class=\"" + cssClass + "\"";

                return "<img  " + cssClass + " alt=\"" + descrip + "\"" + " src=\"" + url +
                            "\"  title =\"" + descrip + "\" " + tam + " />";
            }

            /// <summary>
            /// Crear el html para insertar una imagen
            /// </summary>
            /// <param name="descrip">Name o descrition of file</param>
            /// <param name="url">url for get the file</param>
            /// <param name="maxHeight">Alto de la imagen</param>
            /// <param name="maxWidth">Ancho de la imagen</param>
            /// <returns></returns>
            public static string img(string descrip, string url, int maxHeight = 0, int maxWidth = 0)
            {
                if (string.IsNullOrEmpty(descrip)) descrip = url;
                string tam = "style=\"";
                if (maxWidth > 0) tam += "width:" + maxWidth.ToString() + "px;";
                if (maxHeight > 0) tam += "height:" + maxHeight.ToString() + "px; ";
                tam += "\"";

                return "<img  alt=\"" + descrip + "\"" + "src=\"" + url +
                            "\"  title =\"" + descrip + "\" " + tam + "/>";
            }

            /// <summary>
            /// Crear el HTML para insertar un iframe
            /// </summary>
            /// <param name="src">Ruta url de la pagina a cargar</param>
            /// <param name="w">ancho %</param>
            /// <param name="h">alto %</param>
            /// <returns></returns>
            public static string iframe(string src, int w = 100, int h = 100)
            {

                return "<iframe style=\"border: none;width:" + w.ToString() + "%; height:" + h.ToString() + "%;\" src=\"" + src +
                       "\" allowfullscreen></iframe>";
            }

            /// <summary>
            /// Crear el HTML para insertar un iframe
            /// </summary>
            /// <param name="src">Ruta url de la pagina a cargar</param>
            /// <param name="medida">%, px, em</param>
            /// <param name="w">ancho</param>
            /// <param name="h">alto</param>
            /// <returns></returns>
            public static string iframe(string src, string medida, int w = 100, int h = 100)
            {
                if (string.IsNullOrEmpty(medida)) medida = "%";
                return "<iframe style=\"border: none;overflow: hidden;min-width:" + w.ToString() + medida + "; min-height:" + h.ToString() + medida + ";\" src=\"" + src +
                       "\" allowfullscreen></iframe>";
            }

            /// <summary>
            /// Quitar los saltos de linea y poner el TAG html para el salto y limpiar un texto y suprimir los tags html por su equivalente en ascii
            /// </summary>
            /// <param name="texto">Texto HTML</param>
            /// <returns></returns>
            public static string sanitize(string texto)
            {
                return HTMLUtil.sanitize(texto);
            }

            /// <summary>
            /// Volver a poner el formato HTML en un documento descodificado como ascii
            /// </summary>
            /// <param name="texto">Texto HTML</param>
            /// <returns></returns>
            public static string decodeHTML(string texto)
            {
                return HTMLUtil.decodeHTML(texto);
            }

            /// <summary>
            /// Extract all the images from the HTML
            /// </summary>
            /// <param name="htmlString"></param>
            /// <returns></returns>
            public static List<string> GetImages(string htmlString)
            {
                string pattern = @"<(img)\b[^>]*>";

                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(htmlString);

                List<string> imgSrcs = new List<string>();
                try
                {
                    string sourceText = matches[0].Value;
                    var imgSrcMatches = Regex.Matches(sourceText,
                                string.Format(@"<\s*img\s*src\s*=\s*{0}\s*([^{0}]+)\s*{0}", "\""),
                                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase |
                                RegexOptions.Multiline);
                    foreach (Match match in imgSrcMatches)
                    {
                        imgSrcs.Add(match.Groups[1].Value);
                    }
                }
                catch (Exception)
                {

                    imgSrcs.Add("img/nopicture.jpg");
                }

                return imgSrcs;
            }

            /// <summary>
            /// Get fist image found from the HTML
            /// </summary>
            /// <param name="htmlString"></param>
            /// <returns></returns>
            public static string GetFirstImage(string htmlString)
            {
                return GetImages(htmlString)[0];
            }
        }
    }
}
