// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace Microsoft.Extensions.Options.Validation
{
    internal class ValidationResult
    {
        private readonly bool _isValid;
        private readonly string _errorMessage;

        internal ValidationResult(bool isValid, string errorMessage)
        {
            _isValid = isValid;
            _errorMessage = errorMessage;
        }

        internal bool IsValid => _isValid;

        internal string ErrorMessage => _errorMessage;
    }
}