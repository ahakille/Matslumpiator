﻿using Matslump.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace Matslump.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/account/index")
            });
        }
    }
}