namespace CodeBrix.Develop.UI.Generator.Fixer; //was previously: Generator.Fixer;

public interface Fixer<in T>
{
    void Fixup(T obj);
}
