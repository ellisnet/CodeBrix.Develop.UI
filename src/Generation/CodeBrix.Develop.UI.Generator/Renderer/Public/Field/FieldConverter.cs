namespace CodeBrix.Develop.UI.Generator.Renderer.Public.Field; //was previously: Generator.Renderer.Public.Field;

public interface FieldConverter
{
    bool Supports(GirModel.Field field);
    RenderableField Convert(GirModel.Field field);
}
