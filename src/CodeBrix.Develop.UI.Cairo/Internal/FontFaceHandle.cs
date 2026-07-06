namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class FontFaceOwnedHandle
{
    protected override bool ReleaseHandle()
    {
        FontFace.Destroy(handle);
        return true;
    }
}
