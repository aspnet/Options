// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options.Validation
{
    /// <summary>
    /// Represents something that validate the TOptions type.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being validated.</typeparam>
    public interface IValidateOptions<in TOptions> where TOptions : class
    {
        /// <summary>
        /// Invoked to validate a TOptions instance.
        /// </summary>
        /// <param name="options">The options instance to validate.</param>
        IValidationResult Validate(TOptions options);
    }
}