namespace CodeBrix.Develop.UI.GirLoader.PlatformSupport; //was previously: GirLoader.PlatformSupport;

public partial class Alias : GirModel.Alias
{
    string GirModel.Alias.Name => _alias.Name;
    GirModel.Type GirModel.Alias.Type => _alias.Type;
    GirModel.Namespace GirModel.Alias.Namespace => _alias.Namespace;
}
