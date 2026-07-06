using System.Collections.Generic;

namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public interface Signal
{
    string Name { get; }
    IEnumerable<Parameter> Parameters { get; }
    ReturnType ReturnType { get; }
    bool Introspectable { get; }
}
