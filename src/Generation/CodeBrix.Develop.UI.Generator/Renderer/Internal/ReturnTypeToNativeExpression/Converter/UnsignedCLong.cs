namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnTypeToNativeExpressions; //was previously: Generator.Renderer.Internal.ReturnTypeToNativeExpressions;

internal class UnsignedCLong : ReturnTypeConverter
{
    public bool Supports(GirModel.AnyType type)
        => type.Is<GirModel.UnsignedCLong>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
        => $"new CULong(checked((nuint){fromVariableName}))";
}
