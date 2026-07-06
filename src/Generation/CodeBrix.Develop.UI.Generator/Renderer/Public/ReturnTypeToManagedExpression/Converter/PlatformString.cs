using System.Collections.Generic;
using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ReturnTypeToManagedExpressions; //was previously: Generator.Renderer.Public.ReturnTypeToManagedExpressions;

internal class PlatformString : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.Is<GirModel.PlatformString>();

    public void Initialize(ReturnTypeToManagedData data, IEnumerable<ParameterToNativeData> _)
    {
        // Convert the PlatformStringHandle return type to a string.
        data.SetExpression(fromVariableName => $"{fromVariableName}.ConvertToString()");
    }
}
