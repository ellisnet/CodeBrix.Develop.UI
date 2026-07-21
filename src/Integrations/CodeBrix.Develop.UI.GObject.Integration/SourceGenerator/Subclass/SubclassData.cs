namespace CodeBrix.Develop.UI.GObject.Integration.SourceGenerator; //was previously: GObject.Integration.SourceGenerator;

internal sealed record SubclassData(
    TypeData TypeData,
    string? QualifiedName,
    string Parent,
    string ParentHandle,
    bool IsInitiallyUnowned,
    bool IsSealed,
    bool IsAbstract
);
