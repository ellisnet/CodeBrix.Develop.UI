using System.Collections.Generic;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ReturnTypeToManagedExpressions; //was previously: Generator.Renderer.Public.ReturnTypeToManagedExpressions;

internal class CLong : ReturnTypeConverter
{
    public bool Supports(GirModel.AnyType type)
        => type.Is<GirModel.CLong>();

    public void Initialize(ReturnTypeToManagedData data, IEnumerable<ParameterToNativeData> _)
    {
        data.SetExpression(fromVariableName => data.ReturnType.IsPointer
            ? fromVariableName
            : $"{fromVariableName}.Value"
        );
    }
}
