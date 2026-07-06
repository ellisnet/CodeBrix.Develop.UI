namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnType; //was previously: Generator.Renderer.Internal.ReturnType;

public interface ReturnTypeConverter
{
    bool Supports(GirModel.ReturnType returnType);
    RenderableReturnType Convert(GirModel.ReturnType returnType);
}
