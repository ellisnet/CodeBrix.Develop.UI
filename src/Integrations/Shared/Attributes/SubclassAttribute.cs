using Microsoft.CodeAnalysis;

namespace CodeBrix.Develop.UI.GObject.Integration; //was previously: GObject.Integration;

internal static class SubclassAttribute
{
    public const string MetadataName = "CodeBrix.Develop.UI.GObject.SubclassAttribute`1";
    public const string FullyQualifiedDisplayName = "global::CodeBrix.Develop.UI.GObject.SubclassAttribute<T>";

    public static bool IsSubclassAttribute(this AttributeData data)
    {
        var displayString = data.AttributeClass?.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        return displayString == FullyQualifiedDisplayName;
    }
}
