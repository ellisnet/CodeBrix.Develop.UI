using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.Gtk.Tests; //was previously: Gtk.Tests;

[Trait("Category", "SystemTest")]
public class ConstructorTest : Test
{
    [Fact]
    public void WindowConstructorShouldSetTitle()
    {
        var title = "MyTitle";
        var label = Label.New(title);

        label.GetLabel().Should().Be(title);
    }

    [Fact]
    public void CreateLabelWithNullTextShouldNotThrow()
    {
        System.Action createLabelWithNullText = () => Label.New(null);
        createLabelWithNullText.Should().NotThrow();
    }
}
