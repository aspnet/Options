// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.OptionsModel
{
    public class OptionsWatcher<TOptions> : IOptionsWatcher<TOptions> where TOptions : class, new()
    {
        private TOptions _options;
        private IEnumerable<IConfigureOptions<TOptions>> _setups;
        private IOptionsChangeTracker<TOptions> _tracker;

        // REVIEW: IEnumerable monitors?
        public OptionsWatcher(IEnumerable<IConfigureOptions<TOptions>> setups, IOptionsChangeTracker<TOptions> tracker)
        {
            _tracker = tracker;
            _setups = setups;

        }

        public TOptions CurrentValue
        {
            get
            {
                if (_options == null)
                {
                    _options = SetupValue();
                }
                return _options;
            }
        }

        private TOptions SetupValue()
        {
            return _setups == null
                ? new TOptions()
                : _setups.Aggregate(new TOptions(),
                                    (options, setup) =>
                                    {
                                        setup.Configure(options);
                                        return options;
                                    });

        }

        public IDisposable Watch(Action<TOptions> watcher)
        {
            return ChangeTokenHelper.OnChange(_tracker.GetChangeToken, () =>
            {
                // Recompute the options: REVIEW is there thread safety issues here?
                _options = SetupValue();
                watcher(_options);
            });
        }
    }
}