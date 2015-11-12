// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.OptionsModel
{
    public class OptionsWatcher<TOptions> : IOptionsWatcher<TOptions> where TOptions : class, new()
    {
        private OptionsCache<TOptions> _optionsCache;
        private IEnumerable<IConfigureOptions<TOptions>> _setups;
        private IOptionsChangeTracker<TOptions> _tracker;

        // REVIEW: IEnumerable monitors?
        public OptionsWatcher(IEnumerable<IConfigureOptions<TOptions>> setups, IOptionsChangeTracker<TOptions> tracker)
        {
            _tracker = tracker;
            _setups = setups;
            _optionsCache = new OptionsCache<TOptions>(setups);

        }

        public TOptions CurrentValue
        {
            get
            {
                return _optionsCache.Value;
            }
        }

        public IDisposable Watch(Action<TOptions> watcher)
        {
            return ChangeToken.OnChange(_tracker.GetChangeToken, () =>
            {
                // Recompute the options before calling the watchers
                _optionsCache = new OptionsCache<TOptions>(_setups);
                watcher(_optionsCache.Value);
            });
        }
    }
}