namespace CodeBrix.Develop.UI.GObject; //was previously: GObject;

public delegate int CompareDataFuncT<in T>(T a, T b) where T : GObject.NativeObject;
