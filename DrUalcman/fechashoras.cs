using System;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for drFechashoras
/// </summary>
namespace drualcman
{
    /// <summary>
    /// Manejo de fechas y horas
    /// </summary>
    public class fechashoras
    {
        /// <summary>
        /// Convertir la fecha en formato yyyy/MM/dd
        /// </summary>
        /// <param name="Fecha">Fecha a convertir</param>
        /// <returns></returns>
        public string ConvertirFechaSQL(DateTime Fecha)
        {
            //convertir la fecha al format admitido por SQL
            return string.Format("{0:yyyy/MM/dd HH:mm:ss}", Fecha);
        }

        /// <summary>
        /// Convertir la fecha en formato yyyy/MM/dd
        /// </summary>
        /// <param name="Fecha">Fecha a convertir</param>
        /// <returns></returns>
        public string ConvertirFechaSQL(DateTime Fecha, bool hora)
        {
            //convertir la fecha al format admitido por SQL
            if(hora == false) return string.Format("{0:yyyy/MM/dd}", Fecha);
            else return string.Format("{0:yyyy/MM/dd HH:mm:ss}", Fecha);
        }

        /// <summary>
        /// Convertir la fecha en formato yyyy/MM/dd
        /// </summary>
        /// <param name="Fecha">Objeto con formato de fecha</param>
        /// <returns></returns>
        public string ConvertirFechaSQL(object Fecha)
        {
            //convertir la fecha al format admitido por SQL
            return ConvertirFechaSQL(fechashoras.ConvertToDateTime(Fecha));
        }

        /// <summary>
        /// Convertir la fecha en formato yyyy/MM/dd
        /// </summary>
        /// <param name="Fecha">Objeto con formato de fecha</param>
        /// <returns></returns>
        public string ConvertirFechaSQL(object Fecha, bool hora)
        {
            //convertir la fecha al format admitido por SQL
            return ConvertirFechaSQL(fechashoras.ConvertToDateTime(Fecha), hora);
        }

        /// <summary>
        /// Convertir la fecha en formato yyyy/MM/dd
        /// </summary>
        /// <param name="Fecha">Texto con formato de fecha</param>
        /// <returns></returns>
        public string ConvertirFechaSQL(string Fecha)
        {
            //convertir la fecha al format admitido por SQL
            return ConvertirFechaSQL(fechashoras.ConvertToDateTime(Fecha));
        }

        /// <summary>
        /// Convertir la fecha en formato yyyy/MM/dd
        /// </summary>
        /// <param name="Fecha">Texto con formato de fecha</param>
        /// <returns></returns>
        public string ConvertirFechaSQL(string Fecha, bool hora)
        {
            //convertir la fecha al format admitido por SQL
            return ConvertirFechaSQL(fechashoras.ConvertToDateTime(Fecha), hora);
        }

        /// <summary>
        /// Convertir la hora al fromat 24 horas SIN SEGUNDOS
        /// </summary>
        /// <param name="Hora">Fecha a convertir</param>
        /// <returns></returns>
        public string ConvertirHora(DateTime Hora)
        {
            //convertir la fecha al format admitido por SQL
            return string.Format("{0:HH:mm}", Hora);
        }

        /// <summary>
        /// Convertir la hora al fromat 24 horas SIN SEGUNDOS
        /// </summary>
        /// <param name="Hora">Objeto con formato de fecha</param>
        /// <returns></returns>
        public string ConvertirHora(object Hora)
        {
            //convertir la fecha al format admitido por SQL
            return ConvertirHora(fechashoras.ConvertToDateTime(Hora));
        }

        /// <summary>
        /// Convertir la hora al fromat 24 horas SIN SEGUNDOS
        /// </summary>
        /// <param name="Fecha">Texto con formato de fecha</param>
        /// <returns></returns>
        public string ConvertirHora(string Hora)
        {
            //convertir la fecha al format admitido por SQL
            return ConvertirHora(fechashoras.ConvertToDateTime(Hora));
        }

        /// <summary>
        /// Convert Seconds in Time. Can return string with tag for slip. With tag first are days, hours, min, sec
        /// </summary>
        /// <param name="secs">Total Seconds to convert</param>
        /// <param name="tag">Tag use for can split.</param>
        /// <param name="format">Format for use. Defaut Hour:Min:Sec "{0:D2}:{1:D2}:{2:D2}". If need days wirte a correct format with 4 items.</param>
        /// <returns></returns>
        public string secondsToTime(double secs, string tag = "", string format = "{0:D2}:{1:D2}:{2:D2}", bool showDays = false)
        {
            TimeSpan time = TimeSpan.FromSeconds(secs);
            return secondsToTime(time, tag, format, showDays);
        }

