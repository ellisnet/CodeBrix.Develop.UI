using System.Linq;
using CodeBrix.Develop.UI.GirLoader.Output;

namespace CodeBrix.Develop.UI.GirLoader; //was previously: GirLoader;

internal static class ConstantsResolver
{
    public static void ResolveConstants(this RepositoryTypeReferenceResolver resolver, Repository repository)
    {
        resolver.ResolveTypeReferences(repository.Namespace.Constants.Select(x => x.TypeReference), repository);
    }
}
