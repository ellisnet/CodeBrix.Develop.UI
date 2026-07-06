using System.Reflection;

namespace CodeBrix.Develop.UI.Gtk; //was previously: Gtk;

public class AssemblyResource : TemplateLoader
{
    public static GLib.Bytes Load(string resourceName)
    {
        var data = GObject.AssemblyExtension.ReadResourceAsByteArray(Assembly.GetCallingAssembly(), resourceName);
        return GLib.Bytes.New(data);
    }
}
