﻿
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Matslump.Tools;

namespace Matslump
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            Response.AppendToLog(ex.Data + ex.Message + ex.Source + ex.InnerException);
       
        }
    }
}
