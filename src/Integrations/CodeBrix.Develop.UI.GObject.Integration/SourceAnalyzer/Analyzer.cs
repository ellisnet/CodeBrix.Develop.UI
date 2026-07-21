using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeBrix.Develop.UI.GObject.Integration.SourceAnalyzer; //was previously: GObject.Integration.SourceAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer : DiagnosticAnalyzer
{

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [
        GirCore1002.DiagnosticDescriptor,
        GirCore1004.DiagnosticDescriptor,
        GirCore1005.DiagnosticDescriptor,
        GirCore1006.DiagnosticDescriptor,
        GirCore1008.DiagnosticDescriptor
    ];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(GirCore1002.Analyze, GirCore1002.SyntaxKind);
        context.RegisterSyntaxNodeAction(GirCore1004.Analyze, GirCore1004.SyntaxKind);
        context.RegisterSyntaxNodeAction(GirCore1005.Analyze, GirCore1005.SyntaxKind);
        context.RegisterSyntaxNodeAction(GirCore1006.Analyze, GirCore1006.SyntaxKind);
        context.RegisterSyntaxNodeAction(GirCore1008.Analyze, GirCore1008.SyntaxKind);
    }
}
