using System.Collections.Generic;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ParameterToNativeExpressions; //was previously: Generator.Renderer.Public.ParameterToNativeExpressions;

internal interface ToNativeParameterConverter
{
    bool Supports(GirModel.AnyType type);
    void Initialize(ParameterToNativeData parameterData, IEnumerable<ParameterToNativeData> parameters);
}
