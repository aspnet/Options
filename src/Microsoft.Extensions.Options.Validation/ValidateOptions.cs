// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options.Validation
{
    /// <summary>
    /// Base class to be inherited in order to provide custom validation logic.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being validated.</typeparam>
    public abstract class ValidateOptions<TOptions> : ValidationBase<TOptions>, IValidateOptions<TOptions> where TOptions : class
    {
        protected readonly ValidationStatus ValidationStatus;
        protected readonly string ViolationMessage;

        protected ValidateOptions() : this(ValidationStatus.Invalid, null)
        {
        }

        protected ValidateOptions(ValidationStatus validationStatus) : this(validationStatus, null)
        {
        }

        protected ValidateOptions(ValidationStatus validationStatus, string violationMessage)
        {
            ValidationStatus = validationStatus;
            ViolationMessage = violationMessage;
        }

        /// <summary>
        /// Invoked to validate a TOptions instance.
        /// </summary>
        /// <param name="options">The options instance to validate.</param>
        public IValidationResult Validate(TOptions options)
        {
            return ValidateCore(options);
        }

        protected abstract IValidationResult ValidateCore(TOptions options);

        protected new IValidationResult Invalid() => Result(ValidationStatus.Invalid, ViolationMessage);

        protected new IValidationResult Valid() => Result(ValidationStatus.Valid, ViolationMessage);

        protected new IValidationResult Warning() => Result(ValidationStatus.Warning, ViolationMessage);
    }
}