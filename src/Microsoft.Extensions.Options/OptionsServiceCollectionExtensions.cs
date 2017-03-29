// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding options services to the DI container.
    /// </summary>
    public static class OptionsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for using options.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddOptions(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptions<>), typeof(OptionsManager<>)));
            services.TryAdd(ServiceDescriptor.Scoped(typeof(IOptionsSnapshot<>), typeof(OptionsManager<>)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IOptionsService<>), typeof(OptionsService<>)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IOptionsFactory<>), typeof(OptionsFactory<>)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IOptionsValidator<>), typeof(OptionsValidator<>)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsCache<>), typeof(OptionsCache<>)));
            return services;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.AddSingleton<IConfigureOptions<TOptions>>(new ConfigureOptions<TOptions>(configureOptions));
            return services;
        }

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.AddSingleton<IConfigureNamedOptions<TOptions>>(new ConfigureNamedOptions<TOptions>(name, configureOptions));
            return services;
        }

        public static IServiceCollection ConfigureAll<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            // REVIEW: should this ignore the non named options?  ConfigureAllNamed?
            services.Configure(configureOptions);
            services.AddSingleton<IConfigureNamedOptions<TOptions>>(new ConfigureNamedOptions<TOptions>(name: null, action: configureOptions));
            return services;
        }

        /// <summary>
        /// Registers an action used to validate options with a specific name.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="validateOptions">The action used to validate the options.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Validate<TOptions>(this IServiceCollection services, string name, Action<TOptions> validateOptions)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (validateOptions == null)
            {
                throw new ArgumentNullException(nameof(validateOptions));
            }

            services.AddSingleton<IValidateNamedOptions<TOptions>>(new ValidateNamedOptions<TOptions>(name, validateOptions));
            return services;
        }

        public static IServiceCollection ValidateAll<TOptions>(this IServiceCollection services, Action<TOptions> validateOptions)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (validateOptions == null)
            {
                throw new ArgumentNullException(nameof(validateOptions));
            }

            services.AddSingleton<IValidateNamedOptions<TOptions>>(new ValidateNamedOptions<TOptions>(name: null, action: validateOptions));
            return services;
        }
    }
}