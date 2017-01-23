// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to access the value of TOptions for the lifetime of a request.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IOptionsSnapshot<out TOptions>
    {
        /// <summary>
        /// Returns the value of the TOptions which will be computed once
        /// </summary>
        /// <returns></returns>
        TOptions Value { get; }

        /// <summary>
        /// Returns the configured instance for the name.
        /// </summary>
        /// <param name="name">The name of the configured options.</param>
        /// <returns>The configured instance for the name.</returns>
        TOptions GetNamedInstance(string name);
    }
}