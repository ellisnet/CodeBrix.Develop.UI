using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.GtkSource.Tests;

/*
 * Runtime smoke tests for the GtkSourceView 5 binding. These exercise the
 * native libgtksourceview-5 library (Debian: sudo apt install
 * libgtksourceview-5-0) through the chained DllImportResolver that the
 * shared CodeBrix.Develop.UI assembly registers for both Gtk and GtkSource.
 */

[Trait("Category", "SystemTest")]
public class GtkSourceTest
{
    [Fact]
    public void Buffer_holds_and_returns_text()
    {
        //Arrange
        var buffer = Buffer.New(null);

        //Act
        buffer.SetText("let answer = 42;", -1);

        //Assert
        buffer.GetBounds(out var start, out var end);
        buffer.GetText(start, end, includeHiddenChars: true).Should().Be("let answer = 42;");
    }

    [Fact]
    public void View_can_be_created_with_buffer()
    {
        //Arrange
        var buffer = Buffer.New(null);

        //Act
        var view = View.NewWithBuffer(buffer);

        //Assert
        view.GetBuffer().Should().Be(buffer);
    }

    [Fact]
    public void Language_manager_knows_csharp()
        => LanguageManager.GetDefault().GetLanguage("c-sharp").Should().NotBeNull();

    [Fact]
    public void Buffer_accepts_a_highlighting_language()
    {
        //Arrange
        var buffer = Buffer.New(null);
        var language = LanguageManager.GetDefault().GetLanguage("c-sharp");

        //Act
        buffer.SetLanguage(language);

        //Assert
        buffer.GetLanguage().Should().Be(language);
    }
}
