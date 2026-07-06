using System.Collections.Generic;

namespace CodeBrix.Develop.UI.BindingTool;

/// <summary>
/// Maps a repository's canonical project name (e.g. "Gtk-4.0") to the
/// CodeBrix.Develop.UI project folder its generated files belong in. The
/// GTK 4 binding itself is the packable CodeBrix.Develop.UI project; every
/// other library in its dependency tree gets a CodeBrix.Develop.UI.* sibling.
/// </summary>
internal static class ProjectFolderMapping
{
    private static readonly Dictionary<string, string> KnownProjects = new()
    {
        ["Gtk-4.0"] = "CodeBrix.Develop.UI",
        ["GtkSource-5"] = "CodeBrix.Develop.UI/GtkSource",
        ["Gdk-4.0"] = "CodeBrix.Develop.UI.Gdk",
        ["Gsk-4.0"] = "CodeBrix.Develop.UI.Gsk",
        ["Pango-1.0"] = "CodeBrix.Develop.UI.Pango",
        ["PangoCairo-1.0"] = "CodeBrix.Develop.UI.PangoCairo",
        ["cairo-1.0"] = "CodeBrix.Develop.UI.Cairo",
        ["HarfBuzz-0.0"] = "CodeBrix.Develop.UI.HarfBuzz",
        ["freetype2-2.0"] = "CodeBrix.Develop.UI.Freetype2",
        ["GdkPixbuf-2.0"] = "CodeBrix.Develop.UI.GdkPixbuf",
        ["Graphene-1.0"] = "CodeBrix.Develop.UI.Graphene",
        ["Gio-2.0"] = "CodeBrix.Develop.UI.Gio",
        ["GObject-2.0"] = "CodeBrix.Develop.UI.GObject",
        ["GLib-2.0"] = "CodeBrix.Develop.UI.GLib",
    };

    public static string Map(string canonicalProjectName)
    {
        if (KnownProjects.TryGetValue(canonicalProjectName, out var folderName))
            return folderName;

        // Fallback for libraries that are not (yet) part of the GTK 4
        // dependency tree: strip the version suffix and PascalCase the name.
        var name = canonicalProjectName.Split('-')[0];
        return "CodeBrix.Develop.UI." + char.ToUpperInvariant(name[0]) + name[1..];
    }
}
