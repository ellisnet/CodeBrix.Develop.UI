using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeBrix.Develop.UI.Gtk.Integration.SourceAnalyzer; //was previously: Gtk.Integration.SourceAnalyzer;

internal interface Rule
{
    static abstract SyntaxKind SyntaxKind { get; }
    static abstract void Analyze(SyntaxNodeAnalysisContext context);
}
