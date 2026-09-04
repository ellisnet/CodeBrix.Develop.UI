using System;
using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;

namespace CodeBrix.Develop.UI.GObject.Integration.SourceGenerator; //was previously: GObject.Integration.SourceGenerator;

internal static class SubclassCode
{
    public static void Generate(SourceProductionContext context, SubclassData subclassData)
    {
        context.AddSource(
            hintName: $"{subclassData.TypeData.Filename}.Subclass.g.cs",
            source: ToCode(subclassData)
        );
    }
    private static string ToCode(SubclassData subclassData)
    {
        return subclassData.TypeData.IsGlobalNamespace
            ? RenderGlobalNamespace(subclassData)
            : RenderNamespace(subclassData);
    }

    private static string RenderGlobalNamespace(SubclassData subclassData)
    {
        return $"""
               #nullable enable
               {RenderClassHierarchy(subclassData)}
               """;
    }

    private static string RenderNamespace(SubclassData subclassData)
    {
        return $"""
                #nullable enable
                namespace {subclassData.TypeData.Namespace};
                {RenderClassHierarchy(subclassData)}
                """;
    }

    private static string RenderClassHierarchy(SubclassData subclassData)
    {
        var sb = new StringBuilder();
        foreach (var typeData in subclassData.TypeData.UpperNestedTypes)
            sb.AppendLine(FormattableString.Invariant($"{typeData.Accessibility} partial {typeData.Kind} {typeData.NameGenericArguments} {{"));

        sb.AppendLine(RenderClassContent(subclassData));

        foreach (var _ in subclassData.TypeData.UpperNestedTypes)
            sb.AppendLine("}");

        return sb.ToString();
    }

    private static string RenderClassContent(SubclassData subclassData)
    {
        return subclassData.IsAbstract
            ? RenderAbstractSubclassContent(subclassData)
            : RenderSubclassContent(subclassData);
    }

