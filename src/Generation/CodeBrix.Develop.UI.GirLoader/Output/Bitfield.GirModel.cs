using System.Collections.Generic;

namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public partial class Bitfield : GirModel.Bitfield
{
    GirModel.Method? GirModel.Bitfield.TypeFunction => null; //TODO: Should be implemented
    IEnumerable<GirModel.Member> GirModel.Bitfield.Members => Members;
    bool GirModel.Bitfield.Introspectable => Introspectable;
}
