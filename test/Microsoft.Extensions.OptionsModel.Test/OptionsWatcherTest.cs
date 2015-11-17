// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Microsoft.Extensions.OptionsModel.Tests
{
    public class OptionsWatcherTest
    {
        public int SetupInvokeCount { get; set; }

        public class FakeChangeToken : IChangeToken, IDisposable
        {
            public bool ActiveChangeCallbacks { get; set; }
            public bool HasChanged { get; set; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                _callback = () => callback(state);
                return this;
            }

            public void InvokeChangeCallback()
            {
                if (_callback != null)
                {
                    _callback();
                }
            }

            public void Dispose()
            {
                _callback = null;
            }

            private Action _callback;
        }

        public class CountIncrement : IConfigureOptions<FakeOptions>
        {
            private OptionsWatcherTest _test;

            public CountIncrement(OptionsWatcherTest test)
            {
                _test = test;
            }

            public void Configure(FakeOptions options)
            {
                _test.SetupInvokeCount++;
                options.Message += _test.SetupInvokeCount;
            }
        }


        public class FakeTracker : IOptionsChangeTracker<FakeOptions>
        {
            public FakeTracker(FakeChangeToken token)
            {
                Token = token;
            }

            public FakeChangeToken Token { get; set; }

            public IChangeToken GetChangeToken()
            {
                return Token;
            }

            public void Changed()
            {
                Token.HasChanged = true;
                Token.InvokeChangeCallback();
            }
        }

        [Fact]
        public void CanWatchOptions()
        {
            var services = new ServiceCollection().AddOptions();
            services.AddSingleton<IConfigureOptions<FakeOptions>>(new CountIncrement(this));
            var changeToken = new FakeChangeToken();
            var tracker = new FakeTracker(changeToken);
            services.AddSingleton<IOptionsChangeTracker<FakeOptions>>(tracker);

            var sp = services.BuildServiceProvider();

            var watcher = sp.GetRequiredService<IOptionsWatcher<FakeOptions>>();
            Assert.NotNull(watcher);
            Assert.Equal("1", watcher.CurrentValue.Message);

            string updatedMessage = null;
            watcher.Watch(o => updatedMessage = o.Message);
            changeToken.InvokeChangeCallback();
            Assert.Equal("2", updatedMessage);

            // Verify old watch is changed too
            Assert.Equal("2", watcher.CurrentValue.Message);
        }

        [Fact]
        public void CanWatchOptionsWithMultipleTrackers()
        {
            var services = new ServiceCollection().AddOptions();
            services.AddSingleton<IConfigureOptions<FakeOptions>>(new CountIncrement(this));
            var changeToken = new FakeChangeToken();
            var tracker = new FakeTracker(changeToken);
            services.AddSingleton<IOptionsChangeTracker<FakeOptions>>(tracker);
            var changeToken2 = new FakeChangeToken();
            var tracker2 = new FakeTracker(changeToken2);
            services.AddSingleton<IOptionsChangeTracker<FakeOptions>>(tracker2);

            var sp = services.BuildServiceProvider();

            var watcher = sp.GetRequiredService<IOptionsWatcher<FakeOptions>>();
            Assert.NotNull(watcher);
            Assert.Equal("1", watcher.CurrentValue.Message);

            string updatedMessage = null;
            var cleanup = watcher.Watch(o => updatedMessage = o.Message);
            changeToken.InvokeChangeCallback();
            Assert.Equal("2", updatedMessage);

            // Verify old watch is changed too
            Assert.Equal("2", watcher.CurrentValue.Message);

            changeToken2.InvokeChangeCallback();
            Assert.Equal("3", updatedMessage);

            // Verify old watch is changed too
            Assert.Equal("3", watcher.CurrentValue.Message);

            cleanup.Dispose();
            changeToken.InvokeChangeCallback();
            changeToken2.InvokeChangeCallback();

            // Verify messages aren't changed
            Assert.Equal("3", updatedMessage);
            Assert.Equal("3", watcher.CurrentValue.Message);
        }

        [Fact]
        public void CanMonitorConfigBoundOptions()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection().Build();

            var services = new ServiceCollection().AddOptions();
            services.AddSingleton<IConfigureOptions<FakeOptions>>(new CountIncrement(this));
            services.Configure<FakeOptions>(config, trackConfigChanges: true);

            var sp = services.BuildServiceProvider();

            var watcher = sp.GetRequiredService<IOptionsWatcher<FakeOptions>>();
            Assert.NotNull(watcher);
            Assert.Equal("1", watcher.CurrentValue.Message);

            string updatedMessage = null;
            var cleanup = watcher.Watch(o => updatedMessage = o.Message);

            config.Reload();
            Assert.Equal("2", updatedMessage);

            // Verify old watch is changed too
            Assert.Equal("2", watcher.CurrentValue.Message);

            // REVIEW: need some way to unwatch config?
        }

        public class Controller : IDisposable
        {
            IDisposable _watcher;
            FakeOptions _options;

            public Controller(IOptionsWatcher<FakeOptions> watcher)
            {
                _watcher = watcher.Watch(o => _options = o);
            }

            public void Dispose()
            {
                _watcher?.Dispose();
            }

            public string Message => _options?.Message;
        }

        [Fact]
        public void ControllerCanWatchOptionsThatTrackConfigChanges()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection().Build();

            var services = new ServiceCollection().AddOptions();
            services.AddSingleton<IConfigureOptions<FakeOptions>>(new CountIncrement(this));
            services.AddTransient<Controller, Controller>();
            services.Configure<FakeOptions>(config, trackConfigChanges: true);

            var sp = services.BuildServiceProvider();

            var controller = sp.GetRequiredService<Controller>();
            Assert.Null(controller.Message);

            config.Reload();
            Assert.Equal("1", controller.Message);

            config.Reload();
            Assert.Equal("2", controller.Message);
        }
    }
}