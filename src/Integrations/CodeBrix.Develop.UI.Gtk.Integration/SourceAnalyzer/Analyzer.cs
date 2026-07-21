using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeBrix.Develop.UI.Gtk.Integration.SourceAnalyzer; //was previously: Gtk.Integration.SourceAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [
        GirCore2001.DiagnosticDescriptor,
        GirCore2002.DiagnosticDescriptor,
        GirCore2003.DiagnosticDescriptor
    ];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(GirCore2001.Analyze, GirCore2001.SyntaxKind);
        context.RegisterSyntaxNodeAction(GirCore2002.Analyze, GirCore2002.SyntaxKind);
        context.RegisterSyntaxNodeAction(GirCore2003.Analyze, GirCore2003.SyntaxKind);
    }
}
