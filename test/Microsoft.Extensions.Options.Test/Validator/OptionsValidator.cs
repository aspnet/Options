// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    public static class ValidatorOptionsBuilderExtensions
    {
        /// <summary>
        /// Registers this options instance for validation by the validator.
        /// </summary>
        /// <returns>The current OptionsBuilder.</returns>
        public static OptionsBuilder<TOptions> ValidatorEnabled<TOptions>(this OptionsBuilder<TOptions> builder) where TOptions : class
        {
            builder.Services.AddOptions<OptionsValidatorOptions>()
                .Configure<IOptionsMonitor<TOptions>>(
                    (options, monitor) => options.Actions.Add(() => monitor.Get(builder.Name)));
            return builder;
        }
    }

    /// <summary>
    /// Validates options on by invoking <see cref="OptionsValidatorOptions.Actions"/>.
    /// </summary>
    public class OptionsValidator : IOptionsValidator
    {
        private OptionsValidatorOptions _options;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">The <see cref="OptionsValidatorOptions"/> options.</param>
        public OptionsValidator(IOptions<OptionsValidatorOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Validates options by invoking <see cref="OptionsValidatorOptions.Actions"/>.
        /// </summary>
        public void Validate()
        {
            var errors = new List<OptionsValidationException>();
            foreach (var action in _options.Actions)
            {
                try
                {
                    action.Invoke();
                }
                catch (OptionsValidationException e)
                {
                    errors.Add(e);
                }
            }
            if (errors.Count > 0)
            {
                throw new OptionsValidatorException(errors);
            }
        }
    }
}