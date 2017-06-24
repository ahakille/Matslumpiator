using Quartz;
using Matslump.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matslump.Tools
{
    public class Slumpjob: IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            Email.SendEmail("gorlingy@hotmail.com", "Nicklas", "test", "test" + DateTime.Now);
        }

    }
}