using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace drualcman
{
    /// <summary>
    /// Manejo de números
    /// </summary>
    public  class numeros
    {
        /// <summary>
        /// Convert number to string 
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="conFormato">Return with format</param>
        /// <param name="format">Format to get. para moneda {0:c}</param>
        /// <param name="currency">Use the currency. Send the name</param>
        /// <returns></returns>
        public string number2String(object number, bool conFormato = false, string format = "{0:0.00}", string moneda = "")
        {
            string numReturn = "0";
            // convert to number the var incomming
            try
            {
                double dNum = 0;

                if (double.TryParse(noGlobalization(number.ToString(), moneda), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out dNum) == false) dNum = 0;
                if (conFormato == true)
                {
                    if (moneda == "")
                        numReturn = string.Format(System.Globalization.CultureInfo.InvariantCulture, format, dNum);
                    else
                        numReturn = string.Format(System.Globalization.CultureInfo.GetCultureInfo(moneda), format, dNum);
                }
                else
                    numReturn = dNum.ToString();
            }
            catch (Exception ex)
            {
                numReturn = "Is not a valid number.\r\n" + ex.Message + "\r\n" + ex.StackTrace;
            }
            return numReturn;
        }

        /// <summary>
        /// Convert number to SQL format to store in a DB
        /// </summary>
        /// <param name="number"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string Number2Sql(object number, string format = "{0:0.00}")
        {
            return number2String(number, true, format, "en-EN");       //force always get number in fromat en-EN
        }

        public string noGlobalization(string number, string cultura)
        {
            if (string.IsNullOrEmpty(cultura) || string.IsNullOrWhiteSpace(cultura))
                cultura = System.Globalization.CultureInfo.InvariantCulture.Name;

            decimal monto;
            // convertir a número
            try
            {
                // comprobar que no era una moneda
                string decS = System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
                string decE = System.Globalization.CultureInfo.GetCultureInfo(cultura).NumberFormat.NumberDecimalSeparator;
                string sepS = System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
                string sepE = System.Globalization.CultureInfo.GetCultureInfo(cultura).NumberFormat.NumberGroupSeparator;

                if (decS != decE)
                {
                    //cambiar el decimal
                    if (number.IndexOf(decE) > 0)
                    {
                        number = number.Replace(decS, "#");
                        number = number.Replace(sepS, decE);
                        number = number.Replace("#", "");

                    }
                    else
                    {
                        number = number.Replace(decE, "#");
                        number = number.Replace(decS, decE);
                        number = number.Replace("#", "");
                    }
                    monto = decimal.Parse(number, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.GetCultureInfo(cultura));
                }
                else
                {
                    //comprobar de nuevo quye el numero enviado no necesita cambiar el decimal
                    if (number.IndexOf(decE) < 0)
                    {
                        number = number.Replace(sepS, "#");
                        number = number.Replace(decE, sepS);
                        number = number.Replace("#", decE);

                    }
                    monto = decimal.Parse(number, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.GetCultureInfo(cultura));
                }
            }
            catch
            {
                // quitar todo lo que no sean números '.'  ','
                string newNumber = "";
                bool encontrado = false;
                for (int i = 0; i < number.Length; i++)
                {
                    string caracter = number.Substring(i, 1);

                    switch (caracter)
                    {
                        case ".":
                        case ",":
                            if (encontrado == false)
                            {
                                newNumber += ".";
                                encontrado = true;
                            }
                            else
                            {
                                //ya habia encontrado lo que se supone q era un decimal que  al final era un millar
                                //quitar el punto puesto y volver a ponerlo
                                newNumber = newNumber.Replace(".", "");
                                newNumber += ".";
                            }
                            break;
                        default:
                            int n;
                            if (int.TryParse(caracter, out n) == true)
                                newNumber += caracter;
                            break;
                    }
                }
                number = newNumber;
                monto = decimal.Parse(number, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
            }
            number = monto.ToString(System.Globalization.CultureInfo.InvariantCulture);

            return number;
        }

        /// <summary>
        /// Convert String to float
        /// </summary>
        /// <param name="number">Number to convert</param>
        /// <returns></returns>
        public double string2number(string number, string cultura = "")
        {
            double dNum;
            if (!string.IsNullOrEmpty(number))
            {
                number = noGlobalization(number, cultura);

                // convert to number the var incomming
                try
                {
                    dNum = double.Parse(number, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    return dNum;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Is not a valid number.\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
            else return 0;
        }

        /// <summary>
        /// Convert string number in cents to number formating
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="format">Formato do you want. Defect 2 decimals</param>
        /// <param name="cultura">Tipo de formato del numero. Pais a utilizar</param>
        /// <returns></returns>
        public string string2numberInCents(string number, string format = "", string cultura = "")
        {
            double dNum;

            // convert to number the var incomming
            dNum = string2number(number, cultura);
            dNum /= 100;

            bool formato = false;
            if (!string.IsNullOrEmpty(format) && !string.IsNullOrEmpty(format)) formato = true;

            number = number2String(dNum, formato, format, cultura);

            return number;
        }

        /// <summary>
        /// Extrae solo los numeros de un string
        /// </summary>
        /// <param name="texto">Texto a extraer los numeros</param>
        /// <returns></returns>
        public string soloNumeros(object texto)
        {
            return numeros.SoloNumeros(texto);
        }

        /// <summary>
        /// Extrae solo los numeros de un string
        /// </summary>
        /// <param name="texto">Texto a extraer los numeros</param>
        /// <returns></returns>
        public string soloNumeros(string texto)
        {
            return numeros.SoloNumeros(texto);
        }

        /// <summary>
        /// Extrae solo los numeros de un string
        /// </summary>
        /// <param name="texto">Texto a extraer los numeros</param>
        /// <returns></returns>
        public static string SoloNumeros(object texto)
        {
            string retorno = string.Empty;
            string pattern = @"(?:- *)?\d";
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
            try
            {
                foreach (System.Text.RegularExpressions.Match m in rgx.Matches(texto.ToString()))
                {
                    retorno += m.Value;
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Extrae solo los numeros de un string
        /// </summary>
        /// <param name="texto">Texto a extraer los numeros</param>
        /// <returns></returns>
        public static string SoloNumeros(string texto)
        {
            string retorno = string.Empty;
            string pattern = @"(?:- *)?\d";
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
            try
            {
                foreach (System.Text.RegularExpressions.Match m in rgx.Matches(texto))
                {
                    retorno += m.Value;
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        /// <summary>
        /// Round decimal to up 0.195 = 0.12
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double RoundUp(double number, int decimals)
        {
            return Math.Round(number, decimals, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// Round decimal to up 0.195 = 0.12
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimals"></param>
        public static decimal RoundUp(decimal number, int decimals)
        {
            return Math.Round(number, decimals, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// Calcular el porcentaje entre dos valores
        /// </summary>
        /// <param name="inicial">valor inicial</param>
        /// <param name="final">valod final</param>
        /// <returns></returns>
        public static decimal GetPorcentaje(decimal inicial, decimal final)
        {
            decimal ratio;
            if (inicial > 0)
            {
                //porcentaje positivo
                ratio = final - inicial;
                ratio = (ratio / inicial) * 100;
            }
            else ratio = 100 * final;
            return ratio;
        }

        /// <summary>
        /// Calcular el porcentaje entre dos valores
        /// </summary>
        /// <param name="inicial">valor inicial</param>
        /// <param name="final">valod final</param>
        /// <returns></returns>
        public decimal getPorcentaje(decimal inicial, decimal final)
        {
            decimal ratio;
            if (inicial > 0)
            {
                //porcentaje positivo
                ratio = final - inicial;
                ratio = (ratio / inicial) * 100;
            }
            else ratio = 100 * final;
            return ratio;
        }

        /// <summary>
        /// Generar un numero aleatorio
        /// </summary>
        /// <returns></returns>
        public static int numeroAleatorio()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        /// <summary>
        /// Generar un numero aleatorio
        /// </summary>
        /// <param name="max">Numero maximo a calcular</param>
        /// <returns></returns>
        public static int numeroAleatorio(int max)
        {
            Random rnd = new Random();
            return rnd.Next(max);
        }

        /// <summary>
        /// Generar un numero aleatorio
        /// </summary>
        /// <param name="min">Numero minimo a calcular</param>
        /// <param name="max">Numero maximo a calcular</param>
        /// <returns></returns>
        public static int numeroAleatorio(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        /// <summary>
        /// Generar un numero aleatorio
        /// </summary>
        /// <returns></returns>
        public int NumeroAleatorio()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        /// <summary>
        /// Generar un numero aleatorio
        /// </summary>
        /// <param name="max">Numero maximo a calcular</param>
        /// <returns></returns>
        public int NumeroAleatorio(int max)
        {
            Random rnd = new Random();
            return rnd.Next(max);
        }

        /// <summary>
        /// Generar un numero aleatorio
        /// </summary>
        /// <param name="min">Numero minimo a calcular</param>
        /// <param name="max">Numero maximo a calcular</param>
        /// <returns></returns>
        public int NumeroAleatorio(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        /// <summary>
        /// Convert number to string in cents
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns></returns>
        public string number2StringInCents(object number)
        {
            decimal dNum;
            string numReturn;

            // convert to number the var incomming
            try
            {
                dNum = Convert.ToDecimal(number);

                // know if have decimals
                // later truncate to 2 decimals
                // and calculate int
                if ((dNum % 1) != 0)
                {
                    // have decimals
                    numReturn = dNum.ToString("N2");
                    dNum = decimal.Parse(numReturn) * 100;       // delete decimals
                    numReturn = dNum.ToString("N0");
                }
                else
                {
                    // don't have decimals
                    dNum *= 100;       // delete decimals
                    numReturn = dNum.ToString("N0");
                }

                int pos = numReturn.IndexOf(",");

                if (pos > 0)        // delete character ","
                {
                    do
                    {
                        numReturn = numReturn.Remove(pos, 1);
                        pos = numReturn.IndexOf(",");
                    } while (pos > 0);

                }
            }
            catch
            {
                numReturn = "Is not a valid number.";
            }

            return numReturn;
        }

        /// <summary>
        /// Convert number to string in cents
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="fuerzaNum">Forzar la conversion de la cadena a solo numeros</param>
        /// <returns></returns>
        public string number2StringInCents(object number, bool fuerzaNum)
        {
            if (fuerzaNum == true)
            {
                return number2String(FuerzaNumeros(number.ToString()));
            }
            else
            {
                return number2StringInCents(number);
            }
        }

        /// <summary>
        /// Devuelve solo los numeros de una cadena sin ningun otro caracter
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public string FuerzaNumeros(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\d]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Comprobar si la variable enviada es un numero
        /// </summary>
        /// <param name="numero">numero enviado</param>
        /// <returns></returns>
        public static bool EsNumero(object numero)
        {
            bool resultado;

            int n;

            try
            {
                resultado = int.TryParse(numero.ToString(), out n);
            }
            catch
            {
                resultado = false;
            }

            return resultado;
        }
    }


}
