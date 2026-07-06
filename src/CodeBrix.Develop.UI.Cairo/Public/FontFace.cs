using System;

namespace CodeBrix.Develop.UI.Cairo; //was previously: Cairo;

public partial class FontFace
{
    public Status Status => Internal.FontFace.Status(Handle);
    public FontType FontType => Internal.FontFace.GetType(Handle);
}
