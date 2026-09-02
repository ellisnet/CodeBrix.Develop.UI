================================================================================
MAINTAINER-README: CodeBrix.Develop.UI
Notes for people and agents MAINTAINING this repository — not for package
consumers
================================================================================

If you are CONSUMING the CodeBrix.Develop.UI NuGet package, read
AGENT-README.txt instead. This file is about building, regenerating, testing
and publishing the repository itself.


PURPOSE AND SCOPE
=================
This repository produces exactly ONE NuGet package:

    PackageId   CodeBrix.Develop.UI
    Project     src/CodeBrix.Develop.UI/CodeBrix.Develop.UI.csproj
    Covered by  AGENT-README.txt (repository root)
    License     MIT

Note the package id carries NO license suffix — it is CodeBrix.Develop.UI, not
CodeBrix.Develop.UI.MitLicenseForever. This is a deliberate, user-chosen
deviation from the CodeBrix family convention for this package. Do not
"correct" it.

Every other project in the solution sets IsPackable=false. The single package
carries thirteen library assemblies in lib/net10.0/ and two Roslyn assemblies
in analyzers/dotnet/cs/.


REPOSITORY LAYOUT
=================
    src/CodeBrix.Develop.UI/           GTK 4 binding + GtkSourceView 5 binding.
                                       The one packable project. The
                                       CodeBrix.Develop.UI.Gtk and
                                       CodeBrix.Develop.UI.GtkSource namespaces
                                       both compile into
                                       CodeBrix.Develop.UI.dll; the GtkSource
                                       sources live in its GtkSource/
                                       subfolder.
    src/CodeBrix.Develop.UI.<Lib>/     Twelve sibling library projects (Cairo,
                                       Freetype2, Gdk, GdkPixbuf, Gio, GLib,
                                       GObject, Graphene, Gsk, HarfBuzz, Pango,
                                       PangoCairo). Not packable; they ship
                                       inside the one package.
      Public/                          hand-written public API (committed) plus
                                       generated *.Generated.cs (gitignored)
      Internal/                        hand-written internal support code plus
                                       generated internals
    src/Generation/
      CodeBrix.Develop.UI.GirModel/    object model of the GIR format
      CodeBrix.Develop.UI.GirLoader/   .gir XML parser / loader
      CodeBrix.Develop.UI.Generator/   the C# code emitters
      CodeBrix.Develop.UI.BindingTool/ CLI driver (the fork of upstream GirTool)
      Shared/                          non-project shared sources
    src/Integrations/
      CodeBrix.Develop.UI.GObject.Integration/  Roslyn source generator +
                                                analyzer for GObject
                                                subclassing (GirCore1002,
                                                1004, 1005, 1006, 1008)
      CodeBrix.Develop.UI.Gtk.Integration/      Roslyn source generator +
                                                analyzer for GTK composite
                                                templates (GirCore2001, 2002,
                                                2003)
      Shared/                          shared TypeData / attribute helpers and
                                       the netstandard2.0 compatibility shims
    tests/CodeBrix.Develop.UI.Tests/   xUnit v3 test project
    .gir-cache/                        downloaded GIR snapshots (gitignored)
    CodeBrix.Develop.UI.slnx           the solution
    global.json                        opts `dotnet test` into MTP (see TESTING)

MOST OF THE BINDING IS NOT COMMITTED. Generated files are named
`*.Generated.cs` and are excluded by .gitignore. What IS committed is the
hand-written public/internal support code of each library project, the
generation tooling, the two Roslyn integration projects, and the tests.


GENERATING THE BINDINGS (BindingTool)
=====================================
After a fresh clone the `*.Generated.cs` files do not exist; generate them
before building.

    dotnet run --project src/Generation/CodeBrix.Develop.UI.BindingTool -- \
        generate Gtk-4.0.gir GtkSource-5.gir Gdk-4.0.gir Gsk-4.0.gir \
        Pango-1.0.gir PangoCairo-1.0.gir cairo-1.0.gir HarfBuzz-0.0.gir \
        freetype2-2.0.gir GdkPixbuf-2.0.gir Graphene-1.0.gir Gio-2.0.gir \
        GObject-2.0.gir GLib-2.0.gir --output src

Commands:

    generate <gir files...>     emit C# into the per-project folders
    clean <target folder>       delete every *.Generated.cs beneath a folder

`generate` options (the defaults are what the repository expects):

    -o  | --output <dir>          output root                (default ./src)
    -np | --namespace-prefix <s>  generated namespace prefix
                                  (default CodeBrix.Develop.UI)
    -gs | --gir-source <dir>      use a local GnomeIntrospection checkout and
                                  skip the download
    -gr | --gir-ref <ref>         git ref of the GIR repository to download
                                  (default GirFileAcquisition.DefaultGirRef)
    -gc | --gir-cache <dir>       download cache (default .gir-cache)
    -sl | --search-path-linux <dir>    dependent Linux GIR files
    -sm | --search-path-macos <dir>    dependent macOS GIR files
    -sw | --search-path-windows <dir>  dependent Windows GIR files
    -d  | --disable-async         generate synchronously (for debugging)
    -l  | --log-level <level>     log verbosity

