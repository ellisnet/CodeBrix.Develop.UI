================================================================================
AGENT-README: CodeBrix.Develop.UI
A Comprehensive Guide for AI Coding Agents
================================================================================

OVERVIEW
--------
CodeBrix.Develop.UI is a fully managed C# binding of the GTK 4 user-
interface toolkit, the GtkSourceView 5 source-code editing widget, and
their supporting GNOME platform libraries (Gdk, Gsk, Pango, PangoCairo,
Cairo, HarfBuzz, FreeType2, GdkPixbuf, Graphene, Gio, GObject, and GLib)
for .NET 10.0+. It lets C# applications build GTK 4
desktop UIs, use the GObject type system (subclassing, properties,
signals), and call the underlying GNOME platform APIs.

CodeBrix.Develop.UI is a fork of the gir.core project 
(https://github.com/gircore/gir.core) effectively in line with version 
0.8.1 commit a1fd8955 — see THIRD-PARTY-NOTICES.txt. All namespaces use
"CodeBrix.Develop.UI.*" instead of the upstream names (Gtk, Gdk, GLib,
GObject, ...). Do NOT use the upstream GirCore namespaces or package names.

Most of the binding code is NOT committed to this repository: it is
generated at development time by the CodeBrix.Develop.UI.BindingTool
command-line tool (the fork of upstream GirTool) from GObject
Introspection (.gir) XML data files, into per-project Generated/ folders
that are excluded by .gitignore. What IS committed is the hand-written
public-API / internal support code of each library project, the
generation tooling itself, and two Roslyn integration projects.


INSTALLATION
------------
NuGet Package: CodeBrix.Develop.UI
Dependencies: none (all 13 library assemblies ship inside this one package;
the GtkSourceView binding is inside CodeBrix.Develop.UI.dll itself)

    dotnet add package CodeBrix.Develop.UI

IMPORTANT: unlike most CodeBrix packages, the package id carries NO
license-suffix — it is CodeBrix.Develop.UI, not
CodeBrix.Develop.UI.MitLicenseForever. This is a deliberate, user-chosen
deviation from the family convention for this package.

Requirements: .NET 10.0 or higher.
Native runtime requirement: the GTK 4 native libraries must be installed
on the machine at run time (Debian-based Linux: sudo apt install
libgtk-4-1; included with most Linux desktops; Windows/macOS users
typically install GTK 4 via MSYS2 or Homebrew). The native libraries are
loaded dynamically and are never redistributed by this package.

License: MIT License


KEY NAMESPACES
--------------
    using CodeBrix.Develop.UI.Gtk;        // GTK 4 widgets and application (main API)
    using CodeBrix.Develop.UI.GtkSource;  // GtkSourceView 5 code-editing widget
                                          //   (compiled into CodeBrix.Develop.UI.dll)
    using CodeBrix.Develop.UI.Gdk;        // Windowing / display / events
    using CodeBrix.Develop.UI.Gsk;        // GPU render-node scene graph
    using CodeBrix.Develop.UI.Gio;        // Application framework, I/O, settings
    using CodeBrix.Develop.UI.GObject;    // GObject type system
    using CodeBrix.Develop.UI.GLib;       // Core utility / main loop
    using CodeBrix.Develop.UI.Pango;      // Text layout and shaping
    using CodeBrix.Develop.UI.PangoCairo; // Pango-on-Cairo rendering
    using CodeBrix.Develop.UI.Cairo;      // 2D vector drawing
    using CodeBrix.Develop.UI.HarfBuzz;   // Text shaping primitives
    using CodeBrix.Develop.UI.Freetype2;  // Font-face type stubs
    using CodeBrix.Develop.UI.GdkPixbuf;  // Image loading
    using CodeBrix.Develop.UI.Graphene;   // Geometry types (rects, matrices)

The assembly names match the project names (CodeBrix.Develop.UI.dll for
the Gtk library, CodeBrix.Develop.UI.Gdk.dll, ...); the Gtk binding lives
in the CodeBrix.Develop.UI.Gtk namespace inside CodeBrix.Develop.UI.dll.


CORE API REFERENCE
==================
(The generated API surface mirrors gir.core 0.8.0 with renamed
namespaces. This section will be expanded once the first generated build
is complete; until then, use gir.core 0.8.0 documentation/samples and
substitute namespaces.)

A minimal GTK 4 application:

    using CodeBrix.Develop.UI.Gtk;

    var application = Application.New(
        "com.example.hello",
        CodeBrix.Develop.UI.Gio.ApplicationFlags.FlagsNone);
    application.OnActivate += (sender, args) =>
    {
        var window = ApplicationWindow.New((Application) sender);
        window.Title = "Hello";
        window.SetDefaultSize(400, 200);
        window.Present();
    };
    return application.RunWithSynchronizationContext(null);


REPOSITORY LAYOUT / ARCHITECTURE
================================
    src/CodeBrix.Develop.UI/              GTK 4 binding + GtkSourceView 5 binding
                                          (the packable project; namespaces
                                          CodeBrix.Develop.UI.Gtk and
                                          CodeBrix.Develop.UI.GtkSource both
                                          compile into CodeBrix.Develop.UI.dll;
                                          the GtkSource sources live in its
                                          GtkSource/ subfolder). Because both
                                          bindings share one assembly, the Gtk
                                          Module registers a single CHAINED
                                          DllImportResolver serving both native
                                          libraries (see Public/Module.cs and
                                          GtkSource/Public/Module.cs).
    src/CodeBrix.Develop.UI.<Lib>/        12 sibling library projects (not packable)
      each library project contains:
        Public/     hand-written public-API code (committed)
        Internal/   hand-written internal support code (committed)
        Generated/  BindingTool output (gitignored, regenerated at will)
    src/Generation/
      CodeBrix.Develop.UI.GirModel/       object model of the GIR format
      CodeBrix.Develop.UI.GirLoader/      .gir XML parser/loader
      CodeBrix.Develop.UI.Generator/      C# code emitters
      CodeBrix.Develop.UI.BindingTool/    CLI driver (fork of upstream GirTool)
      Shared/                             non-project shared sources
    src/Integrations/
      CodeBrix.Develop.UI.GObject.Integration/  Roslyn source generator for
                                                GObject subclassing
      CodeBrix.Develop.UI.Gtk.Integration/      Roslyn source generator +
                                                analyzers for GTK composite
                                                templates
      Shared/                             non-project shared sources
    tests/CodeBrix.Develop.UI.Tests/      xUnit v3 test project

The single NuGet package (produced by src/CodeBrix.Develop.UI) ships all
13 library DLLs (and their XML doc files) in lib/net10.0/ and both
integration DLLs as Roslyn analyzers in analyzers/dotnet/cs/.


GENERATING THE BINDINGS (BindingTool)
=====================================
The Generated/ folders are NOT committed. After a fresh clone, generate
them with the BindingTool before building:

    dotnet run --project src/Generation/CodeBrix.Develop.UI.BindingTool -- \
        generate <gir-files> [options]

The full regeneration command, run from the repository root:

    dotnet run --project src/Generation/CodeBrix.Develop.UI.BindingTool -- \
        generate Gtk-4.0.gir GtkSource-5.gir Gdk-4.0.gir Gsk-4.0.gir Pango-1.0.gir \
        PangoCairo-1.0.gir cairo-1.0.gir HarfBuzz-0.0.gir freetype2-2.0.gir \
        GdkPixbuf-2.0.gir Graphene-1.0.gir Gio-2.0.gir GObject-2.0.gir \
        GLib-2.0.gir --output src

BindingTool acquires its .gir input files from the companion snapshot
repository https://github.com/ellisnet/CodeBrix.Develop.UI.GnomeIntrospection
at a PINNED COMMIT (see GirFileAcquisition.DefaultGirRef), caching the
download in ./.gir-cache/ (gitignored) so later runs are offline. It must
NEVER pull anything from github.com/gircore/*. Options:

    -np|--namespace-prefix   prefix for generated namespaces
                             (default: CodeBrix.Develop.UI)
    -gs|--gir-source <dir>   use a local GnomeIntrospection checkout
                             instead of downloading
    -gr|--gir-ref <ref>      download a different commit/tag/branch
    -gc|--gir-cache <dir>    cache folder (default: .gir-cache)

Every generated file gets a "// <auto-generated/>" + blanket
"#pragma warning disable" header (see Generator's Publisher.cs) so compiler
warnings in generated code cannot break the zero-warnings build; the
project-folder mapping from canonical names (Gtk-4.0 -> CodeBrix.Develop.UI,
Gdk-4.0 -> CodeBrix.Develop.UI.Gdk, ...) lives in the BindingTool's
ProjectFolderMapping.cs.


CODING CONVENTIONS (CodeBrix family)
------------------------------------
- Target framework net10.0 only; no multi-targeting. THE ONE EXCEPTION is the
  two Roslyn integration projects under src/Integrations/, which target
  netstandard2.0 because a compiler extension has to be loadable by every
  compiler host (including the .NET Framework-hosted compiler inside Visual
  Studio), and because Roslyn's RS1041 rule enforces exactly that. Do not
  "fix" them to net10.0 - each .csproj opens with a long note explaining why.
  Their two packaging paths in src/CodeBrix.Develop.UI/CodeBrix.Develop.UI.csproj
  name the netstandard2.0 output folder and must move with them if that ever
  changes. Because netstandard2.0 is an older surface, those two projects also
  set <LangVersion>latest</LangVersion>, avoid static abstract interface
  members and the .NET 6+ StringBuilder.AppendLine(IFormatProvider, ...)
  overloads, and pick up compiler-required shim types from
  src/Integrations/Shared/Compatibility/.
- File-scoped namespaces only. Ported files carry a
  "//was previously: <upstream-namespace>;" comment on the namespace line.
  (The two shim files under src/Integrations/Shared/Compatibility/ declare
  System.* namespaces instead; they are BCL back-fills, not CodeBrix code.)
- No global usings; all using directives at the top of each file.
- SITUATIONAL EXCEPTION (documented): unlike most CodeBrix repos, this
  repository keeps <Nullable>enable</Nullable> and nullable reference
  type annotations (`?`), because the upstream gir.core code and its
  generated bindings are deeply NRT-dependent and stripping annotations
  would change public signatures (same rationale as
  CodeBrix.Platform.OpenGL). Also like upstream, generated code is too
  large to fully XML-document, so the library projects pair
  <GenerateDocumentationFile>true</> with <NoWarn>1591</NoWarn> (same
  rationale as CodeBrix.Platform.OpenGL).
- Tests: xUnit v3 + SilverAssertions fluent assertions; test classes named
  <ClassUnderTest>Tests; test methods Member_snake_case or snake_case;
  //Arrange //Act //Assert comments in multi-statement tests.
- The packable csproj uses the canonical CodeBrix date-stamped version
  block (1.<years>.<dayOfYear>.<minuteOfDay>); never hardcode <Version>.
- Copyright line: "Copyright (c) 2026 Jeremy Ellis and contributors"
  prepended with the upstream attribution "Copyright (c) 2020 Gir-Core"
  per MIT notice-preservation.


TESTING
-------
    dotnet test CodeBrix.Develop.UI.slnx

With code coverage (cobertura is the portable format; omit the format
option to get Microsoft's binary .coverage instead). Output lands in
TestResults/ at the repository root, which is gitignored:

    dotnet test CodeBrix.Develop.UI.slnx --coverage \
        --coverage-output-format cobertura

The test project runs on Microsoft.Testing.Platform (MTP), not VSTest:
the legacy VSTest host crashes (SIGSEGV) tearing down the natively
initialized GTK runtime. Do not add Microsoft.NET.Test.Sdk or
xunit.runner.visualstudio back to the test csproj.

Three consequences of the MTP choice, all of which differ from the rest
of the CodeBrix family and none of which should be "simplified" away:

  - global.json at the repository root opts "dotnet test" into the MTP
    runner ("test": { "runner": "Microsoft.Testing.Platform" }). From MTP
    v2 on the .NET 10 SDK this is REQUIRED - driving MTP through the old
    VSTest MSBuild target is a hard error. That file pins no SDK version
    and exists only for this opt-in.
  - The test project references xunit.v3.mtp-v2, not plain xunit.v3.
    Both are the same stable 3.2.2 release; the default package binds to
    MTP v1, and mixing MTP v1 and v2 throws a TypeLoadException at
    startup. See the note in the test csproj.
  - Coverage comes from Microsoft.Testing.Extensions.CodeCoverage, not
    the family's former coverlet.collector. coverlet.collector is a VSTest
    data collector and produced NO coverage at all here while still
    warning (MTP0001). The family is retiring coverlet generally (it was
    referenced everywhere but never invoked); VSTest-based repos can use
    "dotnet test --collect \"Code Coverage\"" via Microsoft.CodeCoverage
    (MIT), which ships inside Microsoft.NET.Test.Sdk. That route is closed
    to this project because it cannot use VSTest. NOTE the licensing
    difference: coverlet is MIT, whereas the Microsoft coverage extension
    ships under the proprietary MICROSOFT .NET LIBRARY license with
    requireLicenseAcceptance=true. It is a dev-time-only dependency and
    is not redistributed by the CodeBrix.Develop.UI package.

Test-assembly parallelization is DISABLED (assembly-level
CollectionBehavior attribute in ModuleInitialization.cs): GTK and the
GObject type system are single-threaded, and parallel test classes
corrupt native state. ModuleInitialization.cs also runs
Gtk.Module.Initialize() (which cascades through the whole dependency
tree) and makes native GLib warnings fatal, matching upstream behavior.

Tests require the native GTK 4 libraries on the host (Debian:
sudo apt install libgtk-4-1; GtkSourceView runtime tests additionally
need libgtksourceview-5-0) and construct real GTK widgets, so a
display session is expected. The suites under GLib/, GObject/, Gio/,
Cairo/, and Gtk/ are ports of the upstream gir.core MSTest suites
(MSTest -> xUnit v3, AwesomeAssertions -> SilverAssertions); three tests
are skipped because they were marked inconclusive upstream.
================================================================================
