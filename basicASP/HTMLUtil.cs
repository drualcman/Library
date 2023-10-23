using System.Text.RegularExpressions;
using System.Web;

namespace drualcman
{
    /// <summary>
    /// Name Space for ASP.NET c#
    /// </summary>
    namespace basicASP
    {
        public static class HTMLUtil
        {
            /// <summary>
            /// Eliminar las etiquetas HTML
            /// </summary>
            /// <param name="strTexto"></param>
            /// <returns></returns>
            public static string EliminaTAGS(string strTexto)
            {
                string result = strTexto;
                Regex rgx = new Regex("<((.|\n)*?)>");
                result = rgx.Replace(strTexto, "");
                return result;
            }

            /// <summary>
            /// Eliminar las etiquetas HTML
            /// </summary>
            /// <param name="strTexto"></param>
            /// <param name="tags">Solo eliminar los tags del array</param>
            /// <returns></returns>
            public static string EliminaTAGS(string strTexto, string[] tags)
            {
                string result = strTexto;
                string param = string.Empty;
                foreach(string tag in tags)
                {
                    param += "(<((.|\n)?)" + tag.ToUpper() + ">)|";
                    param += "(<((.|\n)?)" + tag.ToLower() + ">)|";
                }
                param = param.Remove(param.Length - 1, 1);      //eliminar el ultimo or |
                Regex rgx = new Regex(param);
                result = rgx.Replace(strTexto, "");
                return result;
            }


            /// <summary>
            /// Minificar el HTML de compilar una pagina ASP en el servidor
            /// </summary>
            /// <param name="htmlCadena"></param>
            /// <returns></returns>
            public static string MinificarHTML(string htmlCadena)
            {
                // TODO: Add constructor logic here
                // <Jose A. Fernandez />
                // autor https://geeks.ms/fernandezja/2011/02/15/asp-net-modificar-la-salida-html-para-compactar-minify-en-el-render-regex-yui-compresor-for-net-con-response-filter-y-con-pageparserfilter/
                //
                //minificar el HTML de la pagina
                htmlCadena = htmlCadena.Replace("  ", string.Empty);
                htmlCadena = htmlCadena.Replace(Environment.NewLine, string.Empty);
                const string reduceMultiSpace = @"[ ]{2,}";
                htmlCadena = Regex.Replace(htmlCadena.Replace("\t", " "), reduceMultiSpace, " ");
                return htmlCadena.ToString();
            }

            /// <summary>
            /// Quitar los saltos de linea y poner el TAG html para el salto y limpiar un texto y suprimir los tags html por su equivalente en ascii
            /// </summary>
            /// <param name="texto">Texto HTML</param>
            /// <returns></returns>
            public static string sanitize(string texto)
            {
                if(!string.IsNullOrEmpty(texto))
                {
                    texto = texto.Replace("\n\r", "\n");
                    texto = texto.Replace("\r\n", "\n");
                    texto = texto.Replace("\n", "<br/>");
                    texto = HttpUtility.HtmlAttributeEncode(texto);
                    return texto;
                }
                else return string.Empty;
            }

            /// <summary>
            /// Codificar como ascii una URL
            /// </summary>
            /// <param name="texto">Texto HTML</param>
            /// <returns></returns>
            public static string encodeURL(string texto)
            {
                return HttpUtility.UrlEncode(texto);
            }

            /// <summary>
            /// Descodificar una URL en ascii
            /// </summary>
            /// <param name="texto">Texto HTML</param>
            /// <returns></returns>
            public static string decodeURL(string texto)
            {
                return HttpUtility.UrlDecode(texto);
            }

            /// <summary>
            /// Codificar el formato HTML en un documento
            /// </summary>
            /// <param name="texto">Texto HTML</param>
            /// <returns></returns>
            public static string encodeHTML(string texto)
            {
                return HttpUtility.HtmlEncode(texto);
            }

            /// <summary>
            /// Volver a poner el formato HTML en un documento descodificado como ascii
            /// </summary>
            /// <param name="texto">Texto HTML</param>
            /// <returns></returns>
            public static string decodeHTML(string texto)
            {
                return HttpUtility.HtmlDecode(texto);
            }

            /* control de HTML tags extraido de 
             * https://stackoverflow.com/questions/204646/how-to-validate-that-a-string-doesnt-contain-html-using-c-sharp
             * Modificado un poquito
             */

            /// <summary>
            /// Comprueba si en el texto hay un tag concreto HTML
            /// </summary>
            /// <param name="text">texto enviado</param>
            /// <param name="tag">tag HTML</param>
            /// <returns></returns>
            public static bool ContainsHtmlTag(this string text, string tag)
            {
                string pattern = @"<\s*" + tag + @"\s*\/?>";
                return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
            }

            /// <summary>
            /// Comprueba si en el string enviado hay algun tag HTML
            /// </summary>
            /// <param name="text">texto a comprobar</param>
            /// <param name="tags">lista de tags separados por |</param>
            /// <returns></returns>
            public static bool ContainsHtmlTags(this string text, string tags)
            {
                return tags.Split('|').Select(x => new { tag = x, hastag = text.ContainsHtmlTag(x) }).Where(x => x.hastag).Count() > 0;
            }

            /// <summary>
            /// Compruieba si en el texto enviado hay algun tag HTML.
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public static bool ContainsHtmlTags(this string text)
            {
                return
                    text.ContainsHtmlTags(
                        "a|abbr|acronym|address|area|b|base|bdo|big|blockquote|body|br|button|caption|cite|code|col|colgroup|dd|del|dfn|div|dl|DOCTYPE|dt|em|fieldset|form|h1|h2|h3|h4|h5|h6|head|html|hr|i|img|input|ins|kbd|label|legend|li|link|map|meta|noscript|object|ol|optgroup|option|p|param|pre|q|samp|script|select|small|span|strong|style|sub|sup|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|ul|var|header|article|footer");
            }

            /// <summary>
            /// Comprueba si en el texto hay un tags HTML
            /// </summary>
            /// <param name="text">texto enviado</param>
            /// <returns></returns>
            public static bool ContainsTagHTML(this string text)
            {
                return text.ContainsHtmlTags(text);
            }
        }
    }
}
