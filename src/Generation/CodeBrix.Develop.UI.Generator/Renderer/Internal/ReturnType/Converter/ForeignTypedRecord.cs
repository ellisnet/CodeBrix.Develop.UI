using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnType; //was previously: Generator.Renderer.Internal.ReturnType;

internal class ForeignTypedRecord : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.Is<GirModel.Record>(out var record) && Model.Record.IsForeignTyped(record);
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        var type = (GirModel.Record) returnType.AnyType.AsT0;

        var typeName = returnType switch
        {
            { Transfer: Transfer.Full } => Model.ForeignTypedRecord.GetFullyQuallifiedOwnedHandle(type),
            _ => Model.ForeignTypedRecord.GetFullyQuallifiedUnownedHandle(type)
        };

        //Returned SafeHandles are never "null" but "invalid" in case of C NULL.
        return new RenderableReturnType(typeName);
    }
}
