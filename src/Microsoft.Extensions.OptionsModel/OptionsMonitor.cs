// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.OptionsModel
{
    public class OptionsMonitor<TOptions> : IOptionsMonitor<TOptions> where TOptions : class, new()
    {
        private OptionsCache<TOptions> _optionsCache;
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly IEnumerable<IOptionsChangeTokenSource<TOptions>> _sources;

        public OptionsMonitor(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IOptionsChangeTokenSource<TOptions>> sources)
        {
            _sources = sources;
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

        public IDisposable OnChange(Action<TOptions> listener)
        {
            var disposable = new ChangeTrackerDisposable();
            foreach (var source in _sources)
            {

                Action<object> callback = null;
                callback = (s) =>
                {
                    // The order here is important. We need to take the token and then apply our changes BEFORE
                    // registering. This prevents us from possible having two change updates to process concurrently.
                    //
                    // If the token changes after we take the token, then we'll process the update immediately upon
                    // registering the callback.
                    var token = source.GetChangeToken();

                    // Recompute the options before calling the watchers
                    _optionsCache = new OptionsCache<TOptions>(_setups);
                    listener(_optionsCache.Value);
                    disposable.Disposables.Add(token.RegisterChangeCallback(callback, s));
                };

                disposable.Disposables.Add(source.GetChangeToken().RegisterChangeCallback(callback, state: null));
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