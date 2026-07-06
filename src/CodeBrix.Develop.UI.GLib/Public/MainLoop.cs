using System.Threading;

namespace CodeBrix.Develop.UI.GLib; //was previously: GLib;

public sealed partial class MainLoop
{
    public void RunWithSynchronizationContext()
    {
        var original = SynchronizationContext.Current;

        SynchronizationContext.SetSynchronizationContext(new Internal.MainLoopSynchronizationContext());

        try
        {
            Run();
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(original);
        }
    }
}
