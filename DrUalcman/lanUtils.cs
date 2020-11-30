using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
/// <summary>
/// Summary description for drLanUtils
/// </summary>
namespace drualcman
{
    /// <summary>
    /// Clase librería de Function útiles para el uso de redes e internet
    /// </summary>
    public partial class lanUtils
    {
        /// <summary>
        /// Función genérica de envío de correo electrónico
        /// </summary>
        /// <param name="eMail">Mail del destinatario. Se admiten varias direccion separadas por ; La ultima tiene que tener ; para saber que esta formateado</param>
        /// <param name="Asunto">Asunto del envio del correo</param>    
        /// <param name="cuerpoTexto">Texto que aparecera en el mensaje. Se admite HTML</param>
        /// <param name="empresaRemitente">Nombre de la empresa o persona que envia el mail</param>
        /// <param name="mailRemitente">Mail del remitente (opcional)</param>
        /// <param name="hostSMTP">Servidor de envio</param>
        /// <param name="userName">Nombre de usuario</param>
        /// <param name="userPass">Password de la cuenta</param>
        /// <param name="numDestinatarios">Numero de destinatarios en cada envio. Defecto 50</param>
        /// <param name="filename">Nombre del archivo a enviar</param>
        /// <param name="folder">Carpeta para almacenar el archivo con letra de unidad (c:\tmp, e:\tmp)</param>
        /// <param name="temp">Indicar si el archiv es temporal o debe ser borrado</param>
        /// <returns>
        /// Devuelve true si el envío ha sido satisfactorio
        /// </returns>
        public bool EnviarMail(string eMail, string Asunto, string cuerpoTexto,
            string empresaRemitente, string mailRemitente, string hostSMTP, string userName,
            string userPass, int numDestinatarios = 50, bool IsBodyHtml = false,
            string filename = "", string folder = "", bool temp = false)
        {
            bool bResutado = true;
            // preparar el correo en fotmato HTML   
            if (eMail != "")
            {
                // ENVÍO DEL FORMULARIO DE CONTACTO
                // variables para la gestión del correo
                MailMessage correo = new MailMessage();
                SmtpClient smtp = new SmtpClient(hostSMTP);
                // identificación de usuario
                NetworkCredential userCredentials = new NetworkCredential(userName, userPass);
                smtp.EnableSsl = true;
                smtp.Credentials = userCredentials;
                // agregar remitente
                MailAddress emailRemitente;
                try
                {
                    emailRemitente = new MailAddress(mailRemitente, empresaRemitente);
                }
                catch
                {
                    emailRemitente = new MailAddress("info@mibiografia.name", empresaRemitente);
                }
                correo.From = emailRemitente;
                correo.ReplyToList.Add(emailRemitente);
                // agregar el asunto
                correo.Subject = Asunto;
                // propiedades del mail
                correo.Priority = MailPriority.Normal;
                correo.IsBodyHtml = IsBodyHtml;
                // cuerpo del mail
                correo.Body = cuerpoTexto;

                //comprobar que no tiene un fichero adjunto
                if (filename != "")
                {
                    //adjuntar el archivo fisicamente
                    filename = folder + filename;
                    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    archivos a = new archivos();
                    if (a.existeFichero(filename))
                    {                        
                        correo.Attachments.Add(new Attachment(a.GetStreamFile(filename), System.IO.Path.GetFileName(filename)));                        
                        //
                        // se elimina el archivo porque no estara bloqueado y y era un archivo temporal
                        //
                        if (temp == true) a.borrarArchivo(filename);
                    }
                    a = null;

                }

                //hacer el envio a todas las direcciones encontradas
                if (eMail.IndexOf(";") > 0)
                {
                    // extraer las direcciones
                    string[] Direcciones = eMail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);


                    byte s = 1;
                    bool enviado = false;

                    // recorrer las direcciones para realizar el envio
                    foreach (string item in Direcciones)
                    {
                        if (item != "")
                        {
                            //comprobar que tiene @
                            if (item.IndexOf("@") > 0)
                            {
                                MailAddress nuevoCorreo = new MailAddress(item);
                                correo.Bcc.Add(nuevoCorreo);
                            }

                            if (s <= numDestinatarios)
                            {
                                try
                                {
                                    smtp.Send(correo);
                                    enviado = true;
                                }
                                catch
                                {
                                    enviado = false;
                                    bResutado = false;
                                }
                                finally
                                {
                                    correo.Bcc.Clear();
                                }
                                s = 0;          // reseteamos para volver a enviar a otro grupo de correos
                            }
                            else
                                enviado = false;
                        }
                        s++;
                    }
                    // enviar al resto de destinatarios
                    if (enviado == false)
                    {
                        try
                        {
                            smtp.Send(correo);
                        }
                        catch
                        {
                            bResutado = false;
                        }
                    }
                }
                else
                {
                    // solo hay un destinatario
                    try
                    {
                        //MailAddress nuevoCorreo = new MailAddress(eMail);                        
                        //correo.To.Add(nuevoCorreo);
                        correo.To.Add(eMail);
                        smtp.Send(correo);
                    }
                    catch
                    {
                        bResutado = false;
                    }
                }
                correo.Dispose();
                smtp.Dispose();
            }

            return bResutado;
        }

        /// <summary>
        /// Función para validar una dirección de correo electrónico
        /// </summary>
        /// <param name="email">Mail a validar</param>
        /// <returns>Devuelve verdadero si es valido, falso si no lo es.</returns>
        public bool validarEmail(string email)
        {
            validar nt = new validar();
            bool resultado = nt.Email(email);
            nt = null;
            return resultado;
        }

        /// <summary>
        /// Comprueba si en el texto hay un tags HTML
        /// </summary>
        /// <param name="text">texto enviado</param>
        /// <returns></returns>
        public static bool ContainsTagHTML(string text)
        {
            string[] tags = ("a|abbr|acronym|address|area|b|base|bdo|big|blockquote|body|br|button|caption|cite|code|col|colgroup|dd|del|dfn|div|dl|DOCTYPE|dt|em|fieldset|form|h1|h2|h3|h4|h5|h6|head|html|hr|i|img|input|ins|kbd|label|legend|li|link|map|meta|noscript|object|ol|optgroup|option|p|param|pre|q|samp|script|select|small|span|strong|style|sub|sup|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|ul|var|header|article|footer").Split('|');
            bool retorno = false;
            string tag = string.Empty;
            string pattern = string.Empty;
            int c = 0;
            do
            {
                tag = tags[c];
                pattern = @"<\s*" + tag + @"\s*\/?>";
                retorno = Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
                c++;
            } while (c < tags.Count() && retorno == false);
            return retorno;
        }

    }

}