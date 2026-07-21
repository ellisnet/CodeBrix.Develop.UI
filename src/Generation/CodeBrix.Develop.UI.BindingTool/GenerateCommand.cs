using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeBrix.Develop.UI.GirLoader;
using CodeBrix.Develop.UI.GirLoader.PlatformSupport;
using Repository = CodeBrix.Develop.UI.GirLoader.Output.Repository;

namespace CodeBrix.Develop.UI.BindingTool; //was previously: GirTool;

public partial class GenerateCommand : Command
{
    private int Execute(string[] input, string output, string namespacePrefix, string? girSource, string girRef, string girCache, string? searchPathLinux, string? searchPathMacos, string? searchPathWindows, bool disableAsync, LogLevel logLevel)
    {
        try
        {
            Configuration.SetupLogLevel(logLevel);

            CodeBrix.Develop.UI.Generator.Configuration.NamespacePrefix = namespacePrefix;
            CodeBrix.Develop.UI.Generator.Configuration.ProjectFolderNameMapper = ProjectFolderMapping.Map;

            // When no explicit search paths are given, acquire the gir files
            // from the GnomeIntrospection snapshot repository (or a local
            // checkout of it) and search all three platform folders.
            if (searchPathLinux is null && searchPathMacos is null && searchPathWindows is null)
                (searchPathLinux, searchPathMacos, searchPathWindows) =
                    GirFileAcquisition.EnsurePlatformFolders(girSource, girRef, girCache);

            // A failure to load the inputs is reported through the exit code, but
            // generation still runs over the (empty) result, matching the behaviour
            // this command had when it set InvocationContext.ExitCode directly.
            var (allNamespaces, generatedNamespaces, inputsLoaded) = GetNamespaces(
                searchPathLinux, searchPathMacos, searchPathWindows, disableAsync, input);

            if (disableAsync)
            {
                foreach (var @namespace in allNamespaces)
                    PlatformGenerator.Fixup(@namespace);

                foreach (var @namespace in generatedNamespaces)
                    PlatformGenerator.Generate(@namespace, output);
            }
            else
            {
                Parallel.ForEach(allNamespaces, PlatformGenerator.Fixup);
                Parallel.ForEach(generatedNamespaces, x => PlatformGenerator.Generate(x, output));
            }

            Log.Information("Done");
            return inputsLoaded ? 0 : 1;
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
            Log.Error("An error occurred while writing files. Please save a copy of your log output and open an issue at: https://github.com/ellisnet/CodeBrix.Develop.UI/issues/new");
            return 1;
        }
    }

    private static (IEnumerable<Namespace>, IEnumerable<Namespace>, bool) GetNamespaces(string? searchPathLinux, string? searchPathMacos, string? searchPathWindows, bool disableAsync, string[] input)
    {
        try
        {
            (var linuxRepositories, var macosRepositories, var windowsRepositories) = LoadRepositories(searchPathLinux, searchPathMacos, searchPathWindows, disableAsync, input);

            var linuxNamespaceNames = linuxRepositories.Repositories.Select(x => GetNamespaceName(x.Namespace));
            var macosNamespaceNames = macosRepositories.Repositories.Select(x => GetNamespaceName(x.Namespace));
            var windowsNamespaceNames = windowsRepositories.Repositories.Select(x => GetNamespaceName(x.Namespace));
            var namespacesNames = linuxNamespaceNames
                .Union(macosNamespaceNames)
                .Union(windowsNamespaceNames)
                .Distinct();

            // We only generate code for namespaces that came from the set of inputs
            var generatedNamespaceNames = linuxRepositories.InputNamespaceNames
                .Concat(macosRepositories.InputNamespaceNames)
                .Concat(windowsRepositories.InputNamespaceNames)
                .ToHashSet();

            var allNamespaces = new List<Namespace>();
            var generatedNamespaces = new List<Namespace>();

            foreach (var namespaceName in namespacesNames)
            {
                var linuxNamespace = linuxRepositories.Repositories.FirstOrDefault(x => GetNamespaceName(x.Namespace) == namespaceName)?.Namespace;
                var macosNamespace = macosRepositories.Repositories.FirstOrDefault(x => GetNamespaceName(x.Namespace) == namespaceName)?.Namespace;
                var windowsNamespace = windowsRepositories.Repositories.FirstOrDefault(x => GetNamespaceName(x.Namespace) == namespaceName)?.Namespace;

                if (linuxNamespace is null)
                    Log.Information($"There is no {namespaceName} repository for linux");

                if (macosNamespace is null)
                    Log.Information($"There is no {namespaceName} repository for macos");

                if (windowsNamespace is null)
                    Log.Information($"There is no {namespaceName} repository for windows");

                var @namespace = new Namespace(new PlatformHandler(linuxNamespace, macosNamespace, windowsNamespace));
                allNamespaces.Add(@namespace);

                if (generatedNamespaceNames.Contains(namespaceName))
                {
                    generatedNamespaces.Add(@namespace);
                }
            }

            return (allNamespaces, generatedNamespaces, true);
        }
        catch (FileNotFoundException fileNotFoundException)
        {
            Log.Exception(fileNotFoundException);
            Log.Error("Please make sure that the given input files are readable.");
        }

        return (Enumerable.Empty<Namespace>(), Enumerable.Empty<Namespace>(), false);
    }

