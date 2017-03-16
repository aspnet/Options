// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.Extensions.Options.Tests
{
    public class OptionsTest
    {
        public class ComplexOptions
        {
            public ComplexOptions()
            {
                Nested = new NestedOptions();
                Virtual = "complex";
            }
            public NestedOptions Nested { get; set; }
            public int Integer { get; set; }
            public bool Boolean { get; set; }
            public virtual string Virtual { get; set; }

            public string PrivateSetter { get; private set; }
            public string ProtectedSetter { get; protected set; }
            public string InternalSetter { get; internal set; }
            public static string StaticProperty { get; set; }

            public string ReadOnly
            {
                get { return null; }
            }
        }

        public class NestedOptions
        {
            public int Integer { get; set; }
        }

        public class DerivedOptions : ComplexOptions
        {
            public override string Virtual
            {
                get
                {
                    return base.Virtual;
                }
                set
                {
                    base.Virtual = "Derived:" + value;
                }
            }
        }

        public class NullableOptions
        {
            public bool? MyNullableBool { get; set; }
            public int? MyNullableInt { get; set; }
            public DateTime? MyNullableDateTime { get; set; }
        }

        public class EnumOptions
        {
            public UriKind UriKind { get; set; }
        }

        [Fact]
        public void CanReadComplexProperties()
        {
            var dic = new Dictionary<string, string>
            {
                {"Integer", "-2"},
                {"Boolean", "TRUe"},
                {"Nested:Integer", "11"}
            };
            var builder = new ConfigurationBuilder().AddInMemoryCollection(dic);
            var config = builder.Build();
            var options = new ComplexOptions();
            ConfigurationBinder.Bind(config, options);
            Assert.True(options.Boolean);
            Assert.Equal(-2, options.Integer);
            Assert.Equal(11, options.Nested.Integer);
        }

        [Fact]
        public void CanReadInheritedProperties()
        {
            var dic = new Dictionary<string, string>
            {
                {"Integer", "-2"},
                {"Boolean", "TRUe"},
                {"Nested:Integer", "11"},
                {"Virtual","Sup"}
            };
            var builder = new ConfigurationBuilder().AddInMemoryCollection(dic);
            var config = builder.Build();
            var options = new DerivedOptions();
            ConfigurationBinder.Bind(config, options);
            Assert.True(options.Boolean);
            Assert.Equal(-2, options.Integer);
            Assert.Equal(11, options.Nested.Integer);
            Assert.Equal("Derived:Sup", options.Virtual);
        }

        [Fact]
        public void CanReadStaticProperty()
        {
            var dic = new Dictionary<string, string>
            {
                {"StaticProperty", "stuff"},
            };
            var builder = new ConfigurationBuilder().AddInMemoryCollection(dic);
            var config = builder.Build();
            var options = new ComplexOptions();
            ConfigurationBinder.Bind(config, options);
            Assert.Equal("stuff", ComplexOptions.StaticProperty);
        }

        [Theory]
        [InlineData("ReadOnly")]
        [InlineData("PrivateSetter")]
        [InlineData("ProtectedSetter")]
        [InlineData("InternalSetter")]
        public void ShouldBeIgnoredTests(string property)
        {
            var dic = new Dictionary<string, string>
            {
                {property, "stuff"},
            };
            var builder = new ConfigurationBuilder().AddInMemoryCollection(dic);
            var config = builder.Build();
            var options = new ComplexOptions();
            ConfigurationBinder.Bind(config, options);
            Assert.Null(options.GetType().GetProperty(property).GetValue(options));
        }

        [Fact]
        public void SetupCallsInOrder()
        {
            var services = new ServiceCollection().AddOptions();
            var dic = new Dictionary<string, string>
            {
                {"Message", "!"},
            };
            var builder = new ConfigurationBuilder().AddInMemoryCollection(dic);
            var config = builder.Build();
            services.Configure<FakeOptions>(o => o.Message += "Igetstomped");
            services.Configure<FakeOptions>(config);
            services.Configure<FakeOptions>(o => o.Message += "a");
            services.Configure<FakeOptions>(o => o.Message += "z");

            var service = services.BuildServiceProvider().GetService<IOptions<FakeOptions>>();
            Assert.NotNull(service);
            var options = service.Value;
            Assert.NotNull(options);
            Assert.Equal("!az", options.Message);
        }

        public static TheoryData Configure_GetsNullableOptionsFromConfiguration_Data
        {
            get
            {
                return new TheoryData<IDictionary<string, string>, IDictionary<string, object>>
                {
                    {
                        new Dictionary<string, string>
                        {
                            { nameof(NullableOptions.MyNullableBool), "true" },
                            { nameof(NullableOptions.MyNullableInt), "1" },
                            { nameof(NullableOptions.MyNullableDateTime), new DateTime(2015, 1, 1).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) }
                        },
                        new Dictionary<string, object>
                        {
                            { nameof(NullableOptions.MyNullableBool), true },
                            { nameof(NullableOptions.MyNullableInt), 1 },
                            { nameof(NullableOptions.MyNullableDateTime), new DateTime(2015, 1, 1) }
                        }
                    },
                    {
                        new Dictionary<string, string>
                        {
                            { nameof(NullableOptions.MyNullableBool), "false" },
                            { nameof(NullableOptions.MyNullableInt), "-1" },
                            { nameof(NullableOptions.MyNullableDateTime), new DateTime(1995, 12, 31).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) }
                        },
                        new Dictionary<string, object>
                        {
                            { nameof(NullableOptions.MyNullableBool), false },
                            { nameof(NullableOptions.MyNullableInt), -1 },
                            { nameof(NullableOptions.MyNullableDateTime), new DateTime(1995, 12, 31) }
                        }
                    },
                    {
                        new Dictionary<string, string>
                        {
                            { nameof(NullableOptions.MyNullableBool), null },
                            { nameof(NullableOptions.MyNullableInt), null },
                            { nameof(NullableOptions.MyNullableDateTime), null }
                        },
                        new Dictionary<string, object>
                        {
                            { nameof(NullableOptions.MyNullableBool), null },
                            { nameof(NullableOptions.MyNullableInt), null },
                            { nameof(NullableOptions.MyNullableDateTime), null }
                        }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Configure_GetsNullableOptionsFromConfiguration_Data))]
        public void Configure_GetsNullableOptionsFromConfiguration(
            IDictionary<string, string> configValues,
            IDictionary<string, object> expectedValues)
        {
            // Arrange
            var services = new ServiceCollection().AddOptions();
            var builder = new ConfigurationBuilder().AddInMemoryCollection(configValues);
            var config = builder.Build();
            services.Configure<NullableOptions>(config);

            // Act
            var options = services.BuildServiceProvider().GetService<IOptions<NullableOptions>>().Value;

            // Assert
            var optionsProps = options.GetType().GetProperties().ToDictionary(p => p.Name);
            var assertions = expectedValues
                .Select(_ => new Action<KeyValuePair<string, object>>(kvp =>
                    Assert.Equal(kvp.Value, optionsProps[kvp.Key].GetValue(options))));
            Assert.Collection(expectedValues, assertions.ToArray());
        }

        public static TheoryData Configure_GetsEnumOptionsFromConfiguration_Data
        {
            get
            {
                return new TheoryData<IDictionary<string, string>, IDictionary<string, object>>
                {
                    {
                        new Dictionary<string, string>
                        {
                            { nameof(EnumOptions.UriKind), (UriKind.Absolute).ToString() },
                        },
                        new Dictionary<string, object>
                        {
                            { nameof(EnumOptions.UriKind), UriKind.Absolute },
                        }
                    },
                    {
                        new Dictionary<string, string>
                        {
                            { nameof(EnumOptions.UriKind), ((int)UriKind.Absolute).ToString() },
                        },
                        new Dictionary<string, object>
                        {
                            { nameof(EnumOptions.UriKind), UriKind.Absolute },
                        }
                    },
                    {
                        new Dictionary<string, string>
                        {
                            { nameof(EnumOptions.UriKind), null },
                        },
                        new Dictionary<string, object>
                        {
                            { nameof(EnumOptions.UriKind), UriKind.RelativeOrAbsolute },  //default enum, since not overridden by configuration
                        }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Configure_GetsEnumOptionsFromConfiguration_Data))]
        public void Configure_GetsEnumOptionsFromConfiguration(
            IDictionary<string, string> configValues,
            IDictionary<string, object> expectedValues)
        {
            // Arrange
            var services = new ServiceCollection().AddOptions();
            var builder = new ConfigurationBuilder().AddInMemoryCollection(configValues);
            var config = builder.Build();
            services.Configure<EnumOptions>(config);

            // Act
            var options = services.BuildServiceProvider().GetService<IOptions<EnumOptions>>().Value;

            // Assert
            var optionsProps = options.GetType().GetProperties().ToDictionary(p => p.Name);
            var assertions = expectedValues
                .Select(_ => new Action<KeyValuePair<string, object>>(kvp =>
                    Assert.Equal(kvp.Value, optionsProps[kvp.Key].GetValue(options))));
            Assert.Collection(expectedValues, assertions.ToArray());
        }

        [Fact]
        public void Options_StaticCreateCreateMakesOptions()
        {
            var options = Options.Create(new FakeOptions
            {
                Message = "This is a message"
            });

            Assert.Equal("This is a message", options.Value.Message);
        }

        [Fact]
        public void OptionsWrapper_MakesOptions()
        {
            var options = new OptionsWrapper<FakeOptions>(new FakeOptions
            {
                Message = "This is a message"
            });

            Assert.Equal("This is a message", options.Value.Message);
        }

        [Fact]
        public void Options_CanOverrideForSpecificTOptions()
        {
            var services = new ServiceCollection().AddOptions();

            services.Configure<FakeOptions>(options =>
            {
                options.Message = "Initial value";
            });

            services.AddSingleton(Options.Create(new FakeOptions
            {
                Message = "Override"
            }));

            var sp = services.BuildServiceProvider();
            Assert.Equal("Override", sp.GetRequiredService<IOptions<FakeOptions>>().Value.Message);
        }

        [Fact]
        public void TryConfigure_ConfiguresOptionsOnce()
        {
            // Arrange
            var services = new ServiceCollection().AddOptions();

            services.TryConfigure<FakeOptions>(o => o.Message = "First");
            services.TryConfigure<FakeOptions>(o => o.Message = "Second");

            // Act
            var options = services.BuildServiceProvider().GetService<IOptions<FakeOptions>>().Value;

            // Assert
            Assert.Equal("First", options.Message);
            Assert.Equal(1, services.Count(s => s.ServiceType == typeof(IConfigureOptions<FakeOptions>)));
        }
    }
}
