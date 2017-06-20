// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Microsoft.Extensions.Options.Validation.ValidateOptions
{
    internal class ActionValidateOptions<TOptions> : ValidateOptions<TOptions> where TOptions : class
    {
        private readonly Action<TOptions> _validateAction;

        public ActionValidateOptions(Action<TOptions> validateAction, ValidationStatus validationStatus, string violationMessage) :
            base(validationStatus, violationMessage)
        {
            _validateAction = validateAction;
        }

        protected override IValidationResult ValidateCore(TOptions options)
        {
            try
            {
                _validateAction(options);

                return Valid();
            }
            catch (Exception e)
            {
                return Result(ValidationStatus, ViolationMessage ?? e.Message);
            }
        }
    }
}