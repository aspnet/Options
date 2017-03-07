// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Options.Validation
{
    internal class OptionsValidator<TOptions> : IOptionsValidator
        where TOptions : class, new()
    {
        private readonly IOptions<TOptions> _options;
        private readonly IEnumerable<IValidateOptions<TOptions>> _validateFuncs;

        public OptionsValidator(IOptions<TOptions> options, IEnumerable<IValidateOptions<TOptions>> validateFuncs)
        {
            _options = options;
            _validateFuncs = validateFuncs;
        }

        public ValidationResult Validate()
        {
            try
            {
                var isValid = true;
                var errorSB = new StringBuilder().AppendLine($"{typeof(TOptions)} object is invalid:");

                foreach (var validateFunc in _validateFuncs)
                {
                    if (!validateFunc.Validate(_options.Value))
                    {
                        isValid = false;
                        errorSB.AppendLine(validateFunc.ErrorMessage);
                    }
                }

                var errorMessage = !isValid ? errorSB.ToString() : string.Empty;

                var validationResult = new ValidationResult(isValid, errorMessage);

                return validationResult;
            }
            catch (Exception e)
            {
                var validationErrorResult = new ValidationResult(false, e.Message);

                return validationErrorResult;
            }
        }
    }
}