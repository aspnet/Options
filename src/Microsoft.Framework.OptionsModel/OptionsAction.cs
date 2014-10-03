// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Framework.OptionsModel
{
    public class OptionsAction<T>
    {
        public Action<T> Action { get; set; }
        public string Name { get; set; } = "";
        public int Order = OptionsConstants.DefaultOrder;

        public void Invoke(T obj)
        {
            if (Action != null)
            {
                Action(obj);
            }
        }
    }
}
