using Quartz;
using Quartz.Impl;

namespace Confere.Processos.Monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            //a cada x tempo, consultar a web para verificar se existem atualizações
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<VerificadorDeAtualizacoesNoProcessoJob>().Build();

            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                            .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
