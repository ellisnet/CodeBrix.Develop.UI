namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnType; //was previously: Generator.Renderer.Internal.ReturnType;

internal class PrimitiveValueTypeAlias : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.IsAlias<GirModel.PrimitiveValueType>();
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        var nullableTypeName = returnType.IsPointer
            ? Model.Type.Pointer
            : Model.Type.GetName(((GirModel.Alias) returnType.AnyType.AsT0).Type);

        return new RenderableReturnType(nullableTypeName);
    }
}
