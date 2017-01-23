// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to retreive configured TOptions instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptions<out TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// The configured TOptions instance.
        /// </summary>
        TOptions Value { get; }

        /// <summary>
        /// Returns the configured instance for the name.
        /// </summary>
        /// <param name="name">The name of the configured options.</param>
        /// <returns>The configured instance for the name.</returns>
        TOptions GetNamedInstance(string name);
    }
}