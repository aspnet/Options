// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.Options
{
    public interface IOptionsInitializer<TOptions> where TOptions : class
    {
        /// <summary>
        /// Initializes the options instance.
        /// </summary>
        /// <param name="instance">The options instance.</param>
        /// <returns></returns>
        TOptions Initialize(TOptions instance);
    }
}