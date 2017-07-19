// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding configuration related options services to the DI container.
    /// </summary>
    public static class OptionsConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, IConfiguration config) where TOptions : class
            => services.Configure<TOptions>(Options.Options.DefaultName, config);

        /// <summary>
        /// Registers a configuration access function that produces the configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configAccessor">A function that returns the <see cref="IConfiguration"/> instance that will be bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">Either <paramref name="services"/> or <paramref name="configAccessor"/> is <c>null</c>.</exception>
        /// <remarks>The argument passed on to the <paramref name="configAccessor"/> function will be the root <see cref="IConfiguration"/> instance that is registered as a service in <paramref name="services"/>.</remarks>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Func<IConfiguration, IConfiguration> configAccessor)
            where TOptions : class
            => services.Configure<TOptions>(Options.Options.DefaultName, configAccessor);


        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string name, IConfiguration config)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, config));
            return services.AddSingleton<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name, config));
        }

        /// <summary>
        /// Registers a configuration access function that produces the configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="configAccessor">A function that returns the <see cref="IConfiguration"/> instance that will be bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">Either <paramref name="services"/> or <paramref name="configAccessor"/> is <c>null</c>.</exception>
        /// <remarks>The argument passed on to the <paramref name="configAccessor"/> function will be the root <see cref="IConfiguration"/> instance that is registered as a service in <paramref name="services"/>.</remarks>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string name, Func<IConfiguration, IConfiguration> configAccessor)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configAccessor == null)
            {
                throw new ArgumentNullException(nameof(configAccessor));
            }

            services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(serviceProvider =>
            {
                var config = configAccessor(serviceProvider.GetRequiredService<IConfiguration>());
                return new ConfigurationChangeTokenSource<TOptions>(name, config);
            });
            return services.AddSingleton<IConfigureOptions<TOptions>>(serviceProvider =>
            {
                var config = configAccessor(serviceProvider.GetRequiredService<IConfiguration>());
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, config);
            });
        }
    }
}