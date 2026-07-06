using System;
using System.Runtime.InteropServices;

namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public partial class Region
{
    [DllImport(CairoImportResolver.Library, EntryPoint = "cairo_region_destroy")]
    public static extern void Destroy(IntPtr handle);
}
