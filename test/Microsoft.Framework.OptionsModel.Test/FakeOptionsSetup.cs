// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Framework.OptionsModel.Tests
{
    public class FakeOptionsSetupA : OptionsAction<FakeOptions>
    {
        public FakeOptionsSetupA() : base(o => o.Message += "A")
        {
            Order = -1;
        }
    }

    public class FakeOptionsSetupB : OptionsAction<FakeOptions>
    {
        public FakeOptionsSetupB() : base(o => o.Message += "B")
        {
            Order = 10;
        }
    }

    public class FakeOptionsSetupC : OptionsAction<FakeOptions>
    {
        public FakeOptionsSetupC() : base(o => o.Message += "C")
        {
            Order = 1000;
        }
    }
}
