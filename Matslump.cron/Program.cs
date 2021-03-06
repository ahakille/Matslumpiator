﻿using Quartz;
using Quartz.Impl;
using System;
using Matslump.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Matslump.Cron
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
                    //.WithDailyTimeIntervalSchedule
                    //  (s =>
                    //     s.WithIntervalInHours(24)
                    //    .OnEveryDay()
                    //    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 00))
                    //  )
                    .WithSimpleSchedule(x => x
                          .WithIntervalInMinutes(5)
                          .RepeatForever())
                    .Build();

                scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
