using Confere.Processos.Database;
using Confere.Processos.Modelo;
using Confere.Processos.Service;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Confere.Processos.Monitor
{
    /*para cada processo sendo monitorado:
    * - ir na internet e buscar suas info
    * - comparar data da última tramitação da internet com data da última tramitação gravada 
    * - se data tramitação internet for mais recente, guardar processo numa lista
    *
    * se lista de processos atualizados não estiver vazia:
    * > para cada processo: atualizar data de última tramitação
    * 
    * > agrupar por interessado; para cada interessado:
    * - enviar email com lista de processos atualizados
    * - conteúdo do email será a lista de processos <li> com link para a página de detalhe no sistema <a>
    */
    public class VerificadorDeAtualizacoesNoProcessoJob : IJob
    {
        private async Task VerificaAtualizacoes()
        {
            Trace.TraceInformation("Iniciando a verificação de novas tramitações.");
            EnviarEmailAdministrativo("Iniciando a verificação de novas tramitações.");

            var processosAtualizados = new List<Processo>();
            using (var servico = new MateriasService())
            {
                foreach (var processo in servico.ProcessosMonitorados)
                {
                    EnviarEmailAdministrativo($"Verificando atualizações para o processo {processo}...");
                    //consulta o web service para recuperar as tramitações da matéria:
                    var movimentacao = await servico.TramitacoesDaMateria(processo.Codigo);
                    //recupera a data mais recente de tramitação:
                    var dataUltimaTramitacao = movimentacao.Materia.Tramitacoes
                        .OrderByDescending(t => t.Identificacao.Data)
                        .Select(t => t.Identificacao.Data)
                        .First();
                    if (dataUltimaTramitacao.CompareTo(processo.DataUltimaAtualizacao) > 0)
                    {
                        string mensagem = $"Processo {processo} teve atualizações!";
                        Trace.TraceInformation(mensagem);
                        EnviarEmailAdministrativo(mensagem);
                        
                        processo.DataUltimaAtualizacao = dataUltimaTramitacao;
                        processosAtualizados.Add(processo);
                    }
                }
            }

            if (processosAtualizados.Count > 0)
            {
                using (var contexto = new ProcessoContext())
                {
                    try
                    {
                        contexto.Processos.UpdateRange(processosAtualizados);
                        contexto.SaveChanges();
                        EnviarEmailAtualizacao(processosAtualizados.Select(p => p.Id));
                    } catch (Exception e)
                    {
                        EnviarEmailAdministrativo($"Ocorreu um erro: {e.Message}\r\nInner Exception: {e.InnerException.Message}\r\n{e.StackTrace}");
                    }
                }
            } else
            {
                EnviarEmailAdministrativo("Não houveram novas tramitações para os processos monitorados.");
            }
        }

        private void EnviarEmailAdministrativo(string mensagem)
        {
            
            EnviarEmail(
                toEmail: "marcinhagarciarj@gmail.com", 
                assuntoMensagem: "[Consulta Processos] Log", 
                corpoMensagem: mensagem
            );
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                VerificaAtualizacoes().Wait();
            }
            catch (SystemException e)
            {
                Trace.TraceError($"Ocorreu um erro: {e.Message}");
                Trace.TraceError(e.StackTrace);
                throw e;
            }
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
                Credentials = new NetworkCredential("marciagarcia.confere@gmail.com", "xdbkoicdauqqfjry")
            };

            try
            {
                // Create instance of message
                MailMessage message = new MailMessage();

                // Add receiver
                message.To.Add(toEmail);

                // Sender
                message.From = new MailAddress("marciagarcia.confere@gmail.com");

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
                Trace.TraceError("Could not send e-mail. Exception caught: " + e);
            }
        }

        private void EnviarEmailAtualizacao(IEnumerable<int> processosAtualizados)
        {
            //Trace.TraceInformation("Entrada no método ")
            EnviarEmailAdministrativo("Entrada no método EnviarEmailAtualização.");
            using (var contexto = new ProcessoContext())
            {
                var interessados = contexto.Processos
                    .Where(p => processosAtualizados.Contains(p.Id))
                    .SelectMany(p => p.Interesses)
                    .Select(i => i.Interessado)
                    .Distinct()
                    .ToList();

                EnviarEmailAdministrativo($"Total de interessados encontrados: {interessados.Count}.");

                foreach (var interessado in interessados)
                {
                    var processos = contexto.Interessados
                        .Where(i => i.Id == interessado.Id)
                        .SelectMany(i => i.Interesses)
                        .Select(ip => ip.Processo)
                        .Where(p => processosAtualizados.Contains(p.Id))
                        .Distinct();

                    EnviarEmailAdministrativo($"Total de processos encontrados para o processo: {processos.Count()}.");

                    var corpoMensagem = $"Olá, {interessado.Nome}\n\r";
                    corpoMensagem += "Dos processos que você está acompanhando, os seguintes foram atualizados:";
                    foreach (var p in processos)
                    {
                        corpoMensagem += "\n\r" + p.ToString();
                    }

                    EnviarEmail(interessado.Email, "[Consulta Processos] Notificação de processos atualizados", corpoMensagem);
                    EnviarEmail("marciagarcia.confere@gmail.com", "[Consulta Processos] Notificação de processos atualizados", corpoMensagem);
                    EnviarEmail("marcinhagarciarj@gmail.com", "[Consulta Processos] Notificação de processos atualizados", corpoMensagem);

                }
            }
        }
    }
}
