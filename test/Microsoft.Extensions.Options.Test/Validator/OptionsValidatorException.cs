// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Aggregates <see cref="OptionsValidationException"/> triggered by <see cref="IOptionsValidator"/>.
    /// </summary>
    public class OptionsValidatorException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errors">The errors from the <see cref="IOptionsValidator"/>.</param>
        public OptionsValidatorException(IEnumerable<OptionsValidationException> errors)
        {
            ValidatorExceptions = errors;
        }

        /// <summary>
        /// The errors from the <see cref="IOptionsValidator"/>.
        /// </summary>
        public IEnumerable<OptionsValidationException> ValidatorExceptions { get; }

        // make sure error is displayed on startup
    }
}