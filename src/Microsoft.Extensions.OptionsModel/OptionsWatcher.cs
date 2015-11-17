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
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly IEnumerable<IOptionsChangeTracker<TOptions>> _trackers;

        public OptionsWatcher(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IOptionsChangeTracker<TOptions>> trackers)
        {
            _trackers = trackers;
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
            var disposable = new ChangeTrackerDisposable();
            foreach (var tracker in _trackers)
            {
                disposable.Disposables.Add(ChangeToken.OnChange(tracker.GetChangeToken, () =>
                {
                    // Recompute the options before calling the watchers
                    _optionsCache = new OptionsCache<TOptions>(_setups);
                    watcher(_optionsCache.Value);
                }));
            }
            return disposable;
        }

        private class ChangeTrackerDisposable : IDisposable
        {
            public List<IDisposable> Disposables { get; } = new List<IDisposable>();

            public void Dispose()
            {
                foreach (var d in Disposables)
                {
                    d?.Dispose();
                }
                Disposables.Clear();
            }
        }
    }
}