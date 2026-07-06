namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public class VoidPointer : Pointer
{
    public VoidPointer() : base("void") { }

    internal override bool Matches(TypeReference typeReference)
    {
        if (typeReference.CTypeReference is null)
            return false;

        return typeReference.CTypeReference.CType == CType
               && typeReference.CTypeReference.IsPointer;
    }
}
