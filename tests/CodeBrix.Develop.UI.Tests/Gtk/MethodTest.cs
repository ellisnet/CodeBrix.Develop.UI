using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.Gtk.Tests; //was previously: Gtk.Tests;

[Trait("Category", "SystemTest")]
public class MethodTest
{
    [Fact]
    public void CanReturnNullIfReturnTypeIsInterface()
    {
        var fc = FileChooserNative.New("test", null, FileChooserAction.SelectFolder, "OK", "Cancel");
        fc.GetFile().Should().BeNull();
    }

    [Fact]
    public void CanReturnNullIfReturnTypeIsClass()
    {
        var l = Label.New("test");
        l.GetExtraMenu().Should().BeNull();
    }
}
