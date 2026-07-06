# CodeBrix.Develop.UI

A fully managed C# binding of the GTK 4 user-interface toolkit, the
GtkSourceView source-code editing widget, and the supporting GNOME platform
libraries (Gdk, Gsk, Pango, Cairo, HarfBuzz, GdkPixbuf, Graphene, Gio,
GObject, and GLib) for .NET, derived from the
[gir.core](https://github.com/gircore/gir.core) project. CodeBrix.Develop.UI
has no dependencies other than .NET (plus the native GTK 4 runtime libraries at
run time), and is provided as a .NET 10 library and associated
`CodeBrix.Develop.UI` NuGet package.

CodeBrix.Develop.UI supports applications and assemblies that target Microsoft
.NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and
was released on Nov 11, 2025; and will be actively supported by Microsoft until
Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of
Microsoft .NET.

## CodeBrix.Develop.UI supports:

* Building GTK 4 desktop applications in C# — windows, widgets, layouts,
  dialogs, signals, and events
* The GtkSourceView 5 source-code editing widget — syntax highlighting,
  line numbers, code completion, and style schemes (the
  `CodeBrix.Develop.UI.GtkSource` namespace, inside the main assembly)
* The full GObject type system — subclassing, properties, and signals from
  managed code
* The supporting GNOME platform APIs that GTK 4 rests on: Gdk, Gsk, Pango,
  PangoCairo, Cairo, HarfBuzz, FreeType2, GdkPixbuf, Graphene, Gio, GObject,
  and GLib — each in its own namespace, all shipped in this one package
* Compile-time Roslyn source generators for GObject subclassing and GTK
  composite-template widgets, included in the package as analyzers
* Linux, Windows, and macOS, with the platform differences resolved by the
  binding generator

At run time your machine needs the native GTK 4 libraries installed (on
Debian-based Linux: `sudo apt install libgtk-4-1`; they ship with most Linux
desktops). Using the GtkSourceView binding additionally requires the native
GtkSourceView 5 library (`sudo apt install libgtksourceview-5-0`).

## Sample Code

### A minimal GTK 4 application

```csharp
using CodeBrix.Develop.UI.Gtk;
using GioFlags = CodeBrix.Develop.UI.Gio.ApplicationFlags;

var application = Application.New("com.example.hello", GioFlags.FlagsNone);
application.OnActivate += (sender, args) =>
{
    var window = ApplicationWindow.New((Application) sender);
    window.Title = "Hello from CodeBrix.Develop.UI";
    window.SetDefaultSize(400, 200);

    var button = Button.NewWithLabel("Click me");
    button.OnClicked += (_, _) => window.Close();

    window.SetChild(button);
    window.Present();
};
return application.RunWithSynchronizationContext(null);
```

## License

The project is licensed under the MIT License. see: https://en.wikipedia.org/wiki/MIT_License