        /// <summary>
        /// Convert Seconds in Time. Can return string with tag for slip. With tag first are days, hours, min, sec
        /// </summary>
        /// <param name="secs">Total Seconds to convert</param>
        /// <param name="tag">Tag use for can split.</param>
        /// <param name="format">Format for use. Defaut Hour:Min:Sec "{0:D2}:{1:D2}:{2:D2}". If need days wirte a correct format with 4 items.</param>
        /// <returns></returns>
        public string secondsToTime(TimeSpan time, string tag = "", string format = "{0:D2}:{1:D2}:{2:D2}", bool showDays = false)
        {
            string formatHour = "";

            if(tag == "")
                if(showDays == false)
                    formatHour = string.Format(format, time.Hours, time.Minutes, time.Seconds);
                else
                    formatHour = string.Format("{3}D" + format, time.Hours, time.Minutes, time.Seconds, time.Days);
            else
                formatHour = time.Days.ToString() + tag + string.Format(format, time.Hours, time.Minutes, time.Seconds);

            return formatHour;
        }

        /// <summary>
        /// Obtener el tiempo representado en horas:minutos:segundos
        /// </summary>
        /// <param name="segundos">Segundos a calcular</param>
        /// <param name="showDays">Mostrar dias, meses y años  (años, meses, dias, horas:minutos:segundos)</param>
        /// <param name="formatHours">Formato para mostrar las horas</param>
        /// <param name="formatDays">Formato para mostrar los dias. Recordar hay que separar dias de horas</param>
        /// <returns></returns>
        public string secondsToTime(double secs, bool showDays, string formatHours = "{0:D2}:{1:D2}:{2:D2}", string formatDays = "{0}:{1}:{2}")
        {
            int hor = 0, min = 0, seg = 0;
            int days = 0, month = 0, years = 0;
            hor = (int)(secs / 3600);            //calcular las horas
            while(hor > 23)                    //calcular los dias, meses o años pasados
            {
                secs = secs - 86400;
                hor = (int)(secs / 3600);
                days++;
                if(days > 30)
                {
                    month++;
                    if(month > 12)
                    {
                        years++;
                        month = 0;
                    }
                    days = 0;
                }
            }
            secs = secs - (hor * 3600);     //calcular el resto de segundos que quedan despues de quitar las horas pasadas
            min = (int)(secs / 60);                  //calcular los minutos que hay en los segundos restantes
            seg = (int)(secs - (min * 60));            //calcular los segundos restantes al quitar los minutos

            if(showDays == true) return string.Format(formatDays, years, month, days) + string.Format(formatHours, hor, min, seg);        //decuelve años, minutos, dias, horas:minutos:segundos
            else return string.Format(formatHours, hor, min, seg);            //devuelve solo horas:minutos:segundos
        }

        /// <summary>
        /// Obtener el tiempo representado en horas:minutos:segundos
        /// </summary>
        /// <param name="segundos">Segundos a calcular</param>
        /// <param name="showDays">Mostrar dias, meses y años  (años, meses, dias, horas:minutos:segundos)</param>
        /// <param name="formatHours">Formato para mostrar las horas</param>
        /// <param name="formatDays">Formato para mostrar los dias. Recordar hay que separar dias de horas</param>
        /// <returns></returns>
        public string secondsToTime(TimeSpan secs, bool showDays, string formatHours = "{0:D2}:{1:D2}:{2:D2}", string formatDays = "{0}:{1}:{2}")
        {
            int hor = 0, min = 0, seg = 0;
            int days = 0, month = 0, years = 0;
            hor = secs.Hours;           //calcular las horas
            min = secs.Minutes;         //calcular los minutos que hay en los segundos restantes
            seg = secs.Seconds;         //calcular los segundos restantes al quitar los minutos
            //calcular los años, meses y dias
            days = secs.Days;          //recoger el total de dias
            while(days > 30)
            {
                days -= 30;
                if(month > 12)
                {
                    years++;
                    month = 0;
                }
                else month++;
            }

            if(showDays == true) return string.Format(formatDays, years, month, days) + string.Format(formatHours, hor, min, seg);        //decuelve años, minutos, dias, horas:minutos:segundos
            else return string.Format(formatHours, hor, min, seg);            //devuelve solo horas:minutos:segundos
        }

