namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class PatternOwnedHandle
{
    protected override bool ReleaseHandle()
    {
        Pattern.Destroy(handle);
        return true;
    }
}
