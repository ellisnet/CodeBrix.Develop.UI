using System;

namespace CodeBrix.Develop.UI.Gtk; //was previously: Gtk;

[AttributeUsage(AttributeTargets.Field)]
public class ConnectAttribute(string? objectId = null) : Attribute
{
    public string? ObjectId { get; } = objectId;
}