        /// <summary>
        /// Comprueba si el formato de una hora que esta en una cadena de caracteres es correcta. Si es así la función devolverá True.
        /// </summary>
        /// <param name="hora"></param>
        /// <returns></returns>
        public static bool ValidarHora(string hora)
        {
            string regex = "((?:(?:[0-1][0-9])|(?:[2][0-3])|(?:[0-9])):(?:[0-5][0-9])(?::[0-5][0-9])?(?:\\s?(?:am|AM|pm|PM))?)";
            Regex reg = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match match = reg.Match(hora);
            return match.Success;
        }

        /// <summary>
        /// Convertir fecha UNIX en DateTime
        /// </summary>
        /// <param name="unixTimeStamp">feha/hora en estilo unix</param>
        /// <returns></returns>
        public DateTime ConvertirFechaUnix(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Concertir a formato de fecha
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string fecha)
        {
            DateTime convertedDate;
            try
            {
                convertedDate = Convert.ToDateTime(fecha);
            }
            catch
            {
                // in error it's possible date return in Spanish format
                // try to convert with CultureInfo of Spain
                try
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-EN");
                    convertedDate = DateTime.Parse(fecha, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                }
                catch(Exception ex)
                {
                    // it's error the string is not in the proper format or format unknow.
                    ArgumentException err = new ArgumentException(string.Format("'{0}' is not in the proper format.\r\nSystem Error: {1}", fecha, ex));
                    throw err;
                }
            }

            return convertedDate;
        }

        /// <summary>
        /// Concertir a formato de fecha
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(object fecha)
        {
            DateTime convertedDate;

            try
            {
                convertedDate = Convert.ToDateTime(fecha);
            }
            catch
            {
                // in error it's possible date return in Spanish format
                // try to convert with CultureInfo of Spain
                try
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-AU");
                    convertedDate = DateTime.Parse(fecha.ToString(), culture);
                }
                catch(Exception ex)
                {
                    // it's error the string is not in the proper format or format unknown.
                    ArgumentException err = new ArgumentException(string.Format("'{0}' is not in the proper format.\r\nSystem Error: {1}", fecha, ex));
                    throw err;
                }
            }

            return convertedDate;
        }


        /// <summary>
        /// Devolver el nombre del mes. Con error devuelve el número de mes enviado
        /// </summary>
        /// <param name="meses">Meses en el idioma que corresponda</param>
        /// <param name="nMes">Numero de mes</param>
        /// <param name="reduct">Numero de caracteres a devolver</param>
        /// <returns></returns>
        public string QueMes(string[] meses, byte nMes, int reduct = 0)
        {
            string retorno;

            try
            {
                if(reduct == 0)
                {
                    retorno = meses[nMes - 1];
                }
                else
                {
                    if(reduct > meses[nMes - 1].Length) reduct = meses[nMes - 1].Length;
                    retorno = meses[nMes - 1].Substring(0, reduct);
                }
            }
            catch
            {
                retorno = "-" + nMes.ToString() + "-";
            }

            return retorno;
        }

        /// <summary>
        /// Calcular la edad de la persona
        /// </summary>
        /// <param name="dNacido">Fecha de nacimiento</param>
        /// <param name="dFin">Fecha en la que se quiere saber la edad</param>
        /// <returns></returns>
        public int TuEdad(object dNacido, DateTime dFin)
        {
            int Anos = 0;

            try
            {
                DateTime MiNacimiento = fechashoras.ConvertToDateTime(dNacido);
                TimeSpan Difference = dFin.Subtract(MiNacimiento);
                // 1 year have 365 days
                // every 4 years have 1 day more
                // that's why divide by 365.25
                Anos = (int)Math.Ceiling(Difference.Days / 365.25);

                Anos = dFin.Year - MiNacimiento.Year;
                if(dFin.Month <= MiNacimiento.Month)
                {
                    if(dFin.Day < MiNacimiento.Day) Anos--;
                }
            }
            catch
            {
                Anos = 0;
            }
            return Anos;
        }


