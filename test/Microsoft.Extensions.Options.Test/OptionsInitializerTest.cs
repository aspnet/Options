// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Microsoft.Extensions.Options.Tests
{
    public class OptionsInitializerTest
    {
        public int SetupInvokeCount { get; set; }

        private class CountIncrement : IConfigureOptions<FakeOptions>
        {
            private OptionsInitializerTest _test;

            public CountIncrement(OptionsInitializerTest test)
            {
                _test = test;
            }

            public void Configure(FakeOptions options)
            {
                _test.SetupInvokeCount++;
                options.Message += _test.SetupInvokeCount;
            }
        }

        private class DoubleCountIncrement : OptionsInitializer<FakeOptions>
        {
            private OptionsInitializerTest _test;

            public DoubleCountIncrement(OptionsInitializerTest test)
            {
                _test = test;
            }

            public override void InitializeOptionsCore(FakeOptions options)
            {
                _test.SetupInvokeCount += 2;
                options.Message += _test.SetupInvokeCount;
            }
        }

        public class FakeSource : IOptionsChangeTokenSource<FakeOptions>
        {
            public FakeSource(FakeChangeToken token)
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

        private class InvalidOptions
        {
            public int Value { get; set; }
        }

        private class InvalidConfiguration : IConfigureOptions<InvalidOptions>
        {
            public void Configure(InvalidOptions options)
            {
                throw new Exception();
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InitializerDetectsInvalidConfigSourceAtStartup(int sourceId)
        {
            var services = new ServiceCollection().AddOptions();

            services.ConfigureInitializer<InvalidOptions>();

            switch (sourceId)
            {
                case 0:
                    services.Configure<InvalidOptions>(o => throw new Exception());
                    break;
                case 1:
                    services.AddSingleton<IConfigureOptions<InvalidOptions>>(new InvalidConfiguration());
                    break;
                default:
                    var invalidConfigBuilder = new ConfigurationBuilder();
                    invalidConfigBuilder.AddInMemoryCollection(new Dictionary<string,string>
                    {
                        {"Value","InvalidValue"}
                    });
                    services.Configure<InvalidOptions>(invalidConfigBuilder.Build());
                    break;
            }

            var sp = services.BuildServiceProvider();

            // it will throw an exception only at time when the options were requiested.
            Assert.ThrowsAny<Exception>(() => sp.GetRequiredService<IOptions<InvalidOptions>>().Value);

            var initializerManager = sp.GetRequiredService<IOptionsInitializerManager>();
            Assert.NotNull(initializerManager);

            // callint 'Initialize' method at startup of app it will initialize all registered options initializers
            // and if something wrong it will throw an exception
            Assert.ThrowsAny<Exception>(() => initializerManager.Initialize());
        }

        [Fact]
        public void InitializerOverridesConfigurationOptions()
        {
            var services = new ServiceCollection().AddOptions();

            services.AddSingleton<IConfigureOptions<FakeOptions>>(new CountIncrement(this));
            var changeToken = new FakeChangeToken();
            var tracker = new FakeSource(changeToken);
            services.AddSingleton<IOptionsChangeTokenSource<FakeOptions>>(tracker);

            services.ConfigureInitializer(new DoubleCountIncrement(this));

            var sp = services.BuildServiceProvider();

            var options = sp.GetRequiredService<IOptions<FakeOptions>>();
            var optionsSnapshot = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();

            Assert.NotNull(options);
            Assert.Equal("2", options.Value.Message);

            Assert.NotNull(optionsSnapshot);
            Assert.Equal("2", optionsSnapshot.Value.Message);
        }

        [Fact]
        public void InitializerUsedByOptionsMonitorToReinitializeOptions()
        {
            var services = new ServiceCollection().AddOptions();

            services.AddSingleton<IConfigureOptions<FakeOptions>>(new CountIncrement(this));
            var changeToken = new FakeChangeToken();
            var tracker = new FakeSource(changeToken);
            services.AddSingleton<IOptionsChangeTokenSource<FakeOptions>>(tracker);

            services.ConfigureInitializer(new DoubleCountIncrement(this));

            var sp = services.BuildServiceProvider();

            var monitor = sp.GetRequiredService<IOptionsMonitor<FakeOptions>>();
            Assert.NotNull(monitor);
            Assert.Equal("2", monitor.CurrentValue.Message);

            string updatedMessage = null;
            monitor.OnChange(o => updatedMessage = o.Message);
            changeToken.InvokeChangeCallback();
            Assert.Equal("4", updatedMessage);

            // Verify old watch is changed too
            Assert.Equal("4", monitor.CurrentValue.Message);
        }

        [Fact]
        public void OptionsArePreInitializedBeforeAnyUsage()
        {
            var services = new ServiceCollection().AddOptions();

            services.ConfigureInitializer(new DoubleCountIncrement(this));

            Assert.Equal(0, this.SetupInvokeCount);

            var sp = services.BuildServiceProvider();

            var initializerManager = sp.GetRequiredService<IOptionsInitializerManager>();
            Assert.NotNull(initializerManager);

            initializerManager.Initialize();

            Assert.Equal(2, this.SetupInvokeCount);

            var options = sp.GetRequiredService<IOptions<FakeOptions>>();
            var optionsSnapshot = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();

            Assert.NotNull(options);
            Assert.Equal("2", options.Value.Message);

            Assert.NotNull(optionsSnapshot);
            Assert.Equal("2", optionsSnapshot.Value.Message);
        }

        [Fact]
        public void OptionsArePreInitializedByDefaultInitializerBeforeAnyUsage()
        {
            var services = new ServiceCollection().AddOptions();

            services.AddSingleton<IConfigureOptions<FakeOptions>>(new CountIncrement(this));
            var changeToken = new FakeChangeToken();
            var tracker = new FakeSource(changeToken);
            services.AddSingleton<IOptionsChangeTokenSource<FakeOptions>>(tracker);

            services.ConfigureInitializer<FakeOptions>();

            Assert.Equal(0, this.SetupInvokeCount);

            var sp = services.BuildServiceProvider();

            var initializerManager = sp.GetRequiredService<IOptionsInitializerManager>();
            Assert.NotNull(initializerManager);

            initializerManager.Initialize();

            Assert.Equal(1, this.SetupInvokeCount);

            var options = sp.GetRequiredService<IOptions<FakeOptions>>();
            var optionsSnapshot = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();

            Assert.NotNull(options);
            Assert.Equal("1", options.Value.Message);

            Assert.NotNull(optionsSnapshot);
            Assert.Equal("1", optionsSnapshot.Value.Message);
        }
    }
}