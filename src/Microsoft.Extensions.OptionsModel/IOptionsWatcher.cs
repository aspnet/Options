// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.OptionsModel
{
    public interface IOptionsWatcher<out TOptions>
    {
        TOptions CurrentValue { get; }
        IDisposable Watch(Action<TOptions> watcher);
    }
}