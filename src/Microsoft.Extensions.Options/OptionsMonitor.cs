// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsMonitor.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsMonitor<TOptions> : IOptionsMonitor<TOptions> where TOptions : class, new()
    {
        private readonly Func<TOptions> _createOptions;
        private object _optionsLock = new object();
        private bool _optionsInitialized;
        private TOptions _options;

        private readonly List<Action<TOptions>> _listeners = new List<Action<TOptions>>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="setups">The configuration actions to run on an options instance.</param>
        /// <param name="sources">The sources used to listen for changes to the options instance.</param>
        /// <param name="initializer">The options initializer object to initialize an options instance.</param>
        public OptionsMonitor(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IOptionsChangeTokenSource<TOptions>> sources, IOptionsInitializer<TOptions> initializer = null)
        {
            initializer = initializer ?? new DefaultOptionsInitializer<TOptions>(setups);

            _createOptions = () =>
            {
                initializer.InitializeOptions(reinitialize: true);

                var result = initializer.Options;

                return result;
            };

            foreach (var source in sources)
            {
                ChangeToken.OnChange(
                    () => source.GetChangeToken(),
                    () => InvokeChanged());
            }
        }

        private void InvokeChanged()
        {
            _optionsInitialized = false;

            foreach (var listener in _listeners)
            {
                listener?.Invoke(CurrentValue);
            }
        }

        /// <summary>
        /// The present value of the options.
        /// </summary>
        public TOptions CurrentValue => LazyInitializer.EnsureInitialized(ref _options, ref _optionsInitialized, ref _optionsLock, _createOptions);

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

            public void Dispose()
            {
                _listeners.Remove(_originalListener);
            }
        }
    }
}