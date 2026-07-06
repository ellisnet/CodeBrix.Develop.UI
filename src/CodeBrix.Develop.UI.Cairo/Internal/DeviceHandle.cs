namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class DeviceOwnedHandle
{
    protected override bool ReleaseHandle()
    {
        Device.Destroy(handle);
        return true;
    }
}
