using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.GLib.Tests; //was previously: GLib.Tests;

[Trait("Category", "UnitTest")]
public class MemoryManagementTest : Test
{
    [Fact]
    public void UnownedHandleIsNotFreed()
    {
        //TODO: Enable once Dir annotations are fixed
        //See: https://gitlab.gnome.org/GNOME/glib/-/merge_requests/3566
        Assert.Skip("Marked inconclusive in the upstream gir.core test suite");

        var reference = new System.WeakReference(null);
        CollectAfter(() =>
        {
            reference.Target = Dir.Open("..", 0);
        });

        // Dir.Open creates a handle to a directory.
        // This handle does not transfer ownership. This
        // means we are not allowed to free the handle.
        // There is no method to free the handle, for this
        // reason we throw an exception if the handle is
        // tried to be freed.

        reference.IsAlive.Should().BeFalse();
    }
}
