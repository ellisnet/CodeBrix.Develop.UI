using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.GLib.Tests; //was previously: GLib.Tests;

[Trait("Category", "UnitTest")]
public class VariantTypeTest : Test
{
    [Theory]
    [InlineData("s")]
    [InlineData("b")]
    [InlineData("y")]
    [InlineData("n")]
    [InlineData("q")]
    [InlineData("i")]
    [InlineData("u")]
    [InlineData("x")]
    [InlineData("t")]
    [InlineData("h")]
    [InlineData("v")]
    public void CanCreateTypeFromString(string type)
    {
        var variantType = VariantType.New(type);

        variantType.DupString().Should().Be(type);
    }

    [Fact]
    public void TypeStringIsString()
    {
        VariantType.String.DupString().Should().Be("s");
    }

    [Fact]
    public void TypeVariantIsVariant()
    {
        VariantType.Variant.DupString().Should().Be("v");
    }
}
