using Microsoft.CodeAnalysis;

namespace CodeBrix.Develop.UI.Gtk.Integration; //was previously: Gtk.Integration;

internal static class ConnectAttribute
{
    public const string FullyQualifiedDisplayName = "global::CodeBrix.Develop.UI.Gtk.ConnectAttribute";

    public static bool IsConnectAttribute(this AttributeData data)
    {
        var displayString = data.AttributeClass?.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        return displayString == FullyQualifiedDisplayName;
    }
}
