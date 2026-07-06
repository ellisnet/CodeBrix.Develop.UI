using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ReturnType; //was previously: Generator.Renderer.Public.ReturnType;

internal class OpaqueUntypedRecord : ReturnTypeConverter
{
    public RenderableReturnType Create(GirModel.ReturnType returnType)
    {
        var typeName = ComplexType.GetFullyQualified((GirModel.Record) returnType.AnyType.AsT0);

        return new RenderableReturnType(typeName + Nullable.Render(returnType));
    }

    public bool Supports(GirModel.ReturnType returnType)
        => returnType.AnyType.Is<GirModel.Record>(out var record) && Model.Record.IsOpaqueUntyped(record);
}
