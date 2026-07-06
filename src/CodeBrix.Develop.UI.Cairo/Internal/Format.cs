using System.Runtime.InteropServices;

namespace CodeBrix.Develop.UI.Cairo.Internal; //was previously: Cairo.Internal;

public class Format
{
    [DllImport(CairoImportResolver.Library, EntryPoint = "cairo_format_stride_for_width")]
    public static extern int StrideForWidth(Cairo.Format format, int width);
}
