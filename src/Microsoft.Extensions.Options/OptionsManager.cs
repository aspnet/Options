// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptions.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsManager<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        //private readonly Dictionary<string, TOptions> _cache = new Dictionary<string, TOptions>();
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly object _cacheLock = new object();
        private readonly IOptionsNameSelector _selector;
        private IOptionsCache<TOptions> _cache;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        /// <param name="selector">The selector which decides which named cache to use.</param>
        /// <param name="cache">The options instance cache which determines the lifetime of the instances.</param>
        public OptionsManager(IEnumerable<IConfigureOptions<TOptions>> setups, IOptionsNameSelector selector, IOptionsCache<TOptions> cache)
        {
            _cache = cache;
            _setups = setups;
            _selector = selector;
        }

        private TOptions CreateOptions(string namedInstance)
        {
            var filteredSetups = _setups.Where(s => s.NamedInstance == namedInstance);
            var result = new TOptions();
            foreach (var setup in filteredSetups)
            {
                setup.Configure(result);
            }
            return result;
        }

        /// <summary>
        /// The configured options instance.
        /// </summary>
        public virtual TOptions Value
        {
            get
            {
                var namedInstance = _selector.ResolveName();
                var result = _cache.Get(namedInstance);
                if (result == null)
                {
                    lock (_cache)
                    {
                        result = _cache.Get(namedInstance);
                        if (result == null)
                        {
                            result = CreateOptions(namedInstance);
                            _cache.Put(namedInstance, result);
                        }
                    }
                }
                return result;
            }
        }
    }
}