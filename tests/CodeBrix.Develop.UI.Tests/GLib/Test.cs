using System;

namespace CodeBrix.Develop.UI.GLib.Tests; //was previously: GLib.Tests;

public abstract class Test : IDisposable
{
    public void Dispose()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    protected static void CollectAfter(Action action)
    {
        action();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
