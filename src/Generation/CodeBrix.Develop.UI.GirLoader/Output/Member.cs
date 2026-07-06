namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public partial class Member
{
    public string Name { get; }
    public string Value { get; }

    public Member(string name, string value)
    {
        Name = name;
        Value = value;
    }
}
