// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Concurrent;
using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsFactory.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class OptionsService<TOptions> : IOptionsService<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsCache<TOptions> _cache;
        private readonly IOptionsFactory<TOptions> _factory;
        private readonly IOptionsValidator<TOptions> _validator;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="cache">The cache to use.</param>
        /// <param name="factory">The factory to use to create options.</param>
        /// <param name="validator">The validator used to validate options.</param>
        public OptionsService(IOptionsCache<TOptions> cache, IOptionsFactory<TOptions> factory, IOptionsValidator<TOptions> validator)
        {
            _cache = cache;
            _factory = factory;
            _validator = validator;
        }

        public virtual void Add(string name, TOptions options)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (!_cache.TryAdd(name, options))
            {
                throw new InvalidOperationException("An option named {name} already exists.");
            }
        }

        public virtual TOptions Get(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return _cache.GetOrAdd(name, () =>
            {
                var options = _factory.Create(name);
                _validator.Validate(name, options);
                return options;
            });
        }

        public virtual bool Remove(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return _cache.TryRemove(name);
        }
    }
}