    private static string RenderSubclassContent(SubclassData subclassData)
    {
        var qualifiedName = subclassData.QualifiedName is null
            ? "null"
            : $"\"{subclassData.QualifiedName}\"";

        var handleConstructorAccessModifier = subclassData.IsSealed
            ? "internal"
            : "protected internal";

        return $$"""
                  {{subclassData.TypeData.Properties.Accessibility}} unsafe partial class {{subclassData.TypeData.Properties.NameGenericArguments}} : {{subclassData.Parent}}, global::CodeBrix.Develop.UI.GObject.GTypeProvider
                  {
                       {{GeneratedCodeAttribute.Render()}}
                       private static readonly global::CodeBrix.Develop.UI.GObject.Type GType = global::CodeBrix.Develop.UI.GObject.Internal.SubclassRegistrar.Register<{{subclassData.TypeData.Properties.NameGenericArguments}}, {{subclassData.Parent}}>(&ClassInit, &InstanceInit, {{qualifiedName}});
                       
                       /// <summary>
                       /// Return the registered GObject type for this class.
                       /// </summary>
                       {{GeneratedCodeAttribute.Render()}}
                       public static new global::CodeBrix.Develop.UI.GObject.Type GetGType() => GType;
                  
                       /// <summary>
                       /// Creates a new "{{subclassData.TypeData.Properties.Name}}" instance and sets the properties specified by the construct arguments.
                       /// </summary>
                       /// <param name="constructArguments">The properties to set.</param>
                       /// <returns>A new instance of "{{subclassData.TypeData.Properties.Name}}".</returns>
                       /// <remarks>Currently it is only supported to set properties which are defined in C. Any properties defined in C# are not known. This will be fixed once https://github.com/gircore/gir.core/issues/1433 is implemented.</remarks> 
                       {{GeneratedCodeAttribute.Render()}}
                       public static new {{subclassData.TypeData.Properties.NameGenericArguments}} NewWithProperties(global::CodeBrix.Develop.UI.GObject.ConstructArgument[] constructArguments)
                       {
                            var ptr = global::CodeBrix.Develop.UI.GObject.Internal.Object.NewWithProperties(GetGType(), constructArguments);

                            if (!global::CodeBrix.Develop.UI.GObject.Internal.InstanceCache.TryGetObject(ptr, out var obj))
                                throw new System.Exception($"Could not find {{subclassData.TypeData.Properties.NameGenericArguments}} instance for {ptr}.");

                            {{RenderUnref(subclassData)}}

                            return ({{subclassData.TypeData.Properties.NameGenericArguments}}) obj;
                       }
                       
                       /// <summary>
                       /// Creates a new managed {{subclassData.TypeData.Properties.NameGenericArguments}} instance for a given pointer.
                       /// </summary>
                       {{GeneratedCodeAttribute.Render()}}
                       public static new {{subclassData.TypeData.Properties.NameGenericArguments}} NewFromPointer(System.IntPtr ptr, bool ownsHandle) => ({{subclassData.TypeData.Properties.NameGenericArguments}}) global::CodeBrix.Develop.UI.GObject.Internal.InstanceWrapper.WrapHandle<{{subclassData.TypeData.Properties.NameGenericArguments}}>(ptr, ownsHandle);
                  
                       /// <summary>
                       /// Creates a new instance of {{subclassData.TypeData.Properties.Name}}.
                       /// </summary>
                       /// <param name="handle">A handle to the C instance.</param>
                       /// <remarks> To create a new instance call <see cref="NewWithProperties" />. 
                       /// If you want to initialize any custom dotnet properties, create a new static factory method to set those properties.
                       /// </remarks>
                       {{GeneratedCodeAttribute.Render()}}
                       {{handleConstructorAccessModifier}} {{subclassData.TypeData.Properties.Name}}({{subclassData.ParentHandle}} handle) : base(handle) 
                       {
                           CompositeTemplateInitialize();
                           Initialize();
                       }
                       
                       {{GeneratedCodeAttribute.Render()}}
                       [System.Runtime.InteropServices.UnmanagedCallersOnly]
                       private static void ClassInit(System.IntPtr cls, System.IntPtr clsData)
                       {
                           var classDefinition = (global::CodeBrix.Develop.UI.GObject.Internal.ObjectClassUnmanaged*) cls;
                           classDefinition->Dispose = &Dispose;
                           classDefinition->Constructed = &Constructed;

                           CompositeTemplateClassInit(cls, clsData);
                       }

                       {{GeneratedCodeAttribute.Render()}}
                       [System.Runtime.InteropServices.UnmanagedCallersOnly]
                       private static void InstanceInit(System.IntPtr instance, System.IntPtr cls)
                       {
                           CompositeTemplateInstanceInit(instance, cls);
                       }

                       {{GeneratedCodeAttribute.Render()}}
                       [System.Runtime.InteropServices.UnmanagedCallersOnly]
                       private static void Dispose(System.IntPtr instance)
                       {
                           CompositeTemplateDispose(instance);
                           GetParentTypeClass()->Dispose(instance);
                       }

                       {{GeneratedCodeAttribute.Render()}}
                       [System.Runtime.InteropServices.UnmanagedCallersOnly]
                       private static void Constructed(System.IntPtr instance)
                       {
                           if(!global::CodeBrix.Develop.UI.GObject.Internal.InstanceCache.TryGetObject(instance, out _))
                           {
                                var handle = new {{subclassData.ParentHandle}}(instance);
                                var obj = new {{subclassData.TypeData.Properties.NameGenericArguments}}(handle);
                                global::CodeBrix.Develop.UI.GObject.Internal.InstanceCache.AddToggleRef(obj);
                           }
                           else
                           {
                                // If the instance is already in the cache we are somewhere up
                                // in the inheritance chain and don't need to create a new dotnet instance.
                           }

                           GetParentTypeClass()->Constructed(instance);
                       }

                       private static global::CodeBrix.Develop.UI.GObject.Internal.ObjectClassUnmanaged* GetParentTypeClass()
                       {
                            var parentType = global::CodeBrix.Develop.UI.GObject.Internal.Functions.TypeParent(GType);
                            return (global::CodeBrix.Develop.UI.GObject.Internal.ObjectClassUnmanaged*) global::CodeBrix.Develop.UI.GObject.Internal.TypeClass.Peek(parentType).DangerousGetHandle();
                       }

                       /// <summary>
                       /// This method is called by all generated constructors.
                       /// Implement this partial method to initialize all members.
                       /// Decorating this method with "MemberNotNullAttribute" for
                       /// the appropriate members can remove nullable warnings.
                       /// </summary>
                       partial void Initialize();
                       
                       /// <summary>
                       /// This method is called during GObject class initialization. It is
                       /// meant to set up Gtk composite templates.
                       /// </summary>
                       /// <remarks>
                       /// The content of this method can be generated by the CodeBrix.Develop.UI
                       /// nuget package if a class is decorated with the [CodeBrix.Develop.UI.Gtk.Template] attribute.
                       /// </remarks>
                       static partial void CompositeTemplateClassInit(System.IntPtr cls, System.IntPtr clsData);
                      
                       /// <summary>
                       /// This method is called during GObject instance initialization. It is
                       /// meant to set up Gtk composite templates.
                       /// </summary>
                       /// <remarks>
                       /// The content of this method can be generated by the CodeBrix.Develop.UI
                       /// nuget package if a class is decorated with the [CodeBrix.Develop.UI.Gtk.Template] attribute.
                       /// </remarks>
                       static partial void CompositeTemplateInstanceInit(System.IntPtr instance, System.IntPtr cls);
                      
                       /// <summary>
                       /// This method is called during GObject instance disposal. It is
                       /// meant to dispose Gtk composite templates.
                       /// </summary>
                       /// <remarks>
                       /// The content of this method can be generated by the CodeBrix.Develop.UI
                       /// nuget package if a class is decorated with the [CodeBrix.Develop.UI.Gtk.Template] attribute.
                       /// </remarks>
                       static partial void CompositeTemplateDispose(System.IntPtr instance);
                       
                       /// <summary>
                       /// This method is called during the dotnet constructor call. It is
                       /// meant to initialize composite templates.
                       /// </summary>
                       /// <remarks>
                       /// The content of this method can be generated by the CodeBrix.Develop.UI
                       /// nuget package if a member is decorated with the [CodeBrix.Develop.UI.Gtk.Connect] attribute.
                       /// </remarks>
                       partial void CompositeTemplateInitialize();
                  }
                  """;
    }

