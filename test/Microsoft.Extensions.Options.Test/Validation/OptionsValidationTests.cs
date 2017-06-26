// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options.Validation;
using Xunit;

namespace Microsoft.Extensions.Options.Tests
{
    public class OptionsValidationTests
    {
        [Fact]
        public void CanValidateNamedOptionsByValidationManager()
        {
            var services = new ServiceCollection().AddOptions().AddOptionsValidation();

            services.Configure<FakeOptions>("1", options =>
            {
                options.Message = "one";
            });
            services.Validate<FakeOptions>("1", options => options.Message != "one", violationMessage: "Message should not be equal to 'one'");
            services.Validate<FakeOptions>("2", options => options.Message == "two", violationMessage: "Message should be equal to 'two'");

            var sp = services.BuildServiceProvider();
            var validationManager = sp.GetService<IOptionsValidatorManager>();
            var ex = Record.Exception(() => validationManager.Validate());

            Assert.NotNull(ex);
            Assert.Contains("Message should not be equal to 'one'", ex.Message);
            Assert.DoesNotContain("Message should be equal to 'two'", ex.Message);

            var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
            Assert.Equal("one", option.Get("1").Message);
        }

        [Fact]
        public void CanValidateAllOptionsByValidationManager()
        {
            var services = new ServiceCollection().AddOptions().AddOptionsValidation();

            services.Configure<FakeOptions>("1", options =>
            {
                options.Message = "one";
            });
            services.Validate<FakeOptions>("1", options => options.Message != "one", violationMessage: "Message should not be equal to 'one'");
            services.ValidateAll<FakeOptions>(options => options.Message == "two", violationMessage: "Message should be equal to 'two'");

            var sp = services.BuildServiceProvider();
            var validationManager = sp.GetService<IOptionsValidatorManager>();

            var ex = Record.Exception(() => validationManager.Validate());

            Assert.NotNull(ex);
            Assert.Contains("Message should not be equal to 'one'", ex.Message);
            Assert.Contains("Message should be equal to 'two'", ex.Message);

            var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
            Assert.Equal("one", option.Get("1").Message);
        }

        [Fact]
        public void CanValidateNamedOptionsByValidatorWithManualHandling()
        {
            var services = new ServiceCollection().AddOptions().AddOptionsValidation();

            services.Configure<FakeOptions>("1", options =>
            {
                options.Message = "one";
            });
            services.Validate<FakeOptions>("1", options => options.Message != "one", violationMessage: "Message should not be equal to 'one'");
            services.ValidateAll<FakeOptions>(options => options.Message == "two", violationMessage: "Message should be equal to 'two'");

            var sp = services.BuildServiceProvider();
            var validator = sp.GetService<IOptionsValidator<FakeOptions>>();
            var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();

            var validationResult = validator.Validate("1", option.Get("1"));

            Assert.NotNull(validator);
            Assert.NotNull(option);
            Assert.NotNull(validationResult);
            Assert.Equal(ValidationStatus.Invalid, validationResult.Status);
            Assert.Contains("Message should not be equal to 'one'", validationResult.Message);
            Assert.Contains("Message should be equal to 'two'", validationResult.Message);
        }

        // [Fact]
        // public void NamedSnapshotsConfiguresInRegistrationOrder()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     services.Configure<FakeOptions>("-", o => o.Message += "-");
        //     services.ConfigureAll<FakeOptions>(o => o.Message += "A");
        //     services.Configure<FakeOptions>("+", o => o.Message += "+");
        //     services.ConfigureAll<FakeOptions>(o => o.Message += "B");
        //     services.ConfigureAll<FakeOptions>(o => o.Message += "C");
        //     services.Configure<FakeOptions>("+", o => o.Message += "+");
        //     services.Configure<FakeOptions>("-", o => o.Message += "-");

        //     var sp = services.BuildServiceProvider();
        //     var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
        //     Assert.Equal("ABC", option.Get("1").Message);
        //     Assert.Equal("A+BC+", option.Get("+").Message);
        //     Assert.Equal("-ABC-", option.Get("-").Message);
        // }

        // [Fact]
        // public void CanConfigureAllDefaultAndNamedOptions()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     services.ConfigureAll<FakeOptions>(o => o.Message += "Default");
        //     services.Configure<FakeOptions>(o => o.Message += "0");
        //     services.Configure<FakeOptions>("1", o => o.Message += "1");

        //     var sp = services.BuildServiceProvider();
        //     var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
        //     Assert.Equal("Default", option.Get("Default").Message);
        //     Assert.Equal("Default0", option.Value.Message);
        //     Assert.Equal("Default1", option.Get("1").Message);
        // }

        // [Fact]
        // public void CanConfigureAndPostConfigureAllDefaultAndNamedOptions()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     services.ConfigureAll<FakeOptions>(o => o.Message += "Default");
        //     services.Configure<FakeOptions>(o => o.Message += "0");
        //     services.Configure<FakeOptions>("1", o => o.Message += "1");
        //     services.PostConfigureAll<FakeOptions>(o => o.Message += "PostConfigure");
        //     services.PostConfigure<FakeOptions>(o => o.Message += "2");
        //     services.PostConfigure<FakeOptions>("1", o => o.Message += "3");

