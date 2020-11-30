using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace drualcman
{
    /// <summary>
    /// Utilidades y ayudas para c#
    /// </summary>
    /// 
    public partial class utilidades
    {
        /// <summary>
        /// Eliminar las etiquetas HTML
        /// </summary>
        /// <param name="strTexto"></param>
        /// <returns></returns>
        public static string CleanHTML(string text)
        {
            string result = text;
            result = System.Text.RegularExpressions.Regex.Replace(result, "<((.|\n)*?)>", "");
            return result;
        }

        /// <summary>
        /// Convert ArrayList in string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delimeter">delimeter for separate words</param>
        /// <returns></returns>
        public string arrayList2string(ArrayList list, string delimeter = ",")
        {
            string retorno = "";
            delimeter += " ";
            try
            {
                foreach (var item in list)
                {
                    retorno += item.ToString() + delimeter;
                }

                //delete de last ", "
                retorno = retorno.Substring(0, retorno.Length - 2);
            }
            catch
            {
                retorno = "";
            }

            return retorno;
        }

        /// <summary>
        /// Coger una cadena y quitar todos los signos o simbolos especiales
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public string SoloLetrasNumeros(string cadena)
        {
            string textoNormalizado = cadena.Normalize(NormalizationForm.FormD);
            //coincide todo lo que no sean letras y números ascii o espacio
            //y lo reemplazamos por una cadena vacía.
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }

        /// <summary>
        /// Devuelve la cadena con una longitud fija. Si no tiene esa longitud lo rellena con espacios en blanco
        /// </summary>
        /// <param name="MyValue">string para conseguir longitud fija</param>
        /// <param name="length">longitud</param>
        /// <returns></returns>
        public static string FixedLengthString(string MyValue, int length)
        {
            string retorno = string.Empty;
            if (MyValue.Length < length)
            {
                //rellenar con espacios en blanco hasta el final del string
                retorno = MyValue;
                for (int i = MyValue.Length; i < length; i++)
                {
                    retorno += " ";
                }
            }
            else if (MyValue.Length > length) retorno = MyValue.Substring(0, length);
            else retorno = MyValue;
            return retorno;
        }

        /// <summary>
        /// Devuelve la cadena con una longitud fija. Si no tiene esa longitud lo rellena con espacios en blanco
        /// </summary>
        /// <param name="length">longitud</param>
        /// <param name="MyValue">string para conseguir longitud fija</param>
        /// <returns></returns>
        public static string FixedLengthString(int length, string MyValue)
        {
            string retorno = string.Empty;
            if (MyValue.Length < length)
            {
                //rellenar con espacios en blanco hasta el final del string
                retorno = MyValue;
                for (int i = MyValue.Length; i < length; i++)
                {
                    retorno += " ";
                }
            }
            else if (MyValue.Length > length) retorno = MyValue.Substring(0, length);
            else retorno = MyValue;
            return retorno;
        }

        /// <summary>
        /// Devuelve la cadena con una longitud fija. Si no tiene esa longitud lo rellena con el string enviado en relleno
        /// </summary>
        /// <param name="length">longitud</param>
        /// <param name="MyValue">string para conseguir longitud fija</param>
        /// <param name="relleno">string con el caracter para rellenar</param>
        /// <returns></returns>
        public static string FixedLengthString(int length, string MyValue, string relleno)
        {
            string retorno = string.Empty;
            if (MyValue.Length < length)
            {
                //rellenar con espacios en blanco hasta el final del string
                retorno = MyValue;
                for (int i = MyValue.Length; i < length; i++)
                {
                    retorno += relleno;
                }
            }
            else if (MyValue.Length > length) retorno = MyValue.Substring(0, length);
            else retorno = MyValue;
            return retorno;
        }

        /// <summary>
        /// Devuelve la cadena con una longitud fija. Si no tiene esa longitud lo rellena con el string enviado en relleno
        /// </summary>
        /// <param name="MyValue">string para conseguir longitud fija</param>
        /// <param name="length">longitud</param>
        /// <param name="relleno">string con el caracter para rellenar</param>
        /// <returns></returns>
        public static string FixedLengthString(string MyValue, int length, string relleno)
        {
            string retorno = string.Empty;
            if (MyValue.Length < length)
            {
                //rellenar con espacios en blanco hasta el final del string
                retorno = MyValue;
                for (int i = MyValue.Length; i < length; i++)
                {
                    retorno += relleno;
                }
            }
            else if (MyValue.Length > length) retorno = MyValue.Substring(0, length);
            else retorno = MyValue;
            return retorno;
        }

        /// <summary>
        /// Genera un texto de X caracteres aleatoriamente
        /// </summary>
        /// <param name="size">Numero de caracteres</param>
        /// <returns></returns>
        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            int num1 = 0;
            int num2 = 0;
            for (int i = 0; i < size; i++)
            {
                while (num1 == num2)
                {
                    num2 = numeros.numeroAleatorio(65, 90);
                }
                num1 = num2;
                ch = Convert.ToChar(num1);
                builder.Append(ch);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Solo admite letras. Se pueden agregar otros caracteres. Se usa Expresiones regulares.
        /// </summary>
        /// <param name="texto">Caracteres a comprobar</param>
        /// <param name="adicionales">Otros caracteres a admitir. Para espacios en blanco agregar '\s' para numeros '\d'</param>
        /// <returns>Solo las letras sin espacios en blanco (salvo que este en los adicionales)</returns>
        public static string SoloLetras(string texto, string adicionales = "")
        {
            Regex comprueba = new Regex(@"[a-zA-ZñÑ" + adicionales + "]");      //definicion de los caracteres permitidos
            MatchCollection matches = comprueba.Matches(texto);
            string retorno = string.Empty;
            if (matches.Count > 0)
            {
                foreach (Match item in matches)
                {
                    retorno += item.Value;
                }
            }
            return retorno;
        }

        /// <summary>
        /// Comprueba que la tarjeta de credito es correcta
        /// </summary>
        /// <param name="url">Url con el video que deseamos extraer el ID</param>
        /// <returns></returns>
        public string idYoutube(string url)
        {
            Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);

            Match youtubeMatch = YoutubeVideoRegex.Match(url);

            string id = string.Empty;

            if (youtubeMatch.Success)
                id = youtubeMatch.Groups[youtubeMatch.Groups.Count - 1].Value;

            return id;
        }

        /// <summary>
        /// Conocer el tipo del objeto enviado
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static string getTipo(object sender)
        {
            return Objetos.GetTipo(sender);
        }

        /// <summary>
        /// Conver CHAR to int
        /// </summary>
        /// <param name="c">1 solo caracter</param>
        /// <returns></returns>
        public int toAsc(char c)
        {
            return (int)c;
        }

        /// <summary>
        /// Converto int to CHAT
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public string fromAsc(int c)
        {
            return Convert.ToString((char)c);
        }

        /// <summary>
        /// Generate TABS with white spaces
        /// </summary>
        /// <param name="tabs">num of tabs to do</param>
        /// <param name="spaces">num of spaces per tab</param>
        /// <returns></returns>
        public string tabs(int tabs, int spaces = 1)
        {
            string tab = string.Empty;

            for (int i = 0; i < spaces; i++)
            {
                tab += " ";
            }

            string retorno = string.Empty; ;
            for (int i = 0; i < tabs; i++)
            {
                retorno += tab;
            }
            return retorno;
        }
    }

}
