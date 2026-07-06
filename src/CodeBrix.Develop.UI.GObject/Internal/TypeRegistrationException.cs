using System;

namespace CodeBrix.Develop.UI.GObject.Internal; //was previously: GObject.Internal;

/// <summary>
/// Thrown when type registration with GType fails
/// </summary>
public class TypeRegistrationException : Exception
{
    internal TypeRegistrationException(string message) : base(message) { }
}
