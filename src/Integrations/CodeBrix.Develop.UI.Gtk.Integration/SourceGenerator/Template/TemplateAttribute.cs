using Microsoft.CodeAnalysis;

namespace CodeBrix.Develop.UI.Gtk.Integration; //was previously: Gtk.Integration;

internal static class TemplateAttribute
{
    public const string MetadataName = "CodeBrix.Develop.UI.Gtk.TemplateAttribute`1";
    public const string FullyQualifiedDisplayName = "global::CodeBrix.Develop.UI.Gtk.TemplateAttribute<TLoader>";

    public static bool IsTemplateAttribute(this AttributeData data)
    {
        var displayString = data.AttributeClass?.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        return displayString == FullyQualifiedDisplayName;
    }
}