    private static string RenderAbstractSubclassContent(SubclassData subclassData)
    {
        var qualifiedName = subclassData.QualifiedName is null
            ? "null"
            : $"\"{subclassData.QualifiedName}\"";

        return $$"""
                  {{subclassData.TypeData.Properties.Accessibility}} unsafe partial class {{subclassData.TypeData.Properties.NameGenericArguments}} : {{subclassData.Parent}}, global::CodeBrix.Develop.UI.GObject.GTypeProvider
                  {
                       {{GeneratedCodeAttribute.Render()}}
                       private static readonly global::CodeBrix.Develop.UI.GObject.Type GType = global::CodeBrix.Develop.UI.GObject.Internal.SubclassRegistrar.RegisterAbstract<{{subclassData.TypeData.Properties.NameGenericArguments}}, {{subclassData.Parent}}>(&ClassInit, &InstanceInit, {{qualifiedName}});

                       /// <summary>
                       /// Return the registered GObject type for this class.
                       /// </summary>
                       {{GeneratedCodeAttribute.Render()}}
                       public static new global::CodeBrix.Develop.UI.GObject.Type GetGType() => GType;

                       /// <summary>
                       /// Initializes the managed abstract base for a derived instance of {{subclassData.TypeData.Properties.Name}}.
                       /// </summary>
                       /// <param name="handle">A handle to the C instance.</param>
                       {{GeneratedCodeAttribute.Render()}}
                       protected internal {{subclassData.TypeData.Properties.Name}}({{subclassData.ParentHandle}} handle) : base(handle)
                       {
                           CompositeTemplateInitialize();
                           Initialize();
                       }

                       {{GeneratedCodeAttribute.Render()}}
                       [System.Runtime.InteropServices.UnmanagedCallersOnly]
                       private static void ClassInit(System.IntPtr cls, System.IntPtr clsData)
                       {
                           var classDefinition = (global::CodeBrix.Develop.UI.GObject.Internal.ObjectClassUnmanaged*) cls;
                           classDefinition->Dispose = &Dispose;

                           CompositeTemplateClassInit(cls, clsData);
                       }

                       {{GeneratedCodeAttribute.Render()}}
                       [System.Runtime.InteropServices.UnmanagedCallersOnly]
                       private static void InstanceInit(System.IntPtr instance, System.IntPtr cls)
                       {
                           CompositeTemplateInstanceInit(instance, cls);
                       }

                       {{GeneratedCodeAttribute.Render()}}
                       [System.Runtime.InteropServices.UnmanagedCallersOnly]
                       private static void Dispose(System.IntPtr instance)
                       {
                           CompositeTemplateDispose(instance);
                           GetParentTypeClass()->Dispose(instance);
                       }

                       // NOTE: unlike the concrete-subclass renderer, no "Constructed" vtable hook is
                       // installed here. That hook exists to create the managed wrapper instance, and an
                       // abstract type can never be instantiated - the concrete type further down the
                       // inheritance chain installs its own hook and creates the wrapper. Leaving this
                       // slot untouched lets the derived type's chained call reach the parent directly.
                       private static global::CodeBrix.Develop.UI.GObject.Internal.ObjectClassUnmanaged* GetParentTypeClass()
                       {
                            var parentType = global::CodeBrix.Develop.UI.GObject.Internal.Functions.TypeParent(GType);
                            return (global::CodeBrix.Develop.UI.GObject.Internal.ObjectClassUnmanaged*) global::CodeBrix.Develop.UI.GObject.Internal.TypeClass.Peek(parentType).DangerousGetHandle();
                       }

                       /// <summary>
                       /// This method is called by all generated constructors.
                       /// Implement this partial method to initialize all members.
                       /// Decorating this method with "MemberNotNullAttribute" for
                       /// the appropriate members can remove nullable warnings.
                       /// </summary>
                       partial void Initialize();

                       /// <summary>
                       /// This method is called during GObject class initialization. It is
                       /// meant to set up Gtk composite templates.
                       /// </summary>
                       /// <remarks>
                       /// The content of this method can be generated by the CodeBrix.Develop.UI
                       /// nuget package if a class is decorated with the [CodeBrix.Develop.UI.Gtk.Template] attribute.
                       /// </remarks>
                       static partial void CompositeTemplateClassInit(System.IntPtr cls, System.IntPtr clsData);

                       /// <summary>
                       /// This method is called during GObject instance initialization. It is
                       /// meant to set up Gtk composite templates.
                       /// </summary>
                       /// <remarks>
                       /// The content of this method can be generated by the CodeBrix.Develop.UI
                       /// nuget package if a class is decorated with the [CodeBrix.Develop.UI.Gtk.Template] attribute.
                       /// </remarks>
                       static partial void CompositeTemplateInstanceInit(System.IntPtr instance, System.IntPtr cls);

                       /// <summary>
                       /// This method is called during GObject instance disposal. It is
                       /// meant to dispose Gtk composite templates.
                       /// </summary>
                       /// <remarks>
                       /// The content of this method can be generated by the CodeBrix.Develop.UI
                       /// nuget package if a class is decorated with the [CodeBrix.Develop.UI.Gtk.Template] attribute.
                       /// </remarks>
                       static partial void CompositeTemplateDispose(System.IntPtr instance);

                       /// <summary>
                       /// This method is called during the dotnet constructor call. It is
                       /// meant to initialize composite templates.
                       /// </summary>
                       /// <remarks>
                       /// The content of this method can be generated by the CodeBrix.Develop.UI
                       /// nuget package if a member is decorated with the [CodeBrix.Develop.UI.Gtk.Connect] attribute.
                       /// </remarks>
                       partial void CompositeTemplateInitialize();
                  }
                  """;
    }

    private static string RenderUnref(SubclassData subclassData)
    {
        /*
         * - Regular objects can be unrefed because there was an additional toggle ref added so one valid ref remains
         * - Initially unowned objects have a floating reference which must be sunk and then removed
         */

        /*
         * Why RefSink?
         * CodeBrix.Develop.UI.Gtk.Button: Floating after creation -> gets sunk.
         * CodeBrix.Develop.UI.Gtk.Window: Not floating after creation, because GTK owns a ref -> Ref count increased by 1 (implicit no ownership transfer)
         */

        return subclassData.IsInitiallyUnowned
            ? """
              global::CodeBrix.Develop.UI.GObject.Internal.Object.RefSink(ptr);
              global::CodeBrix.Develop.UI.GObject.Internal.Object.Unref(ptr);
              """
            : "global::CodeBrix.Develop.UI.GObject.Internal.Object.Unref(ptr);";
    }
}
