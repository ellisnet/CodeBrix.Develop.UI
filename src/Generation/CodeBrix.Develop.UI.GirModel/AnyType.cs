using OneOf;

namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public class AnyType : OneOfBase<Type, ArrayType>
{
    private AnyType(OneOf<Type, ArrayType> input) : base(input) { }

    public static AnyType From(Type type) => new(OneOf<Type, ArrayType>.FromT0(type));
    public static AnyType From(ArrayType arrayType) => new(OneOf<Type, ArrayType>.FromT1(arrayType));
}
