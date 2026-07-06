using System;
using System.Linq;
using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Renderer.Public; //was previously: Generator.Renderer.Public;

public static class ClassSignal
{
    public static string Render(GirModel.ComplexType type, GirModel.Signal signal)
    {
        try
        {
            return $@"
#region {Signal.GetName(signal)}
{SignalDescriptor.Render(type, signal)}
{SignalEvent.Render(type, signal)}
{SignalArgs.Render(signal)}
#endregion
";
        }
        catch (Exception ex)
        {
            var message = $"Did not generate signal '{type.Name}.{Signal.GetName(signal)}': {ex.Message}";

            if (ex is NotImplementedException)
                Log.Debug(message);
            else
                Log.Warning(message);

            return string.Empty;
        }
    }
}
