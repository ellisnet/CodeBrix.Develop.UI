using System;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;
using System.Net.Http;

namespace CodeBrix.Develop.UI.BindingTool;

/// <summary>
/// Acquires the GObject Introspection (.gir) input files from the
/// CodeBrix.Develop.UI.GnomeIntrospection snapshot repository on GitHub,
/// caching them locally so repeated runs work offline. A local checkout can
/// be used instead via the --gir-source option. This tool never pulls
/// anything from github.com/gircore/*.
/// </summary>
internal static class GirFileAcquisition
{
    public const string RepositoryOwner = "ellisnet";
    public const string RepositoryName = "CodeBrix.Develop.UI.GnomeIntrospection";

    /// <summary>
    /// The pinned commit of the GnomeIntrospection repository holding the
    /// complete gir-files set corresponding to gir.core 0.8.0 (gir-files
    /// commit fa9a30ef).
    /// </summary>
    public const string DefaultGirRef = "fad5496fd525e55e9d91e38e553da85fedf24be9";

    public const string DefaultCacheFolder = ".gir-cache";

    private static readonly string[] Platforms = { "linux", "macos", "windows" };

    /// <summary>
    /// Returns the linux/macos/windows gir folders, either from a local
    /// GnomeIntrospection checkout (girSource) or from the local cache,
    /// downloading the pinned snapshot from GitHub on first use.
    /// </summary>
    public static (string SearchPathLinux, string SearchPathMacos, string SearchPathWindows) EnsurePlatformFolders(
        string? girSource, string girRef, string cacheFolder)
    {
        if (girSource is not null)
            return ValidatePlatformFolders(girSource, $"--gir-source directory '{girSource}'");

        var snapshotFolder = Path.Combine(cacheFolder, girRef);
        var markerFile = Path.Combine(snapshotFolder, ".complete");

        if (!File.Exists(markerFile))
        {
            DownloadSnapshot(girRef, snapshotFolder);
            File.WriteAllText(markerFile, DateTime.UtcNow.ToString("O"));
        }
        else
        {
            Log.Information($"Using cached gir files: {snapshotFolder}");
        }

        return ValidatePlatformFolders(snapshotFolder, $"gir cache directory '{snapshotFolder}'");
    }

    private static (string, string, string) ValidatePlatformFolders(string root, string description)
    {
        foreach (var platform in Platforms)
        {
            var folder = Path.Combine(root, platform);
            if (!Directory.Exists(folder) || Directory.GetFiles(folder, "*.gir").Length == 0)
                throw new Exception($"The {description} does not contain a '{platform}' folder with .gir files.");
        }

        return (
            Path.Combine(root, "linux"),
            Path.Combine(root, "macos"),
            Path.Combine(root, "windows")
        );
    }

    private static void DownloadSnapshot(string girRef, string snapshotFolder)
    {
        var url = $"https://codeload.github.com/{RepositoryOwner}/{RepositoryName}/tar.gz/{girRef}";
        Log.Information($"Downloading gir files from https://github.com/{RepositoryOwner}/{RepositoryName} at ref {girRef}");

        Directory.CreateDirectory(snapshotFolder);

        using var httpClient = new HttpClient();
        using var downloadStream = httpClient.GetStreamAsync(url).GetAwaiter().GetResult();
        using var gzipStream = new GZipStream(downloadStream, CompressionMode.Decompress);
        using var tarReader = new TarReader(gzipStream);

        var extracted = 0;
        while (tarReader.GetNextEntry() is { } entry)
        {
            if (entry.EntryType != TarEntryType.RegularFile)
                continue;

            // Entry names look like "<RepositoryName>-<ref>/linux/Gtk-4.0.gir"
            var segments = entry.Name.Split('/');
            if (segments.Length != 3 || !segments[2].EndsWith(".gir", StringComparison.Ordinal))
                continue;

            var platform = segments[1];
            if (!platform.IsOneOf(Platforms))
                continue;

            var platformFolder = Path.Combine(snapshotFolder, platform);
            Directory.CreateDirectory(platformFolder);
            entry.ExtractToFile(Path.Combine(platformFolder, segments[2]), overwrite: true);
            extracted++;
        }

        if (extracted == 0)
            throw new Exception($"No .gir files were found in the downloaded snapshot for ref '{girRef}'.");

        Log.Information($"Downloaded {extracted} gir files to {snapshotFolder}");
    }
}
