using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeBrix.Develop.UI.GObject.Integration.SourceAnalyzer; //was previously: GObject.Integration.SourceAnalyzer;

internal interface Rule
{
    static abstract SyntaxKind SyntaxKind { get; }
    static abstract void Analyze(SyntaxNodeAnalysisContext context);
}
