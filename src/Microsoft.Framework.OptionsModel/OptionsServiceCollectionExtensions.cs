// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.Framework.DependencyInjection
{
    public static class OptionsServiceCollectionExtensions
    {

        public static IServiceCollection AddSetup([NotNull]this IServiceCollection services, Type setupType)
        {
            var serviceTypes = setupType.GetTypeInfo().ImplementedInterfaces
                .Where(t => t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(IOptionsSetup<>));
            foreach (var serviceType in serviceTypes)
            {
                services.Add(new ServiceDescriptor
                {
                    ServiceType = serviceType,
                    ImplementationType = setupType,
                    Lifecycle = LifecycleKind.Transient
                });
            }
            // TODO: consider throwing if we add no services?
            return services;
        }

        public static IServiceCollection AddSetup<TSetup>([NotNull]this IServiceCollection services)
        {
            return services.AddSetup(typeof(TSetup));
        }

        public static IServiceCollection AddSetup([NotNull]this IServiceCollection services, [NotNull]object setupInstance)
        {
            var setupType = setupInstance.GetType();
            var serviceTypes = setupType.GetTypeInfo().ImplementedInterfaces
                .Where(t => t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(IOptionsSetup<>));
            foreach (var serviceType in serviceTypes)
            {
                services.Add(new ServiceDescriptor
                {
                    ServiceType = serviceType,
                    ImplementationInstance = setupInstance,
                    Lifecycle = LifecycleKind.Singleton
                });
            }
            // TODO: consider throwing if we add no services?
            return services;
        }

        [Obsolete("Use ConfigureOptions instead")]
        public static IServiceCollection SetupOptions<TOptions>([NotNull]this IServiceCollection services,
            Action<TOptions> setupAction)
        {
            return services.ConfigureOptions<TOptions>(setupAction);
        }

        public static IServiceCollection ConfigureOptions<TOptions>([NotNull]this IServiceCollection services,
            Action<TOptions> setupAction,
            string optionsName)
        {
            return services.ConfigureOptions(setupAction, OptionsConstants.DefaultOrder, optionsName);
        }

        public static IServiceCollection ConfigureOptions<TOptions>([NotNull]this IServiceCollection services,
            Action<TOptions> setupAction,
            int order = OptionsConstants.DefaultOrder,
            string optionsName = "")
        {
            services.AddSetup(new OptionsSetup<TOptions>(
                new OptionsAction<TOptions>
                {
                    Action = setupAction,
                    Name = optionsName,
                    Order = order
                }));
            return services;
        }

        public static IServiceCollection ConfigureOptions<TOptions>([NotNull]this IServiceCollection services,
            [NotNull] IConfiguration config, string optionsName)
        {
            services.AddSetup(new ConfigOptionsSetup<TOptions>(config, OptionsConstants.ConfigurationOrder, optionsName));
            return services;
        }

        public static IServiceCollection ConfigureOptions<TOptions>([NotNull]this IServiceCollection services,
            [NotNull] IConfiguration config,
            int order = OptionsConstants.ConfigurationOrder, 
            string optionsName = "")
        {
            services.AddSetup(new ConfigOptionsSetup<TOptions>(config, order, optionsName));
            return services;
        }
    }
}