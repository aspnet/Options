// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Implementation of IOptionsSnapshot.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsSnapshot<TOptions> : OptionsManager<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="setups">The configuration actions to run.</param>
        public OptionsSnapshot(IEnumerable<IConfigureOptions<TOptions>> setups) : base(setups)
        { }
    }
}