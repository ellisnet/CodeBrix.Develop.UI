using System.CommandLine;
using CodeBrix.Develop.UI.BindingTool;

var rootCommand = new RootCommand("CodeBrix.Develop.UI.BindingTool generates C# bindings from GIR files.");
rootCommand.Add(new GenerateCommand());
rootCommand.Add(new CleanCommand());

return rootCommand.Parse(args).Invoke();
