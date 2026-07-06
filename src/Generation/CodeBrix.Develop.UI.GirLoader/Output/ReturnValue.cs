namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public partial class ReturnValue
{
    public Transfer Transfer { get; }
    public bool Nullable { get; }
    public TypeReference TypeReference { get; }

    public ReturnValue(TypeReference typeReference, Transfer transfer, bool nullable)
    {
        TypeReference = typeReference;
        Transfer = transfer;
        Nullable = nullable;
    }
}
