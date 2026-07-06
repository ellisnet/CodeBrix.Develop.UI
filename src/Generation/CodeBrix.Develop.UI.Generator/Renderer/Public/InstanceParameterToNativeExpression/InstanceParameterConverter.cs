namespace CodeBrix.Develop.UI.Generator.Renderer.Public.InstanceParameterToNativeExpressions; //was previously: Generator.Renderer.Public.InstanceParameterToNativeExpressions;

internal interface InstanceParameterConverter
{
    bool Supports(GirModel.Type type);
    string GetExpression(GirModel.InstanceParameter instanceParameter);
}
