namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class PathOwnedHandle
{
    protected override partial bool ReleaseHandle()
    {
        Path.Destroy(handle);
        return true;
    }
}
