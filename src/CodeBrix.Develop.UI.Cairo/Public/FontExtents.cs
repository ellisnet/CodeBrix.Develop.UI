using System.Runtime.InteropServices;

namespace CodeBrix.Develop.UI.Cairo; //was previously: Cairo;

[StructLayout(LayoutKind.Sequential)]
public struct FontExtents
{
    public double Ascent;
    public double Descent;
    public double Height;
    public double MaxXAdvance;
    public double MaxYAdvance;
}
