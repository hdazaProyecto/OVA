using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Proyecto.Funciones;

namespace Proyecto.Funciones
{
    public class EnvioCorreo
    {
        public string smtp_User = "";
        public string smtp_Pass = "";
        public int smtp_Port = 0;
        public string smtp_Server = "";
        public string smtp_From = "";
        public string smtp_FromAlias = "";
        public bool smtp_Ssl = false;

        private Conexion conexion;
        private SqlConnectionStringBuilder con;
        private List<SqlParameter> parametros;
        private MailMessage agMail = new MailMessage();

        /// <summary>
        /// Método que permite intanciar la clase "contructor".
        /// </summary>
        /// <returns></returns>
        public EnvioCorreo(string agSubject, string agBody)
        {
            try
            {
                agMail.Subject = agSubject;
                agMail.SubjectEncoding = System.Text.Encoding.UTF8;
                agMail.Body = agBody;
                agMail.BodyEncoding = System.Text.Encoding.UTF8;
                agMail.IsBodyHtml = true;
            }
            catch
            { }
        }

        /// <summary>
        /// Método que peromite hacer el elvio de correo de la plataforma.
        /// </summary>
        /// <returns>Retorna tur o false si sepudo enviar o no el correo electronico</returns>
        public bool envio(out string Error, string email,bool CCOculto = false)
        {
            bool envio = false;
            Error = null;
            try
            {
                Initialize();

                using (SmtpClient client = new SmtpClient(this.smtp_Server, this.smtp_Port))
                {
                    agMail.IsBodyHtml = true;
                    agMail.From = new MailAddress(this.smtp_From, this.smtp_FromAlias ?? this.smtp_From);
                    agMail.To.Add(email);
                    agMail.Priority = MailPriority.High;
                    agMail.Headers.Add("Read-Receipt-To", this.smtp_From);
                    agMail.Headers.Add("Disposition-Notification-To", this.smtp_From);
                    agMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.Delay;
                    client.Port = this.smtp_Port;
                    client.Credentials = new NetworkCredential(this.smtp_User, this.smtp_Pass);
                    client.EnableSsl = this.smtp_Ssl;

                    if (CCOculto)
                        agMail.Bcc.Add(agMail.From);

                    #region para implementar si mas adelante se desea enviar adjuntos en los correos
                    //if (ArchivosAdjuntos != null)
                    //{
                    //    ArchivosAdjuntos.AddRange(this.Archivos);

                    //    foreach (Archivo item in ArchivosAdjuntos)
                    //    {
                    //        Attachment archAdj = null;

                    //        if (String.IsNullOrEmpty(item.Ruta))
                    //            archAdj = new Attachment(item.Contenido, item.Nombre);
                    //        else
                    //            archAdj = new Attachment(item.Ruta);

                    //        archAdj.ContentId = archAdj.Name;

                    //        ContentDisposition disposition = archAdj.ContentDisposition;
                    //        disposition.CreationDate = DateTime.Now;
                    //        disposition.ModificationDate = DateTime.Now;
                    //        disposition.ReadDate = DateTime.Now;
                    //        disposition.FileName = item.Nombre;
                    //        disposition.Size = archAdj.ContentDisposition.Size;
                    //        disposition.DispositionType = DispositionTypeNames.Attachment;

                    //        agMail.Attachments.Add(archAdj);
                    //    }
                    //}
                    #endregion

                    ServicePointManager.MaxServicePointIdleTime = 1000;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.Send(agMail);
                    client.Dispose();
                    agMail.Attachments.Dispose();
                    envio = true;
                }
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
                envio = false;
            }

            return envio;
        }

        /// <summary>
        /// Método que peromite nicializar valores de correo configurado en el base de datos.
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool cargado = false;
            try
            {
                DataSet dconfiguracion;
                DataTable dtconfiguracion;
                conexion = new Conexion();
                con = new SqlConnectionStringBuilder();
                con = conexion.ConexionSQLServer();
                ConSqlServer server = new ConSqlServer(con);
                parametros = new List<SqlParameter>();
                server.ejecutarQuery(@"SELECT top 1 nombre, servidor, usuario, clave, puerto, ssl FROM configEmail", parametros, out dconfiguracion);
                server.close();

                if (dconfiguracion != null && dconfiguracion.Tables[0].Rows.Count > 0)
                {
                    dtconfiguracion = new DataTable();
                    dtconfiguracion = dconfiguracion.Tables[0];
                    if (dtconfiguracion.Rows.Count > 0)
                    {
                        this.smtp_From = Funcion.ValidateEmail(dtconfiguracion.Rows[0].Field<string>("usuario")).FirstOrDefault().Key;
                        this.smtp_FromAlias = dtconfiguracion.Rows[0].Field<string>("nombre");
                        this.smtp_Pass = Funcion.base64String(dtconfiguracion.Rows[0].Field<string>("clave"));
                        this.smtp_Port = dtconfiguracion.Rows[0].Field<int>("puerto");
                        this.smtp_Server = dtconfiguracion.Rows[0].Field<string>("servidor");
                        this.smtp_Ssl = dtconfiguracion.Rows[0].Field<bool>("ssl");
                        this.smtp_User = dtconfiguracion.Rows[0].Field<string>("usuario");

                        cargado = true;
                    }
                }
            }
            catch
            {

            }
            return cargado;
        }
    }
}