using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.Gtk.Tests; //was previously: Gtk.Tests;

[Trait("Category", "SystemTest")]
public class InterfaceTest : Test
{
    [Fact]
    public void CanSetInterfaceProperty()
    {
        var entry = Entry.New();
        entry.Editable.Should().BeTrue();
        entry.Editable = false;
        entry.Editable.Should().BeFalse();
        entry.Editable = true;
        entry.Editable.Should().BeTrue();
    }

    [Fact]
    public void CanCallInterfaceMethod()
    {
        const string Text = "test";
        var entry = Entry.New();
        entry.SetText(Text);
        entry.GetText().Should().Be(Text);
    }
}
