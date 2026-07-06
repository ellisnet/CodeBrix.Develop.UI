using System.Collections.Generic;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ReturnTypeToManagedExpressions; //was previously: Generator.Renderer.Public.ReturnTypeToManagedExpressions;

internal interface ReturnTypeConverter
{
    bool Supports(GirModel.AnyType type);
    void Initialize(ReturnTypeToManagedData data, IEnumerable<ParameterToNativeData> parameters);
}
