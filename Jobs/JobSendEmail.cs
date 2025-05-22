
using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Principal;
using System.Xml.Linq;

namespace Quartz.Jobs
{
    public class JobSendEmail : IJob
    {
        private readonly IConfiguration configuration;

        public JobSendEmail(IConfiguration configuration)
        {
                this.configuration =  configuration;
        }
        public async Task Execute(IJobExecutionContext context)
        {

			try
			{
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(configuration["Email:Name"], configuration["Email:Account"]));
                message.To.Add(address: new MailboxAddress("leandrorafael_24@hotmail.com", "leandrorafael_24@hotmail.com"));
                message.Subject = "Email desde el Job";

                message.Body = new TextPart("plain")
                {
                    Text = "Hola, este correo fue enviado desde un job Quartz usando Hotmail SMTP."
                };

                using var client = new SmtpClient();

                try
                {
                    // Conecta al servidor SMTP de Hotmail
                    await client.ConnectAsync(configuration["Email:Host"], Convert.ToInt32(configuration["Email:Port"]), MailKit.Security.SecureSocketOptions.StartTls);

                    // Autentica con tu cuenta Hotmail
                    await client.AuthenticateAsync(configuration["Email:User"], configuration["Email:Password"]);

                    // Envía el correo
                    await client.SendAsync(message);

                    await client.DisconnectAsync(true);

                    Console.WriteLine("Correo enviado con éxito usando Hotmail SMTP.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error enviando correo: {ex.Message}");
                }

                Console.WriteLine($"[MyJob] Ejecutado: {DateTime.Now}");
               
            }
			catch (Exception ex )
			{

				Console.WriteLine($"Error en el JOB: {ex.Message}");
			}
        }
    }
}
