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
    }
}