        /// <summary>
        /// calculate how many days ahead has between 2 dates
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public int DaysToGo(DateTime start, DateTime finish)
        {
            // Difference in days, hours, and minutes.
            TimeSpan ts = finish - start;

            // Difference in days.
            return ts.Days;
        }

        /// <summary>
        /// calculate how many days in back has between 2 dates
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public int DaysToBack(DateTime start, DateTime finish)
        {
            // Difference in days, hours, and minutes.
            TimeSpan ts = start - finish;

            // Difference in days.
            return ts.Days;
        }

        /// <summary>
        /// Calcula cuantos dias faltan desde una fecha a otra
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public int diasFaltan(DateTime start, DateTime finish) => DaysToGo(start, finish);

        /// <summary>
        /// Calcula cuantos dias han pasado desde una fecha a otra
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public int diasPasados(DateTime start, DateTime finish) => DaysToBack(start, finish);

        /// <summary>
        /// Get current time or date
        /// </summary>
        /// <param name="format">Hmm=2330, ddddd=Monday</param>
        /// <returns></returns>
        public string getDateTime(string format)
        {
            string retorno = string.Empty;
            DateTime hour = DateTime.Now;

            retorno = hour.ToString(format);

            return retorno;
        }

        /// <summary>
        /// Get current time or date
        /// </summary>
        /// <param name="dias">Agregar numero de dias a la fecha</param>
        /// <param name="format">Hmm=2330, ddddd=Monday</param>
        /// <returns></returns>
        public string getDateTime(int dias, string format)
        {
            string retorno = string.Empty;
            DateTime hour = DateTime.Now.AddDays(dias);

            retorno = hour.ToString(format);

            return retorno;
        }

        /// <summary>
        /// Tipo de tiempo a ulitizar para agregar o quitar en los formatos de fecha/hora
        /// </summary>
        public enum TipoTiempo
        {
            Years,
            Meses,
            Dias,
            Horas,
            Minutos,
            Segundos,
            Milisegundos,
            Ticks
        }

        /// <summary>
        /// Get date/time from server
        /// </summary>
        /// <param name="format">Hmm=2330, ddddd=Monday</param>
        /// <param name="add">Add or remove time</param>
        /// <param name="tipo">Type of time to add or remove</param>
        /// <returns></returns>
        public string getDateTime(string format, decimal add, TipoTiempo tipo)
        {
            string retorno = string.Empty;
            DateTime hour = DateTime.Now;

            switch(tipo)
            {
                case TipoTiempo.Years:
                    hour = hour.AddYears(Convert.ToInt16(add));
                    break;
                case TipoTiempo.Meses:
                    hour = hour.AddMonths(Convert.ToInt16(add));
                    break;
                case TipoTiempo.Dias:
                    hour = hour.AddDays(Convert.ToInt16(add));
                    break;
                case TipoTiempo.Horas:
                    hour = hour.AddHours(Convert.ToDouble(add));
                    break;
                case TipoTiempo.Minutos:
                    hour = hour.AddMinutes(Convert.ToDouble(add));
                    break;
                case TipoTiempo.Segundos:
                    hour = hour.AddSeconds(Convert.ToDouble(add));
                    break;
                case TipoTiempo.Milisegundos:
                    hour = hour.AddMilliseconds(Convert.ToDouble(add));
                    break;
                case TipoTiempo.Ticks:
                    hour = hour.AddTicks((long)add);
                    break;
            }

            retorno = hour.ToString(format);

            return retorno;
        }

        /// <summary>
        /// Get date/time to sender
        /// </summary>
        /// <param name="fechahora">fecha u hora deseada para cambiar</param>
        /// <param name="format">Hmm=2330, ddddd=Monday</param>
        /// <param name="add">Add or remove time</param>
        /// <param name="tipo">Type of time to add or remove</param>
        /// <returns></returns>
        public string getDateTime(DateTime fechahora, string format, decimal add, TipoTiempo tipo)
        {
            string retorno = string.Empty;

            switch(tipo)
            {
                case TipoTiempo.Years:
                    fechahora = fechahora.AddYears(Convert.ToInt16(add));
                    break;
                case TipoTiempo.Meses:
                    fechahora = fechahora.AddMonths(Convert.ToInt16(add));
                    break;
                case TipoTiempo.Dias:
                    fechahora = fechahora.AddDays(Convert.ToInt16(add));
                    break;
                case TipoTiempo.Horas:
                    fechahora = fechahora.AddHours(Convert.ToDouble(add));
                    break;
                case TipoTiempo.Minutos:
                    fechahora = fechahora.AddMinutes(Convert.ToDouble(add));
                    break;
                case TipoTiempo.Segundos:
                    fechahora = fechahora.AddSeconds(Convert.ToDouble(add));
                    break;
                case TipoTiempo.Milisegundos:
                    fechahora = fechahora.AddMilliseconds(Convert.ToDouble(add));
                    break;
                case TipoTiempo.Ticks:
                    fechahora = fechahora.AddTicks((long)add);
                    break;
            }

            retorno = fechahora.ToString(format);

            return retorno;
        }


