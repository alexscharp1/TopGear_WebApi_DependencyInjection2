using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Host.SystemWeb;

[assembly: OwinStartup(typeof(TopGear_WebApi_DependencyInjection2.Startup))]

namespace TopGear_WebApi_DependencyInjection2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}