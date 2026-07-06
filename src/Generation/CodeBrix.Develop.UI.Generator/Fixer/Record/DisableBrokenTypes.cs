using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Fixer.Record; //was previously: Generator.Fixer.Record;

internal class DisableBrokenTypes : Fixer<GirModel.Record>
{
    public void Fixup(GirModel.Record record)
    {
        switch (record.Name)
        {
            case "Win32NetworkMonitor":
            case "Win32NetworkMonitorClass":
                Type.Disable(record);
                break;
        }
    }
}
