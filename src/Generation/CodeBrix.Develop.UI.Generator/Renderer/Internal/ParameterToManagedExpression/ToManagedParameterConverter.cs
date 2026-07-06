using System.Collections.Generic;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ParameterToManagedExpressions; //was previously: Generator.Renderer.Internal.ParameterToManagedExpressions;

internal interface ToManagedParameterConverter
{
    bool Supports(GirModel.AnyType type);
    void Initialize(ParameterToManagedData parameterData, IEnumerable<ParameterToManagedData> parameters);
}
