namespace CodeBrix.Develop.UI.Generator.Model; //was previously: Generator.Model;

internal static class Union
{
    public static string GetFullyQualifiedInternalStructName(GirModel.Union union)
        => Namespace.GetInternalName(union.Namespace) + "." + GetInternalStructName(union);

    public static string GetInternalStructName(GirModel.Union union)
        => union.Name + "Data";
}
