using System.Runtime.InteropServices;

namespace CodeBrix.Develop.UI.GLib.Internal; //was previously: GLib.Internal;

public partial class Error
{
    [DllImport(ImportResolver.Library, EntryPoint = "g_error_new_literal")]
    public static extern ErrorUnownedHandle NewLiteralUnowned(uint domain, int code, NonNullableUtf8StringHandle message);
}
