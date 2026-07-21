using System.Linq;
using CodeBrix.Develop.UI.GObject.Integration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeBrix.Develop.UI.Gtk.Integration.SourceAnalyzer; //was previously: Gtk.Integration.SourceAnalyzer;

internal sealed class GirCore2002
{
    public static SyntaxKind SyntaxKind => SyntaxKind.ClassDeclaration;

    public static DiagnosticDescriptor DiagnosticDescriptor { get; } = new(
        id: "GirCore2002",
        title: "GObject subclass must inherit from CodeBrix.Develop.UI.Gtk.Widget for composite template support.",
        messageFormat: "The \"CodeBrix.Develop.UI.GObject.Subclass\" attribute must define a subclass which inherits from CodeBrix.Develop.UI.Gtk.Widget.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        helpLinkUri: DiagnosticLink.Create(2002)
    );

    public static void Analyze(SyntaxNodeAnalysisContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax) context.Node;

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

        if (classSymbol is null)
            return;

        if (!classSymbol.GetAttributes().Any(x => x.IsTemplateAttribute()))
            return;

        var subclassAttribute = classSymbol
            .GetAttributes()
            .FirstOrDefault(x => x.IsSubclassAttribute());

        if (subclassAttribute is null)
            return;

        var subClassType = subclassAttribute.AttributeClass?.TypeArguments.FirstOrDefault();

        if (subClassType is null || InheritsFromGtkWidget(subClassType))
            return;

        var subclassAttributeLocation = classDeclarationSyntax
            .AttributeLists
            .SelectMany(x => x.Attributes)
            .First(x => context.SemanticModel.GetTypeInfo(x).Type?.Name == "SubclassAttribute")
            .GetLocation();

        var diagnostic = Diagnostic.Create(DiagnosticDescriptor, subclassAttributeLocation);
        context.ReportDiagnostic(diagnostic);
    }

    private static bool InheritsFromGtkWidget(ITypeSymbol type)
    {
        var current = type;
        while (current is not null)
        {
            if (current.ToDisplayString() == "CodeBrix.Develop.UI.Gtk.Widget")
                return true;

            current = current.BaseType;
        }
        return false;
    }
}
