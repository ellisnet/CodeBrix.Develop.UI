using System;
using Xunit;

namespace CodeBrix.Develop.UI.GLib.Tests; //was previously: GLib.Tests;

[Trait("Category", "UnitTest")]
public class DirTest
{
    [Fact]
    public void CanDispose()
    {
        //TODO: Enable once Dir annotations are fixed
        //See: https://gitlab.gnome.org/GNOME/glib/-/merge_requests/3566
        Assert.Skip("Marked inconclusive in the upstream gir.core test suite");

        var dir = (IDisposable) Dir.Open(".", 0);
        dir.Dispose();
    }
}
