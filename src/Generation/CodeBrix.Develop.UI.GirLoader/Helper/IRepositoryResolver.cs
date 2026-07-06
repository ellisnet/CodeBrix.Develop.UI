namespace CodeBrix.Develop.UI.GirLoader; //was previously: GirLoader;

/// <summary>
/// Resolves input repository definitions from GIR file names
/// </summary>
public interface IRepositoryResolver
{
    Input.Repository? ResolveRepository(string fileName);
}
