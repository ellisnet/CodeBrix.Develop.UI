using System;
using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnTypeToNativeExpressions; //was previously: Generator.Renderer.Internal.ReturnTypeToNativeExpressions;

internal class Class : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.Is<GirModel.Class>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
    {
        if (!returnType.IsPointer)
            throw new NotImplementedException($"{returnType.AnyType}: class return type which is no pointer can not be converted to native");

        return returnType.Nullable
            ? fromVariableName + "?.Handle.DangerousGetHandle() ?? IntPtr.Zero"
            : fromVariableName + ".Handle.DangerousGetHandle()";
    }
}