GIR INPUTS
----------
The .gir XML data files are NOT in this repository. BindingTool downloads them
from the companion snapshot repository

    https://github.com/ellisnet/CodeBrix.Develop.UI.GnomeIntrospection

at a PINNED commit recorded in
src/Generation/CodeBrix.Develop.UI.BindingTool/GirFileAcquisition.cs
(`RepositoryOwner`, `RepositoryName`, `DefaultGirRef`, `DefaultCacheFolder`).
Downloads are cached under ./.gir-cache/<ref>/{linux,macos,windows}/ so later
runs are offline. The snapshot repository carries per-platform GIR files, which
is how the generator resolves platform differences.

BindingTool must NEVER pull anything from github.com/gircore/*. To move to a
newer GNOME release, update the GnomeIntrospection repository first, then bump
`DefaultGirRef`.

Generator conventions worth knowing before changing the emitters:

  * Every generated file starts with `// <auto-generated/>` and a blanket
    `#pragma warning disable` (see the Generator's Publisher), so warnings in
    generated code can never break the zero-warnings build.
  * The canonical-name to project-folder mapping (Gtk-4.0 ->
    CodeBrix.Develop.UI, Gdk-4.0 -> CodeBrix.Develop.UI.Gdk, ...) lives in the
    BindingTool's ProjectFolderMapping.
  * GtkSource-5 maps into the main project's GtkSource/ subfolder, not a
    project of its own.
  * Callback handlers BORROW their transfer-none record arguments. The record
    converters under Renderer/Internal/ParameterToManagedExpression/Converter/
    register a post-call Dispose (BorrowedRecordDisposal.cs, same folder's
    parent) so the private copy or reference that OwnedHandle.FromUnowned
    took is released when the managed callback returns, not at finalization.
    Upstream gir.core does not do this (checked 2026-09-01); the omission was
    the frame-rate cairo-context leak in DrawingArea draw functions that
    OOM-killed CodeBrix.Develop. Keep the Register() call when porting
    upstream changes to those converters, and never emit it for a record
    whose public class is not IDisposable (opaque untyped records without a
    free function).


BUILDING
========
    dotnet build CodeBrix.Develop.UI.slnx -c Release

Build the WHOLE solution, not the single packable project: the two Roslyn
integration assemblies must exist in the same configuration before the package
can pick them up (see PACKAGING AND PUBLISHING).

Target framework is net10.0 everywhere EXCEPT the two projects under
src/Integrations/, which target netstandard2.0 because a Roslyn compiler
extension must be loadable by every compiler host, including the .NET
Framework-hosted compiler inside Visual Studio, and because Roslyn's RS1041
rule enforces exactly that. Do NOT "fix" them to net10.0; each .csproj opens
with a note explaining why, and the two packaging paths in
src/CodeBrix.Develop.UI/CodeBrix.Develop.UI.csproj name the netstandard2.0
output folder and must move with them if that ever changes.


TESTING
=======
    dotnet test CodeBrix.Develop.UI.slnx

With coverage (cobertura is the portable format; omit the format option for
Microsoft's binary .coverage). Output lands in TestResults/ at the repository
root, which is gitignored:

    dotnet test CodeBrix.Develop.UI.slnx --coverage \
        --coverage-output-format cobertura

Host prerequisites: the native GTK 4 libraries (Debian: `sudo apt install
libgtk-4-1`), plus `libgtksourceview-5-0` for the GtkSource suite. The tests
construct real GTK widgets, so a display session is expected.

The test project runs on Microsoft.Testing.Platform (MTP), not VSTest: the
legacy VSTest host crashes (SIGSEGV) tearing down the natively initialized GTK
runtime, while the in-process xunit.v3 runner works reliably. Do not add
Microsoft.NET.Test.Sdk or xunit.runner.visualstudio back to the test csproj.

Three consequences of the MTP choice, all differing from the rest of the
CodeBrix family, none of which should be "simplified" away:

  * global.json at the repository root opts `dotnet test` into the MTP runner
    (`"test": { "runner": "Microsoft.Testing.Platform" }`). From MTP v2 on the
    .NET 10 SDK this is REQUIRED — driving MTP through the old VSTest MSBuild
    target is a hard error. That file pins no SDK version and exists only for
    this opt-in.
  * The test project references `xunit.v3.mtp-v2`, not plain `xunit.v3`. Both
    are the same stable xunit.v3 release; the default package binds to MTP v1,
    and mixing MTP v1 and v2 throws a TypeLoadException at startup. See the
    long note in the test csproj.
  * Coverage comes from Microsoft.Testing.Extensions.CodeCoverage, not the
    family's former coverlet.collector. coverlet.collector is a VSTest data
    collector: it produced NO coverage here while still warning (MTP0001). Note
    the licensing difference — coverlet is MIT, whereas the Microsoft coverage
    extension ships under the proprietary MICROSOFT .NET LIBRARY license with
    requireLicenseAcceptance=true. It is a dev-time-only dependency and is not
    redistributed by the package.

Test-assembly parallelization is DISABLED (an assembly-level
`[CollectionBehavior(DisableTestParallelization = true)]` in
ModuleInitialization.cs): GTK and the GObject type system are single-threaded,
and parallel test classes corrupt native state. ModuleInitialization.cs also
runs `GtkSource.Module.Initialize()` (which cascades through the whole
dependency tree) from a `[ModuleInitializer]`, and calls
`GLib.Functions.LogSetAlwaysFatal(...)` so native GLib criticals, errors and
warnings surface as test failures — matching upstream behaviour.

Suites: Cairo/, Gio/, GLib/, GObject/, Gtk/, GtkSource/, plus PackageSmoke.cs.
The Cairo, GLib, GObject, Gio and Gtk suites are ports of the upstream MSTest
suites (MSTest -> xUnit v3, AwesomeAssertions -> SilverAssertions); a few tests
are skipped because they were marked inconclusive upstream. PackageSmoke.cs is
native-free and asserts the shipped assembly/namespace shape.


PACKAGING AND PUBLISHING
========================
There is NO separate pack step. src/CodeBrix.Develop.UI sets
GeneratePackageOnBuild=true, so an ordinary Release build of the solution also
writes the .nupkg:

    dotnet build CodeBrix.Develop.UI.slnx -c Release
    -> src/CodeBrix.Develop.UI/bin/Release/CodeBrix.Develop.UI.<version>.nupkg

BEWARE: the build log never mentions the .nupkg. Every project line in the
output ends in a .dll path, including the one for CodeBrix.Develop.UI, so a
successful build looks exactly like a build that packed nothing. The only way
to confirm the package exists is to look in bin/Release. Do not conclude from a
clean build log that packaging is unconfigured.

Versioning: the canonical CodeBrix date-stamped scheme, computed in the csproj
from `System.DateTime.UtcNow` as
1.<years since _VersionBaseYear>.<UTC day of year>.<UTC minute of day>. Never
hardcode <Version>. Because the value depends on the clock, EVERY build mints a
new version and bin/Release ACCUMULATES packages (they are gitignored and never
cleaned). The highest version number is the newest file:

    ls -la src/CodeBrix.Develop.UI/bin/Release/*.nupkg

Two builds within the SAME UTC minute produce the SAME version, so never
publish two packages from within one minute of each other.

Verify the package contents before uploading (a nupkg is a zip):

    cd $(mktemp -d) && unzip -q <path-to>/CodeBrix.Develop.UI.<version>.nupkg
    find . -name '*.dll' | sort

A correct package contains exactly 15 assemblies:

    lib/net10.0/          13 DLLs (CodeBrix.Develop.UI.dll plus the twelve
                          sibling library DLLs) with their .xml doc files
    analyzers/dotnet/cs/  2 DLLs (CodeBrix.Develop.UI.Gtk.Integration,
                          CodeBrix.Develop.UI.GObject.Integration)

plus icon-codebrix-128.png, README.md, AGENT-README.txt and
THIRD-PARTY-NOTICES.txt at the package root. AGENT-README.txt is packed from
the repository root by the packable csproj — if that file is renamed or moved,
the csproj `<None Include="..\..\AGENT-README.txt" ...>` entry must move with
it. MAINTAINER-README.txt, EXTRAS-README.txt and README-INDEX.txt are NOT
packed; they are repository-only.

If lib/net10.0 holds only CodeBrix.Develop.UI.dll, or analyzers/ is missing,
the package is broken — do NOT upload it. The sibling DLLs get in through the
custom `IncludeReferencedProjectBuildOutputs` target (the project references
are PrivateAssets="all" so they are not emitted as NuGet dependencies). The
analyzer entries are packed from hard-coded
..\Integrations\...\bin\$(Configuration)\netstandard2.0\ paths, so they
silently pack nothing unless those two projects were built in the SAME
configuration first. Building the whole .slnx guarantees that ordering; packing
the single project in a clean tree may not.

Publishing (by hand; there is no CI publish for this repository):

    cd src/CodeBrix.Develop.UI/bin/Release
    dotnet nuget push CodeBrix.Develop.UI.<version>.nupkg \
        --source https://api.nuget.org/v3/index.json --api-key <key>

Tag the published version in git as v<version>, matching the
`<repository commit="...">` recorded in the package's .nuspec. Build from a
CLEAN working tree so that commit hash actually describes what shipped.


PROVENANCE AND VENDORED SOURCES
===============================
This repository is a fork of the gir.core project
(https://github.com/gircore/gir.core), upstream commit
41bd0a6b0855ec4f50c2f702d394333c4c17c3f3, with six later upstream commits
cherry-picked. THIRD-PARTY-NOTICES.txt is the authoritative record: it lists
the exact upstream commits, the scope of incorporation (generation tooling,
the thirteen library projects, the two integration projects, the test suites),
the modifications made in this fork, two deliberate divergences (the abstract
subclass renderer, and the post-call disposal of borrowed record arguments in
callback handlers), and the MIT notice that must be preserved. Update it
whenever upstream code is pulled in.

The GIR data files are covered by entry 2 of THIRD-PARTY-NOTICES.txt and live
in the separate snapshot repository named under GIR INPUTS above; they carry
the licenses of the libraries they describe (predominantly
LGPL-2.1-or-later).

The upstream diagnostic IDs (GirCore1002 ... GirCore2003) and their help links
are KEPT AS-IS on purpose. Do not renumber or rebrand them.


CODING CONVENTIONS
==================
* Target framework net10.0 only; no multi-targeting. The one exception is the
  two Roslyn integration projects (netstandard2.0) — see BUILDING. Because
  netstandard2.0 is an older surface, those two projects also set
  <LangVersion>latest</LangVersion>, avoid static abstract interface members
  and the .NET 6+ StringBuilder.AppendLine(IFormatProvider, ...) overloads, and
  pick up compiler-required shim types from
  src/Integrations/Shared/Compatibility/.

* File-scoped namespaces only. Ported files carry a
  `//was previously: <upstream-namespace>;` comment on the namespace line. (The
  two shim files under src/Integrations/Shared/Compatibility/ declare System.*
  namespaces instead; they are BCL back-fills, not CodeBrix code.)

* No global usings; all using directives at the top of each file.

* SITUATIONAL EXCEPTION (documented): unlike most CodeBrix repositories, this
  one KEEPS <Nullable>enable</Nullable> and nullable reference type annotations
  (`?`), because the upstream code and its generated bindings are deeply
  NRT-dependent and stripping annotations would change public signatures (the
  same rationale as CodeBrix.Platform.OpenGL). Also like upstream, generated
  code is too large to fully XML-document, so the library projects pair
  <GenerateDocumentationFile>true</> with <NoWarn>1591</NoWarn>.

* src/CodeBrix.Develop.UI/InternalsVisibleTo.cs grants the test project access,
  per the family convention.

* Tests: xUnit v3 + SilverAssertions; test classes named <ClassUnderTest>Tests
  (the ported upstream suites keep their upstream names, e.g. ConstructorTest,
  SignalTest); test methods snake_case or Member_snake_case;
  //Arrange //Act //Assert comments in multi-statement tests.

* Copyright line: "Copyright (c) 2026 Jeremy Ellis and contributors",
  alongside the upstream attribution "Copyright (c) 2020 Gir-Core" preserved
  per the MIT notice requirement.

* AGENT-README.txt must never contain package version numbers, and must never
  reference the upstream namespaces as if they were usable. Provenance is one
  line naming one upstream commit.


NOTES
=====
* The Gtk and GtkSource bindings share ONE assembly, and .NET allows only one
  DllImportResolver per assembly. `Gtk.Module.Initialize()` therefore registers
  a single CHAINED resolver serving both native libraries; the GtkSource module
  registers none and exposes `internal static ResolveNativeLibrary(...)` for
  the chain. See src/CodeBrix.Develop.UI/Public/Module.cs and
  src/CodeBrix.Develop.UI/GtkSource/Public/Module.cs. If GtkSource is ever
  split into its own assembly, that chaining must be undone.

* The hand-written `...Async` methods on FileDialog, AlertDialog, FontDialog,
  FileLauncher and UriLauncher exist because the generator does not yet emit
  async wrappers. Each carries a TODO pointing at the upstream issue. If
  upstream starts generating them, delete the hand-written copies rather than
  keeping both.

* `Gio.Application.RunWithSynchronizationContext`,
  `GLib.MainLoop.RunWithSynchronizationContext`, `Gio.ListStore.New<T>()` and
  `Gtk.CustomSorter.New<T>(...)` are hand-written conveniences layered on the
  generated surface; they live in the Public/ folders next to the generated
  files and must survive regeneration.

* `src/CodeBrix.Develop.UI.Freetype2` has no Public/Module.cs and its generated
  ImportResolver has null library names on every platform — it is a type-stub
  project only. That is correct, not a generation failure.

* TestResults/ at the repository root is generated by `dotnet test --coverage`
  and is gitignored.
================================================================================
