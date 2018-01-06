using System;
using Quartz;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace Confere.Processos.Monitor
{
    internal class EnviarEmailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            EnviarEmail("marcinhagarciarj@gmail.com", "Teste", "Testando o envio de email");
        }

        private void EnviarEmail(string toEmail, string assuntoMensagem, string corpoMensagem)
        {
            Trace.TraceInformation($"Email para: {toEmail}, assunto: {assuntoMensagem}, corpo: {corpoMensagem}");
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("marcinhagarciarj@gmail.com", "sgdixptpycnaqrmk")
            };

            try
            {
                // Create instance of message
                MailMessage message = new MailMessage();

                // Add receiver
                message.To.Add(toEmail);

                // Sender
                message.From = new MailAddress("marcinhagarciarj@gmail.com");

                // Subject
                message.Subject = assuntoMensagem;

                // Body
                message.Body = corpoMensagem;

                // Send the message
                client.Send(message);

                // Clean up
                message = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not send e-mail. Exception caught: " + e);
            }
        }
    }
}