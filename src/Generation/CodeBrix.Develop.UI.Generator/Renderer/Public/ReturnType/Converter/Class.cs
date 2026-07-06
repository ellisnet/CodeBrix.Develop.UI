using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public.ReturnType; //was previously: Generator.Renderer.Public.ReturnType;

internal class Class : ReturnTypeConverter
{
    public RenderableReturnType Create(GirModel.ReturnType returnType)
    {
        var nullableTypeName = ComplexType.GetFullyQualified((GirModel.Class) returnType.AnyType.AsT0) + Nullable.Render(returnType);

        return new RenderableReturnType(nullableTypeName);
    }

    public bool Supports(GirModel.ReturnType returnType)
        => returnType.AnyType.Is<GirModel.Class>();
}
