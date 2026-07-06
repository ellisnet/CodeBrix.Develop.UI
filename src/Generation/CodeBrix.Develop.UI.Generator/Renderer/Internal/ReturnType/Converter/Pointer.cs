using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnType; //was previously: Generator.Renderer.Internal.ReturnType;

internal class Pointer : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.Is<GirModel.Pointer>();
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        return new RenderableReturnType(Type.Pointer);
    }
}
