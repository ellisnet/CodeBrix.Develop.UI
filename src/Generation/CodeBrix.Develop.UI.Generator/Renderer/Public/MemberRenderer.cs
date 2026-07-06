using CodeBrix.Develop.UI.Generator.Model;
using static System.FormattableString;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public; //was previously: Generator.Renderer.Public;

internal static class MemberRenderer
{
    public static string Render(GirModel.Member member)
        => Invariant($"{Member.GetName(member)} = {member.Value},");
}
