namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class FontOptionsOwnedHandle
{
    protected override bool ReleaseHandle()
    {
        FontOptions.Destroy(handle);
        return true;
    }
}
