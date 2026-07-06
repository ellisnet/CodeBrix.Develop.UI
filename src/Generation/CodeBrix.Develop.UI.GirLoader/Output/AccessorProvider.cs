using System.Collections.Generic;

namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public interface AccessorProvider
{
    IEnumerable<Method> Methods { get; }
    IEnumerable<Property> Properties { get; }
}
