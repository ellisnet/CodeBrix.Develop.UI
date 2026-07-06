using System.Collections.Generic;
using System.Linq;
using CodeBrix.Develop.UI.GirLoader.Output;

namespace CodeBrix.Develop.UI.GirLoader; //was previously: GirLoader;

internal class ShadowableAdapter : ShadowableProvider
{
    public IEnumerable<Constructor>? Constructors => null;
    public IEnumerable<Method> Methods => Enumerable.Empty<Method>();
    public IEnumerable<Function> Functions { get; }

    public ShadowableAdapter(IEnumerable<Function> functions)
    {
        Functions = functions;
    }
}
