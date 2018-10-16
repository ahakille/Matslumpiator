using Quartz;
using Quartz.Impl;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace matslump.Crono
{
    class Program
    {
        static void Main(string[] args)
        {
            var started= false;
            var cout = 1;
            Console.WriteLine("Press ESC to abort");
            do
            {
                Task.Delay(1000).Wait();
                Console.Write(".");
                cout++;
                if (cout > 5)
                {
                    started = true;
                    Console.WriteLine("");
                    Startcrono(8, 00);                    
                }

            }
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) && cout < 6);
           
            if(!started)
            {
                Console.WriteLine("Enter time for crono, Add hours");
                int hours = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("Add min");
                int min = Convert.ToInt16(Console.ReadLine());
                Startcrono(hours, min);
            }
         
        }
        public static void Startcrono(int hours, int min)
        {
            try
            {
                Cron.Start(hours, min);
                Console.WriteLine("CronJob started");
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }


        }


    }


    public class Slumpjob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://matslumpiator.se/Slumpiator/RunSlump?code=asdfg").Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Crono run " + DateTime.Now);
            }
            else
            {
                Console.WriteLine("Error  " + DateTime.Now);
            }

        }

    }
    public class Cron
    {
        public static void Start(int hours, int min)
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
    

