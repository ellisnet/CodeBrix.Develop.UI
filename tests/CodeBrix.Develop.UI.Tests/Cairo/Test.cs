using System;

namespace CodeBrix.Develop.UI.Cairo.Tests; //was previously: Cairo.Tests;

public abstract class Test : IDisposable
{
    public void Dispose()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
