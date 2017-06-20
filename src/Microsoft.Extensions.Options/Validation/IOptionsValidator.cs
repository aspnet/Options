// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Microsoft.Extensions.Options.Validation
{
    internal interface IOptionsValidator
    {
        IValidationResult Validate();
    }

    internal interface IOptionsValidator<in TOptions>
    {
        IValidationResult Validate(TOptions options);

        void ValidateOptions(TOptions options);
    }
}