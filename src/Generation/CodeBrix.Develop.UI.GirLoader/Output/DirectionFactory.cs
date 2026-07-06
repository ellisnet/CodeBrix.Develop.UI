namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public static class DirectionFactory
{
    public static Direction Create(string? direction)
    {
        return direction switch
        {
            "in" => Direction.In,
            "out" => Direction.Out,
            "inout" => Direction.Ref,
            _ => Direction.Default
        };
    }
}
