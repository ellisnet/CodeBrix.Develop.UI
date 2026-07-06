using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.Cairo.Tests; //was previously: Cairo.Tests;

[Trait("Category", "UnitTest")]
public class MatrixTest : Test
{
    [Fact]
    public void BindingsShouldSucceed()
    {
        var matrix = new Matrix();
        matrix.InitIdentity();
        matrix.Init(1, 0, 0, 1, 2, 3);

        matrix.InitTranslate(1, 2);
        matrix.InitScale(3, 4);
        matrix.InitRotate(System.Math.PI);

        matrix.Translate(1, 2);
        matrix.Scale(3, 4);
        matrix.Rotate(System.Math.PI);

        matrix.Invert().Should().Be(Status.Success);

        double x = 0;
        double y = 0;
        matrix.TransformPoint(ref x, ref y);
        x.Should().NotBe(0);

        x = 1;
        y = 1;
        matrix.TransformDistance(ref x, ref y);
        x.Should().NotBe(1);
    }
}
