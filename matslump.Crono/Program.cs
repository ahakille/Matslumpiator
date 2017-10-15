using Matslump.Services;
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
                Console.WriteLine("Enter time for crono, Add hours");
                int hours = Convert.ToInt16( Console.ReadLine());
                Console.WriteLine("Add min");
                int min = Convert.ToInt16(Console.ReadLine());
                Cron.Start(hours,min);
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
                Console.WriteLine("Crono run " + DateTime.Now);
            }

        }
        public class Cron
        {
            public static void Start(int hours ,int min)
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                IJobDetail job = JobBuilder.Create<Slumpjob>().Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule
                      (s =>
                         s.WithIntervalInHours(24)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hours, min))
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
