using CodeBrix.Develop.UI.Gio;

namespace CodeBrix.Develop.UI.Gtk; //was previously: Gtk;

public class GResource : TemplateLoader
{
    public static GLib.Bytes Load(string resourceName)
    {
        File file = Gio.Functions.FileNewForUri($"resource://{resourceName}");
        return file.LoadBytes(null, out _);
    }
}
