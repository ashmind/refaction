using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace Refactored
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule(new WebApiModule());
            config.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());

            // Web API configuration and services
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.Indent = true;

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
