namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class RegionOwnedHandle
{
    protected override bool ReleaseHandle()
    {
        Region.Destroy(handle);
        return true;
    }
}
