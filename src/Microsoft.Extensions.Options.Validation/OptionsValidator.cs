// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Options.Validation
{
    internal class OptionsValidator<TOptions> : IOptionsValidator
        where TOptions : class, new()
    {
        private readonly IOptions<TOptions> _options;
        private readonly IOptionsValidator<TOptions> _optionsValidator;

        public OptionsValidator(IOptions<TOptions> options, IOptionsValidator<TOptions> optionsValidator)
        {
            _options = options;
            _optionsValidator = optionsValidator;
        }

        public IValidationResult Validate()
        {
            return _optionsValidator.Validate(_options.Value);
        }
    }

    internal class InnerOptionsValidator<TOptions> : ValidationBase<TOptions>, IOptionsValidator<TOptions>
        where TOptions : class, new()
    {
        private readonly IEnumerable<IValidateOptions<TOptions>> _validateFuncs;
        private readonly IValidationResultHandler _validationResultHandler;

        public InnerOptionsValidator(IEnumerable<IValidateOptions<TOptions>> validateFuncs, IValidationResultHandler validationResultHandler = null)
        {
            _validateFuncs = validateFuncs;
            _validationResultHandler = validationResultHandler;
        }

        public IValidationResult Validate(TOptions options)
        {
            try
            {
                return Aggregate(_validateFuncs.Select(vf => vf.Validate(options)));
            }
            catch (Exception e)
            {
                return Invalid(e.Message);
            }
        }

        public void ValidateOptions(TOptions options)
        {
            var result = Aggregate(_validateFuncs.Select(vf => vf.Validate(options)));

            _validationResultHandler?.Handle(result);
        }
    }
}