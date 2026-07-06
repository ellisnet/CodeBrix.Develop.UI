using System;

namespace CodeBrix.Develop.UI.GObject; //was previously: GObject;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class HandleAttribute<T> : Attribute where T : Internal.ObjectHandle;
