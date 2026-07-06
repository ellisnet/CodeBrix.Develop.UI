using System;

namespace CodeBrix.Develop.UI.GObject; //was previously: GObject;

public abstract class Fundamental
{
    public IntPtr Handle { get; }

    protected Fundamental(IntPtr ptr)
    {
        Handle = ptr;
    }
}