        /// <summary>
        /// Get the first day in number of the selected day. Week start in Sunday. 
        /// </summary>
        /// <param name="sel">Selected day.</param>
        /// <param name="month">Selected month</param>
        /// <param name="year">Selected year</param>
        /// <param name="addSemana">Number of weeks you want to add, for get the seccond Monday for example.</param>
        /// <returns></returns>
        public int primerDia(DayOfWeek sel, byte month, int year, byte addSemana = 0)
        {
            DateTime fecha;

            int dia = 0;
            int c = 1;

            do
            {
                fecha = new DateTime(year, month, c);

                if(fecha.DayOfWeek == sel)
                    dia = c;
                else
                    c++;

            } while(dia == 0);

            return dia;
        }

        /// <summary>
        /// Parse a date/time string.
        /// 
        /// If the relative time includes hours, minutes or seconds, it's relative to now,
        /// else it's relative to today.
        /// </summary>
        /// <param name="input">
        ///  - "-1 day": Yesterday
        ///  - "+12 weeks": Today twelve weeks later
        ///  - "1 seconds": One second later from now.
        ///  - "5 days 1 hour ago"
        ///  - "1 year 2 months 3 weeks 4 days 5 hours 6 minutes 7 seconds"
        ///  - "today": This day at midnight.
        ///  - "now": Right now (date and time).
        ///  - "next week"
        ///  - "last month"
        ///  - "2010-12-31"
        ///  - "01/01/2010 1:59 PM"
        ///  - "23:59:58": Today at the given time.
        /// </param>
        public static DateTime str2time(string input)
        {
            RelativeDateParser r = new RelativeDateParser();
            DateTime retorno;
            retorno = r.Parse(input);
            r = null;

            return retorno;
        }

        /// <summary>
        /// Parse a date/time string.
        /// 
        /// Can handle relative English-written date times like:
        ///  - "-1 day": Yesterday
        ///  - "+12 weeks": Today twelve weeks later
        ///  - "1 seconds": One second later from now.
        ///  - "5 days 1 hour ago"
        ///  - "1 year 2 months 3 weeks 4 days 5 hours 6 minutes 7 seconds"
        ///  - "today": This day at midnight.
        ///  - "now": Right now (date and time).
        ///  - "next week"
        ///  - "last month"
        ///  - "2010-12-31"
        ///  - "01/01/2010 1:59 PM"
        ///  - "23:59:58": Today at the given time.
        /// 
        /// If the relative time includes hours, minutes or seconds, it's relative to now,
        /// else it's relative to today.
        /// </summary>
        internal class RelativeDateParser
        {
            private const string ValidUnits = "year|month|week|day|hour|minute|second";

            /// <summary>
            /// Ex: "last year"
            /// </summary>
            private readonly Regex _basicRelativeRegex = new Regex(@"^(last|next) +(" + ValidUnits + ")$");

            /// <summary>
            /// Ex: "+1 week"
            /// Ex: " 1week"
            /// </summary>
            private readonly Regex _simpleRelativeRegex = new Regex(@"^([+-]?\d+) *(" + ValidUnits + ")s?$");

            /// <summary>
            /// Ex: "2 minutes"
            /// Ex: "3 months 5 days 1 hour ago"
            /// </summary>
            private readonly Regex _completeRelativeRegex = new Regex(@"^(?: *(\d) *(" + ValidUnits + ")s?)+( +ago)?$");

