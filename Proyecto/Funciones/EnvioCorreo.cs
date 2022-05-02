using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Proyecto.Funciones
{
    public class EnvioCorreo
    {
        public bool envio()
        {
            bool envio = false;

            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("hdaza1@gmail.com", "NombreCorreo", System.Text.Encoding.UTF8);//Correo de salida
            correo.To.Add("heimandaza@gmail.com"); //Correo destino?
            correo.Subject = "Correo de prueba"; //Asunto
            correo.Body = "Este es un correo de prueba desde c#"; //Mensaje del correo
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com"; //Host del servidor de correo
            smtp.Port = 25; //Puerto de salida
            smtp.Credentials = new System.Net.NetworkCredential("hdaza1@gmail.com", "heiman7571839");//Cuenta de correo
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtp.EnableSsl = true;//True si el servidor de correo permite ssl
            ServicePointManager.MaxServicePointIdleTime = 1000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            smtp.Send(correo);

            return envio;

            //bool envio = false;
            //Error = null;
            //try
            //{
            //    if (String.IsNullOrEmpty(this.smtp_Server))
            //        if (!Initialize())
            //            throw new Exception("No se cargó la configuración de la cuenta de correo.");

            //    using (SmtpClient client = new SmtpClient(this.smtp_Server, this.smtp_Port))
            //    {
            //        agMail.IsBodyHtml = true;
            //        agMail.From = new MailAddress(this.smtp_From, this.smtp_FromAlias ?? this.smtp_From);
            //        agMail.Priority = MailPriority.High;
            //        agMail.Headers.Add("Read-Receipt-To", this.smtp_From);
            //        agMail.Headers.Add("Disposition-Notification-To", this.smtp_From);
            //        agMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.Delay;
            //        client.Port = this.smtp_Port;
            //        client.Credentials = new System.Net.NetworkCredential(this.smtp_User, this.smtp_Pass);
            //        client.EnableSsl = this.smtp_Ssl;

            //        if (CCOculto)
            //            agMail.Bcc.Add(agMail.From);

            //        if (ArchivosAdjuntos != null)
            //        {
            //            ArchivosAdjuntos.AddRange(this.Archivos);

            //            foreach (Archivo item in ArchivosAdjuntos)
            //            {
            //                Attachment archAdj = null;

            //                if (String.IsNullOrEmpty(item.Ruta))
            //                    archAdj = new Attachment(item.Contenido, item.Nombre);
            //                else
            //                    archAdj = new Attachment(item.Ruta);

            //                archAdj.ContentId = archAdj.Name;

            //                ContentDisposition disposition = archAdj.ContentDisposition;
            //                disposition.CreationDate = DateTime.Now;
            //                disposition.ModificationDate = DateTime.Now;
            //                disposition.ReadDate = DateTime.Now;
            //                disposition.FileName = item.Nombre;
            //                disposition.Size = archAdj.ContentDisposition.Size;
            //                disposition.DispositionType = DispositionTypeNames.Attachment;

            //                agMail.Attachments.Add(archAdj);
            //            }
            //        }
            //        ServicePointManager.MaxServicePointIdleTime = 1000;
            //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //        client.Send(agMail);
            //        client.Dispose();
            //        agMail.Attachments.Dispose();
            //        envio = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Error = ex.ToString();
            //    envio = false;
            //}
            //return envio;
        }
    }
}