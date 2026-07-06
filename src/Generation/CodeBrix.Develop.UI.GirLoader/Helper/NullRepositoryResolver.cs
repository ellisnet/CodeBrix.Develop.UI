namespace CodeBrix.Develop.UI.GirLoader; //was previously: GirLoader;

public class NullRepositoryResolver : IRepositoryResolver
{
    public Input.Repository? ResolveRepository(string fileName)
    {
        return null;
    }
}
