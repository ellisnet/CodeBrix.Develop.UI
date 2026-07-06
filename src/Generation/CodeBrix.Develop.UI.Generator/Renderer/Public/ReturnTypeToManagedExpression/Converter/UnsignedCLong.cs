using System.Collections.Generic;
using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ReturnTypeToManagedExpressions; //was previously: Generator.Renderer.Public.ReturnTypeToManagedExpressions;

internal class UnsignedCLong : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.Is<GirModel.UnsignedCLong>();

    public void Initialize(ReturnTypeToManagedData data, IEnumerable<ParameterToNativeData> _)
    {
        data.SetExpression(fromVariableName => data.ReturnType.IsPointer
            ? fromVariableName
            : $"{fromVariableName}.Value");
    }
}
