// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Represents something that validate the TOptions type.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IValidateNamedOptions<in TOptions> where TOptions : class
    {
        /// <summary>
        /// The name of the options instance to validate.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Invoked to validate a TOptions instance.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The options instance to validate.</param>
        void Validate(string name, TOptions options);
    }
}