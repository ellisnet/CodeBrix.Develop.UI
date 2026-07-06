namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnType; //was previously: Generator.Renderer.Internal.ReturnType;

internal class GLibPointerArrayCallback : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.IsGLibPtrArray();
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        return new RenderableReturnType(Model.Type.Pointer);
    }
}
