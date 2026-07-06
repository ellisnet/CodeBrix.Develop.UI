using System.CommandLine;
using CodeBrix.Develop.UI.BindingTool;

var rootCommand = new RootCommand("CodeBrix.Develop.UI.BindingTool generates C# bindings from GIR files.")
{
    new GenerateCommand(),
    new CleanCommand()
};

return rootCommand.Invoke(args);
