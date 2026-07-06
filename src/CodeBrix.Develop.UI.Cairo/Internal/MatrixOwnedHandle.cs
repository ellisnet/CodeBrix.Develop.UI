using System;
using System.Runtime.InteropServices;

namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class MatrixOwnedHandle
{
    public static MatrixOwnedHandle Create()
    {
        var size = Marshal.SizeOf<MatrixData>();
        var ptr = GLib.Functions.Malloc((nuint) size);

        var str = new MatrixData();
        Marshal.StructureToPtr(str, ptr, false);

        return new MatrixOwnedHandle(ptr);
    }

    protected override partial bool ReleaseHandle()
    {
        GLib.Functions.Free(handle);
        return true;
    }
}
