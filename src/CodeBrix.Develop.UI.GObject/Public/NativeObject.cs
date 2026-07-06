using CodeBrix.Develop.UI.GObject.Internal;

namespace CodeBrix.Develop.UI.GObject; //was previously: GObject;

public interface NativeObject
{
    ObjectHandle Handle { get; }
}
