namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.InstanceParameter; //was previously: Generator.Renderer.Internal.InstanceParameter;

public interface InstanceParameterConverter
{
    bool Supports(GirModel.Type type);
    RenderableInstanceParameter Convert(GirModel.InstanceParameter instanceParameter);
}
