using System;

namespace CodeBrix.Develop.UI.Gtk.Tests; //was previously: Gtk.Tests;

public abstract class Test : IDisposable
{
    public void Dispose()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
