using System.Linq;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.Tests;

/*
 * Native-free smoke checks that exercise the shape of the produced
 * assemblies without loading the native GTK libraries.
 */

public class PackageSmoke
{
    [Fact]
    public void Gtk_application_type_lives_in_main_assembly()
        => typeof(Gtk.Application).Assembly.GetName().Name.Should().Be("CodeBrix.Develop.UI");

    [Fact]
    public void Gtk_namespace_is_prefixed()
        => typeof(Gtk.Application).Namespace.Should().Be("CodeBrix.Develop.UI.Gtk");

    [Fact]
    public void GObject_namespace_is_prefixed()
        => typeof(GObject.Object).Namespace.Should().Be("CodeBrix.Develop.UI.GObject");

    [Fact]
    public void GtkSource_view_type_lives_in_main_assembly()
        => typeof(GtkSource.View).Assembly.GetName().Name.Should().Be("CodeBrix.Develop.UI");

    [Fact]
    public void GtkSource_namespace_is_prefixed()
        => typeof(GtkSource.Buffer).Namespace.Should().Be("CodeBrix.Develop.UI.GtkSource");

    [Fact]
    public void Main_assembly_references_gdk_and_gsk()
    {
        //Arrange
        var referenced = typeof(Gtk.Application).Assembly
            .GetReferencedAssemblies()
            .Select(a => a.Name)
            .ToArray();

        //Assert
        referenced.Should().Contain("CodeBrix.Develop.UI.Gdk");
        referenced.Should().Contain("CodeBrix.Develop.UI.Gsk");
    }
}
