using System;
using System.Runtime.InteropServices;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.Gtk.Tests;

/// <summary>
/// Regression tests for the generated callback handlers: a transfer-none record
/// argument (here the cairo context of a draw function) is borrowed for the
/// duration of the managed callback and its wrapper is released as soon as the
/// callback returns, instead of pinning the native object until finalization.
/// </summary>
[Trait("Category", "SystemTest")]
public class DrawingAreaDrawFuncCallHandlerTests : Test
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint CairoGetReferenceCount(IntPtr cr);

    [Fact]
    public void Borrowed_cairo_context_is_released_when_the_managed_callback_returns()
    {
        //Arrange
        using var surface = new Cairo.ImageSurface(Cairo.Format.Argb32, 8, 8);
        using var context = new Cairo.Context(surface);
        var drawingArea = DrawingArea.New();
        var nativeContext = context.Handle.DangerousGetHandle();

        Cairo.Context? received = null;
        var usableInsideCallback = false;
        var referenceCountInsideCallback = 0u;
        var handler = new Internal.DrawingAreaDrawFuncCallHandler((_, cr, _, _) =>
        {
            received = cr;
            cr.MoveTo(1, 1);
            cr.LineTo(2, 2);
            usableInsideCallback = cr.Status == Cairo.Status.Success;
            referenceCountInsideCallback = GetReferenceCount(nativeContext);
        });

        //Act
        handler.NativeCallback(drawingArea.Handle.DangerousGetHandle(), nativeContext, 8, 8, IntPtr.Zero);

        //Assert
        usableInsideCallback.Should().BeTrue();
        referenceCountInsideCallback.Should().Be(2u); //the caller's reference plus the wrapper's private one
        received.Should().NotBeNull();
        received!.Handle.IsClosed.Should().BeTrue(); //the wrapper was disposed once the callback returned
        GetReferenceCount(nativeContext).Should().Be(1u); //and its reference went with it
        context.Status.Should().Be(Cairo.Status.Success); //while the caller's context is untouched
    }

    private static uint GetReferenceCount(IntPtr cr)
    {
        var libraryName = OperatingSystem.IsWindows() ? "libcairo-2.dll"
            : OperatingSystem.IsMacOS() ? "libcairo.2.dylib"
            : "libcairo.so.2";

        var library = NativeLibrary.Load(libraryName);
        try
        {
            var export = NativeLibrary.GetExport(library, "cairo_get_reference_count");
            return Marshal.GetDelegateForFunctionPointer<CairoGetReferenceCount>(export)(cr);
        }
        finally
        {
            NativeLibrary.Free(library);
        }
    }
}
