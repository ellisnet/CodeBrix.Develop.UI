using System.Collections.Generic;
using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ReturnTypeToManagedExpressions; //was previously: Generator.Renderer.Public.ReturnTypeToManagedExpressions;

internal class Void : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.Is<GirModel.Void>();

    public void Initialize(ReturnTypeToManagedData data, IEnumerable<ParameterToNativeData> _)
    {
        //Do nothing in case of void. There is no expression to create
    }
}
