using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Autofac;
using Autofac.Integration.WebApi;

namespace Refactored
{
    public class WebApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            // Connection Factory
            builder.RegisterInstance<Func<IDbConnection>>(() => {
                var connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    return connection;
                }
                catch
                {
                    connection.Dispose();
                    throw;
                }
            });

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();
            builder.RegisterApiControllers(ThisAssembly);
        }
    }
}