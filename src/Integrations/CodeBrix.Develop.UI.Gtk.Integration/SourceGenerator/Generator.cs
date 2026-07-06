using Microsoft.CodeAnalysis;

namespace CodeBrix.Develop.UI.Gtk.Integration.SourceGenerator; //was previously: Gtk.Integration.SourceGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.EnableTemplateSupport();
    }
}
