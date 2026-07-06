using System;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.Cairo.Tests; //was previously: Cairo.Tests;

[Trait("Category", "UnitTest")]
public class FontFaceTest : Test
{
    [Fact]
    public void ImplementsIDisposable()
    {
        typeof(FontFace).Should().Implement<IDisposable>();
    }

    [Fact]
    public void BindingsShouldSucceed()
    {
        var face = new ToyFontFace("serif", FontSlant.Italic, FontWeight.Bold);
        face.Status.Should().Be(Status.Success);
        face.FontType.Should().Be(FontType.Toy);
        face.Family.Should().Be("serif");
        face.Slant.Should().Be(FontSlant.Italic);
        face.Weight.Should().Be(FontWeight.Bold);
    }
}
