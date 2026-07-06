using System.Runtime.CompilerServices;
using Xunit;

// GTK and the GObject type system are single-threaded: running test classes
// in parallel corrupts native state (Gtk-CRITICAL assertions). Everything in
// this assembly runs sequentially.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace CodeBrix.Develop.UI.Tests;

/*
 * Consolidates the per-suite [AssemblyInitialize] methods of the upstream
 * gir.core test projects: GtkSource.Module.Initialize() cascades through every
 * module in the GTK 4 dependency tree (Gdk, Gsk, Pango, Cairo, Gio,
 * GObject, GLib, ...), and the log setup makes native warnings fatal so
 * binding errors surface as test failures, matching upstream behavior.
 */

internal static class ModuleInitialization
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        GtkSource.Module.Initialize(); // cascades into Gtk.Module and the whole tree
        GLib.Functions.LogSetAlwaysFatal(
            GLib.LogLevelFlags.LevelCritical
            | GLib.LogLevelFlags.LevelError
            | GLib.LogLevelFlags.LevelWarning
        );
    }
}
