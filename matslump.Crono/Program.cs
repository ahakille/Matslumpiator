using Matslump.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matslump.Crono
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Cron.Start();
                Console.WriteLine("CronJob started");
            }

            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
        public class Slumpjob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                Slumpcron.StartCron();

            }

        }
        public class Cron
        {
            public static void Start()
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                IJobDetail job = JobBuilder.Create<Slumpjob>().Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule
                      (s =>
                         s.WithIntervalInHours(24)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(16, 15))
                      )
                    //.WithSimpleSchedule(x => x
                    //      .WithIntervalInMinutes(5)
                    //      .RepeatForever())
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }
        }
    }
    
}
