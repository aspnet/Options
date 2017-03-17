// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsValidator.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class OptionsValidator<TOptions> : IOptionsValidator<TOptions> where TOptions : class, new()
    {
        private readonly IEnumerable<IValidateNamedOptions<TOptions>> _checks;

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="validations">The validation actions to run.</param>
        public OptionsValidator(IEnumerable<IValidateNamedOptions<TOptions>> validations)
        {
            _checks = validations;
        }

        public void Validate(string name, TOptions options)
        {
            foreach (var check in _checks)
            {
                check.Validate(name, options);
            }
        }
    }
}