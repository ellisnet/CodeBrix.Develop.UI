using Microsoft.CodeAnalysis;

namespace CodeBrix.Develop.UI.GObject.Integration.SourceGenerator; //was previously: GObject.Integration.SourceGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.EnableSubclassSupport();
    }
}
