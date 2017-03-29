// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to validate TOptions instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptionsValidator<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Validates the options instance.
        /// </summary>
        void Validate(string name, TOptions options);
    }
}