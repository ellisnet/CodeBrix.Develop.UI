namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.Parameter; //was previously: Generator.Renderer.Internal.Parameter;

public interface ParameterConverter
{
    bool Supports(GirModel.AnyType anyType);
    RenderableParameter Convert(GirModel.Parameter parameter);
}
