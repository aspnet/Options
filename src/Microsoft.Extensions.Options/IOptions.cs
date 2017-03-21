// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to retreive configured TOptions instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptions<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// The default configured TOptions instance, equivalent to Get(string.Empty).
        /// </summary>
        TOptions Value { get; }

        /// <summary>
        /// Returns a configured TOptions instance with the given name.
        /// </summary>
        TOptions Get(string name);

        void Add(string name, TOptions options);

        bool Remove(string name);
    }
}