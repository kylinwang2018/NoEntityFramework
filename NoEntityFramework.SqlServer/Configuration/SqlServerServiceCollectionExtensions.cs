using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NoEntityFramework.SqlServer;
using NoEntityFramework.Utilities;
using System;
using System.Linq;
using System.Reflection;

namespace NoEntityFramework
{
    /// <summary>
    ///     SQL Server specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        ///     Configures the context to connect to a Microsoft SQL Server database, must set up 
        ///     connection string before use it.
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static DbContext<TDbContext> AddSqlServerDbContext<TDbContext>(
            this IServiceCollection services, Action<RelationalDbOptions> setupAction) 
            where TDbContext : SqlServerDbContext
        {

            services.AddOptions();
            services.Configure(typeof(TDbContext).ToString(), setupAction);

            var options = new RelationalDbOptions();
            setupAction(options);

            // register dbProvider in service collection
            var contextType = typeof(TDbContext);
            services.TryAdd(new ServiceDescriptor(contextType,
                contextType,
                options.ContextLifetime));

            // register sql factory for create connection, command and dataAdapter
            services.TryAdd(
                new ServiceDescriptor(
                    typeof(ISqlConnectionFactory<TDbContext, RelationalDbOptions>),
                    typeof(SqlConnectionFactory<TDbContext, RelationalDbOptions>),
                    options.ContextLifetime));

            services.TryAdd(
                new ServiceDescriptor(
                    typeof(ISqlServerOptions<TDbContext>),
                    typeof(SqlServerOptions<TDbContext>),
                    options.ContextLifetime));

            services.TryAdd(
                new ServiceDescriptor(
                    typeof(ISqlServerLogger<TDbContext>),
                    typeof(SqlServerLogger<TDbContext>),
                    options.ContextLifetime));

            return new DbContext<TDbContext> 
            {
                ServiceCollection = services
            };
        }


        /// <summary>
        /// <para>
        /// inject all project-related repository with [SqlServerRepo]
        /// (<see cref="SqlServerRepoAttribute"/>) attribute to <see cref="IServiceCollection"/>.
        /// </para>
        /// <para>
        /// Make sure your repository projects has been add to your main project and its assembly
        /// name must start as same as your main project's, otherwise it cannot find the repositories.
        /// </para>
        /// <para>
        /// For example: your main project named "ExampleService", and its repository project must be
        /// "ExampleService.Repo" or "ExampleService[anything]"
        /// </para>
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static DbContext<TDbContext> RegisterSqlServerRepositories<TDbContext>(
            this DbContext<TDbContext> dbContext, params string[] assemblyName) 
            where TDbContext : class, IDbContext
        {
            var allAssembly = AppAssembly.GetAll(assemblyName);

            dbContext.ServiceCollection?.RegisterServiceByAttribute(allAssembly);

            return dbContext;
        }

        private static void RegisterServiceByAttribute(this IServiceCollection services, params Assembly[] allAssembly)
        {

            var types = allAssembly
                .SelectMany(t =>
                    t.GetTypes())
                .Where(t => t.IsClass && !t.IsInterface && !t.IsSealed && !t.IsAbstract &&
                            t.GetCustomAttributes(typeof(SqlServerRepoAttribute), false).Length > 0);

            foreach (var type in types)
            {
                var serviceLifetime = type.GetCustomAttribute<SqlServerRepoAttribute>().Lifetime;
                var typeInterface = type.GetInterfaces().FirstOrDefault();
                if (typeInterface != null)
                {
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.TryAddSingleton(typeInterface, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.TryAddTransient(typeInterface, type);
                            break;
                        case ServiceLifetime.Scoped:
                        default:
                            services.TryAddScoped(typeInterface, type);
                            break;
                    }
                }
            }
        }
    }
}
