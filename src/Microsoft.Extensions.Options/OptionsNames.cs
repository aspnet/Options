// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Options
{
    public class OptionsNames<TOptions> : IOptionsNames<TOptions>
    {
        public OptionsNames(IEnumerable<IOptionsName<TOptions>> names)
        {
            Names = names.GroupBy(n => n.Name).Select(n => n.Key).ToList();
        }

        public IReadOnlyCollection<string> Names { get; }
    }

    public class OptionsName<TOptions> : IOptionsName<TOptions>
    {
        public OptionsName(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}