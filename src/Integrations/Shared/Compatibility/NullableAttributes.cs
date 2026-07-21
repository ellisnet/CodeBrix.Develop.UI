// netstandard2.0 compatibility shim - see the long note at the top of either
// Integration .csproj for why those projects target netstandard2.0.
//
// These attributes drive the compiler's nullable flow analysis. They live in
// the BCL from netstandard2.1 / .NET Core 3.0 onward, but netstandard2.0's
// reference assemblies predate them, so with <Nullable>enable</Nullable> the
// code below would not compile without these declarations.
//
// They are declared as internal shims rather than pulled in from a package
// because a Roslyn analyzer has to ship as a single self-contained assembly:
// the compiler loads only the analyzer DLL, so any package dependency would be
// missing at analysis time. When a real reference assembly also defines these
// types, the compiler prefers that definition over these.

namespace System.Diagnostics.CodeAnalysis; //netstandard2.0 shim; not a CodeBrix namespace

/// <summary>
/// Specifies that when a method returns <see cref="ReturnValue" />, the
/// associated parameter will not be <c>null</c> even if the type allows it.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class NotNullWhenAttribute : Attribute
{
    /// <summary>
    /// Initializes the attribute with the specified return-value condition.
    /// </summary>
    /// <param name="returnValue">
    /// The return value condition. If the method returns this value, the
    /// associated parameter will not be <c>null</c>.
    /// </param>
    public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

    /// <summary>
    /// Gets the return value condition.
    /// </summary>
    public bool ReturnValue { get; }
}

/// <summary>
/// Specifies that when a method returns <see cref="ReturnValue" />, the
/// associated parameter may be <c>null</c> even if the type disallows it.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class MaybeNullWhenAttribute : Attribute
{
    /// <summary>
    /// Initializes the attribute with the specified return-value condition.
    /// </summary>
    /// <param name="returnValue">
    /// The return value condition. If the method returns this value, the
    /// associated parameter may be <c>null</c>.
    /// </param>
    public MaybeNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

    /// <summary>
    /// Gets the return value condition.
    /// </summary>
    public bool ReturnValue { get; }
}

/// <summary>
/// Applied to a method that will never return under any circumstance.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal sealed class DoesNotReturnAttribute : Attribute
{
}
