namespace CodeBrix.Develop.UI.Generator.Model; //was previously: Generator.Model;

internal static class Constant
{
    public static string GetName(GirModel.Constant constant)
    {
        return constant.Name.EscapeIdentifier();
    }
}
