using NoEntityFramework.Utilities;
using NoEntityFramework.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace NoEntityFramework
{
    /// <summary>
    ///     MySql specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class MySqlServiceCollectionExtensions
    {
        /// <summary>
        ///     Configures the context to connect to a MySql database, must set up 
        ///     connection string before use it.
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static DbContext<TDbContext> AddMySqlDbContext<TDbContext>(
            this IServiceCollection services, Action<RelationalDbOptions> setupAction) 
            where TDbContext : MySqlDbContext
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
                    typeof(IMySqlConnectionFactory<TDbContext, RelationalDbOptions>),
                    typeof(MySqlConnectionFactory<TDbContext, RelationalDbOptions>),
                    options.ContextLifetime));

            services.TryAdd(
                new ServiceDescriptor(
                    typeof(IMySqlOptions<TDbContext>),
                    typeof(MySqlOptions<TDbContext>),
                    options.ContextLifetime));

            services.TryAdd(
                new ServiceDescriptor(
                    typeof(IMySqlLogger<TDbContext>),
                    typeof(MySqlLogger<TDbContext>),
                    options.ContextLifetime));

            return new DbContext<TDbContext> 
            {
                ServiceCollection = services
            };
        }


        /// <summary>
        /// <para>
        /// inject all project-related repository with [MySqlRepo]
        /// (<see cref="MySqlRepoAttribute"/>) attribute to <see cref="IServiceCollection"/>.
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
        public static DbContext<TDbContext> RegisterMySqlRepositories<TDbContext>(
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
                    t.GetCustomAttributes(typeof(MySqlRepoAttribute), false).Length > 0);

            foreach (var type in types)
            {
                var serviceLifetime = type.GetCustomAttribute<MySqlRepoAttribute>().Lifetime;
                var typeInterface = type.GetInterfaces().FirstOrDefault();
                if (typeInterface != null)
                {
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.TryAddSingleton(typeInterface, type);
                            break;
                        case ServiceLifetime.Scoped:
                            services.TryAddScoped(typeInterface, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.TryAddTransient(typeInterface, type);
                            break;
                    }
                }
            }
        }
    }
}
