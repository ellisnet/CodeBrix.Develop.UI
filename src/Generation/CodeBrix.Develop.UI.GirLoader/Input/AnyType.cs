namespace CodeBrix.Develop.UI.GirLoader.Input; //was previously: GirLoader.Input;

public interface AnyType
{
    Type? Type { get; set; }
    ArrayType? Array { get; set; }
}
