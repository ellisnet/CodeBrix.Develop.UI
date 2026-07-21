// netstandard2.0 compatibility shim - see the long note at the top of either
// Integration .csproj for why those projects target netstandard2.0.
//
// The C# compiler requires System.Runtime.CompilerServices.IsExternalInit to
// exist before it will emit an "init"-only property setter, which it does for
// every positional record. netstandard2.0's reference assemblies predate the
// type, so it is declared here.
//
// It is declared as an internal shim rather than pulled in from a package
// (PolySharp, IsExternalInit, ...) because a Roslyn analyzer has to ship as a
// single self-contained assembly: the compiler loads only the analyzer DLL, so
// any package dependency would be missing at analysis time.
//
// Nothing references this type at runtime - it exists purely so the compiler
// has a type to emit a modreq against. When a real reference assembly also
// defines it, the compiler prefers that definition over this one.

namespace System.Runtime.CompilerServices; //netstandard2.0 shim; not a CodeBrix namespace

/// <summary>
/// Marker type the C# compiler requires in order to emit <c>init</c>-only
/// property setters (and therefore any positional record).
/// </summary>
internal static class IsExternalInit
{
}
