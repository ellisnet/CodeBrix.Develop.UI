using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CodeBrix.Develop.UI.GtkSource; //was previously: GtkSource;

public static class Module
{
    private static bool IsInitialized;
    private static DllImportResolver? CustomDllImportResolver;

    /// <summary>
    /// Initialize the <c>GtkSource</c> module.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Calling this method is necessary to correctly initialize the bindings
    /// and should be done before using anything else in the <see cref="GtkSource" />
    /// namespace.
    /// </para>
    /// <para>
    /// Calling this method will also initialize the modules this module
    /// depends on:
    /// </para>
    /// <list type="table">
    /// <item><description><see cref="Gtk.Module" /></description></item>
    /// </list>
    /// </remarks>
    public static void Initialize()
    {
        if (IsInitialized)
            return;

        // The GtkSource binding is compiled into the same assembly as the Gtk
        // binding, and .NET allows only one DllImportResolver per assembly -
        // Gtk.Module.Initialize() registers a chained resolver that also
        // serves GtkSource (via ResolveNativeLibrary below), so no resolver
        // is registered here.
        Gtk.Module.Initialize();

        Internal.TypeRegistration.RegisterTypes();
        Internal.Functions.Init();

        IsInitialized = true;
    }

    internal static IntPtr ResolveNativeLibrary(string libraryName, System.Reflection.Assembly assembly, DllImportSearchPath? searchPath)
    {
        var resolver = CustomDllImportResolver ?? Internal.ImportResolver.Resolve;
        return resolver(libraryName, assembly, searchPath);
    }

    /// <summary>
    /// Set a custom DllImportResolver. This disables the automatic loading of native binaries for
    /// GtkSource. If the given DllImportResolver receives the library name "GtkSource" it has to return a pointer
    /// to the desired native GtkSource binary.
    /// </summary>
    /// <remarks>
    /// Please be aware that using this API means you are out of the officially supported area
    /// as you are able to combine CodeBrix.Develop.UI with some binary the package was not built for. Please consider
    /// to generate a custom package which exactly matches your binary.
    /// </remarks>
    /// <param name="customDllImportResolver">Custom DllImportResolver to use.</param>
    /// <exception cref="Exception">Throws an exception if the method is called after module initialization.</exception>
    [Experimental("GirCore1009", UrlFormat = "https://gircore.github.io/docs/integration/diagnostic/1009.html")]
    public static void SetCustomDllImportResolver(DllImportResolver customDllImportResolver)
    {
        if (IsInitialized)
            throw new Exception("Can't set a custom DllImportResolver after initialization is done.");

        CustomDllImportResolver = customDllImportResolver;
    }
}
