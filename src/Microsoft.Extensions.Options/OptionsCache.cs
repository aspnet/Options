// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Extensions.Options
{
    public interface IOptionsCache<TOptions> where TOptions : class, new()
    {
        TOptions Get(string namedInstance);
        void Put(string namedInstance, TOptions value);
    }

    public class DefaultOptionsCache<TOptions> : IOptionsCache<TOptions> where TOptions : class, new()
    {
        private readonly Dictionary<string, TOptions> _cache = new Dictionary<string, TOptions>();
        private TOptions _value;

        public TOptions Get(string namedInstance)
        {
            if (namedInstance == null)
            {
                return _value;
            }
            if (_cache.ContainsKey(namedInstance))
            {
                return _cache[namedInstance];
            }
            return null;
        }

        public void Put(string namedInstance, TOptions value)
        {
            if (namedInstance == null)
            {
                _value = value;
            }
            else
            {
                _cache[namedInstance] = value;
            }
        }
    }


    internal class OptionsCache<TOptions> where TOptions : class, new()
    {
        private readonly Func<TOptions> _createCache;
        private object _cacheLock = new object();
        private bool _cacheInitialized;
        private TOptions _options;
        private IEnumerable<IConfigureOptions<TOptions>> _setups;

        public OptionsCache(IEnumerable<IConfigureOptions<TOptions>> setups)
        {
            _setups = setups;
            _createCache = CreateOptions;
        }

        private TOptions CreateOptions()
        {
            var result = new TOptions();
            if (_setups != null)
            {
                foreach (var setup in _setups)
                {
                    setup.Configure(result);
                }
            }
            return result;
        }

        public TOptions Value
        {
            get
            {
                return LazyInitializer.EnsureInitialized(
                    ref _options,
                    ref _cacheInitialized,
                    ref _cacheLock,
                    _createCache);
            }
        }
    }
}