using Microsoft.CodeAnalysis;

namespace CodeBrix.Develop.UI.GObject.Integration.SourceGenerator; //was previously: GObject.Integration.SourceGenerator;

internal static class Subclass
{
    public static void EnableSubclassSupport(this IncrementalGeneratorInitializationContext context)
    {
        var subclassValuesProvider = context.GetSubclassValuesProvider();

        context.RegisterImplementationSourceOutput(
            source: subclassValuesProvider,
            action: SubclassCode.Generate
        );

        context.RegisterImplementationSourceOutput(
            source: subclassValuesProvider.Collect(),
            action: IntegrationCode.Generate
        );
    }
}
