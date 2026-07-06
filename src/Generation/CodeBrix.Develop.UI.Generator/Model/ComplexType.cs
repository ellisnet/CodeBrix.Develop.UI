namespace CodeBrix.Develop.UI.Generator.Model; //was previously: Generator.Model;

internal static class ComplexType
{
    public static string GetFullyQualified(GirModel.ComplexType type)
        => Namespace.GetPublicName(type.Namespace) + "." + type.Name;
}
