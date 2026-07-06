namespace CodeBrix.Develop.UI.Generator.Renderer.Public.InstanceParameterToNativeExpressions; //was previously: Generator.Renderer.Public.InstanceParameterToNativeExpressions;

public class Pointer : InstanceParameterConverter
{
    public bool Supports(GirModel.Type type)
    {
        return type is GirModel.Pointer;
    }

    public string GetExpression(GirModel.InstanceParameter instanceParameter)
    {
        return "this.Handle.DangerousGetHandle()";
    }
}
