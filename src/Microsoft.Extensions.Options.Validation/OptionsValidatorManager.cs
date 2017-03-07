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

        public void Validate()
        {
            var errors = _validators.Select(v => v.Validate()).Where(vr => !vr.IsValid);

            if(errors.Any())
            {
                throw new OptionsValidationException(errors);
            }
        }
    }
}