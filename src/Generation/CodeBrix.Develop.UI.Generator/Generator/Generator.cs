namespace CodeBrix.Develop.UI.Generator.Generator; //was previously: Generator.Generator;

internal interface Generator<T>
{
    void Generate(T obj);
}
