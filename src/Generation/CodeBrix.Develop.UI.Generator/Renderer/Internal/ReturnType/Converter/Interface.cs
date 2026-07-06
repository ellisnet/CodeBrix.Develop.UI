using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnType; //was previously: Generator.Renderer.Internal.ReturnType;

internal class Interface : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.Is<GirModel.Interface>();
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        var nullableTypeName = returnType.IsPointer
            ? Type.Pointer
            : Type.GetName(returnType.AnyType.AsT0);

        return new RenderableReturnType(nullableTypeName);
    }
}
