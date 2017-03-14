// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Options.Factory;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsManager<TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        private readonly ICachedOptionsFactory<TOptions> _cachedOptionsFactory;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="cachedOptionsFactory">The factory to instantiate an options instance.</param>
        public OptionsManager(ICachedOptionsFactory<TOptions> cachedOptionsFactory)
        {
            _cachedOptionsFactory = cachedOptionsFactory;
        }

        /// <summary>
        /// The configured options instance.
        /// </summary>
        public virtual TOptions Value => _cachedOptionsFactory.Get();
    }
}