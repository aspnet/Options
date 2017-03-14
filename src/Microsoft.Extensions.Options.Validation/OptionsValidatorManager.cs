// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Options.Validation
{
    internal class OptionsValidatorManager : IOptionsValidatorManager
    {
        private readonly IEnumerable<IOptionsValidator> _validators;

        public OptionsValidatorManager(IEnumerable<IOptionsValidator> validators)
        {
            _validators = validators;
        }

        public void Validate(ValidationLevel validationLevel = ValidationLevel.Invalid)
        {
            var shouldThrow = ValidationResultHandlerFactory.GetHandleCondition(validationLevel);

            var violations = _validators.Select(v => v.Validate()).Where(shouldThrow).ToList();

            if(violations.Any())
            {
                throw new OptionsValidationException(violations);
            }
        }
    }
}