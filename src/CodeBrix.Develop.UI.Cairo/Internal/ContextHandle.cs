namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class ContextOwnedHandle
{
    protected override bool ReleaseHandle()
    {
        Context.Destroy(handle);
        return true;
    }
}
