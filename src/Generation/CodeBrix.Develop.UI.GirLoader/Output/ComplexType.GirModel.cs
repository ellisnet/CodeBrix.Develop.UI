namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public abstract partial class ComplexType : GirModel.ComplexType
{
    GirModel.Namespace GirModel.ComplexType.Namespace => Repository.Namespace;
    string GirModel.ComplexType.Name => Name;
}
