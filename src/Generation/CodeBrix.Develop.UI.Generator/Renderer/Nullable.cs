namespace CodeBrix.Develop.UI.Generator.Renderer; //was previously: Generator.Renderer;

internal static class Nullable
{
    public static string Render(GirModel.Nullable nullable)
    {
        return nullable.Nullable ? "?" : string.Empty;
    }
}
