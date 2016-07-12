// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Represents something that configures the TOptions type.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IConfigureOptions<in TOptions> where TOptions : class
    {
        /// <summary>
        /// Invoked to configure a TOptions instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        void Configure(TOptions options);

        /// <summary>
        /// Used to determine the order which IConfigureOptions will be run.
        /// </summary>
        int Order { get; }
    }
}