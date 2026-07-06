using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ParameterToNativeExpressions; //was previously: Generator.Renderer.Public.ParameterToNativeExpressions;

internal class InterfaceArray : ToNativeParameterConverter
{
    public bool Supports(GirModel.AnyType type)
        => type.IsArray<GirModel.Interface>();

    public void Initialize(ParameterToNativeData parameter, IEnumerable<ParameterToNativeData> _)
    {
        var parameterName = Model.Parameter.GetName(parameter.Parameter);
        var nativevariableName = parameterName + "Native";

        parameter.SetSignatureName(() => parameterName);
        parameter.SetCallName(() => nativevariableName);
        parameter.SetExpression(() => $"var {nativevariableName} = {parameterName}.Select(iface => (iface as GObject.Object).Handle.DangerousGetHandle()).ToArray();");
    }
}
