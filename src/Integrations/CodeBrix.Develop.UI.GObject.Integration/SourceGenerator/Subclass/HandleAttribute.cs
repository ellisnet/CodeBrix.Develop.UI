using Microsoft.CodeAnalysis;

namespace CodeBrix.Develop.UI.GObject.Integration; //was previously: GObject.Integration;

internal static class HandleAttribute
{
    private const string FullyQualifiedDisplayName = "global::CodeBrix.Develop.UI.GObject.HandleAttribute<T>";

    public static bool IsHandleAttribute(this AttributeData data)
    {
        var displayString = data.AttributeClass?.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        return displayString == FullyQualifiedDisplayName;
    }
}
