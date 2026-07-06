using System.Collections.Generic;

namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public interface ShadowableProvider
{
    IEnumerable<Constructor>? Constructors { get; }
    IEnumerable<Method> Methods { get; }
    IEnumerable<Function> Functions { get; }
}
