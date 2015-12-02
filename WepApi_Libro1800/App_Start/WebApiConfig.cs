using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WepApi_Libro1800
{
    /// <summary>
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// </summary>
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "GetAll", id = RouteParameter.Optional }
            );

        }
    }
}
