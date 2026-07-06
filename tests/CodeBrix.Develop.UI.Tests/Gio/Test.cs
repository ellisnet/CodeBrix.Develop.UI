using System;

namespace CodeBrix.Develop.UI.Gio.Tests; //was previously: Gio.Tests;

public abstract class Test : IDisposable
{
    public void Dispose()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
