// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Represents a builder that configures a named TOptions instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class OptionsBuilder<TOptions> where TOptions : class
    {
        private readonly IServiceCollection _services;
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance being configured.</param>
        public OptionsBuilder(IServiceCollection services, string name)
        {
            _services = services;
            _name = name;
        }

        /// <summary>
        /// The name of the options instance being configured.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// The <see cref="IServiceCollection"/> to add the services to.
        /// </summary>
        public IServiceCollection Services => _services;
    }
}