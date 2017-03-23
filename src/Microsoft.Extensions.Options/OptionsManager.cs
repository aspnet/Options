// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsFactory.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class OptionsManager<TOptions> : IOptionsManager<TOptions>, IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsCache<TOptions> _cache;
        private readonly IOptionsFactory<TOptions> _factory;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="cache">The cache to use.</param>
        /// <param name="factory">The factory to use to create options.</param>
        public OptionsManager(IOptionsCache<TOptions> cache, IOptionsFactory<TOptions> factory)
        {
            _cache = cache;
            _factory = factory;
        }

        public TOptions Value
        {
            get
            {
                return Get(Options.DefaultName);
            }
        }

        public virtual TOptions Get(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return _cache.GetOrAdd(name, () => _factory.Create(name));
        }
    }
}