    private static (DeserializedInput, DeserializedInput, DeserializedInput) LoadRepositories(string? searchPathLinux, string? searchPathMacos, string? searchPathWindows, bool disableAsync, string[] input)
    {
        if (searchPathLinux is null && searchPathMacos is null && searchPathWindows is null)
            throw new Exception("Please define at least one search parth for a specific platform");

        DeserializedInput? linuxRepositories = null;
        DeserializedInput? macosRepositories = null;
        DeserializedInput? windowsRepositories = null;

        void SetLinuxRepositories() => linuxRepositories = DeserializeInput("linux", searchPathLinux, input);
        void SetMacosRepositories() => macosRepositories = DeserializeInput("macos", searchPathMacos, input);
        void SetWindowsRepositories() => windowsRepositories = DeserializeInput("windows", searchPathWindows, input);

        if (disableAsync)
        {
            SetLinuxRepositories();
            SetMacosRepositories();
            SetWindowsRepositories();
        }
        else
        {
            Parallel.Invoke(
                SetLinuxRepositories,
                SetMacosRepositories,
                SetWindowsRepositories
            );
        }

        return (linuxRepositories!, macosRepositories!, windowsRepositories!);
    }

    private static DeserializedInput DeserializeInput(string platformName, string? searchPath, string[] input)
    {
        var repositoryResolver = new RepositoryResolverFactory(
            platformName, searchPath, typeof(GenerateCommand).Assembly).Create();

        var inputRepositories = input
            .Select(fileName => repositoryResolver.ResolveRepository(fileName))
            .OfType<GirLoader.Input.Repository>()
            .ToList();

        // Get the namespaces corresponding to the input gir files.
        // There may be more namespaces included in the output if they are resolved from includes.
        var inputNamespaces = inputRepositories
            .Select(repository => repository.Namespace == null ? "" : GetNamespaceName(repository.Namespace))
            .ToList();

        var includeResolver = new IncludeResolver(repositoryResolver);
        var loader = new GirLoader.Loader(includeResolver.ResolveInclude);
        var outputRepositories = loader.Load(inputRepositories).ToList();

        return new DeserializedInput(outputRepositories, inputNamespaces);
    }

    private static string GetNamespaceName(GirModel.Namespace ns)
    {
        return $"{ns.Name}-{ns.Version}";
    }

    private static string GetNamespaceName(GirLoader.Input.Namespace ns)
    {
        return $"{ns.Name}-{ns.Version}";
    }

    private class DeserializedInput
    {
        public DeserializedInput(List<Repository> repositories, List<string> inputNamespaceNames)
        {
            Repositories = repositories;
            InputNamespaceNames = inputNamespaceNames;
        }

        public static DeserializedInput Empty() =>
            new DeserializedInput(new List<Repository>(), new List<string>());

        /// <summary>
        /// All resolved output repositories
        /// </summary>
        public List<Repository> Repositories { get; }

        /// <summary>
        /// Namespace names corresponding to the input gir files
        /// </summary>
        public List<string> InputNamespaceNames { get; }
    }
}
