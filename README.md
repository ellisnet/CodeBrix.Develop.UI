# CodeBrix.Develop.UI

A fully managed C# binding of the GTK 4 user-interface toolkit, the
GtkSourceView source-code editing widget, and the supporting GNOME platform
libraries (Gdk, Gsk, Pango, PangoCairo, Cairo, HarfBuzz, FreeType2, GdkPixbuf,
Graphene, Gio, GObject, and GLib), for developers building desktop
applications in C#. CodeBrix.Develop.UI is provided as a .NET 10 library and
associated `CodeBrix.Develop.UI` NuGet package.

CodeBrix.Develop.UI supports applications and assemblies that target Microsoft
.NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of
Microsoft .NET.

## Installation

```
dotnet add package CodeBrix.Develop.UI
```

The package has no NuGet dependencies. Note that the NuGet package ID names
the package root, but no types live in a namespace called plain
`CodeBrix.Develop.UI` - every type is in a per-library sub-namespace, and it is
those you import:

* NuGet package ID: `CodeBrix.Develop.UI`
* Namespaces: `CodeBrix.Develop.UI.Gtk`, `.GtkSource`, `.Gdk`, `.Gsk`, `.Gio`,
  `.GObject`, `.GLib`, `.Pango`, `.PangoCairo`, `.Cairo`, `.HarfBuzz`,
  `.Freetype2`, `.GdkPixbuf`, `.Graphene` - i.e.
  `using CodeBrix.Develop.UI.Gtk;`

All of those namespaces ship inside this one package, together with the two
Roslyn source generators as analyzers. XML documentation (IntelliSense) ships
alongside the assemblies.

No native binaries are bundled, and there is no self-contained mode: the
machine that runs your application needs the native GTK 4 libraries installed
(on Debian-based Linux, `sudo apt install libgtk-4-1`; they ship with most
Linux desktops). Using the GtkSourceView binding additionally requires the
native GtkSourceView 5 library (`sudo apt install libgtksourceview-5-0`).

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

### A syntax-highlighting source editor

```csharp
using CodeBrix.Develop.UI.GtkSource;

var buffer = Buffer.New(null);
buffer.SetLanguage(LanguageManager.GetDefault().GetLanguage("c-sharp"));
buffer.SetHighlightSyntax(true);
buffer.SetText("public class Hello { }", -1);

var view = View.NewWithBuffer(buffer);
view.SetShowLineNumbers(true);
view.SetTabWidth(4);
view.SetMonospace(true);
```

## Documentation

The NuGet package includes `AGENT-README.txt`, a complete API reference and usage guide written for AI coding agents - point your agent at that file when it is writing code against this library.

Additional sample code and usage examples are available in the `CodeBrix.Develop.UI.Tests` project:
https://github.com/ellisnet/CodeBrix.Develop.UI/tree/main/tests/CodeBrix.Develop.UI.Tests

## License

CodeBrix.Develop.UI is licensed under the MIT License - see the
[LICENSE](https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/LICENSE) file.

For licensing and provenance information about the open source code included in
this package, see [THIRD-PARTY-NOTICES.txt](https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/THIRD-PARTY-NOTICES.txt).
