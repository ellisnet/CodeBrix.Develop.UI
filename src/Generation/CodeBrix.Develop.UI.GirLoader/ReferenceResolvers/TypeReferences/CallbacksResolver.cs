using System.Linq;
using CodeBrix.Develop.UI.GirLoader.Output;

namespace CodeBrix.Develop.UI.GirLoader; //was previously: GirLoader;

internal static class CallbacksResolver
{
    public static void ResolveCallbacks(this RepositoryTypeReferenceResolver resolver, Repository repository)
    {
        resolver.ResolveTypeReferences(repository.Namespace.Callbacks.Select(x => x.ReturnValue.TypeReference), repository);
        resolver.ResolveParameterLists(repository.Namespace.Callbacks.Select(x => x.ParameterList), repository);
    }
}
