namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public interface ReturnType : Nullable
{
    AnyType AnyType { get; }
    Transfer Transfer { get; }
    bool IsPointer { get; }
}
