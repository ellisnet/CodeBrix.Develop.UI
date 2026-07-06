using System.Collections.Generic;
using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ReturnTypeToManagedExpressions; //was previously: Generator.Renderer.Public.ReturnTypeToManagedExpressions;

internal class Long : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.Is<GirModel.Long>();

    public void Initialize(ReturnTypeToManagedData data, IEnumerable<ParameterToNativeData> _)
        => data.SetExpression(fromVariableName => fromVariableName); //Valid for IsPointer = true && IsPointer = false
}
