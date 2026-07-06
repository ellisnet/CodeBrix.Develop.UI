using System.Linq;
using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Fixer.Class; //was previously: Generator.Fixer.Class;

internal class PropertyLikeInterfacePropertyFixer : Fixer<GirModel.Class>
{
    public void Fixup(GirModel.Class @class)
    {
        foreach (var interfaceProperty in @class.Implements.SelectMany(x => x.Properties))
            foreach (var property in @class.Properties)
                if (Property.GetName(property) == Property.GetName(interfaceProperty))
                    Property.SetImplementExplicitly(interfaceProperty);
    }
}
