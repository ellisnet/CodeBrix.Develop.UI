using System;
using System.Runtime.InteropServices;

namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class Device
{
    [DllImport(CairoImportResolver.Library, EntryPoint = "cairo_device_destroy")]
    public static extern void Destroy(IntPtr handle);
}
