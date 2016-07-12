// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Microsoft.Extensions.Options
{
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
                var ordered = _setups.OrderBy(setup => setup.Order);
                foreach (var setup in ordered)
                {
                    setup.Configure(result);
                }
            }
            return result;
        }

        public virtual TOptions Value
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