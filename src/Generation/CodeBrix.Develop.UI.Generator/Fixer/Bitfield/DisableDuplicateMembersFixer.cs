using System.Linq;
using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Fixer.Bitfield; //was previously: Generator.Fixer.Bitfield;

internal class DisableDuplicateMembersFixer : Fixer<GirModel.Bitfield>
{
    public void Fixup(GirModel.Bitfield bitfield)
    {
        foreach (var grouping in bitfield.Members.GroupBy(member => member.Name))
        {
            if (grouping.Count() <= 1)
                continue;

            foreach (var member in grouping.Skip(1)) //Disable all but the first member
            {
                Member.Disable(member);
                Log.Debug($"{bitfield.Name}: Disabled member {member.Name} with value {member.Value} because there is another member with the same name");
            }
        }
    }
}
