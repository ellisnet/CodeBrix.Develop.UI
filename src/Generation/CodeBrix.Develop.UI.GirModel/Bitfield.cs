using System.Collections.Generic;

namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public interface Bitfield : ComplexType
{
    Method? TypeFunction { get; }
    IEnumerable<Member> Members { get; }
    bool Introspectable { get; }
}
