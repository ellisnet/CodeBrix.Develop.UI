using System;

namespace CodeBrix.Develop.UI.GObject; //was previously: GObject;

public interface InstanceFactory
{
    static abstract object Create(IntPtr handle, bool ownsHandle);
}
