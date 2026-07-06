using System;

namespace CodeBrix.Develop.UI.Generator; //was previously: Generator;

public static class Configuration
{
    /// <summary>
    /// Optional namespace prefix prepended to every generated namespace
    /// (e.g. "CodeBrix.Develop.UI" turns the generated "Gtk" namespace into
    /// "CodeBrix.Develop.UI.Gtk"). Empty means upstream gir.core behavior.
    /// </summary>
    public static string NamespacePrefix { get; set; } = string.Empty;

    /// <summary>
    /// Optional mapper from a repository's canonical project name (e.g.
    /// "Gtk-4.0") to the on-disk project folder the generated files are
    /// written into (e.g. "CodeBrix.Develop.UI"). Null means the canonical
    /// name is used as the folder name, matching upstream gir.core behavior.
    /// </summary>
    public static Func<string, string>? ProjectFolderNameMapper { get; set; }

    public static void EnableVerboseOutput()
        => Log.EnableVerboseOutput();

    public static void EnableDebugOutput()
        => Log.EnableDebugOutput();
}
