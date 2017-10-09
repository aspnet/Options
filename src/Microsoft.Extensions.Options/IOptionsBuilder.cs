// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Represents a builder that configures a named TOptions instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptionsBuilder<TOptions> where TOptions : class
    {
        /// <summary>
        /// The name of the options instance being configured.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The <see cref="IServiceCollection"/> to add the services to.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
