using System.Text.RegularExpressions;

namespace drualcman
{
    /// <summary>
    /// Validaciones basicas de formatos
    /// </summary>
    public class validar
    {
        /// <summary>
        /// Comprueba que la URL es correcta
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool URL(string url)
        {
            return Regex.IsMatch(url.ToLower(), @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");
        }

        /// <summary>
        /// Comprueba que la URL es correcta
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool URLN(string url)
        {
            return Regex.IsMatch(url.ToLower(), @"^(http:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \?=.-]*)*\/?$");
        }

        /// <summary>
        /// Comprueba que la URL es correcta y segura
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool URLS(string url)
        {
            return Regex.IsMatch(url.ToLower(), @"^(https:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \?=.-]*)*\/?$");
        }

        /// <summary>
        /// Comprueba que e-mail es correcto
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public bool mail(string mail)
        {
            return Regex.IsMatch(mail.ToLower(), @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$");
        }

        /// <summary>
        /// Función para validar una dirección de correo electrónico
        /// </summary>
        /// <param name="email">Mail a validar</param>
        /// <returns>Devuelve verdadero si es valido, falso si no lo es.</returns>
        public bool Email(string email)
        {
            bool bResutado = true;

            if(string.IsNullOrEmpty(email)) bResutado = false;
            else
            {
                string c;

                // rompo el mail en dos partes, antes y despues de la @
                string[] partes = email.ToLower().Split('@');
                string strComparacion = "._-abcdefghijklmnopqrstuvwxyz0123456789";
                int nElementos = partes.GetUpperBound(0);
                if(nElementos == 1)
                {
                    // para cada parte, compruebo varias cosas
                    foreach(string parte in partes)
                    {
                        // comprueba que tiene algún carácter
                        if(parte.Length > 0)
                        {
                            // para cada caracter  de la parte
                            for(int i = 0; i < parte.Length; i++)
                            {
                                // tomo el caracter actual
                                c = parte.Substring(i, 1).ToLower();
                                // comprobar que es un caracter permitido
                                if(strComparacion.IndexOf(c) < 0)
                                    bResutado = false;
                            }
                            // si la parte actual acaba o empieza en punto (.) la dirección no es válida                    
                            if((parte.Substring(0, 1) == ".") ||
                                (parte.Substring(parte.Length - 1, 1) == "."))
                                bResutado = false;
                            // calcular cuantos caracteres hay después del último punto (.) de la segunda
                            // parte del mail.
                            int l = parte.Length - parte.LastIndexOf('.');
                            // si el número de caracteres es distinto de 2 y 3, el mail no es válido
                            if(l < 3)
                                bResutado = false;
                            // si encuentro dos puntos (..) seguidos, tampoco es válido
                            c = "..";
                            if(partes[1].IndexOf(c) > 0)
                                bResutado = false;
                        }
                        else
                        {
                            bResutado = false;
                        }
                    }
                }
                else // si el mayor indice del array es distinto de 1 es que no he obtenido las dos partes
                    bResutado = false;
            }

            return bResutado;
        }


        /// <summary>
        /// Comprueba que la contraseña es fuerte.
        /// Contraseñas que contengan al menos una letra mayúscula.
        /// Contraseñas que contengan al menos una letra minúscula.
        /// Contraseñas que contengan al menos un número o caracter especial.
        /// Contraseñas cuya longitud sea como mínimo 8 caracteres.
        /// Contraseñas cuya longitud máxima no debe ser arbitrariamente limitada.
        /// </summary>
        /// <param name="pass">Password a validar</param>
        /// <returns></returns>        
        public bool password(string pass)
        {
            return Regex.IsMatch(pass, @"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");
        }

        /// <summary>
        /// Comprueba que tlf es correcto. Segun listados en la Wikipedia https://en.wikipedia.org/wiki/List_of_country_calling_codes
        /// </summary>
        /// <param name="tlf"></param>
        /// <returns></returns>
        public bool tlf(string tlf)
        {
            return Regex.IsMatch(tlf, @"^\+?\d{1,3}?[- .]?\(?(?:\d{1,4})\)?[- .]?\d{3,4}[- .]?\d{3,4}$");
        }

        /// <summary>
        /// Comprueba que la tarjeta de credito es correcta
        /// </summary>
        /// <param name="number">Credit Cart Number</param>
        /// <returns></returns>
        public bool tarjeta(string number)
        {
            return Regex.IsMatch(number, @"^((67\d{2})|(4\d{3})|(5[1-5]\d{2})|(6011))(-?\s?\d{4}){3}|(3[4,7])\ d{2}-?\s?\d{6}-?\s?\d{5}$");
        }
    }

}
