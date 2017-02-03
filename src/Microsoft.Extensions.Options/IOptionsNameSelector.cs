// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Used to select which named IOptions instance to use.
    /// </summary>
    public interface IOptionsNameSelector
    {
        /// <summary>
        /// Resolve the named options to use.
        /// </summary>
        string ResolveName();
    }

    public class DefaultOptionsNameSelector : IOptionsNameSelector
    {
        public string ResolveName()
        {
            return null;
        }
    }
}