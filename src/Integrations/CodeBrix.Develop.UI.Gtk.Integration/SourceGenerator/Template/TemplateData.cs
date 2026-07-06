using System.Collections.Generic;

namespace CodeBrix.Develop.UI.Gtk.Integration.SourceGenerator; //was previously: Gtk.Integration.SourceGenerator;

internal sealed record TemplateData(
    TypeData TypeData,
    string ResourceName,
    string Loader,
    HashSet<TemplateData.Connect> Connections
)
{
    internal sealed record Connect(
        string ObjectId,
        string Type,
        string MemberName
    );
}
