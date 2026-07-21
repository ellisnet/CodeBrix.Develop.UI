using System;
using System.CommandLine;
using System.IO;
using System.Linq;

namespace CodeBrix.Develop.UI.BindingTool; //was previously: GirTool;

public class CleanCommand : Command
{
    public CleanCommand() : base(
        name: "clean",
        description: "Cleans the output directories")
    {
        var target = new Argument<string>("target")
        {
            Description = "Target folder to clean of all generated C# files (*.Generated.cs)"
        };
        Add(target);

        SetAction(parseResult => Execute(
            folder: parseResult.GetValue(target) ?? string.Empty
        ));
    }

    private static int Execute(string folder)
    {
        try
        {
            if (!VerifyFolderExits(folder))
                return 1;

            var deletedFiles = 0;
            var searchedFolders = 0;

            foreach (var d in Directory.EnumerateDirectories(folder, "*", SearchOption.AllDirectories))
            {
                foreach (var file in Directory.EnumerateFiles(d).Where(FileIsGenerated))
                {
                    File.Delete(file);
                    deletedFiles++;
                }

                searchedFolders++;
            }

            Log.Information($"Deleted {deletedFiles} files in {searchedFolders} folders");
            return 0;
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
            Log.Error("An error occurred while cleaning files. Please save a copy of your log output and open an issue at: https://github.com/ellisnet/CodeBrix.Develop.UI/issues/new");
            return 1;
        }
    }

    private static bool VerifyFolderExits(string folder)
    {
        if (Directory.Exists(folder))
            return true;

        Log.Error($"Folder {folder} does not exist");
        return false;
    }

    private static bool FileIsGenerated(string file) => file.EndsWith(".Generated.cs");
}
