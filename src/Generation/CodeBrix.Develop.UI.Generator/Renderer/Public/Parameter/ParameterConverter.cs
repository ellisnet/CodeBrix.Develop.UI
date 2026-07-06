namespace CodeBrix.Develop.UI.Generator.Renderer.Public.Parameter; //was previously: Generator.Renderer.Public.Parameter;

internal interface ParameterConverter
{
    bool Supports(GirModel.AnyType anyType);
    ParameterTypeData Create(GirModel.Parameter parameter);
}
