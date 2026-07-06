using System.Collections.Generic;
using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.Signals; //was previously: Generator.Renderer.Public.Signals;

public class Default : SignalArgsParameterConverter
{
    public bool Supports(AnyType type)
    {
        return true;
    }

    public void Initialize(SignalArgsParameterData parameter, int index, IEnumerable<SignalArgsParameterData> parameters)
    {
        var p = ParameterRenderer.Render(parameter.Parameter);
        parameter.SetExpression(() => $"public {p.NullableTypeName} {parameter.Parameter.Name.ToPascalCase()} => Extract<{p.NullableTypeName}>(Args[{index}]);");
    }
}
