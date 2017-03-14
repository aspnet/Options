// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options.Factory;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsMonitor.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsMonitor<TOptions> : IOptionsMonitor<TOptions> where TOptions : class, new()
    {
        private readonly ICachedOptionsFactory<TOptions> _cachedOptionsFactory;

        private readonly List<Action<TOptions>> _listeners = new List<Action<TOptions>>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cachedOptionsFactory">The factory to instantiate an options instance.</param>
        /// <param name="sources">The sources used to listen for changes to the options instance.</param>
        public OptionsMonitor(ICachedOptionsFactory<TOptions> cachedOptionsFactory, IEnumerable<IOptionsChangeTokenSource<TOptions>> sources)
        {
            _cachedOptionsFactory = cachedOptionsFactory;

            foreach (var source in sources)
            {
                ChangeToken.OnChange(source.GetChangeToken, InvokeChanged);
            }
        }

        private void InvokeChanged()
        {
            _cachedOptionsFactory.ResetCache();

            foreach (var listener in _listeners)
            {
                listener?.Invoke(CurrentValue);
            }
        }

        /// <summary>
        /// The present value of the options.
        /// </summary>
        public TOptions CurrentValue => _cachedOptionsFactory.Get();

        /// <summary>
        /// Registers a listener to be called whenever TOptions changes.
        /// </summary>
        /// <param name="listener">The action to be invoked when TOptions has changed.</param>
        /// <returns>An IDisposable which should be disposed to stop listening for changes.</returns>
        public IDisposable OnChange(Action<TOptions> listener)
        {
            var disposable = new ChangeTrackerDisposable(_listeners, listener);

            _listeners.Add(listener);

            return disposable;
        }

        internal class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<TOptions> _originalListener;
            private readonly IList<Action<TOptions>> _listeners;

            public ChangeTrackerDisposable(IList<Action<TOptions>> listeners, Action<TOptions> listener)
            {
                _originalListener = listener;
                _listeners = listeners;
            }

            public void Dispose() => _listeners.Remove(_originalListener);
        }
    }
}