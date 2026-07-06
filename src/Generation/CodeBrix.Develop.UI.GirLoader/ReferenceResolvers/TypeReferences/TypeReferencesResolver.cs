using System.Collections.Generic;

namespace CodeBrix.Develop.UI.GirLoader; //was previously: GirLoader;

internal static class TypeReferencesResolver
{
    public static void ResolveTypeReferences(this RepositoryTypeReferenceResolver resolver, IEnumerable<Output.TypeReference> typeReferences, Output.Repository repository)
    {
        foreach (var typeReference in typeReferences)
            resolver.ResolveTypeReference(typeReference, repository);
    }
}
