namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnTypeToNativeExpressions; //was previously: Generator.Renderer.Internal.ReturnTypeToNativeExpressions;

internal class CLong : ReturnTypeConverter
{
    public bool Supports(GirModel.AnyType type)
        => type.Is<GirModel.CLong>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
        => $"new CLong(checked((nint){fromVariableName}))";
}
