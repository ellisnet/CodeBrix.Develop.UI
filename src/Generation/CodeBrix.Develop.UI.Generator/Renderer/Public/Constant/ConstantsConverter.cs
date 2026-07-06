namespace CodeBrix.Develop.UI.Generator.Renderer.Public.Constant; //was previously: Generator.Renderer.Public.Constant;

internal interface ConstantsConverter
{
    bool Supports(GirModel.Type type);
    RenderableConstant Convert(GirModel.Constant constant);
}
