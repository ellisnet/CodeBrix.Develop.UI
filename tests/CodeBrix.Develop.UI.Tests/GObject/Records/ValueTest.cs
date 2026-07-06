using System;
using System.Runtime.InteropServices;
using SilverAssertions;
using CodeBrix.Develop.UI.GObject.Internal;
using Xunit;

namespace CodeBrix.Develop.UI.GObject.Tests; //was previously: GObject.Tests;

[Trait("Category", "UnitTest")]
public class ValueTest : Test
{
    private static void EnsureBasicType(Value v, Internal.BasicType basicType)
    {
        var data = Marshal.PtrToStructure<Internal.ValueData>(v.Handle.DangerousGetHandle());
        data.GType.Should().Be((nuint) basicType);
        Internal.Functions.TypeCheckValue(v.Handle).Should().Be(true);
        Internal.Functions.TypeCheckValueHolds(v.Handle, data.GType).Should().BeTrue();
        Internal.Functions.TypeCheckValueHolds(v.Handle, (nuint) basicType).Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(-10)]
    public void SupportsInt(int value)
    {
        var v = new Value(value);
        v.GetInt().Should().Be(value);

        EnsureBasicType(v, BasicType.Int);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SupportsBool(bool value)
    {
        var v = new Value(value);
        v.GetBoolean().Should().Be(value);

        EnsureBasicType(v, BasicType.Boolean);
    }

    [Theory]
    [InlineData(2.0)]
    [InlineData(-2.0)]
    public void SupportsDouble(double value)
    {
        var v = new Value(value);
        v.GetDouble().Should().Be(value);

        EnsureBasicType(v, BasicType.Double);
    }

    [Theory]
    [InlineData(2.0f)]
    [InlineData(-2.0f)]
    public void SupportsFloat(float value)
    {
        var v = new Value(value);
        v.GetFloat().Should().Be(value);

        EnsureBasicType(v, BasicType.Float);
    }

    [Theory]
    [InlineData(7u)]
    [InlineData(1000u)]
    public void SupportsLong(long value)
    {
        var v = new Value(value);
        v.GetInt64().Should().Be(value);

        //BasicType.Int64 because Int64 is 64bit wide on all systems (which long is not)
        EnsureBasicType(v, BasicType.Int64);
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("")]
    public void SupportsString(string value)
    {
        var v = new Value(value);
        v.GetString().Should().Be(value);

        EnsureBasicType(v, BasicType.String);
    }

    [Fact]
    public void VariantFromDataShouldContainGivenData()
    {
        var text = "foo";
        var variant = GLib.Variant.NewString(text);
        var v = new Value(variant);
        v.Extract<GLib.Variant>().GetString(out _).Should().Be(text);
    }

    [Fact]
    public void SupportsStringArray()
    {
        var array = new[] { "A", "B", "C" };
        var v = new Value(array);

        var result = v.Extract<string[]>();
        result.Should().ContainInOrder(array);

        v.GetStringArray().Should().ContainInOrder(array);
    }

    [Fact]
    public void GetStringArrayThrowsIfTypeIsNotAStringArray()
    {
        var v = new Value(true);

        var a = () => v.GetStringArray();
        a.Should().Throw<Exception>().WithMessage("Value does not hold a string array, but a 'gboolean'");
    }

    [Fact]
    // This test relies on the memory being zeroed out after it is freed, which is not guaranteed.
    // This does not happen on Windows, but seems reliable on other platforms.
    public void DisposeShouldFreeUnmanagedMemory()
    {
        if (System.OperatingSystem.IsWindows())
            Assert.Skip("Relies on freed memory being zeroed out, which is not reliable on Windows.");

        var value = 1;
        var v = new Value(value);
        var ptr = v.Handle.DangerousGetHandle();

        var d1 = Marshal.PtrToStructure<Internal.ValueData>(ptr);
        d1.Data[0].VInt.Should().Be(value);

        v.Dispose();

        //Try to read the data again despite the fact that the value is already disposed
        //If the value changed we can assume that the memory got freed.
        var d2 = Marshal.PtrToStructure<Internal.ValueData>(ptr);
        d2.Data[0].VInt.Should().NotBe(value);
    }
}
