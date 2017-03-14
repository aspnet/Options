// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options.Validation.ValidateOptions
{
    internal class FuncValidateOptions<TOptions> : ValidateOptions<TOptions> where TOptions : class
    {
        private readonly Func<TOptions, bool> _validateFunc;

        public FuncValidateOptions(Func<TOptions, bool> validateFunc, ValidationStatus validationStatus, string violationMessage) :
            base(validationStatus, violationMessage)
        {
            _validateFunc = validateFunc;
        }

        protected override IValidationResult ValidateCore(TOptions options)
        {
            try
            {
                var result = _validateFunc(options);

                if (!result)
                {
                    return Result(ValidationStatus, ViolationMessage);
                }

                return Valid();
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }
    }
}