            public DateTime Parse(string input)
            {
                // Remove the case and trim spaces.
                input = input.Trim().ToLower();

                // Try common simple words like "yesterday".
                var result = TryParseCommonDateTime(input);
                if(result.HasValue)
                    return result.Value;

                // Try common simple words like "last week".
                result = TryParseLastOrNextCommonDateTime(input);
                if(result.HasValue)
                    return result.Value;

                // Try simple format like "+1 week".
                result = TryParseSimpleRelativeDateTime(input);
                if(result.HasValue)
                    return result.Value;

                // Try first the full format like "1 day 2 hours 10 minutes ago".
                result = TryParseCompleteRelativeDateTime(input);
                if(result.HasValue)
                    return result.Value;

                // Try parse fixed dates like "01/01/2000". or not english
                return ConvertToDateTime(input);  //DateTime.Parse(input);
            }

            private static DateTime? TryParseCommonDateTime(string input)
            {
                switch(input)
                {
                    case "now":
                        return DateTime.Now;
                    case "today":
                        return DateTime.Today;
                    case "tomorrow":
                        return DateTime.Today.AddDays(1);
                    case "yesterday":
                        return DateTime.Today.AddDays(-1);
                    default:
                        return null;
                }
            }

            private DateTime? TryParseLastOrNextCommonDateTime(string input)
            {
                var match = _basicRelativeRegex.Match(input);
                if(!match.Success)
                    return null;

                var unit = match.Groups[2].Value;
                var sign = string.Compare(match.Groups[1].Value, "next", StringComparison.OrdinalIgnoreCase) == 0 ? 1 : -1;
                return AddOffset(unit, sign);
            }

            private DateTime? TryParseSimpleRelativeDateTime(string input)
            {
                var match = _simpleRelativeRegex.Match(input);
                if(!match.Success)
                    return null;

                var delta = Convert.ToInt32(match.Groups[1].Value);
                var unit = match.Groups[2].Value;
                return AddOffset(unit, delta);
            }

            private DateTime? TryParseCompleteRelativeDateTime(string input)
            {
                var match = _completeRelativeRegex.Match(input);
                if(!match.Success)
                    return null;

                var values = match.Groups[1].Captures;
                var units = match.Groups[2].Captures;
                var sign = match.Groups[3].Success ? -1 : 1;
                System.Diagnostics.Debug.Assert(values.Count == units.Count);

                var dateTime = UnitIncludeTime(units) ? DateTime.Now : DateTime.Today;

                for(int i = 0; i < values.Count; ++i)
                {
                    var value = sign * Convert.ToInt32(values[i].Value);
                    var unit = units[i].Value;

                    dateTime = AddOffset(unit, value, dateTime);
                }

                return dateTime;
            }

            /// <summary>
            /// Add/Remove years/days/hours... to a datetime.
            /// </summary>
            /// <param name="unit">Must be one of ValidUnits</param>
            /// <param name="value">Value in given unit to add to the datetime</param>
            /// <param name="dateTime">Relative datetime</param>
            /// <returns>Relative datetime</returns>
            private static DateTime AddOffset(string unit, int value, DateTime dateTime)
            {
                switch(unit)
                {
                    case "year":
                        return dateTime.AddYears(value);
                    case "month":
                        return dateTime.AddMonths(value);
                    case "week":
                        return dateTime.AddDays(value * 7);
                    case "day":
                        return dateTime.AddDays(value);
                    case "hour":
                        return dateTime.AddHours(value);
                    case "minute":
                        return dateTime.AddMinutes(value);
                    case "second":
                        return dateTime.AddSeconds(value);
                    default:
                        throw new Exception("Internal error: Unhandled relative date/time case.");
                }
            }

            /// <summary>
            /// Add/Remove years/days/hours... relative to today or now.
            /// </summary>
            /// <param name="unit">Must be one of ValidUnits</param>
            /// <param name="value">Value in given unit to add to the datetime</param>
            /// <returns>Relative datetime</returns>
            private static DateTime AddOffset(string unit, int value)
            {
                var now = UnitIncludesTime(unit) ? DateTime.Now : DateTime.Today;
                return AddOffset(unit, value, now);
            }

            private static bool UnitIncludeTime(CaptureCollection units)
            {
                foreach(Capture unit in units)
                    if(UnitIncludesTime(unit.Value))
                        return true;
                return false;
            }

            private static bool UnitIncludesTime(string unit)
            {
                switch(unit)
                {
                    case "hour":
                    case "minute":
                    case "second":
                        return true;

                    default:
                        return false;
                }
            }

        }
    }
}