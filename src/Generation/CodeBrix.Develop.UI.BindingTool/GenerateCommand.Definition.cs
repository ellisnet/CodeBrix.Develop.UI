using System;
using System.CommandLine;

namespace CodeBrix.Develop.UI.BindingTool; //was previously: GirTool;

public partial class GenerateCommand : Command
{
    public GenerateCommand() : base(
        name: "generate",
        description: "Generate C# bindings from gir files"
    )
    {
        var inputArgument = new Argument<string[]>("input")
        {
            Description = "The names of gir files which should be processed"
        };

        var outputOption = new Option<string>("--output", "-o")
        {
            Description = "The directory to write the generated C# files to",
            DefaultValueFactory = _ => "./src"
        };

        var namespacePrefixOption = new Option<string>("--namespace-prefix", "-np")
        {
            Description = "The prefix prepended to every generated namespace",
            DefaultValueFactory = _ => "CodeBrix.Develop.UI"
        };

        var girSourceOption = new Option<string?>("--gir-source", "-gs")
        {
            Description = "A local CodeBrix.Develop.UI.GnomeIntrospection checkout to read gir files from (skips the GitHub download)"
        };

        var girRefOption = new Option<string>("--gir-ref", "-gr")
        {
            Description = "The git ref (commit/tag/branch) of the GnomeIntrospection repository to download gir files from",
            DefaultValueFactory = _ => GirFileAcquisition.DefaultGirRef
        };

        var girCacheOption = new Option<string>("--gir-cache", "-gc")
        {
            Description = "The folder downloaded gir files are cached in",
            DefaultValueFactory = _ => GirFileAcquisition.DefaultCacheFolder
        };

        var searchPathOptionLinux = new Option<string>("--search-path-linux", "-sl")
        {
            Description = "The directory which is searched for dependent linux gir files"
        };

        var searchPathOptionMacos = new Option<string>("--search-path-macos", "-sm")
        {
            Description = "The directory which is searched for dependent macos gir files"
        };

        var searchPathOptionWindows = new Option<string>("--search-path-windows", "-sw")
        {
            Description = "The directory which is searched for dependent windows gir files"
        };

        var disableAsyncOption = new Option<bool>("--disable-async", "-d")
        {
            Description = "Generate files synchronously - useful for debugging",
            DefaultValueFactory = _ => false
        };

        var logLevelOption = new Option<LogLevel>("--log-level", "-l")
        {
            Description = "Set the log level",
            DefaultValueFactory = _ => LogLevel.Standard
        };

        Add(inputArgument);
        Add(outputOption);
        Add(namespacePrefixOption);
        Add(girSourceOption);
        Add(girRefOption);
        Add(girCacheOption);
        Add(searchPathOptionLinux);
        Add(searchPathOptionMacos);
        Add(searchPathOptionWindows);
        Add(disableAsyncOption);
        Add(logLevelOption);

        SetAction(parseResult => Execute(
            input: parseResult.GetValue(inputArgument) ?? Array.Empty<string>(),
            output: parseResult.GetValue(outputOption) ?? throw new Exception("Output unknown"),
            namespacePrefix: parseResult.GetValue(namespacePrefixOption) ?? string.Empty,
            girSource: parseResult.GetValue(girSourceOption),
            girRef: parseResult.GetValue(girRefOption) ?? GirFileAcquisition.DefaultGirRef,
            girCache: parseResult.GetValue(girCacheOption) ?? GirFileAcquisition.DefaultCacheFolder,
            searchPathLinux: parseResult.GetValue(searchPathOptionLinux),
            searchPathMacos: parseResult.GetValue(searchPathOptionMacos),
            searchPathWindows: parseResult.GetValue(searchPathOptionWindows),
            disableAsync: parseResult.GetValue(disableAsyncOption),
            logLevel: parseResult.GetValue(logLevelOption)
        ));
    }
}
