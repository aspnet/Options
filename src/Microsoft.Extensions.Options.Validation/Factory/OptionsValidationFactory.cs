// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.Options.Factory;

namespace Microsoft.Extensions.Options.Validation.Factory
{
    /// <summary>
    /// Used to create configured TOptions instances and validate created instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    internal class OptionsValidationFactory<TOptions> : OptionsFactory<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsValidator<TOptions> _optionsValidator;

        /// <summary> 
        /// Initializes a new instance with the specified options configurations. 
        /// </summary>
        /// <param name="optionsValidator">The options validator instance is used to validate TOptions instance after creation.</param>
        /// <param name="setups">The configuration actions to run for TOptions instance creation.</param> 
        public OptionsValidationFactory(IOptionsValidator<TOptions> optionsValidator, IEnumerable<IConfigureOptions<TOptions>> setups) : base(setups)
        {
            _optionsValidator = optionsValidator;
        }

        /// <summary>
        /// Creates and validates (by configured validation rules) TOptions instance.
        /// </summary>
        public override TOptions Get()
        {
            var result = base.Get();

            _optionsValidator.ValidateOptions(result);

            return result;
        }
    }
}