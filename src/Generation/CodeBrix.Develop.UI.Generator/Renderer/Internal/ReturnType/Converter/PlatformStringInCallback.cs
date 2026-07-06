using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal.ReturnType; //was previously: Generator.Renderer.Internal.ReturnType;

internal class PlatformStringInCallback : ReturnTypeConverter
{
    public bool Supports(GirModel.ReturnType returnType)
    {
        return returnType.AnyType.Is<GirModel.PlatformString>();
    }

    public RenderableReturnType Convert(GirModel.ReturnType returnType)
    {
        // This must be IntPtr since SafeHandle's cannot be returned from managed to unmanaged.
        return new RenderableReturnType(Type.Pointer);
    }
}
