namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public class SymbolNameReference
{
    public string SymbolName { get; }
    public NamespaceName? NamespaceName { get; }

    public SymbolNameReference(string symbolName, NamespaceName? namespaceName)
    {
        SymbolName = symbolName;
        NamespaceName = namespaceName;
    }

    public override string ToString()
    {
        var ns = NamespaceName is not null
            ? NamespaceName + "."
            : string.Empty;

        return ns + SymbolName;
    }
}
