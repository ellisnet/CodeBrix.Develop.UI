using System;

namespace CodeBrix.Develop.UI.GLib.Internal; //was previously: GLib.Internal;

public abstract partial class TreeNodeHandle
{
    public partial TreeNodeOwnedHandle OwnedCopy()
    {
        throw new NotImplementedException();
    }

    public partial TreeNodeUnownedHandle UnownedCopy()
    {
        throw new NotImplementedException();
    }
}

public partial class TreeNodeOwnedHandle
{
    public static partial TreeNodeOwnedHandle FromUnowned(IntPtr ptr)
    {
        throw new NotImplementedException();
    }

    protected override partial bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}
