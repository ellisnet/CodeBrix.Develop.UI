namespace CodeBrix.Develop.UI.Gtk; //was previously: Gtk;

public interface TemplateLoader
{
    static abstract GLib.Bytes Load(string resourceName);
}
