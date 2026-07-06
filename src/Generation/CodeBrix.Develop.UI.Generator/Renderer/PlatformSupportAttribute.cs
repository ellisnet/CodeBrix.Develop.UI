using System;
using System.Collections.Generic;
using CodeBrix.Develop.UI.GirModel;

namespace CodeBrix.Develop.UI.Generator.Renderer; //was previously: Generator.Renderer;

internal static class PlatformSupportAttribute
{
    public static string Render(PlatformDependent? platformDependent)
    {
        if (platformDependent is null)
            return string.Empty;

        var attributes = new List<string>();

        if (!platformDependent.SupportsLinux)
            attributes.Add("[UnsupportedOSPlatform(\"Linux\")]");

        if (!platformDependent.SupportsMacos)
            attributes.Add("[UnsupportedOSPlatform(\"OSX\")]");

        if (!platformDependent.SupportsWindows)
            attributes.Add("[UnsupportedOSPlatform(\"Windows\")]");

        return string.Join(Environment.NewLine, attributes);
    }
}
