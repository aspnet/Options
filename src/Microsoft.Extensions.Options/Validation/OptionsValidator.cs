// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Options.Validation
{
    internal class OptionsValidator<TOptions> : ValidationBase<TOptions>, IOptionsValidator
        where TOptions : class, new()
    {
        private readonly IOptionsNames<TOptions> _optionsNames;
        private readonly IOptionsFactory<TOptions> _optionsFactory;
        private readonly IOptionsValidator<TOptions> _optionsValidator;

        public OptionsValidator(IOptionsNames<TOptions> optionsNames, IOptionsFactory<TOptions> optionsFactory, IOptionsValidator<TOptions> optionsValidator)
        {
            _optionsNames = optionsNames;
            _optionsFactory = optionsFactory;
            _optionsValidator = optionsValidator;
        }

        public IValidationResult Validate()
        {
            try
            {
                return Aggregate(_optionsNames.Names.Select(ValidateOptions).ToList());
            }
            catch (Exception e)
            {
                return Invalid(e.Message);
            }
        }

        private IValidationResult ValidateOptions(string optionsName)
        {
            var options = _optionsFactory.Create(optionsName);

            var validationResult = _optionsValidator.Validate(optionsName, options);

            return validationResult;
        }
    }

    internal class InnerOptionsValidator<TOptions> : ValidationBase<TOptions>, IOptionsValidator<TOptions>
        where TOptions : class, new()
    {
        private readonly IEnumerable<IValidateOptions<TOptions>> _validateFuncs;

        public InnerOptionsValidator(IEnumerable<IValidateOptions<TOptions>> validateFuncs)
        {
            _validateFuncs = validateFuncs;
        }

        public IValidationResult Validate(string name, TOptions options)
        {
            try
            {
                return Aggregate(name, _validateFuncs.Select(vf => vf.Validate(name, options)).ToList());
            }
            catch (Exception e)
            {
                return Invalid(e.Message);
            }
        }
    }
}