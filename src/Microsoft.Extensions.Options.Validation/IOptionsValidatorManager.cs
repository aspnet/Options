// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options.Validation
{
    /// <summary>
    /// Represents aggregator for validation all configured to validation Options instances.
    /// </summary>
    public interface IOptionsValidatorManager
    {
        /// <summary>
        /// Invoked to validate a TOptions instance by all configured Options validators.
        /// </summary>
        /// <param name="validationLevel">The validation level. Specifies the level to throw an exception.</param>
        void Validate(ValidationLevel validationLevel = ValidationLevel.Invalid);
    }
}