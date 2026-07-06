namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class ScaledFontOwnedHandle
{
    protected override bool ReleaseHandle()
    {
        ScaledFont.Destroy(handle);
        return true;
    }
}
