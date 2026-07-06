using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnTypeToNativeExpressions; //was previously: Generator.Renderer.Internal.ReturnTypeToNativeExpressions;

internal class Long : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.Is<GirModel.Long>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
        => fromVariableName;
}
