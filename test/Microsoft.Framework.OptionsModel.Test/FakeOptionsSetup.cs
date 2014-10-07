// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Framework.OptionsModel.Tests
{
    public class FakeOptionsSetupAlways : IConfigureOptions<FakeOptions>
    {
        public int Order
        {
            get { return -999; }
        }

        public string TargetOptionsName { get; set; } = "";

        public void Configure(string optionsName, FakeOptions options)
        {
            options.Message += "#";
        }
    }

    public class FakeOptionsSetupA : ConfigureOptions<FakeOptions>
    {
        public FakeOptionsSetupA() : base(o => o.Message += "A")
        {
            Order = -1;
        }
    }

    public class FakeOptionsSetupB : ConfigureOptions<FakeOptions>
    {
        public FakeOptionsSetupB() : base(o => o.Message += "B")
        {
            Order = 10;
        }
    }

    public class FakeOptionsSetupC : ConfigureOptions<FakeOptions>
    {
        public FakeOptionsSetupC() : base(o => o.Message += "C")
        {
            Order = 1000;
        }
    }
}
