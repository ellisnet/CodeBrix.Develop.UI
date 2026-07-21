using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeBrix.Develop.UI.Gtk.Integration.SourceAnalyzer; //was previously: Gtk.Integration.SourceAnalyzer;

internal sealed class GirCore2003
{
    public static SyntaxKind SyntaxKind => SyntaxKind.FieldDeclaration;

    public static DiagnosticDescriptor DiagnosticDescriptor { get; } = new(
        id: "GirCore2003",
        title: "Defining a \"CodeBrix.Develop.UI.Gtk.Connect\" attribute requires the containing class to be annotated with \"CodeBrix.Develop.UI.Gtk.Template\" attribute.",
        messageFormat: "A \"CodeBrix.Develop.UI.Gtk.Connect\" attribute is only evaluated when the containing class is using Gtk composite templates.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        helpLinkUri: DiagnosticLink.Create(2003)
    );

    public static void Analyze(SyntaxNodeAnalysisContext context)
    {
        var syntax = (FieldDeclarationSyntax) context.Node;

        foreach (var variable in syntax.Declaration.Variables)
        {
            var variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable);

            if (variableSymbol is null)
                return;

            var connectAttribute = variableSymbol.GetAttributes().FirstOrDefault(x => x.IsConnectAttribute());
            if (connectAttribute is null)
                return;

            var containingType = variableSymbol.ContainingType;
            if (containingType == null)
                return;

            var hasTemplateAttribute = containingType
                .GetAttributes()
                .Any(attr => attr.IsTemplateAttribute());

            if (hasTemplateAttribute)
                return;

            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor, connectAttribute.ApplicationSyntaxReference?.GetSyntax().GetLocation()));
        }
    }
}
