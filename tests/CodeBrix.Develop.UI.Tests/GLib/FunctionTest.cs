using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.GLib.Tests; //was previously: GLib.Tests;

[Trait("Category", "UnitTest")]
public class FunctionTest : Test
{
    [Fact]
    public void CanSetApplicationName()
    {
        // Simple test of global functions.
        GLib.Functions.GetApplicationName().Should().BeNull();
        GLib.Functions.SetApplicationName("foo");
        GLib.Functions.GetApplicationName().Should().Be("foo");
    }
}

