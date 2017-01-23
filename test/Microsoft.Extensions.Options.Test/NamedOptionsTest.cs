// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.Extensions.Options.Tests
{
    public class NamedOptionsTest
    {
        public class MyNameSelector : IOptionsNameSelector
        {
            public static string Name { get; set; }

            public string ResolveName() => Name;
        }

        [Fact]
        public void CanResolveDifferentOptionsBasedOnScopeName()
        {
            var services = new ServiceCollection()
                .AddScoped<IOptionsNameSelector, MyNameSelector>()
                .AddOptions();

            services.Configure<FakeOptions>("1", options =>
            {
                options.Message = "one";
            });
            services.Configure<FakeOptions>("2", options =>
            {
                options.Message = "two";
            });

            var sp = services.BuildServiceProvider();

            var factory = sp.GetRequiredService<IServiceScopeFactory>();
            using (var scope = factory.CreateScope())
            {
                MyNameSelector.Name = "1";
                var option = scope.ServiceProvider.GetRequiredService<IOptions<FakeOptions>>();
                Assert.Equal("one", option.Value.Message);
                Assert.Equal("one", option.GetNamedInstance("1").Message);
                Assert.Equal("two", option.GetNamedInstance("2").Message);
                var snapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
                Assert.Equal("one", snapshot.Value.Message);
            }
            using (var scope = factory.CreateScope())
            {
                MyNameSelector.Name = "2";
                var option = scope.ServiceProvider.GetRequiredService<IOptions<FakeOptions>>();
                Assert.Equal("two", option.Value.Message);
                Assert.Equal("one", option.GetNamedInstance("1").Message);
                Assert.Equal("two", option.GetNamedInstance("2").Message);
                var snapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
                Assert.Equal("two", snapshot.Value.Message);
            }
        }
    }
}
