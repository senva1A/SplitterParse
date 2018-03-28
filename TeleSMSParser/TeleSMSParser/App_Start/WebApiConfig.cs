﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TeleSMSParser
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{value}",
                defaults: new { value = RouteParameter.Optional }
            );
        }
    }
}