        //     var sp = services.BuildServiceProvider();
        //     var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
        //     Assert.Equal("DefaultPostConfigure", option.Get("Default").Message);
        //     Assert.Equal("Default0PostConfigure2", option.Value.Message);
        //     Assert.Equal("Default1PostConfigure3", option.Get("1").Message);
        // }

        // [Fact]
        // public void CanPostConfigureAllOptions()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     services.PostConfigureAll<FakeOptions>(o => o.Message = "Default");

        //     var sp = services.BuildServiceProvider();
        //     var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
        //     Assert.Equal("Default", option.Get("1").Message);
        //     Assert.Equal("Default", option.Get("2").Message);
        // }

        // [Fact]
        // public void CanConfigureAndPostConfigureAllOptions()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     services.ConfigureAll<FakeOptions>(o => o.Message = "D");
        //     services.PostConfigureAll<FakeOptions>(o => o.Message += "f");
        //     services.ConfigureAll<FakeOptions>(o => o.Message += "e");
        //     services.PostConfigureAll<FakeOptions>(o => o.Message += "ault");

        //     var sp = services.BuildServiceProvider();
        //     var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
        //     Assert.Equal("Default", option.Get("1").Message);
        //     Assert.Equal("Default", option.Get("2").Message);
        // }

        // [Fact]
        // public void NamedSnapshotsPostConfiguresInRegistrationOrder()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     services.PostConfigure<FakeOptions>("-", o => o.Message += "-");
        //     services.PostConfigureAll<FakeOptions>(o => o.Message += "A");
        //     services.PostConfigure<FakeOptions>("+", o => o.Message += "+");
        //     services.PostConfigureAll<FakeOptions>(o => o.Message += "B");
        //     services.PostConfigureAll<FakeOptions>(o => o.Message += "C");
        //     services.PostConfigure<FakeOptions>("+", o => o.Message += "+");
        //     services.PostConfigure<FakeOptions>("-", o => o.Message += "-");

        //     var sp = services.BuildServiceProvider();
        //     var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
        //     Assert.Equal("ABC", option.Get("1").Message);
        //     Assert.Equal("A+BC+", option.Get("+").Message);
        //     Assert.Equal("-ABC-", option.Get("-").Message);
        // }

        // [Fact]
        // public void CanPostConfigureAllDefaultAndNamedOptions()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     services.PostConfigureAll<FakeOptions>(o => o.Message += "Default");
        //     services.PostConfigure<FakeOptions>(o => o.Message += "0");
        //     services.PostConfigure<FakeOptions>("1", o => o.Message += "1");

        //     var sp = services.BuildServiceProvider();
        //     var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
        //     Assert.Equal("Default", option.Get("Default").Message);
        //     Assert.Equal("Default0", option.Value.Message);
        //     Assert.Equal("Default1", option.Get("1").Message);
        // }

        // [Fact]
        // public void CustomIConfigureOptionsShouldOnlyAffectDefaultInstance()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     services.AddSingleton<IConfigureOptions<FakeOptions>, CustomSetup>();

        //     var sp = services.BuildServiceProvider();
        //     var option = sp.GetRequiredService<IOptionsSnapshot<FakeOptions>>();
        //     Assert.Equal("", option.Get("NotDefault").Message);
        //     Assert.Equal("Stomp", option.Get(Options.DefaultName).Message);
        //     Assert.Equal("Stomp", option.Value.Message);
        //     Assert.Equal("Stomp", sp.GetRequiredService<IOptions<FakeOptions>>().Value.Message);
        // }

        // private class CustomSetup : IConfigureOptions<FakeOptions>
        // {
        //     public void Configure(FakeOptions options)
        //     {
        //         options.Message = "Stomp";
        //     }
        // }

        // [Fact]
        // public void EnsureAddOptionsLifetimes()
        // {
        //     var services = new ServiceCollection().AddOptions();
        //     CheckLifetime(services, typeof(IOptions<>), ServiceLifetime.Singleton);
        //     CheckLifetime(services, typeof(IOptionsMonitor<>), ServiceLifetime.Singleton);
        //     CheckLifetime(services, typeof(IOptionsSnapshot<>), ServiceLifetime.Scoped);
        //     CheckLifetime(services, typeof(IOptionsCache<>), ServiceLifetime.Singleton);
        //     CheckLifetime(services, typeof(IOptionsFactory<>), ServiceLifetime.Transient);
        // }

        // private void CheckLifetime(IServiceCollection services, Type serviceType, ServiceLifetime lifetime)
        // {
        //     Assert.NotNull(services.Where(s => s.ServiceType == serviceType && s.Lifetime == lifetime).SingleOrDefault());
        // }
    }
}
