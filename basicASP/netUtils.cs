using drualcman.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

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
        public class netUtils
        {
            #region variables
            private HttpContext Context;
            private IMailReadUri MailUri;
            #endregion

            #region Constructor
            public netUtils() : this(null) { }
            public netUtils(HttpContext context) : this(context, null) { }
            public netUtils(HttpContext context, IMailReadUri mailUri)
            {
                this.Context = context;
                MailUri = mailUri;
            }
            #endregion

            #region url data
            /// <summary>
            /// Descargar un archivo desde internet
            /// </summary>
            /// <param name="url">Url del archivo</param>
            /// <returns></returns>
            public static byte[] GetFileFromUrl(string url)
            {
                using HttpClient client = new HttpClient();
                byte[] result = client.GetByteArrayAsync(url).Result;
                return result;
            }

            /// <summary>
            /// Obtener la respuesta de una url
            /// </summary>
            /// <param name="url">Url del archivo</param>
            /// <returns></returns>
            public static string GetStringFromUrl(string url)
            {

                using HttpClient client = new HttpClient();
                string result = client.GetStringAsync(url).Result;
                return result;
            }
            #endregion

            #region server
            /// <summary>
            /// Comprobar si una url no esta rota
            /// </summary>
            /// <param name="url">url a comprobar</param>
            /// <returns></returns>
            public static bool IsLinkWorking(string url)
            {
                return IsLinkWorking(url, 2000);
            }

            /// <summary>
            /// Comprobar si una url no esta rota
            /// </summary>
            /// <param name="url">url a comprobar</param>
            /// <param name="timeout">tiempo de espera maximo en milisegundos</param>
            /// <returns></returns>
            public static bool IsLinkWorking(string url, int timeout)
            {
                try
                {
                    bool retorno;
                    using HttpClient client = new HttpClient();
                    client.Timeout = new TimeSpan(timeout);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    switch(response.StatusCode)
                    {
                        case HttpStatusCode.NonAuthoritativeInformation:
                        case HttpStatusCode.NoContent:
                        case HttpStatusCode.ResetContent:
                        case HttpStatusCode.PartialContent:
                        case HttpStatusCode.MultipleChoices:
                        case HttpStatusCode.MovedPermanently:
                        case HttpStatusCode.Found:
                        case HttpStatusCode.SeeOther:
                        case HttpStatusCode.NotModified:
                        case HttpStatusCode.UseProxy:
                        case HttpStatusCode.Unused:
                        case HttpStatusCode.TemporaryRedirect:
                        case HttpStatusCode.BadRequest:
                        case HttpStatusCode.Unauthorized:
                        case HttpStatusCode.PaymentRequired:
                        case HttpStatusCode.Forbidden:
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.MethodNotAllowed:
                        case HttpStatusCode.NotAcceptable:
                        case HttpStatusCode.ProxyAuthenticationRequired:
                        case HttpStatusCode.RequestTimeout:
                        case HttpStatusCode.Conflict:
                        case HttpStatusCode.Gone:
                        case HttpStatusCode.LengthRequired:
                        case HttpStatusCode.PreconditionFailed:
                        case HttpStatusCode.RequestEntityTooLarge:
                        case HttpStatusCode.RequestUriTooLong:
                        case HttpStatusCode.UnsupportedMediaType:
                        case HttpStatusCode.RequestedRangeNotSatisfiable:
                        case HttpStatusCode.ExpectationFailed:
                        case HttpStatusCode.UpgradeRequired:
                        case HttpStatusCode.InternalServerError:
                        case HttpStatusCode.NotImplemented:
                        case HttpStatusCode.BadGateway:
                        case HttpStatusCode.ServiceUnavailable:
                        case HttpStatusCode.GatewayTimeout:
                        case HttpStatusCode.HttpVersionNotSupported:
                            retorno = false;
                            break;
                        case HttpStatusCode.Continue:
                        case HttpStatusCode.SwitchingProtocols:
                        case HttpStatusCode.OK:
                        case HttpStatusCode.Created:
                        case HttpStatusCode.Accepted:
                            retorno = true;
                            break;
                        default:
                            retorno = false;
                            break;
                    }
                    return retorno;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// Conocer la IP real del cliente
            /// </summary>
            /// <returns></returns>
            public string clientIP()
            {
                string ClientIP = string.Empty;
                string Forwaded = string.Empty;
                string RealIP = string.Empty;

                RealIP = "";

                try
                {
                    ClientIP = this.Context.Request.Headers["HTTP_CLIENT-IP"];
                }
                catch
                {
                    ClientIP = string.Empty;
                }

                if(!string.IsNullOrEmpty(ClientIP))
                {
                    RealIP = ClientIP;
                }
                else
                {
                    try
                    {
                        Forwaded = this.Context.Request.Headers["HTTP_X_FORWARDED_FOR"];
                    }
                    catch
                    {
                        Forwaded = string.Empty;
                    }

                    if(!string.IsNullOrEmpty(Forwaded))
                    {
                        RealIP = Forwaded;
                    }
                    else
                    {
                        try
                        {
                            RealIP = this.Context.Request.Headers["REMOTE_ADDR"];
                        }
                        catch
                        {
                            RealIP = string.Empty;
                        }
                    }
                }

                return RealIP;
            }

            /// <summary>
            /// Obtiene la web que se esta utilizando
            /// </summary>
            /// <returns>IdEmpresa como string</returns>
            public string knowServerURL()
            {
                string serverURL;
                if(this.Context is not null)
                {
                    try
                    {
                        serverURL = this.Context.Request.Scheme + "://" + this.Context.Request.Host.Value;
                    }
                    catch
                    {
                        serverURL = string.Empty;
                    }
                }
                else
                    serverURL = string.Empty;

                return serverURL;

            }

            /// <summary>
            /// Obtiene la web que se esta utilizando
            /// </summary>
            /// <param name="soloNombre">No pone el http://</param>
            /// <returns>IdEmpresa como string</returns>
            public string knowServerURL(bool soloNombre)
            {
                string serverURL;

                try
                {
                    string path = this.Context.Request.PathBase.Value;
                    if(path.Length > 1) serverURL = this.Context.Request.Host.Host + path;
                    else serverURL = this.Context.Request.Host.Host;
                }
                catch
                {
                    serverURL = string.Empty;
                }

                if(soloNombre == false)
                {
                    return this.Context.Request.Scheme + "://" + serverURL;
                }
                else
                    return serverURL;
            }

            /// <summary>
            /// Obtiene la web que se esta utilizando
            /// </summary>
            /// <param name="seguro">Indica si es un servidor seguro</param>
            /// <param name="soloNombre">No pone el https://</param>
            /// <returns>IdEmpresa como string</returns>
            public string knowServerURL(bool seguro, bool soloNombre)
            {
                string serverURL;

                try
                {
                    serverURL = this.Context.Request.Host.Host;
                }
                catch
                {
                    serverURL = string.Empty;
                }

                if(seguro == false)
                {
                    if(soloNombre == false)
                    {
                        serverURL = this.Context.Request.Scheme + "://" + serverURL;
                    }
                }
                else
                {
                    if(soloNombre == false)
                    {
                        serverURL = "https://" + serverURL;
                    }
                }
                return serverURL;
            }
            #endregion

            #region utils
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
            /// <param name="IsBodyHtml">Indica si el texto tiene formato HTML</param>
            /// <param name="filename">Nombre del archivo a enviar</param>
            /// <param name="folder">Carpeta para almacenar el archivo. Adminte ; para separar nombres de archivo</param>
            /// <param name="temp">Indicar si el archiv es temporal o debe ser borrado</param>
            /// <param name="enableSsl">Habilitar la seguridad del servidor</param>
            /// <returns>
            /// Devuelve true si el envío ha sido satisfactorio
            /// </returns>
            public bool EnviarMail(string eMail, string Asunto, string cuerpoTexto,
                string empresaRemitente, string mailRemitente, string hostSMTP, string userName,
                string userPass, int numDestinatarios = 10, bool IsBodyHtml = false,
                string filename = "", string folder = "~/", bool temp = false, bool enableSsl = false)
            {
                bool bResutado = EnviarMail(eMail, Asunto, cuerpoTexto, empresaRemitente, mailRemitente, hostSMTP,
                    userName, userPass, "/", numDestinatarios, IsBodyHtml, filename, folder, temp, enableSsl);

                return bResutado;
            }

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
            /// <param name="notUse">Obsolete, nohting to do</param>
            /// <param name="userPass">Password de la cuenta</param>
            /// <param name="numDestinatarios">Numero de destinatarios en cada envio. Defecto 50</param>
            /// <param name="IsBodyHtml">Indica si el texto tiene formato HTML</param>
            /// <param name="filename">Nombre del archivo a enviar. Adminte ; para separar nombres de archivo</param>
            /// <param name="folder">Carpeta para almacenar el archivo.</param>
            /// <param name="temp">Indicar si el archiv es temporal o debe ser borrado</param>
            /// <param name="enableSsl">Habilitar la seguridad del servidor</param>
            /// <returns>
            /// Devuelve true si el envío ha sido satisfactorio
            /// </returns>
            public bool EnviarMail(string eMail, string Asunto, string cuerpoTexto,
                string empresaRemitente, string mailRemitente, string hostSMTP, string userName,
                string userPass, string notUse, int numDestinatarios = 10, bool IsBodyHtml = false,
                string filename = "", string folder = "mails", bool temp = false, bool enableSsl = false)
            {
                bool bResutado = true;
                // preparar el correo en fotmato HTML   
                if(eMail != "")
                {
                    // ENVÍO DEL FORMULARIO DE CONTACTO
                    // variables para la gestión del correo
                    MailMessage correo = new MailMessage();
                    SmtpClient smtp = new SmtpClient(hostSMTP);
                    // identificación de usuario
                    NetworkCredential userCredentials = new NetworkCredential(userName, userPass);
                    smtp.EnableSsl = enableSsl;
                    smtp.Credentials = userCredentials;
                    // agregar remitente
                    MailAddress emailRemitente;
                    try
                    {
                        emailRemitente = new MailAddress(mailRemitente, empresaRemitente);
                    }
                    catch
                    {
                        emailRemitente = new MailAddress("info@community-mall.com", "DrUalcman API");
                    }
                    correo.From = emailRemitente;
                    correo.ReplyToList.Add(emailRemitente);
                    // agregar el asunto
                    correo.Subject = Asunto;
                    // propiedades del mail
                    correo.Priority = MailPriority.Normal;
                    correo.IsBodyHtml = IsBodyHtml;


                    string fileHTML = string.Empty;
                    //guardar una copia del correo para poner un enlace a la copia HTML del mismo
                    try
                    {
                        if(MailUri is not null) fileHTML = MailUri.GetReadUri(cuerpoTexto);
                        else fileHTML = string.Empty;
                    }
                    catch
                    {
                        fileHTML = string.Empty;
                    }

                    cuerpoTexto += Environment.NewLine;

                    //comprobar que no tiene un fichero adjunto
                    if(filename != "")
                    {
                        //adjuntar el archivo fisicamente
                        archivos a = new archivos();
                        folder = a.checkCarpeta(folder);
                        ficheros f = new ficheros();

                        //comprobar que no es una lista de archivo
                        string fichero = string.Empty;
                        if(filename.IndexOf(";") > 0)
                        {
                            //es una lista de archivos
                            string[] files = filename.Split(';');
                            foreach(string file in files)
                            {
                                if(f.existeFichero(file.Trim(), folder))
                                {
                                    // solo es un archivo                                  
                                    fichero = Path.Combine(folder, file.Trim());
                                    correo.Attachments.Add(new Attachment(a.GetStreamFile(fichero), System.IO.Path.GetFileName(fichero)));
                                    //
                                    // se elimina el archivo porque no estara bloqueado y era un archivo temporal
                                    //
                                    if(temp == true) f.borrarArchivo(file.Trim(), folder);
                                }
                                else
                                {
                                    if(IsUrl(file.Trim()))
                                        cuerpoTexto += basicHTML.a(file.Trim(), file.Trim()) + Environment.NewLine;
                                }
                            }
                        }
                        else
                        {
                            if(f.existeFichero(filename.Trim(), folder))
                            {
                                // solo es un archivo
                                fichero = Path.Combine(folder, filename.Trim());
                                correo.Attachments.Add(new Attachment(a.GetStreamFile(fichero), System.IO.Path.GetFileName(fichero)));
                                //
                                // se elimina el archivo porque no estara bloqueado y era un archivo temporal
                                //
                                if(temp == true) f.borrarArchivo(filename.Trim(), folder);
                            }
                            else
                            {
                                if(IsUrl(filename.Trim()))
                                    cuerpoTexto += basicHTML.a(filename.Trim(), filename.Trim()) + Environment.NewLine;
                            }
                        }
                        a = null;
                    }


                    cuerpoTexto += Environment.NewLine;

                    if(!string.IsNullOrEmpty(fileHTML))
                    {
                        if(IsBodyHtml == true)
                        {
                            //insert link
                            cuerpoTexto += basicHTML.a(fileHTML, "If you cannot read the message well click here to read it online", "_blank");
                        }
                        else
                        {
                            //inser texto where is it the file
                            cuerpoTexto += " If you cannot read the message well click here " + fileHTML + " to read it online (copy the link in your Internet browser)";
                        }
                    }

                    // cuerpo del mail
                    correo.Body = cuerpoTexto;

                    //hacer el envio a todas las direcciones encontradas
                    if(eMail.IndexOf(";") > 0)
                    {
                        // extraer las direcciones
                        string[] Direcciones = eMail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        bool seguimiento;
                        if(eMail.IndexOf("invite.trustpilot.com") < 0) seguimiento = false;
                        else seguimiento = true;

                        byte s = 1;
                        bool enviado = false;

                        // recorrer las direcciones para realizar el envio
                        foreach(string item in Direcciones)
                        {
                            if(item != "")
                            {
                                //comprobar que tiene @
                                if(item.IndexOf("@") > 0)
                                {
                                    MailAddress nuevoCorreo = new MailAddress(item);
                                    if(seguimiento)
                                    {
                                        if(s < 2) correo.To.Add(nuevoCorreo);
                                        else correo.Bcc.Add(nuevoCorreo);
                                    }
                                    else correo.Bcc.Add(nuevoCorreo);
                                }

                                if(s >= numDestinatarios)
                                {
                                    try
                                    {
                                        smtp.Send(correo);
                                        enviado = true;
                                    }
                                    catch(Exception ex)
                                    {
                                        string err = ex.ToString();
                                        enviado = false;
                                        bResutado = false;
                                    }
                                    finally
                                    {
                                        correo.To.Clear();
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
                        if(enviado == false)
                        {
                            try
                            {
                                smtp.Send(correo);
                            }
                            catch(Exception ex)
                            {
                                string err = ex.ToString();
                                //string datos = hostSMTP + ";" + userName + ";" + userPass + enableSsl.ToString();
                                //DrSeguridad.reportError(ex,err,datos);

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
                        catch(Exception ex)
                        {
                            string err = ex.ToString();
                            //string datos = hostSMTP + ";" + userName + ";" + userPass + enableSsl.ToString();
                            //DrSeguridad.reportError(ex, err, datos);

                            bResutado = false;
                        }
                    }
                    //string datos1 = hostSMTP + ";" + userName + ";" + userPass + enableSsl.ToString();
                    //DrSeguridad.reportError(new Exception("nada"), "error mio",datos1);

                    correo.Dispose();
                    smtp.Dispose();
                }

                return bResutado;
            }

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
            /// <param name="filesname">Nombre de los archivos a adjuntar</param>
            /// <param name="numDestinatarios">Numero de destinatarios en cada envio. Defecto 50</param>
            /// <param name="IsBodyHtml">Indica si el texto tiene formato HTML</param>
            /// <param name="folder">Carpeta para almacenar el archivo</param>
            /// <param name="temp">Indicar si el archiv es temporal o debe ser borrado</param>
            /// <param name="enableSsl">Habilitar la seguridad del servidor</param>
            /// <returns>
            /// Devuelve true si el envío ha sido satisfactorio
            /// </returns>
            public bool EnviarMail(string eMail, string Asunto, string cuerpoTexto,
                string empresaRemitente, string mailRemitente, string hostSMTP, string userName,
                string userPass, string[] filesname, int numDestinatarios = 10, bool IsBodyHtml = false,
                string folder = "~/", bool temp = false, bool enableSsl = false)
            {
                string filename = string.Empty;

                foreach(string item in filesname)
                {
                    filename += item + "; ";
                }
                if(string.IsNullOrEmpty(filename)) filename = filename.TrimEnd().Remove(filename.Length - 2, 1);

                return EnviarMail(eMail, Asunto, cuerpoTexto, empresaRemitente, mailRemitente, hostSMTP, userName,
                                  userPass, numDestinatarios, IsBodyHtml, filename, folder, temp, enableSsl);
            }

            /// <summary>
            /// Función genérica de envío de correo electrónico devuelve el error producido ostring.empty si todo ok
            /// </summary>
            /// <param name="eMail">Mail del destinatario. Se admiten varias direccion separadas por ; La ultima tiene que tener ; para saber que esta formateado</param>
            /// <param name="Asunto">Asunto del envio del correo</param>    
            /// <param name="cuerpoTexto">Texto que aparecera en el mensaje. Se admite HTML</param>
            /// <param name="empresaRemitente">Nombre de la empresa o persona que envia el mail</param>
            /// <param name="mailRemitente">Mail del remitente (opcional)</param>
            /// <param name="hostSMTP">Servidor de envio</param>
            /// <param name="userName">Nombre de usuario</param>
            /// <param name="userPass">Password de la cuenta</param>
            /// <param name="enableSsl">Habilitar la seguridad del servidor</param>
            /// <returns>
            /// String empty if it's success. If not return the error message
            /// </returns>
            public string EnviarMail(string eMail, string Asunto, string cuerpoTexto,
                string empresaRemitente, string mailRemitente, string hostSMTP, string userName,
                string userPass, bool IsBodyHtml, bool enableSsl = false)
            {
                string err = string.Empty;
                // preparar el correo en fotmato HTML   
                if(eMail != "")
                {
                    // ENVÍO DEL FORMULARIO DE CONTACTO
                    // variables para la gestión del correo
                    MailMessage correo = new MailMessage();
                    SmtpClient smtp = new SmtpClient(hostSMTP);
                    // identificación de usuario
                    if(enableSsl) smtp.Port = 587;
                    smtp.EnableSsl = enableSsl;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.UseDefaultCredentials = false;
                    NetworkCredential userCredentials = new NetworkCredential(userName, userPass);
                    smtp.Credentials = userCredentials;
                    // agregar remitente
                    MailAddress emailRemitente;
                    try
                    {
                        emailRemitente = new MailAddress(mailRemitente, empresaRemitente);
                    }
                    catch
                    {
                        emailRemitente = new MailAddress("info@community-mall.com", "DrUalcman API");
                    }
                    correo.From = emailRemitente;
                    correo.ReplyToList.Add(emailRemitente);
                    // agregar el asunto
                    correo.Subject = Asunto;
                    // propiedades del mail
                    correo.Priority = MailPriority.Normal;
                    correo.IsBodyHtml = IsBodyHtml;


                    string fileHTML;
                    //guardar una copia del correo para poner un enlace a la copia HTML del mismo
                    try
                    {
                        fileHTML = MailUri.GetReadUri(cuerpoTexto);
                    }
                    catch
                    {
                        fileHTML = string.Empty;
                    }

                    cuerpoTexto += Environment.NewLine;

                    if(!string.IsNullOrEmpty(fileHTML))
                    {
                        if(IsBodyHtml == true)
                        {
                            //insert link
                            cuerpoTexto += basicHTML.a(fileHTML, "If you cannot read the message well click here to read it online", "_blank");
                        }
                        else
                        {
                            //inser texto where is it the file
                            cuerpoTexto += " If you cannot read the message well click here " + fileHTML + " to read it online (copy the link in your Internet browser)";
                        }
                    }

                    // cuerpo del mail
                    correo.Body = cuerpoTexto;

                    //hacer el envio a todas las direcciones encontradas
                    if(eMail.IndexOf(";") > 0)
                    {
                        // extraer las direcciones
                        string[] Direcciones = eMail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        bool seguimiento;
                        if(eMail.IndexOf("invite.trustpilot.com") < 0) seguimiento = false;
                        else seguimiento = true;

                        byte s = 1;
                        bool enviado = false;
                        int numDestinatarios = 50;
                        // recorrer las direcciones para realizar el envio
                        foreach(string item in Direcciones)
                        {
                            if(item != "")
                            {
                                //comprobar que tiene @
                                if(item.IndexOf("@") > 0)
                                {
                                    MailAddress nuevoCorreo = new MailAddress(item);
                                    if(seguimiento)
                                    {
                                        if(s < 2) correo.To.Add(nuevoCorreo);
                                        else correo.Bcc.Add(nuevoCorreo);
                                    }
                                    else correo.Bcc.Add(nuevoCorreo);
                                }

                                if(s >= numDestinatarios)
                                {
                                    try
                                    {
                                        smtp.Send(correo);
                                        enviado = true;
                                    }
                                    catch(Exception ex)
                                    {
                                        err += "1: " + ex.ToString();
                                        enviado = false;
                                    }
                                    finally
                                    {
                                        correo.To.Clear();
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
                        if(enviado == false)
                        {
                            try
                            {
                                smtp.Send(correo);
                            }
                            catch(Exception ex)
                            {
                                err += "2: " + ex.ToString();
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
                        catch(Exception ex)
                        {
                            err = "3: " + ex.ToString();
                        }
                    }

                    correo.Dispose();
                    smtp.Dispose();
                }

                return err;
            }
            #endregion

            private bool IsUrl(string fileName)
            {
                bool result;
                result = fileName.Contains("http://");
                if(!result) result = fileName.Contains("https://");
                return result;
            }
        }
    }
}