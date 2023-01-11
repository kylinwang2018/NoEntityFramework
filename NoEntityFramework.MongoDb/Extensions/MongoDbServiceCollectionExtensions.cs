using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoEntityFramework.MongoDb;
using NoEntityFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NoEntityFramework
{
    /// <summary>
    ///     MongoDb specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class MongoDbServiceCollectionExtensions
    {

        /// <summary>
        ///     Configures the context to connect to a MongoDb database, must set up 
        ///     connection string before use it.
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static DbContext<TDbContext> AddMongoDbContext<TDbContext>(
            this IServiceCollection services, Action<MongoDbContextOptions> setupAction)
            where TDbContext : MongoDbContext
        {

            services.AddOptions();
            services.Configure(typeof(TDbContext).ToString(), setupAction);

            // register dbProvider in service collection
            services.TryAddSingleton(typeof(TDbContext));

            // register sql factory for create connection, command and dataAdapter
            services.TryAddSingleton<IMongoDbConnectionFactory<TDbContext, MongoDbContextOptions>, MongoDbConnectionFactory<TDbContext, MongoDbContextOptions>>();

            services.TryAddSingleton<IMongoDbOptions<TDbContext>, MongoDbOptions<TDbContext>>();

            return new DbContext<TDbContext>
            {
                ServiceCollection = services
            };
        }

        /// <summary>
        /// <para>
        /// inject all project-related repository with [MongoDbRepo]
        /// (<see cref="MongoDbRepoAttribute"/>) attribute to <see cref="IServiceCollection"/>.
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
        public static DbContext<TDbContext> RegisterMongoDbRepositories<TDbContext>(
            this DbContext<TDbContext> dbContext, params string[] assemblyName) where TDbContext : class, IDbContext
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
                .Where(t => !t.IsInterface && t is { IsSealed: false, IsAbstract: false })
                    .Where(t =>
                        t.GetCustomAttributes(typeof(MongoDbRepoAttribute), false).Length > 0 &&
                        t is { IsClass: true, IsAbstract: false }).ToList();
            foreach (var type in types)
            {
                var serviceLifetime = type.GetCustomAttribute<MongoDbRepoAttribute>().Lifetime;
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
