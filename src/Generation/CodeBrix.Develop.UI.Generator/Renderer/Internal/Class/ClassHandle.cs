using System;
using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Internal; //was previously: Generator.Renderer.Internal;

internal static class ClassHandle
{
    public static string Render(GirModel.Class cls)
    {
        var handleName = Class.GetInternalHandleName(cls);

        return $$"""
                 using System;
                 using System.Linq;
                 using System.Runtime.InteropServices;
                 using System.Runtime.Versioning;

                 #nullable enable

                 namespace {{Namespace.GetInternalName(cls.Namespace)}};

                 public partial class {{handleName}}(IntPtr handle) : {{RenderParent(cls)}}(handle);
                 """;
    }

    private static string RenderParent(GirModel.Class cls)
    {
        return cls.Parent is null
            ? throw new Exception("Class is missing parent")
            : Class.GetFullyQualifiedInternalHandleName(cls.Parent);
    }
}
