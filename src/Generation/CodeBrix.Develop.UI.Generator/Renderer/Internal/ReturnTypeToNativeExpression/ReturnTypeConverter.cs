using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnTypeToNativeExpressions; //was previously: Generator.Renderer.Internal.ReturnTypeToNativeExpressions;

internal interface ReturnTypeConverter
{
    bool Supports(AnyType type);
    string GetString(GirModel.ReturnType returnType, string fromVariableName);
}
