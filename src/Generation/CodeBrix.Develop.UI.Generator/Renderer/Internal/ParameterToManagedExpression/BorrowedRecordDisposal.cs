namespace CodeBrix.Develop.UI.Generator.Renderer.Internal;

/// <summary>
/// Registers the post-call statement that releases the wrapper a callback handler
/// creates for a transfer-none record parameter.
/// </summary>
/// <remarks>
/// A transfer-none record handed to a native callback is only borrowed for the
/// duration of that call. The record converters wrap it through
/// <c>OwnedHandle.FromUnowned</c>, which takes a private reference (or copy) so the
/// managed wrapper can own its handle. Without an explicit release that reference
/// survives until the SafeHandle is finalized, and a callback that runs at frame rate
/// with a heavy argument (a cairo context pinning a frame's render state, for
/// example) leaks native memory faster than the garbage collector reclaims it.
/// Disposing the wrapper right after the managed callback returns restores the
/// borrowed semantics; the native caller still holds its own reference, so the
/// underlying object stays alive. Transfer-full parameters are left alone: ownership
/// of those genuinely passes to the managed side.
/// </remarks>
internal static class BorrowedRecordDisposal
{
    /// <summary>
    /// Registers a post-call <c>Dispose()</c> of <paramref name="variableName"/> when the
    /// parameter is passed with transfer-none semantics; does nothing otherwise.
    /// </summary>
    /// <param name="parameterData">The parameter whose managed wrapper is released.</param>
    /// <param name="variableName">The name of the wrapper variable in the generated handler.</param>
    public static void Register(ParameterToManagedData parameterData, string variableName)
    {
        if (parameterData.Parameter.Transfer != GirModel.Transfer.None)
            return;

        parameterData.SetPostCallExpression(() => parameterData.Parameter.Nullable
            ? $"{variableName}?.Dispose();"
            : $"{variableName}.Dispose();");
    }
}
