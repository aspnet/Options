// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;

namespace Microsoft.Extensions.Options
{
    public interface IOptionsNames<TOptions>
    {
        IReadOnlyCollection<string> Names { get; }
    }

    public interface IOptionsName<TOptions>
    {
        string Name { get; }
    }
}