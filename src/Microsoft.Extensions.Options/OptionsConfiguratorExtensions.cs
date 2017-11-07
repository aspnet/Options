// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding options services to the DI container via <see cref="OptionsConfigurator{TOptions}"/>.
    /// </summary>
    public static class OptionsConfiguratorExtensions
    {
        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="OptionsServiceCollectionExtensions.PostConfigure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="optionsConfigurator">The options builder to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="OptionsConfigurator{TOptions}"/> so that additional calls can be chained.</returns>
        public static OptionsConfigurator<TOptions> Configure<TOptions>(this OptionsConfigurator<TOptions> optionsConfigurator, Action<TOptions> configureOptions)
            where TOptions : class
        {
            if (optionsConfigurator == null)
            {
                throw new ArgumentNullException(nameof(optionsConfigurator));
            }

            optionsConfigurator.Services.Configure<TOptions>(optionsConfigurator.Name, configureOptions);
            return optionsConfigurator;
        }

        /// <summary>
        /// Registers an action used to initialize a particular type of options.
        /// Note: These are run after all <seealso cref="OptionsServiceCollectionExtensions.Configure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="optionsConfigurator">The options builder to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="OptionsConfigurator{TOptions}"/> so that additional calls can be chained.</returns>
        public static OptionsConfigurator<TOptions> PostConfigure<TOptions>(this OptionsConfigurator<TOptions> optionsConfigurator, Action<TOptions> configureOptions)
            where TOptions : class
        {
            if (optionsConfigurator == null)
            {
                throw new ArgumentNullException(nameof(optionsConfigurator));
            }

            optionsConfigurator.Services.PostConfigure<TOptions>(optionsConfigurator.Name, configureOptions);
            return optionsConfigurator;
        }
    }
}