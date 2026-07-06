using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.Field; //was previously: Generator.Renderer.Internal.Field;

internal class String : FieldConverter
{
    public bool Supports(GirModel.Field field)
    {
        return field.AnyTypeOrCallback.TryPickT0(out var anyType, out _) && anyType.Is<GirModel.String>();
    }

    public RenderableField[] Convert(GirModel.Field field)
    {
        return [new RenderableField(
            Name: Model.Field.GetName(field),
            TypeName: Type.Pointer,
            Array: null
        )];
    }
}
