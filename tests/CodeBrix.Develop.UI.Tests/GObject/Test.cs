using System;

namespace CodeBrix.Develop.UI.GObject.Tests; //was previously: GObject.Tests;

public abstract class Test : IDisposable
{
    public void Dispose()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
