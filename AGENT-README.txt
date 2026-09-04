================================================================================
AGENT-README: CodeBrix.Develop.UI
A Guide for AI Coding Agents — CONSUMING the CodeBrix.Develop.UI NuGet package
================================================================================

OVERVIEW
========
CodeBrix.Develop.UI is a fully managed C# binding of the GTK 4 user-interface
toolkit, the GtkSourceView 5 source-code editing widget, and the supporting
GNOME platform libraries (Gdk, Gsk, Pango, PangoCairo, Cairo, HarfBuzz,
FreeType2, GdkPixbuf, Graphene, Gio, GObject and GLib). It lets a C#
application build GTK 4 desktop user interfaces, use the GObject type system
(subclassing, properties, signals) from managed code, and call the underlying
GNOME platform APIs directly. It targets .NET 10 or later.

Provenance: the binding, its generator tooling and the two Roslyn integration
assemblies are derived from the gir.core project
(https://github.com/gircore/gir.core), upstream commit
41bd0a6b0855ec4f50c2f702d394333c4c17c3f3 plus a few later upstream fixes; see
THIRD-PARTY-NOTICES.txt. Every namespace is renamed: what upstream calls
Gtk, Gdk, GLib, GObject, Gio, Pango, Cairo, ... is
CodeBrix.Develop.UI.Gtk, CodeBrix.Develop.UI.Gdk, CodeBrix.Develop.UI.GLib,
CodeBrix.Develop.UI.GObject, CodeBrix.Develop.UI.Gio,
CodeBrix.Develop.UI.Pango, CodeBrix.Develop.UI.Cairo, ... respectively. Do NOT
write the upstream namespaces or reference the upstream packages; they do not
exist in this package. Type names, method names and signatures inside those
namespaces are otherwise the upstream ones.

The binding is broad: roughly three thousand five hundred public types across
the fourteen namespaces. This file documents every FEATURE AREA and the types a
consumer actually reaches for. Anything not named here almost certainly still
exists under the same name the C library uses, with the naming rules given
under CORE API REFERENCE below.


INSTALLATION
============
PackageId: CodeBrix.Develop.UI

    dotnet add package CodeBrix.Develop.UI

NuGet dependencies: none. All fourteen namespaces ship inside this one package
as thirteen assemblies, plus two Roslyn analyzer/source-generator assemblies
that are applied to your compilation automatically.

License: MIT (the LICENSE file in the repository, and
PackageLicenseExpression=MIT in the packaging metadata). Accepting the package
license is required by the package.

NOTE ON THE PACKAGE ID: unlike most CodeBrix packages, this id carries NO
license suffix. It is CodeBrix.Develop.UI — not
CodeBrix.Develop.UI.MitLicenseForever. This is a deliberate deviation from the
family convention for this package.

Native runtime requirement: the GTK 4 native libraries must be present on the
machine AT RUN TIME. They are loaded dynamically by name and are never
redistributed inside this package.

  * Debian-based Linux:  sudo apt install libgtk-4-1
    (present on most Linux desktops already)
  * Using the GtkSourceView binding additionally needs:
    sudo apt install libgtksourceview-5-0
  * Windows and macOS: install a GTK 4 runtime yourself (MSYS2 on Windows,
    Homebrew on macOS) so the loader can find the library names listed under
    COMMON PITFALLS TO AVOID.

Target framework: net10.0. There is no multi-targeting and no netstandard
surface for the runtime libraries.


KEY NAMESPACES / USINGS
=======================
    using CodeBrix.Develop.UI.Gtk;        // GTK 4 widgets, windows, dialogs
    using CodeBrix.Develop.UI.GtkSource;  // GtkSourceView 5 code editor
    using CodeBrix.Develop.UI.Gdk;        // display, monitors, textures, input
    using CodeBrix.Develop.UI.Gsk;        // GPU render-node scene graph
    using CodeBrix.Develop.UI.Gio;        // application framework, files, I/O,
                                          //   actions, menus, settings
    using CodeBrix.Develop.UI.GObject;    // the GObject type system
    using CodeBrix.Develop.UI.GLib;       // main loop, bytes, variants, errors
    using CodeBrix.Develop.UI.Pango;      // text layout and shaping
    using CodeBrix.Develop.UI.PangoCairo; // Pango rendered onto Cairo
    using CodeBrix.Develop.UI.Cairo;      // 2D vector drawing
    using CodeBrix.Develop.UI.HarfBuzz;   // text-shaping primitives
    using CodeBrix.Develop.UI.Freetype2;  // FreeType type stubs
    using CodeBrix.Develop.UI.GdkPixbuf;  // image loading and scaling
    using CodeBrix.Develop.UI.Graphene;   // geometry (points, rects, matrices)

Namespaces are not assemblies here. CodeBrix.Develop.UI.dll contains BOTH the
CodeBrix.Develop.UI.Gtk and the CodeBrix.Develop.UI.GtkSource namespaces; the
CodeBrix.Develop.UI.GObject namespace is split across two assemblies (the
GObject library plus a few root types such as GTypeProvider, InstanceFactory
and Type that live in the GLib library). Reference the package, not individual
assemblies, and this never matters.

Almost every file needs at least two usings, because Gtk types constantly take
Gio and GObject arguments. A very common opening block:

    using System;
    using CodeBrix.Develop.UI.Gtk;
    using Gio = CodeBrix.Develop.UI.Gio;
    using GObj = CodeBrix.Develop.UI.GObject;
    using GtkSrc = CodeBrix.Develop.UI.GtkSource;

Aliases are recommended because `Gtk.Application` and `Gio.Application` are
different types, `Gtk.File`-like collisions exist between namespaces
(Gio.File vs GtkSource.File), and the fully qualified names are long.


CORE API REFERENCE
==================
Naming rules that hold across the whole generated surface. Learn these five and
you can predict nearly any member without looking it up.

1. CONSTRUCTION IS A STATIC FACTORY, NOT A C# CONSTRUCTOR.
   Every GObject class exposes `static T New(...)` mirroring the C
   `gtk_xxx_new()` function, plus named variants for the other C constructors:

       Button.New()                          // gtk_button_new
       Button.NewWithLabel("OK")             // gtk_button_new_with_label
       Button.NewFromIconName("open")        // gtk_button_new_from_icon_name
       Box.New(Orientation.Vertical, 6)      // gtk_box_new
       Label.New("text")                     // gtk_label_new (accepts null)

   Two more factories exist on every class:

       static T NewWithProperties(GObject.ConstructArgument[] args)
       static T NewFromPointer(IntPtr ptr, bool ownsHandle)

   `NewWithProperties` is the escape hatch for C constructors that were not
   bound (see AlertDialog under COMMON PITFALLS). The regular C# constructor
   `new Button(params ConstructArgument[])` still compiles but is marked
   [Obsolete] with diagnostic id GirCore1007 — do not use it in new code.

2. C FUNCTIONS BECOME Get*/Set* METHODS.
   `gtk_window_set_title` becomes `window.SetTitle(string?)`,
   `gtk_window_get_title` becomes `window.GetTitle()`. Out-parameters stay
   out-parameters: `window.GetDefaultSize(out int width, out int height)`.

3. GOBJECT PROPERTIES ALSO BECOME C# PROPERTIES.
   In addition to Get/Set methods, every GObject property is generated as a
   real C# property plus a static descriptor:

       public static readonly Property<string?, Window> TitlePropertyDefinition;
       public string? Title { get; set; }

   So `window.Title = "Hello";` and `window.SetTitle("Hello");` do the same
   thing. The descriptor is what you use for change notification (see
   SIGNALS below). Not every C getter has a matching property — properties
   exist only where the C library declared a GObject property.

4. SIGNALS BECOME `OnXxx` EVENTS. See SIGNALS (EVENTS) below.

5. ENUMS AND FLAGS KEEP THEIR C VALUES, PASCAL-CASED:
   `Orientation.Horizontal`, `Orientation.Vertical`, `Align.Center`,
   `PolicyType.Automatic`, `Gio.ApplicationFlags.FlagsNone`,
   `Gio.ApplicationFlags.HandlesOpen`, `WrapMode.Word`. Flags enums are
   `[Flags]` and combine with `|`.

Free functions (C functions with no `self` argument) live on a per-namespace
static `Functions` class: `Gtk.Functions.Init()`,
`Gio.Functions.FileNewForPath(path)`, `GLib.Functions.IdleAdd(...)`.
Per-namespace `Constants` classes carry the C #defines:
`Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION`,
`GLib.Constants.PRIORITY_DEFAULT`, `GLib.Constants.SOURCE_REMOVE`.


APPLICATION LIFECYCLE
=====================
The entry point is `Gtk.Application`, which derives from `Gio.Application`.

    namespace CodeBrix.Develop.UI.Gtk
    public partial class Application : Gio.Application, Gio.ActionGroup,
                                       Gio.ActionMap
    {
        public static new Application New(string? applicationId,
                                          Gio.ApplicationFlags flags);
        public void AddWindow(Window window);
        public void RemoveWindow(Window window);
        public Window? GetActiveWindow();
        public Window? GetWindowById(uint id);
        public GLib.List GetWindows();
        public Gio.MenuModel? GetMenubar();
        public void SetMenubar(Gio.MenuModel? menubar);
        public Gio.Menu? GetMenuById(string id);
        public string[] GetAccelsForAction(string detailedActionName);
        public void SetAccelsForAction(string detailedActionName,
                                       string[] accels);
        public string[] GetActionsForAccel(string accel);
        public string[] ListActionDescriptions();
        public uint Inhibit(Window? window, ApplicationInhibitFlags flags,
                            string? reason);
        public void Uninhibit(uint cookie);
    }

Inherited from `Gio.Application` (the ones that matter):

        public int Run(string[]? argv);
        public int RunWithSynchronizationContext(string[]? args);
        public void Activate();
        public void Quit();
        public void Hold();
        public void Release();
        public bool Register(Gio.Cancellable? cancellable);
        public string? GetApplicationId();
        public void SetApplicationId(string? applicationId);
        public Gio.ApplicationFlags GetFlags();
        public void SetFlags(Gio.ApplicationFlags flags);
        public void Open(Gio.File[] files, int nFiles, string hint);
        public void SetResourceBasePath(string? resourcePath);
        public void SendNotification(string? id, Gio.Notification notification);
        public void MarkBusy();  public void UnmarkBusy();

`RunWithSynchronizationContext(string[]?)` is a hand-written addition, not a
C API. It installs a SynchronizationContext backed by the GLib main loop for
the duration of the run, so `await` in your event handlers resumes ON THE UI
THREAD. PREFER IT OVER `Run(...)` in any application that uses async/await.
Both return the process exit code.

Application signals (declared on Gio.Application, so `sender` is typed
`Gio.Application`):

        public event SignalHandler<Gio.Application> OnActivate;
        public event SignalHandler<Gio.Application> OnStartup;
        public event SignalHandler<Gio.Application> OnShutdown;
        public event SignalHandler<Gio.Application,
                                   Gio.Application.OpenSignalArgs> OnOpen;
        public event ReturningSignalHandler<Gio.Application,
                     Gio.Application.CommandLineSignalArgs, int> OnCommandLine;
        public event ReturningSignalHandler<Gio.Application,
                     Gio.Application.HandleLocalOptionsSignalArgs, int>
                     OnHandleLocalOptions;
        public event ReturningSignalHandler<Gio.Application, bool> OnNameLost;

Declared on Gtk.Application itself:

        public event SignalHandler<Application> OnQueryEnd;
        public event SignalHandler<Application,
                                   Application.WindowAddedSignalArgs>
                                   OnWindowAdded;
        public event SignalHandler<Application,
                                   Application.WindowRemovedSignalArgs>
                                   OnWindowRemoved;

Because the OnActivate `sender` is statically a `Gio.Application`, capture your
`Gtk.Application` local instead of casting the sender — it is shorter and
always correct.

`Gio.ApplicationFlags` values: FlagsNone, DefaultFlags, IsService, IsLauncher,
HandlesOpen, HandlesCommandLine, SendEnvironment, NonUnique, CanOverrideAppId,
AllowReplacement, Replace.

Windows:

    public static ApplicationWindow New(Application application);
    public static Window New();

`ApplicationWindow` derives from `Window`; creating one with
`ApplicationWindow.New(app)` also registers it with the application. Key
`Window` members:

        public void SetTitle(string? title);      // or: window.Title = "..."
        public void SetDefaultSize(int width, int height);
        public void GetDefaultSize(out int width, out int height);
        public void SetChild(Widget? child);      // GTK 4: ONE child
        public Widget? GetChild();
        public void SetTitlebar(Widget? titlebar);
        public void SetTransientFor(Window? parent);
        public void SetModal(bool modal);
        public void SetResizable(bool resizable);
        public void SetHideOnClose(bool setting);
        public void SetDecorated(bool setting);
        public void SetIconName(string? name);
        public void Present();       public void PresentWithTime(uint stamp);
        public void Close();         public void Destroy();
        public void Maximize();      public void Unmaximize();
        public void Minimize();      public void Unminimize();
        public void Fullscreen();    public void Unfullscreen();
        public bool IsMaximized();   public bool IsFullscreen();
        public bool IsSuspended();   public bool GetIsActive();
        public Application? GetApplication();
        public void SetApplication(Application? application);

Window signals:

        public event ReturningSignalHandler<Window, bool> OnCloseRequest;
        public event SignalHandler<Window> OnActivateDefault;
        public event SignalHandler<Window> OnActivateFocus;
        public event SignalHandler<Window> OnKeysChanged;

Return `true` from `OnCloseRequest` to VETO the close, `false` to allow it.

Main loop, without an Application (rare — for tests and embedding):

    Gtk.Module.Initialize();          // must run before touching any Gtk type
    var loop = GLib.MainLoop.New(null, false);
    loop.RunWithSynchronizationContext();   // or loop.Run();
    // from anywhere on the loop thread:  loop.Quit();

`Gtk.Functions.Init()`, `Gtk.Functions.InitCheck()` and
`Gtk.Functions.IsInitialized()` exist, but `Gtk.Module.Initialize()` already
calls `Init()` for you, and merely touching `Gtk.Application` triggers
`Gtk.Module.Initialize()` from a static constructor. See RESOURCES, MODULES AND
INITIALIZATION below for the ordering rules.


WIDGETS AND LAYOUT
==================
GTK 4 has NO `Add()` and NO `ShowAll()`. Parenting is done with a
container-specific method, and widgets are visible by default.

    Widget      -> the base class of every widget
    Window      -> SetChild(Widget?)              exactly one child
    Box         -> Append / Prepend / Remove      a linear stack
    Grid        -> Attach(child, col, row, w, h)  a table
    Paned       -> SetStartChild / SetEndChild    a splitter
    ScrolledWindow -> SetChild(Widget?)           scrolls one child
    HeaderBar   -> PackStart / PackEnd / SetTitleWidget
    Notebook    -> AppendPage(child, tabLabel)
    Stack       -> AddNamed / AddTitled (with StackSwitcher / StackSidebar)

Widget: the members every widget has
------------------------------------
        public void SetParent(Widget parent);   // only for custom containers
        public void Unparent();
        public void InsertBefore(Widget parent, Widget? nextSibling);
        public void InsertAfter(Widget parent, Widget? previousSibling);
        public Widget? GetFirstChild();
        public Widget? GetNextSibling();
        public Root? GetRoot();     // usually the Window
        public Native? GetNative();
        public Gdk.Display GetDisplay();

        public void SetVisible(bool visible);
        public void Show();  public void Hide();   // legacy: use SetVisible
        public void SetSensitive(bool sensitive);
        public void SetTooltipText(string? text);
        public void SetName(string name);
        public bool GrabFocus();
        public void SetFocusable(bool focusable);

        public void SetHexpand(bool expand);
        public void SetVexpand(bool expand);
        public void SetHalign(Align align);
        public void SetValign(Align align);
        public void SetSizeRequest(int width, int height);
        public void SetMarginTop(int margin);
        public void SetMarginBottom(int margin);
        public void SetMarginStart(int margin);
        public void SetMarginEnd(int margin);

        public void AddCssClass(string cssClass);
        public void RemoveCssClass(string cssClass);
        public void SetCssClasses(string[] classes);
        public StyleContext GetStyleContext();

        public void AddController(EventController controller);
        public void InsertActionGroup(string name, Gio.ActionGroup? group);
        public void SetLayoutManager(LayoutManager? layoutManager);
        public void QueueDraw();  public void QueueResize();
        public void QueueAllocate();

Equivalent C# properties exist for most of these: `Hexpand`, `Vexpand`,
`Halign`, `Valign`, `MarginTop`, `MarginBottom`, `MarginStart`, `MarginEnd`,
`WidthRequest`, `HeightRequest`, `CssClasses`, `CssName`, `Cursor`, `Focusable`,
`FocusOnClick`, `HasTooltip`, `LayoutManager`, `Name`, ... So
`button.MarginTop = 12;` works as well as `button.SetMarginTop(12);`.

Containers and layout
---------------------
    public static Box New(Orientation orientation, int spacing);
        void Append(Widget child);
        void Prepend(Widget child);
        void Remove(Widget child);
        void InsertChildAfter(Widget child, Widget? sibling);
        void ReorderChildAfter(Widget child, Widget? sibling);
        void SetHomogeneous(bool homogeneous);
        void SetSpacing(int spacing);
        void SetBaselinePosition(BaselinePosition position);

    public static Grid New();
        void Attach(Widget child, int column, int row, int width, int height);
        void AttachNextTo(Widget child, Widget? sibling, PositionType side,
                          int width, int height);
        Widget? GetChildAt(int column, int row);
        void Remove(Widget child);
        void InsertRow(int position);   void RemoveRow(int position);
        void InsertColumn(int position); void RemoveColumn(int position);
        void SetRowSpacing(uint spacing);
        void SetColumnSpacing(uint spacing);
        void SetRowHomogeneous(bool homogeneous);
        void SetColumnHomogeneous(bool homogeneous);
        void QueryChild(Widget child, out int column, out int row,
                        out int width, out int height);

    public static ScrolledWindow New();
        void SetChild(Widget? child);
        void SetPolicy(PolicyType hscrollbarPolicy,
                       PolicyType vscrollbarPolicy);
        void GetPolicy(out PolicyType h, out PolicyType v);
        void SetHasFrame(bool hasFrame);
        void SetMinContentWidth(int width);
        void SetMinContentHeight(int height);
        void SetMaxContentWidth(int width);
        void SetMaxContentHeight(int height);
        void SetPropagateNaturalWidth(bool propagate);
        void SetPropagateNaturalHeight(bool propagate);
        void SetKineticScrolling(bool kineticScrolling);
        void SetOverlayScrolling(bool overlayScrolling);
        Adjustment GetHadjustment();   Adjustment GetVadjustment();

    public static Paned New(Orientation orientation);
        void SetStartChild(Widget? child);   void SetEndChild(Widget? child);
        void SetPosition(int position);      int GetPosition();
        void SetResizeStartChild(bool resize);
        void SetResizeEndChild(bool resize);
        void SetShrinkStartChild(bool resize);
        void SetShrinkEndChild(bool resize);
        void SetWideHandle(bool wide);

    public static HeaderBar New();
        void PackStart(Widget child);   void PackEnd(Widget child);
        void Remove(Widget child);
        void SetTitleWidget(Widget? titleWidget);
        void SetShowTitleButtons(bool setting);
        void SetDecorationLayout(string? layout);

    public static Notebook New();
        int AppendPage(Widget child, Widget? tabLabel);
        int PrependPage(Widget child, Widget? tabLabel);
        int InsertPage(Widget child, Widget? tabLabel, int position);
        void RemovePage(int pageNum);
        int GetNPages();      Widget? GetNthPage(int pageNum);
        int GetCurrentPage(); void SetCurrentPage(int pageNum);
        int PageNum(Widget child);
        void SetTabPos(PositionType pos);
        void SetShowTabs(bool showTabs);
        void SetScrollable(bool scrollable);
        void SetTabReorderable(Widget child, bool reorderable);
        void SetTabLabelText(Widget child, string tabText);
        Gio.ListModel GetPages();

Other layout containers, all with `New()` unless shown:
`CenterBox` (SetStartWidget/SetCenterWidget/SetEndWidget), `Overlay`
(SetChild + AddOverlay), `FlowBox`, `Frame.New(string? label)`,
`Expander.New(string? label)`, `Revealer`, `ActionBar`, `SearchBar`,
`AspectFrame`, `Fixed`, `Viewport`, `WindowHandle`, `Stack`, `StackSwitcher`,
`StackSidebar`, `Statusbar`, `Separator.New(Orientation)`. Layout managers
themselves are objects too: `BoxLayout`, `GridLayout`, `BinLayout`,
`CenterLayout`, `FixedLayout`, `OverlayLayout`, `ConstraintLayout`,
`CustomLayout`.

Controls
--------
    public static Button New();
    public static Button NewWithLabel(string label);
    public static Button NewWithMnemonic(string label);
    public static Button NewFromIconName(string iconName);
        public event SignalHandler<Button> OnClicked;
        public event SignalHandler<Button> OnActivate;

    public static Label New(string? str);
    public static Label NewWithMnemonic(string? str);
        string GetLabel();  void SetLabel(string str);
        string GetText();   void SetText(string str);
        void SetMarkup(string str);      // Pango markup
        void SetSelectable(bool setting);
        void SetWrap(bool wrap);
        void SetWrapMode(Pango.WrapMode wrapMode);
        void SetXalign(float xalign);

    public static Entry New();
    public static Entry NewWithBuffer(EntryBuffer buffer);
        // text access comes from the Editable interface:
        string GetText();   void SetText(string text);
        bool Editable { get; set; }

    public static CheckButton New();
    public static CheckButton NewWithLabel(string? label);
    public static CheckButton NewWithMnemonic(string? label);
    public static ToggleButton New();          // + NewWithLabel/NewWithMnemonic
    public static Switch New();
    public static Spinner New();
    public static ProgressBar New();
    public static LevelBar New();
    public static Scale New(Orientation orientation, Adjustment? adjustment);
    public static Scale NewWithRange(Orientation orientation,
                                     double min, double max, double step);
    public static SpinButton New(Adjustment? adjustment, double climbRate,
                                 uint digits);
    public static SpinButton NewWithRange(double min, double max, double step);
    public static Adjustment New(double value, double lower, double upper,
                                 double stepIncrement, double pageIncrement,
                                 double pageSize);
    public static SearchEntry New();
    public static PasswordEntry New();
    public static EditableLabel New(string str);
    public static LinkButton New(string uri);
    public static MenuButton New();
    public static Popover New();
    public static PopoverMenu NewFromModel(Gio.MenuModel? model);
    public static PopoverMenuBar NewFromModel(Gio.MenuModel? model);
    public static DropDown New(Gio.ListModel? model, Expression? expression);
    public static DropDown NewFromStrings(string[] strings);
    public static ComboBoxText New();       // + NewWithEntry()
    public static Image New();
    public static Image NewFromIconName(string? iconName);
    public static Image NewFromFile(string filename);
    public static Image NewFromResource(string resourcePath);
    public static Image NewFromPaintable(Gdk.Paintable? paintable);
    public static Image NewFromPixbuf(GdkPixbuf.Pixbuf? pixbuf);
    public static Picture New();            // + NewForFile / NewForFilename /
                                            //   NewForResource, NewForPaintable
    public static Calendar New();
    public static TreeExpander New();
    public static Video New();              // media playback widget
    public static GLArea New();             // raw OpenGL drawing surface

Plain text editing (as opposed to the source editor):

    public static TextView New();
    public static TextView NewWithBuffer(TextBuffer buffer);
        TextBuffer GetBuffer();   void SetBuffer(TextBuffer? buffer);
        void SetEditable(bool setting);
        void SetMonospace(bool monospace);
        void SetWrapMode(WrapMode wrapMode);
        bool ScrollToIter(TextIter iter, double withinMargin, bool useAlign,
                          double xalign, double yalign);

    TextBuffer:
        void SetText(string text, int len);        // len -1 = NUL-terminated
        string GetText(TextIter start, TextIter end, bool includeHiddenChars);
        string GetSlice(TextIter start, TextIter end, bool includeHiddenChars);
        void GetBounds(out TextIter start, out TextIter end);
        void GetStartIter(out TextIter iter);
        void GetEndIter(out TextIter iter);
        void Insert(TextIter iter, string text, int len);
        void InsertAtCursor(string text, int len);
        void InsertMarkup(TextIter iter, string markup, int len);
        int GetLineCount();  int GetCharCount();
        bool GetModified();  void SetModified(bool setting);
        TextMark CreateMark(string? markName, TextIter where, bool leftGravity);
        void PlaceCursor(TextIter where);


SIGNALS (EVENTS)
================
Every GObject signal is generated as a C# event named `On` + PascalCase of the
signal name, plus a static descriptor object. For `GtkButton::clicked`:

    public static readonly Signal<Button> ClickedSignal =
        new (unmanagedName: "clicked", managedName: nameof(OnClicked));

    public event SignalHandler<Button> OnClicked
    {
        add    => ClickedSignal.Connect(this, value);
        remove => ClickedSignal.Disconnect(this, value);
    }

There are four delegate shapes, all in CodeBrix.Develop.UI.GObject:

    public delegate void SignalHandler<in TSender>(TSender sender,
                                                   EventArgs args)
        where TSender : NativeObject;

    public delegate void SignalHandler<in TSender, in TSignalArgs>(
        TSender sender, TSignalArgs args)
        where TSender : NativeObject where TSignalArgs : SignalArgs;

    public delegate TReturn ReturningSignalHandler<in TSender, out TReturn>(
        TSender sender, EventArgs args) where TSender : NativeObject;

    public delegate TReturn ReturningSignalHandler<in TSender, in TSignalArgs,
                                                   out TReturn>(
        TSender sender, TSignalArgs args)
        where TSender : NativeObject where TSignalArgs : SignalArgs;

So a handler ALWAYS takes two parameters. A signal that carries no data hands
you `EventArgs.Empty`; a signal that carries data hands you a generated nested
`XxxSignalArgs : SignalArgs` class with one read-only property per C argument:

    var controller = EventControllerKey.New();
    controller.OnKeyPressed += (sender, args) =>
    {
        // args.Keyval (uint), args.Keycode (uint),
        // args.State (Gdk.ModifierType)
        return args.Keyval == (uint) Gdk.Constants.KEY_Escape;  // true=handled
    };
    widget.AddController(controller);

Usage patterns:

    button.OnClicked += (_, _) => Console.WriteLine("clicked");

    void Handler(Button sender, EventArgs args) { }
    button.OnClicked += Handler;      // named method
    button.OnClicked -= Handler;      // removable

The descriptor form gives you two things `+=` cannot: connecting AFTER the
default handler, and connecting to a signal DETAIL:

    public class Signal<TSender> : SignalDefinition
    {
        public string UnmanagedName { get; }
        public string ManagedName { get; }
        public uint Id { get; }
        public void Connect(TSender sender, SignalHandler<TSender> handler,
                            bool after = false, string? detail = null);
        public void Disconnect(TSender sender, SignalHandler<TSender> handler);
    }
    // and Signal<TSender, TSignalArgs>, ReturningSignal<TSender, TReturn>,
    //     ReturningSignal<TSender, TSignalArgs, TReturn> in the same shape.

    Button.ClickedSignal.Connect(button, Handler, after: true);

DISCONNECT WITH THE SAME DELEGATE INSTANCE you connected. The closure lookup is
keyed on the delegate, so `-=` with a fresh lambda silently does nothing (it
asserts in debug builds and returns). Keep the delegate in a field if you need
to detach later.

Property change notification
----------------------------
`GObject.Object` exposes the `notify` signal:

    public event SignalHandler<GObject.Object, GObject.Object.NotifySignalArgs>
        OnNotify;                       // args.Pspec is a GObject.ParamSpec

    window.OnNotify += (sender, args) =>
    {
        var name = Window.TitlePropertyDefinition.UnmanagedName;
        if (args.Pspec.GetName() == name)
            Console.WriteLine("title changed");
    };

For a single property, the generated `Property<T, K>` descriptor is cleaner —
it connects to `notify` with the property name as the signal DETAIL, so your
handler only fires for that property:

    public sealed class Property<T, K> : PropertyDefinition<T>
    {
        public string UnmanagedName { get; }   // e.g. "default-width"
        public string ManagedName { get; }     // e.g. "DefaultWidth"
        public T Get(K obj);
        public void Set(K obj, T value);
        public void Notify(K sender,
            SignalHandler<Object, Object.NotifySignalArgs> handler,
            bool after = false);
        public void Unnotify(K sender,
            SignalHandler<Object, Object.NotifySignalArgs> handler);
    }

    Window.TitlePropertyDefinition.Notify(window, (_, _) => Redraw());


GOBJECT SUBCLASSING AND THE SHIPPED ROSLYN ATTRIBUTES
=====================================================
The package ships two Roslyn assemblies as analyzers (they are applied
automatically when you reference the package; there is nothing to install and
nothing to configure). They provide a source generator for GObject subclassing,
a source generator for GTK composite templates, and eight diagnostics.

Deriving a new GObject type
---------------------------
    namespace CodeBrix.Develop.UI.GObject
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SubclassAttribute<T>(string? qualifiedName = null) : Attribute
        where T : GObject.Object
    {
        public string? QualifiedName { get; }
    }

Write a PARTIAL class, annotate it with the parent type, and the generator
supplies the rest:

    using CodeBrix.Develop.UI.GObject;   // SubclassAttribute
    using CodeBrix.Develop.UI.Gtk;       // Widget, BinLayout

    [Subclass<Widget>]
    public partial class Thermometer
    {
        private double _celsius;

        // Called by every generated constructor. Do initialization HERE.
        partial void Initialize()
        {
            SetLayoutManager(BinLayout.New());
        }

        public double Celsius
        {
            get => _celsius;
            set { _celsius = value; QueueDraw(); }
        }
    }

    var t = Thermometer.NewWithProperties([]);

What the generator adds to your partial class:

    public static new GObject.Type GetGType();
    public static new Thermometer NewWithProperties(
        GObject.ConstructArgument[] constructArguments);
    public static new Thermometer NewFromPointer(IntPtr ptr, bool ownsHandle);
    protected internal Thermometer(<ParentHandle> handle);   // internal when
                                                             // sealed
    partial void Initialize();                               // you implement
    // plus the GObject class_init / instance_init / dispose / constructed
    // vtable plumbing, and the four CompositeTemplate* partial hooks used by
    // the [Template] generator below.

`[Subclass<T>("my-qualified-name")]` sets the registered GType name; omit it and
one is derived for you. `[Subclass<T>]` on an `abstract` class registers an
abstract GType instead (no instance is ever created for it).

Rules the analyzers enforce (all of these are real compile-time diagnostics):

    GirCore1002  Warning  A parameterless constructor on a [Subclass] type must
                          be replaced by `partial void Initialize()`.
    GirCore1004  Error    A [Subclass] type may not be generic. GObject has no
                          generics. Use a non-generic type, or a plain
                          (non-[Subclass]) wrapper for type-safe access.
    GirCore1005  Error    A [Subclass] type may not be nested inside a generic
                          type.
    GirCore1006  Warning  A constructor WITH parameters must be replaced by a
                          static factory method; instances must be creatable
                          through parameterless `NewWithProperties`.
    GirCore1008  Error    A [Subclass] type may not be `private`. `public` and
                          `internal` are supported (the GObject type system
                          must be able to see it).
    GirCore2001  Error    A [Template] class must also carry [Subclass].
    GirCore2002  Error    A [Template] class's [Subclass] parent must derive
                          from CodeBrix.Develop.UI.Gtk.Widget.
    GirCore2003  Warning  [Connect] is only honoured inside a [Template] class.

Two more diagnostics come from attributes on the binding itself:
`GirCore1007` (obsolete: the legacy `new Object(params ConstructArgument[])`
constructors) and `GirCore1009` (experimental:
`Module.SetCustomDllImportResolver`).

Supporting types you will meet:

    public interface NativeObject
    { GObject.Internal.ObjectHandle Handle { get; } }
    public interface GTypeProvider  { static abstract GObject.Type GetGType(); }
    public interface InstanceFactory{ static abstract object Create(IntPtr h,
                                                                   bool owns); }
    public class Object : IDisposable, NativeObject
    {
        public GObject.Internal.ObjectHandle Handle { get; }
        public static Object NewWithProperties(ConstructArgument[] args);
        public static Object NewFromPointer(IntPtr ptr, bool ownsHandle);
        public virtual void Dispose();
    }
    public sealed class ConstructArgument(string name, Value value)
        : IDisposable
    {
        public string Name { get; }
        public Value Value { get; }
    }
    public sealed class Value : IDisposable   // ctors for Object, Variant,
        // bool, int, uint, long, ulong, double, float, string, string[], Enum

    var win = Window.NewWithProperties([
        new GObject.ConstructArgument("title", new GObject.Value("Hi")),
        new GObject.ConstructArgument("default-width", new GObject.Value(640)),
    ]);

    public delegate int CompareDataFuncT<in T>(T a, T b)
        where T : GObject.NativeObject;
    // used by the hand-written convenience overload:
    public static CustomSorter New<T>(GObject.CompareDataFuncT<T> func)
        where T : GObject.NativeObject;


COMPOSITE TEMPLATES: [Template] AND [Connect]
=============================================
A composite template builds a widget's tree from a GTK Builder .ui XML document
instead of C# calls. Three pieces are involved, all in
CodeBrix.Develop.UI.Gtk:

    public interface TemplateLoader
    {
        static abstract GLib.Bytes Load(string resourceName);
    }

    public class GResource : TemplateLoader          // reads resource://<name>
    { public static GLib.Bytes Load(string resourceName); }

    public class AssemblyResource : TemplateLoader   // reads a .NET embedded
    { public static GLib.Bytes Load(string resourceName); }  // resource

    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateAttribute<TLoader>(string resourceName) : Attribute
        where TLoader : TemplateLoader
    { public string ResourceName { get; } }

    [AttributeUsage(AttributeTargets.Field)]
    public class ConnectAttribute(string? objectId = null) : Attribute
    { public string? ObjectId { get; } }

Usage — the .ui file is an EMBEDDED RESOURCE of your assembly, so
`AssemblyResource` is the loader and the resource name is the manifest name:

    using CodeBrix.Develop.UI.GObject;   // SubclassAttribute
    using CodeBrix.Develop.UI.Gtk;       // TemplateAttribute, ConnectAttribute,
                                         // AssemblyResource, Button, Label

    [Subclass<ApplicationWindow>]
    [Template<AssemblyResource>("MyApp.Views.MainWindow.ui")]
    public partial class MainWindow
    {
        [Connect] private readonly Button _openButton;
        [Connect("status_label")] private readonly Label _status;

        partial void Initialize()
        {
            _openButton.OnClicked += (_, _) => _status.SetText("opening...");
        }
    }

    var window = MainWindow.NewWithProperties([]);
    window.Present();

Rules and mechanics, all enforced or generated:

  * The class MUST carry BOTH [Subclass<...>] and [Template<...>], and the
    subclass parent MUST derive from Gtk.Widget (GirCore2001, GirCore2002).
  * [Connect] goes on a FIELD. With no argument, the FIELD NAME is used as the
    .ui object id; pass a string to use a different id
    (`[Connect("status_label")]`).
  * The generator emits `CompositeTemplateClassInit` (loads the bytes through
    your TLoader and calls the GTK "set template" + "bind template child" C
    API), `CompositeTemplateInstanceInit`, `CompositeTemplateDispose`, and a
    `CompositeTemplateInitialize` that assigns every [Connect] field from
    `GetTemplateChild(GetGType(), "<id>")` and is decorated with
    `[MemberNotNull(...)]` for those fields — so the fields can be
    non-nullable without a compiler warning even though you never assign them.
  * `CompositeTemplateInitialize` runs BEFORE your `Initialize()`, so the
    [Connect] fields are already populated when `Initialize()` executes.
  * If you use `GResource` instead, the resource name is the GResource path
    without the scheme (the loader prefixes `resource://` itself), and the
    compiled .gresource must be registered with
    `Gio.Functions.ResourcesRegister(Gio.Functions.ResourceLoad(path))` before
    the first instance is constructed.

For one-off (non-template) Builder use, `Gtk.Builder` is bound directly:

    public static Builder New();
    public static Builder NewFromFile(string filename);
    public static Builder NewFromResource(string resourcePath);
    public static Builder NewFromString(string @string, nint length);
        bool AddFromFile(string filename);
        bool AddFromResource(string resourcePath);
        bool AddFromString(string buffer, nint length);
        GObject.Object? GetObject(string name);
        void ExposeObject(string name, GObject.Object @object);
        void SetTranslationDomain(string? domain);


GTKSOURCEVIEW: THE CODE EDITOR WIDGET
=====================================
`CodeBrix.Develop.UI.GtkSource` is compiled into the SAME assembly as the Gtk
binding, so referencing the package is all it takes — but the native
libgtksourceview-5 library must be installed separately (see INSTALLATION).

    GtkSource.View   : Gtk.TextView        the editor widget
    GtkSource.Buffer : Gtk.TextBuffer      the document + highlighting state

Because they derive from the plain text widgets, EVERY Gtk.TextView and
Gtk.TextBuffer member listed above works on them unchanged (SetText, GetText,
GetBounds, GetLineCount, SetMonospace, SetWrapMode, ScrollToIter, ...).

    public static new View New();
    public static View NewWithBuffer(GtkSource.Buffer buffer);
        void SetShowLineNumbers(bool show);   bool GetShowLineNumbers();
        void SetShowLineMarks(bool show);
        void SetHighlightCurrentLine(bool highlight);
        void SetShowRightMargin(bool show);
        void SetRightMarginPosition(uint pos);
        void SetTabWidth(uint width);         uint GetTabWidth();
        void SetIndentWidth(int width);
        void SetInsertSpacesInsteadOfTabs(bool enable);
        void SetAutoIndent(bool enable);
        void SetIndentOnTab(bool enable);
        void SetSmartBackspace(bool smartBackspace);
        void SetSmartHomeEnd(SmartHomeEndType smartHomeEnd);
        void SetEnableSnippets(bool enableSnippets);
        void SetBackgroundPattern(BackgroundPatternType backgroundPattern);
        void SetIndenter(Indenter? indenter);
        void IndentLines(Gtk.TextIter start, Gtk.TextIter end);
        void UnindentLines(Gtk.TextIter start, Gtk.TextIter end);
        void PushSnippet(Snippet snippet, Gtk.TextIter? location);
        Completion GetCompletion();
        Hover GetHover();
        SpaceDrawer GetSpaceDrawer();
        Gutter GetGutter(Gtk.TextWindowType windowType);
        Annotations GetAnnotations();
        MarkAttributes GetMarkAttributes(string category, ref int priority);
        void SetMarkAttributes(string category, MarkAttributes attributes,
                               int priority);
        uint GetVisualColumn(Gtk.TextIter iter);

    public static new Buffer New(Gtk.TextTagTable? table);   // pass null
    public static Buffer NewWithLanguage(GtkSource.Language language);
        void SetLanguage(Language? language);   Language? GetLanguage();
        void SetStyleScheme(StyleScheme? scheme); StyleScheme? GetStyleScheme();
        void SetHighlightSyntax(bool highlight);
        void SetHighlightMatchingBrackets(bool highlight);
        void SetImplicitTrailingNewline(bool implicitTrailingNewline);
        bool GetLoading();
        void EnsureHighlight(Gtk.TextIter start, Gtk.TextIter end);
        string GetMarkup(Gtk.TextIter start, Gtk.TextIter end);
        string[] GetContextClassesAtIter(Gtk.TextIter iter);
        bool IterHasContextClass(Gtk.TextIter iter, string contextClass);
        void ChangeCase(ChangeCaseType caseType, Gtk.TextIter start,
                        Gtk.TextIter end);
        void SortLines(Gtk.TextIter start, Gtk.TextIter end, SortFlags flags,
                       int column);
        void JoinLines(Gtk.TextIter start, Gtk.TextIter end);
        Mark CreateSourceMark(string? name, string category,
                              Gtk.TextIter where);
        void RemoveSourceMarks(Gtk.TextIter start, Gtk.TextIter end,
                               string? category);

Languages and style schemes are looked up from process-wide managers:

    public static LanguageManager GetDefault();
    public static LanguageManager New();
        Language? GetLanguage(string id);        // e.g. "c-sharp", "xml"
        string[]? GetLanguageIds();
        Language? GuessLanguage(string? filename, string? contentType);
        string[] GetSearchPath();
        void SetSearchPath(string[]? dirs);
        void AppendSearchPath(string path);
        void PrependSearchPath(string path);

    public static StyleSchemeManager GetDefault();
    public static StyleSchemeManager New();
        StyleScheme? GetScheme(string schemeId);
        string[]? GetSchemeIds();
        string[] GetSearchPath();
        void SetSearchPath(string[]? path);
        void AppendSearchPath(string path);
        void PrependSearchPath(string path);
        void ForceRescan();

    Language:     GetId(), GetName(), GetSection(), GetHidden(),
                  GetGlobs(), GetMimeTypes(), GetMetadata(string),
                  GetStyleIds(), GetStyleName(string), GetStyleFallback(string)
    StyleScheme:  GetId(), GetName(), GetDescription(), GetAuthors(),
                  GetFilename(), GetMetadata(string), GetStyle(string styleId)

Both id sets are DISCOVERED AT RUN TIME from the installed GtkSourceView data
files. Do not hard-code a scheme id without a null check — enumerate with
`GetSchemeIds()` / `GetLanguageIds()` and fall back gracefully.

Worked example — a syntax-highlighting editor in a scroller:

    using CodeBrix.Develop.UI.Gtk;
    using GtkSrc = CodeBrix.Develop.UI.GtkSource;

    var buffer = GtkSrc.Buffer.New(null);
    buffer.SetLanguage(GtkSrc.LanguageManager.GetDefault()
                                             .GetLanguage("c-sharp"));

    var scheme = GtkSrc.StyleSchemeManager.GetDefault().GetScheme("classic");
    if (scheme is not null)
        buffer.SetStyleScheme(scheme);

    buffer.SetHighlightSyntax(true);
    buffer.SetHighlightMatchingBrackets(true);
    buffer.SetText("var answer = 42;\n", -1);

    var view = GtkSrc.View.NewWithBuffer(buffer);
    view.SetShowLineNumbers(true);
    view.SetHighlightCurrentLine(true);
    view.SetTabWidth(4);
    view.SetInsertSpacesInsteadOfTabs(true);
    view.SetAutoIndent(true);
    view.SetMonospace(true);          // inherited from Gtk.TextView

    var scroller = ScrolledWindow.New();
    scroller.SetChild(view);
    scroller.SetVexpand(true);

Other GtkSource types worth knowing:

    Map.New() / SetView(View)          the minimap widget
    Completion                         AddProvider / RemoveProvider,
                                       Show / Hide, Block/UnblockInteractive,
                                       GetPageSize / SetPageSize
    CompletionWords.New(string? title) a ready-made word completion provider
    CompletionProvider, CompletionProposal, CompletionContext, CompletionCell
                                       the interfaces to write your own
    SearchSettings.New()               SetSearchText, SetCaseSensitive,
                                       SetRegexEnabled, SetWrapAround,
                                       SetAtWordBoundaries, SetVisibleOnly
    SearchContext.New(Buffer, SearchSettings?)
                                       Forward / Backward (with out matchStart,
                                       out matchEnd, out hasWrappedAround),
                                       Replace, ReplaceAll,
                                       GetOccurrencesCount,
                                       GetOccurrencePosition, SetHighlight
    SnippetManager.GetDefault(), Snippet, SnippetChunk, SnippetContext
    Mark, MarkAttributes               gutter marks (breakpoints, errors)
    Gutter, GutterLines, GutterRenderer, GutterRendererText,
    GutterRendererPixbuf               custom gutter columns
    SpaceDrawer, SpaceTypeFlags, SpaceLocationFlags   whitespace rendering
    Style, StyleSchemeChooserButton, StyleSchemeChooserWidget,
    StyleSchemePreview                 scheme pickers
    PrintCompositor                    paginated printing of a Buffer
    Region, RegionIter, Encoding, Indenter, Hover, HoverContext, HoverDisplay,
    HoverProvider, VimIMContext, Annotation, AnnotationProvider, Annotations
    File, FileLoader, FileSaver        see COMMON PITFALLS - only the *Finish
                                       halves are bound


DIALOGS, FILE PICKERS AND LAUNCHERS
===================================
The GTK 4 async dialog objects are bound, and this package adds hand-written
`Task`-returning wrappers so they can be awaited directly. These `...Async`
methods are NOT part of the C API — they are an addition of this binding.

    public static FileDialog New();
        public Task<Gio.File?>      OpenAsync(Window? parent);
        public Task<Gio.ListModel?> OpenMultipleAsync(Window? parent);
        public Task<Gio.File?>      SaveAsync(Window? parent);
        public Task<Gio.File?>      SelectFolderAsync(Window? parent);
        public Task<Gio.ListModel?> SelectMultipleFoldersAsync(Window? parent);
        void SetTitle(string title);        string GetTitle();
        void SetAcceptLabel(string? acceptLabel);
        void SetModal(bool modal);
        void SetInitialFile(Gio.File? file);
        void SetInitialFolder(Gio.File? folder);
        void SetInitialName(string? name);
        void SetFilters(Gio.ListModel? filters);
        void SetDefaultFilter(FileFilter? filter);

    AlertDialog (created with NewWithProperties - see PITFALLS):
        public Task<int> ChooseAsync(Window? parent);   // index of the button
        void Show(Window? parent);                      // fire and forget
        void SetMessage(string message);   string GetMessage();
        void SetDetail(string detail);     string GetDetail();
        void SetButtons(string[] labels);  string[]? GetButtons();
        void SetDefaultButton(int button);
        void SetCancelButton(int button);
        void SetModal(bool modal);

    public static FontDialog New();
        public Task<Pango.FontDescription?> ChooseFontAsync(
            Window? parent, Pango.FontDescription? fontDescription);
        public Task<Pango.FontFamily?> ChooseFamilyAsync(
            Window? parent, Pango.FontFamily? fontFamily);
        public Task<Pango.FontFace?> ChooseFaceAsync(
            Window? parent, Pango.FontFace? fontFace);

    public static FileLauncher New(Gio.File? file);
        public Task<bool> LaunchAsync(Window? parent);
        public Task<bool> OpenContainingFolderAsync(Window? parent);

    public static UriLauncher New(string? uri);
        public Task<bool> LaunchAsync(Window? parent);

    public static FileFilter New();
        void SetName(string? name);
        void AddPattern(string pattern);      // "*.cs"
        void AddSuffix(string suffix);        // "cs"
        void AddMimeType(string mimeType);
        void AddMimeTypes(string[] mimeTypes);
        void AddPixbufFormats();

Opening a file, end to end:

    var filter = FileFilter.New();
    filter.SetName("C# source");
    filter.AddSuffix("cs");

    var filters = Gio.ListStore.New<FileFilter>();
    filters.Append(filter);

    var dialog = FileDialog.New();
    dialog.SetTitle("Open source file");
    dialog.SetFilters(filters);

    Gio.File? chosen = await dialog.OpenAsync(window);
    if (chosen is not null)
    {
        string? path = chosen.GetPath();       // null for non-local files
        string uri   = chosen.GetUri();
        GLib.Bytes bytes = chosen.LoadBytes(null, out _);
    }

`await` here only resumes on the UI thread if the application was started with
`RunWithSynchronizationContext`. See THREADING AND THE MAIN LOOP.

Also present but superseded by the objects above (they are the older widget
dialogs, still bound): `Dialog`, `MessageDialog`, `AboutDialog`,
`FileChooserDialog`, `FileChooserNative`, `FileChooserWidget`,
`ColorChooserDialog`, `ColorDialog`, `ColorDialogButton`, `FontChooserDialog`,
`FontDialogButton`, `AppChooserDialog`, `PrintDialog`, `PrintUnixDialog`,
`PageSetupUnixDialog`, `Assistant`, `ShortcutsWindow`, `EmojiChooser`.


LIST WIDGETS, MODELS AND SELECTION
==================================
GTK 4's list widgets are model/factory based. Three objects cooperate: a
`Gio.ListModel` of items, a `SelectionModel` wrapping it, and a
`ListItemFactory` that builds and binds the row widget.

    Gio.ListStore.New(GObject.Type itemType)
    Gio.ListStore.New<T>()                    // hand-written convenience
        where T : GObject.GTypeProvider
        void Append(GObject.Object item);
        void Insert(uint position, GObject.Object item);
        void Remove(uint position);   void RemoveAll();
        void Sort(GLib.CompareDataFunc compareFunc);
        uint InsertSorted(GObject.Object item, GLib.CompareDataFunc cmp);
        bool Find(GObject.Object item, out uint position);
        void Splice(uint position, uint nRemovals,
                    GObject.Object[] additions, uint nAdditions);

    Gtk.StringList.New(string[]? strings)      // a ready-made model of strings
        void Append(string @string);   void Take(string @string);
        void Remove(uint position);    uint Find(string @string);
        string? GetString(uint position);
        void Splice(uint position, uint nRemovals, string[]? additions);

    Gtk.SingleSelection.New(Gio.ListModel? model)
    Gtk.MultiSelection.New(...) / Gtk.NoSelection.New(...)

    Gtk.SignalListItemFactory.New()
        public event SignalHandler<SignalListItemFactory,
            SignalListItemFactory.SetupSignalArgs>    OnSetup;
        public event SignalHandler<SignalListItemFactory,
            SignalListItemFactory.BindSignalArgs>     OnBind;
        public event SignalHandler<SignalListItemFactory,
            SignalListItemFactory.UnbindSignalArgs>   OnUnbind;
        public event SignalHandler<SignalListItemFactory,
            SignalListItemFactory.TeardownSignalArgs> OnTeardown;
        // every one of those SignalArgs types has exactly one member:
        //     public GObject.Object Object { get; }
        // which is the Gtk.ListItem for this row.

    Gtk.ListItem
        Widget? GetChild();          void SetChild(Widget? child);
        GObject.Object? GetItem();   uint GetPosition();
        bool GetSelected();          void SetActivatable(bool activatable);
        void SetSelectable(bool selectable);
        // properties: Child, Item, Position, Selected, Activatable, Selectable

    Gtk.ListView.New(SelectionModel? model, ListItemFactory? factory)
    Gtk.GridView.New(SelectionModel? model, ListItemFactory? factory)
    Gtk.ColumnView.New(SelectionModel? model)
        + ColumnViewColumn.New(string? title, ListItemFactory? factory)

A complete string list:

    var model   = StringList.New(["alpha", "beta", "gamma"]);
    var factory = SignalListItemFactory.New();

    factory.OnSetup += (_, args) =>
    {
        var item = (ListItem) args.Object;
        item.SetChild(Label.New(null));
    };
    factory.OnBind += (_, args) =>
    {
        var item  = (ListItem) args.Object;
        var label = (Label) item.GetChild()!;
        var value = (StringObject) item.GetItem()!;
        label.SetText(value.GetString());
    };

    var listView = ListView.New(SingleSelection.New(model), factory);
    var scroller = ScrolledWindow.New();
    scroller.SetChild(listView);

The simpler, widget-per-row container is still there:

    ListBox.New()
        void Append(Widget child);
        ListBoxRow? GetRowAtIndex(int index);
        ListBoxRow? GetSelectedRow();
        void BindModel(Gio.ListModel? model,
                       ListBoxCreateWidgetFunc? createWidgetFunc);
        void SetActivateOnSingleClick(bool single);

Model decorators (all `Gio.ListModel` in, `Gio.ListModel` out) let you sort and
filter without touching the underlying store: `FilterListModel`,
`SortListModel`, `SliceListModel`, `MapListModel`, `FlattenListModel`,
`SelectionFilterModel`, `TreeListModel`, `DirectoryList`, `BookmarkList`.
Sorters and filters: `CustomSorter` (including the hand-written
`CustomSorter.New<T>(GObject.CompareDataFuncT<T>)`), `NumericSorter`,
`StringSorter`, `MultiSorter`, `TreeListRowSorter`, `CustomFilter`,
`BoolFilter`, `StringFilter`, `AnyFilter`, `EveryFilter`.

The old `TreeView` / `TreeStore` / `TreeModel` / `CellRenderer*` family is also
bound (`TreeView.New()`, `TreeView.NewWithModel(TreeModel)`) but GTK 4 treats
it as legacy; prefer `ListView` / `ColumnView` for new code.


ACTIONS AND MENUS
=================
Menus are described by a `Gio.MenuModel` and driven by actions, not by
callbacks attached to menu items.

    Gio.SimpleAction.New(string name, GLib.VariantType? parameterType)
    Gio.SimpleAction.NewStateful(string name, GLib.VariantType? parameterType,
                                 GLib.Variant state)
        public event SignalHandler<SimpleAction,
                                   SimpleAction.ActivateSignalArgs> OnActivate;
        // args.Parameter is a GLib.Variant?

    Gio.ActionMap (implemented by both Gtk.Application and ApplicationWindow)
        void AddAction(Gio.Action action);
        Gio.Action? LookupAction(string actionName);
        void RemoveAction(string actionName);

    Gio.Menu.New()
        void Append(string? label, string? detailedAction);   // "app.quit"
        void AppendItem(Gio.MenuItem item);
        void AppendSection(string? label, Gio.MenuModel section);
        void AppendSubmenu(string? label, Gio.MenuModel submenu);
        void Insert/InsertItem/InsertSection/InsertSubmenu(int position, ...);
        void Prepend/PrependItem/PrependSection/PrependSubmenu(...);
        void Remove(int position);  void RemoveAll();  void Freeze();

    Gio.MenuItem.New(string? label, string? detailedAction)
    Gio.MenuItem.NewSection(string? label, Gio.MenuModel section)
    Gio.MenuItem.NewSubmenu(string? label, Gio.MenuModel submenu)

Wiring it up:

    var quit = Gio.SimpleAction.New("quit", null);
    quit.OnActivate += (_, _) => app.Quit();
    app.AddAction(quit);
    app.SetAccelsForAction("app.quit", ["<Control>q"]);

    var menu = Gio.Menu.New();
    menu.Append("Quit", "app.quit");

    var menuButton = MenuButton.New();
    menuButton.SetMenuModel(menu);
    header.PackEnd(menuButton);

Action name prefixes: `app.` resolves against the Application, `win.` against
the ApplicationWindow, and any prefix you registered with
`widget.InsertActionGroup(name, group)`.


INPUT: EVENT CONTROLLERS AND GESTURES
=====================================
GTK 4 delivers input through controller objects that you attach to a widget
with `widget.AddController(controller)`.

    EventControllerKey.New()
        OnKeyPressed  -> ReturningSignalHandler<..., KeyPressedSignalArgs, bool>
                         args: Keyval (uint), Keycode (uint),
                               State (Gdk.ModifierType)
        OnKeyReleased -> SignalHandler<..., KeyReleasedSignalArgs>
        OnModifiers   -> ReturningSignalHandler<..., ModifiersSignalArgs, bool>
        OnImUpdate    -> SignalHandler<...>

    GestureClick.New()
        OnPressed  -> SignalHandler<..., PressedSignalArgs>
        OnReleased -> SignalHandler<..., ReleasedSignalArgs>
        OnStopped, OnUnpairedRelease

    Also: EventControllerMotion, EventControllerScroll, EventControllerFocus,
    EventControllerLegacy, GestureDrag, GestureSwipe, GesturePan,
    GestureLongPress, GestureRotate, GestureZoom, GestureStylus,
    GestureSingle, DragSource, DropTarget, DropTargetAsync,
    DropControllerMotion, ShortcutController, PadController.

Returning `true` from a returning signal means "handled, stop propagation".

Keyboard shortcuts can also be declared without a controller:
`Shortcut`, `ShortcutTrigger` (`KeyvalTrigger`, `MnemonicTrigger`,
`AlternativeTrigger`, `NeverTrigger`), `ShortcutAction` (`CallbackAction`,
`NamedAction`, `SignalAction`, `ActivateAction`, `MnemonicAction`,
`NothingAction`).


STYLING WITH CSS
================
    var css = CssProvider.New();
    css.LoadFromString("""
        window { background: #202020; }
        .danger { color: #ff6060; font-weight: bold; }
        """);

    StyleContext.AddProviderForDisplay(
        Gdk.Display.GetDefault()!,
        css,
        Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION);

    button.AddCssClass("danger");

    public static CssProvider New();
        void LoadFromString(string @string);
        void LoadFromData(string data, nint length);
        void LoadFromPath(string path);
        void LoadFromResource(string resourcePath);
        void LoadFromFile(Gio.File file);
        void LoadFromBytes(GLib.Bytes data);
        void LoadNamed(string name, string? variant);

    public static void StyleContext.AddProviderForDisplay(
        Gdk.Display display, StyleProvider provider, uint priority);
    public static void StyleContext.RemoveProviderForDisplay(
        Gdk.Display display, StyleProvider provider);

Priorities in `Gtk.Constants`: STYLE_PROVIDER_PRIORITY_FALLBACK,
_THEME, _SETTINGS, _APPLICATION, _USER (ascending).
`Gdk.Display.GetDefault()` returns null before the display is opened, so only
call this from `OnActivate`/`OnStartup` or later.


CUSTOM DRAWING: DRAWINGAREA, CAIRO, PANGO, GDK
==============================================
    public static DrawingArea New();
        void SetContentWidth(int width);
        void SetContentHeight(int height);
        void SetDrawFunc(DrawingAreaDrawFunc? drawFunc);

    public delegate void DrawingAreaDrawFunc(DrawingArea drawingArea,
                                             Cairo.Context cr,
                                             int width, int height);

    var area = DrawingArea.New();
    area.SetDrawFunc((_, cr, width, height) =>
    {
        cr.SetSourceRgb(0.1, 0.4, 0.9);
        cr.Rectangle(0, 0, width, height);
        cr.Fill();
    });

The `cr` handed to a draw function is BORROWED for the duration of the call.
The same holds for the transfer-none record arguments of a callback delegate
whose managed class implements IDisposable - typed records (Gtk.TreeIter in
TreeModelForeachFunc, GObject.Value in binding transform functions), opaque
typed records (Gtk.TreePath), and foreign typed records (Cairo.Context, the
`cr` above): the binding disposes those wrappers the moment your delegate
returns, so the native object is not pinned until garbage collection. Use the
argument inside the callback only. If you need the data afterwards, copy it
before returning; `Copy()` exists only where the C library declares one
(Gtk.TreeIter, Gtk.TreePath and GObject.Value have it, Cairo.Context and
GLib.Variant do not), so otherwise read the fields you need while the callback
is still running. A wrapper kept past the call throws ObjectDisposedException
on its next use. Disposing the argument yourself inside the callback is
harmless.

Record arguments whose managed class is NOT IDisposable are left alone by the
callback. An opaque untyped record implements IDisposable only when the C
library declares a free function for it: GLib.Variant (in settings mappings)
does, so it is disposed like the records above, while GLib.OptionContext,
GLib.SequenceIter, GLib.TreeNode, Gio.IOSchedulerJob and
Gtk.BuildableParseContext do not and are not disposed. Plain untyped records
(GLib.Hook, GLib.HookList, GLib.Node, ...) are never disposed either. Those
wrappers survive the call unharmed - so do not assume such an argument is dead
once your delegate has returned, and equally do not assume a callback that
fires at frame rate can never accumulate them.

`CodeBrix.Develop.UI.Cairo`'s public surface is hand-written on top of the
generated handles, and it uses ORDINARY C# CONSTRUCTORS and IDisposable rather
than `New(...)` factories:

    new Cairo.Context(Cairo.Surface target)
    new Cairo.ImageSurface(Cairo.Format format, int width, int height)
    new Cairo.ImageSurface(GLib.Bytes data, Format format, int width,
                           int height, int stride)
    new Cairo.ImageSurface(string pngFilename)
        Context: Save/Restore, SetSourceRgb/SetSourceRgba/SetSource/
                 SetSourceSurface, MoveTo/LineTo/CurveTo/Arc/ArcNegative/
                 Rectangle/ClosePath/NewPath, Fill/Stroke/Paint/Clip,
                 LineWidth, LineCap, LineJoin, Operator, Antialias, FillRule,
                 SetDash, TextPath, CopyPath, PathExtents, Status
        ImageSurface: Format, Width, Height, Stride, GetData()

Text and images:

    Pango.FontDescription.New()
    Pango.FontDescription.FromString(string str)      // "Monospace Bold 11"
    Pango.Layout.New(Pango.Context context)
    Gdk.Texture.NewFromFilename(string path)
    Gdk.Texture.NewFromResource(string resourcePath)
    Gdk.Texture.NewFromBytes(GLib.Bytes bytes)
    Gdk.Texture.NewForPixbuf(GdkPixbuf.Pixbuf pixbuf)
    GdkPixbuf.Pixbuf.NewFromFile(string filename)              // nullable
    GdkPixbuf.Pixbuf.NewFromFileAtScale(string filename, int width,
                                        int height, bool preserveAspectRatio)
    GdkPixbuf.Pixbuf.NewFromResource(string resourcePath)
    GdkPixbuf.Pixbuf.NewFromStream(Gio.InputStream stream,
                                   Gio.Cancellable? cancellable)
    Gdk.RGBA                  Red/Green/Blue/Alpha (float), Parse(string spec)
    Gdk.Display.GetDefault()  Gdk.Display.Open(string? displayName)
    Graphene.Rect.Alloc() / Graphene.Rect.Zero()

`Gsk` is the render-node scene graph GTK draws through: `Gtk.Snapshot`,
`Gsk.RenderNode` and the concrete node types (`ColorNode`, `TextureNode`,
`ContainerNode`, `ClipNode`, `RoundedClipNode`, `TransformNode`, `TextNode`,
`LinearGradientNode`, `BlurNode`, `ShadowNode`, ...) plus the renderers
(`Renderer`, `CairoRenderer`, `GLRenderer`, `VulkanRenderer`) are all bound.
`HarfBuzz` exposes the shaping primitives Pango uses. `Freetype2` contains only
the handful of type stubs (`Bitmap`, `Face`, `Library`) that other GIR files
reference; it has no native library of its own and no `Module`.


RESOURCES, MODULES AND INITIALIZATION
=====================================
Every namespace has a static `Module` class that registers its native-library
resolver and its GObject types. Initialization CASCADES: initializing a module
initializes everything it depends on.

    public static class Module
    {
        public static void Initialize();
        [Experimental("GirCore1009")]
        public static void SetCustomDllImportResolver(DllImportResolver r);
    }

The dependency cascade, exactly as implemented:

    GtkSource.Module.Initialize()
      -> Gtk.Module.Initialize()
           -> Gdk.Module.Initialize()
                -> Cairo, GdkPixbuf, Gio, Pango, PangoCairo
                   (Pango -> GObject, Gio, Cairo, HarfBuzz;
                    PangoCairo -> GObject, Cairo, Pango;
                    Gio -> GObject; GdkPixbuf -> Gio;
                    Cairo/HarfBuzz/Graphene -> GObject; GObject -> GLib)
           -> Gsk.Module.Initialize()  -> Gdk, Graphene

So calling `GtkSource.Module.Initialize()` once initializes the entire tree.
In an application you usually do not call anything: `Gtk.Application` has a
static constructor that calls `Gtk.Module.Initialize()`, and `Gio.Application`
does the same for `Gio.Module`. You DO need an explicit call when the first
type you touch is not an Application — for example in tests, in a library, or
before using GtkSourceView types on their own.

    GtkSource.Module.Initialize();   // safe, idempotent, do it first

`SetCustomDllImportResolver` must be called BEFORE the matching
`Module.Initialize()`; afterwards it throws. It is marked
`[Experimental("GirCore1009")]`, so using it needs
`#pragma warning disable GirCore1009` or a `<NoWarn>` entry.

Loading UI or asset bytes
-------------------------
    public interface TemplateLoader
    { static abstract GLib.Bytes Load(string resourceName); }

    public class GResource : TemplateLoader          // resource://<name>
    public class AssemblyResource : TemplateLoader   // .NET embedded resource

Both are usable directly, not just as `[Template<...>]` type arguments:

    GLib.Bytes uiBytes = AssemblyResource.Load("MyApp.Views.MainWindow.ui");
    GLib.Bytes icon    = GResource.Load("/com/example/app/icon.png");

`AssemblyResource.Load` reads the manifest resource from the CALLING assembly
(it uses `Assembly.GetCallingAssembly()`), which means the .ui file must be an
`EmbeddedResource` of the same assembly as the code that calls it. Under the
covers it is `assembly.ReadResourceAsByteArray(name)`, an extension method on
`System.Reflection.Assembly` exposed as
`CodeBrix.Develop.UI.GObject.AssemblyExtension`; it throws a plain `Exception`
when the resource name does not exist.

`GResource.Load` builds a `resource://<resourceName>` URI and reads it through
Gio, so the compiled .gresource bundle must already be registered:

    var resource = Gio.Functions.ResourceLoad("/path/to/app.gresource");
    Gio.Functions.ResourcesRegister(resource);
    // ... later ...
    Gio.Functions.ResourcesUnregister(resource);

Other resource helpers: `Gio.Functions.ResourcesLookupData(path, flags)`,
`Gio.Functions.ResourcesOpenStream(path, flags)`,
`Gio.Functions.ResourcesEnumerateChildren(path, flags)`,
`Gio.Resource.NewFromData(GLib.Bytes data)`. Widgets that take a resource path
directly: `Image.NewFromResource`, `Picture.NewForResource`,
`Gdk.Texture.NewFromResource`, `GdkPixbuf.Pixbuf.NewFromResource`,
`CssProvider.LoadFromResource`, `Builder.NewFromResource`.


THREADING AND THE MAIN LOOP
===========================
GTK AND THE GOBJECT TYPE SYSTEM ARE SINGLE-THREADED. Every widget, every
buffer, every GObject property and every signal must be touched from the one
thread that runs the main loop. There is no marshalling layer that will do it
for you, and there is no exception if you get it wrong — you get native
warnings, corrupt state or a crash.

Two mechanisms are provided to get back onto that thread.

1. The synchronization context (preferred).

       return app.RunWithSynchronizationContext(args);    // Gio.Application
       loop.RunWithSynchronizationContext();              // GLib.MainLoop

   While the loop runs, `SynchronizationContext.Current` is a GLib main-loop
   context, so any `await` inside an event handler resumes on the UI thread:

       button.OnClicked += async (_, _) =>
       {
           string text = await File.ReadAllTextAsync(path);  // pool thread
           buffer.SetText(text, -1);                         // UI thread again
       };

   Both methods restore the previous SynchronizationContext when the loop
   exits.

2. Explicit idle / timeout callbacks (from any thread).

       public static uint GLib.Functions.IdleAdd(
           int priority, GLib.SourceFunc function);
       public static uint GLib.Functions.TimeoutAdd(
           int priority, uint interval, GLib.SourceFunc function);
       public static uint GLib.Functions.TimeoutAddSeconds(
           int priority, uint interval, GLib.SourceFunc function);
       public delegate bool GLib.SourceFunc();

   The callback runs on the main-loop thread. Return
   `GLib.Constants.SOURCE_CONTINUE` (true) to be called again or
   `GLib.Constants.SOURCE_REMOVE` (false) to detach. Priorities live in
   `GLib.Constants`: PRIORITY_HIGH (-100), PRIORITY_DEFAULT (0),
   PRIORITY_HIGH_IDLE (100), PRIORITY_DEFAULT_IDLE (200), PRIORITY_LOW (300).

       // from a worker thread, push a result to the UI:
       GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_DEFAULT_IDLE, () =>
       {
           label.SetText(result);
           return GLib.Constants.SOURCE_REMOVE;
       });

       // a repeating tick:
       uint id = GLib.Functions.TimeoutAdd(GLib.Constants.PRIORITY_DEFAULT,
           1000, () => { Tick(); return GLib.Constants.SOURCE_CONTINUE; });

Main-loop objects, if you need to drive one yourself:

       GLib.MainLoop.New(GLib.MainContext? context, bool isRunning)
           void Run();  void RunWithSynchronizationContext();  void Quit();
           bool IsRunning();  GLib.MainContext GetContext();
       GLib.Functions.MainContextDefault()
       GLib.Functions.MainContextGetThreadDefault()
       GLib.Functions.MainDepth()

Keep a strong managed reference to any object whose native lifetime you rely
on (see PERFORMANCE TIPS and COMMON PITFALLS). Do not call GTK from a
`Task.Run` body.


COMPLETE EXAMPLES
=================

Example 1 — hello window with a button
--------------------------------------
    using System;
    using CodeBrix.Develop.UI.Gtk;
    using Gio = CodeBrix.Develop.UI.Gio;

    var app = Application.New("com.example.hello",
                              Gio.ApplicationFlags.FlagsNone);

    app.OnActivate += (_, _) =>
    {
        var window = ApplicationWindow.New(app);
        window.Title = "Hello";
        window.SetDefaultSize(420, 220);

        var label  = Label.New("Nothing yet.");
        var button = Button.NewWithLabel("Click me");
        button.OnClicked += (_, _) => label.SetText("Clicked!");

        var box = Box.New(Orientation.Vertical, 12);
        box.SetMarginTop(24);    box.SetMarginBottom(24);
        box.SetMarginStart(24);  box.SetMarginEnd(24);
        box.Append(label);
        box.Append(button);

        window.SetChild(box);
        window.Present();
    };

    return app.RunWithSynchronizationContext(args);

Example 2 — a window with a header bar and a source editor
----------------------------------------------------------
    using System;
    using System.IO;
    using CodeBrix.Develop.UI.Gtk;
    using Gio = CodeBrix.Develop.UI.Gio;
    using GtkSrc = CodeBrix.Develop.UI.GtkSource;

    internal static class Program
    {
        private static int Main(string[] args)
        {
            var app = Application.New("com.example.editor",
                                      Gio.ApplicationFlags.FlagsNone);
            app.OnActivate += (_, _) => Activate(app);
            return app.RunWithSynchronizationContext(args);
        }

        private static void Activate(Application app)
        {
            var window = ApplicationWindow.New(app);
            window.Title = "Editor";
            window.SetDefaultSize(900, 640);

            // --- document -------------------------------------------------
            var buffer = GtkSrc.Buffer.New(null);
            buffer.SetLanguage(GtkSrc.LanguageManager.GetDefault()
                                                     .GetLanguage("c-sharp"));
            var scheme = GtkSrc.StyleSchemeManager.GetDefault()
                                                  .GetScheme("classic");
            if (scheme is not null)
                buffer.SetStyleScheme(scheme);
            buffer.SetHighlightSyntax(true);
            buffer.SetHighlightMatchingBrackets(true);
            buffer.SetText("// open a .cs file to begin\n", -1);

            var view = GtkSrc.View.NewWithBuffer(buffer);
            view.SetShowLineNumbers(true);
            view.SetHighlightCurrentLine(true);
            view.SetTabWidth(4);
            view.SetInsertSpacesInsteadOfTabs(true);
            view.SetAutoIndent(true);
            view.SetMonospace(true);

            var scroller = ScrolledWindow.New();
            scroller.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroller.SetChild(view);
            scroller.SetVexpand(true);

            // --- status line ----------------------------------------------
            var status = Label.New("ready");
            status.SetXalign(0f);
            status.SetMarginStart(8);
            status.SetMarginEnd(8);
            status.SetMarginBottom(6);

            // --- header bar -----------------------------------------------
            var header = HeaderBar.New();
            header.SetShowTitleButtons(true);

            var openButton = Button.NewWithLabel("Open");
            openButton.OnClicked += async (_, _) =>
            {
                var filter = FileFilter.New();
                filter.SetName("C# source");
                filter.AddSuffix("cs");

                var filters = Gio.ListStore.New<FileFilter>();
                filters.Append(filter);

                var dialog = FileDialog.New();
                dialog.SetTitle("Open source file");
                dialog.SetFilters(filters);

                Gio.File? chosen = await dialog.OpenAsync(window);
                if (chosen?.GetPath() is not { } path)
                    return;

                // resumes on the UI thread thanks to
                // RunWithSynchronizationContext
                string text = await File.ReadAllTextAsync(path);
                buffer.SetText(text, -1);
                buffer.SetLanguage(GtkSrc.LanguageManager.GetDefault()
                                        .GuessLanguage(path, null));
                status.SetText(path);
            };
            header.PackStart(openButton);

            var countButton = Button.NewWithLabel("Count lines");
            countButton.OnClicked += (_, _) =>
                status.SetText($"{buffer.GetLineCount()} line(s)");
            header.PackEnd(countButton);

            window.SetTitlebar(header);

            // --- assemble --------------------------------------------------
            var box = Box.New(Orientation.Vertical, 0);
            box.Append(scroller);
            box.Append(status);
            window.SetChild(box);

            window.OnCloseRequest += (_, _) =>
            {
                if (!buffer.GetModified())
                    return false;      // false = allow the close
                status.SetText("unsaved changes");
                return true;           // true  = veto the close
            };

            window.Present();
        }
    }

Example 3 — a custom GObject widget with a composite template
-------------------------------------------------------------
    MainWindow.ui  (build action: EmbeddedResource)

        <?xml version="1.0" encoding="UTF-8"?>
        <interface>
          <template class="MyAppMainWindow" parent="GtkApplicationWindow">
            <child>
              <object class="GtkBox" id="root">
                <property name="orientation">vertical</property>
                <child>
                  <object class="GtkButton" id="open_button">
                    <property name="label">Open</property>
                  </object>
                </child>
                <child>
                  <object class="GtkLabel" id="status_label"/>
                </child>
              </object>
            </child>
          </template>
        </interface>

    MainWindow.cs

        using CodeBrix.Develop.UI.GObject;
        using CodeBrix.Develop.UI.Gtk;

        namespace MyApp.Views;

        [Subclass<ApplicationWindow>("MyAppMainWindow")]
        [Template<AssemblyResource>("MyApp.Views.MainWindow.ui")]
        public partial class MainWindow
        {
            [Connect("open_button")]  private readonly Button _open;
            [Connect("status_label")] private readonly Label  _status;

            partial void Initialize()
            {
                _open.OnClicked += (_, _) => _status.SetText("opening...");
            }
        }

        // creation:
        var window = MainWindow.NewWithProperties([]);
        window.SetApplication(app);
        window.Present();

    The `class="MyAppMainWindow"` attribute in the .ui file must match the
    qualified name passed to [Subclass<...>].


MINIMUM VIABLE PROJECT
======================
    MyApp/MyApp.csproj

        <Project Sdk="Microsoft.NET.Sdk">
          <PropertyGroup>
            <OutputType>Exe</OutputType>
            <TargetFramework>net10.0</TargetFramework>
            <Nullable>enable</Nullable>
            <ImplicitUsings>disable</ImplicitUsings>
          </PropertyGroup>
          <ItemGroup>
            <PackageReference Include="CodeBrix.Develop.UI" />
          </ItemGroup>
          <!-- only if you use [Template<AssemblyResource>] -->
          <ItemGroup>
            <EmbeddedResource Include="Views\MainWindow.ui" />
          </ItemGroup>
        </Project>

    MyApp/Program.cs

        using CodeBrix.Develop.UI.Gtk;
        using Gio = CodeBrix.Develop.UI.Gio;

        internal static class Program
        {
            private static int Main(string[] args)
            {
                var app = Application.New("com.example.myapp",
                                          Gio.ApplicationFlags.FlagsNone);
                app.OnActivate += (_, _) =>
                {
                    var window = ApplicationWindow.New(app);
                    window.Title = "MyApp";
                    window.SetDefaultSize(640, 480);
                    window.SetChild(Label.New("It works."));
                    window.Present();
                };
                return app.RunWithSynchronizationContext(args);
            }
        }

    Prerequisite on the machine that RUNS it (Debian-based Linux):
        sudo apt install libgtk-4-1
        sudo apt install libgtksourceview-5-0    # only if you use GtkSource

    Then:
        dotnet run


PERFORMANCE TIPS
================
* Batch widget construction, then parent once. Every `Append`/`SetChild`/
  `Attach` can queue a resize; building a `Box` fully and adding it to the
  window at the end is cheaper than adding children to a live tree.

* Prefer `SetVisible(false)` (or a `Stack`/`Revealer`) over destroying and
  re-creating a subtree. Widget construction crosses the managed/native
  boundary many times.

* For long lists use `ListView`/`ColumnView`/`GridView` with a
  `SignalListItemFactory`, never a `ListBox` with one widget per item. The
  view-based widgets recycle row widgets: `OnSetup` runs once per recycled
  row, `OnBind` runs per scroll. Put widget CREATION in `OnSetup` and only
  cheap assignment in `OnBind`.

* Property access through the generated C# property goes through GObject's
  generic get/set machinery (a `GObject.Value` boxed per call) unless the
  generator was able to emit a direct call. The `Get*`/`Set*` METHODS are the
  direct C calls in every case, so in a hot loop prefer
  `window.SetTitle(t)` over `window.Title = t`.

* Cache what is expensive to look up: `LanguageManager.GetDefault()`,
  `StyleSchemeManager.GetDefault()`, `Gdk.Display.GetDefault()`, a
  `CssProvider` you keep re-loading, `Gdk.Texture` instances. Textures and
  pixbufs are immutable and safely shareable across widgets.

* Load a `CssProvider` once and add it to the display once. Re-adding
  providers at the same priority stacks them.

* Do file and network I/O with `await` inside an application started by
  `RunWithSynchronizationContext`, rather than blocking the main loop. A
  blocked main loop is a frozen UI: GTK has no separate render thread you can
  fall back on.

* `GLib.Functions.TimeoutAddSeconds` is much cheaper than `TimeoutAdd` for
  intervals of a second or more, because it lets GLib align the wakeups.

* Prefer `Gdk.Texture` over `GdkPixbuf.Pixbuf` for anything you only draw:
  textures can live in GPU memory, pixbufs are always CPU-side pixels.

* Do not fight the GC for widget lifetime. Native GTK owns the widget tree; the
  managed wrapper holds a toggle reference. Keep a managed field for objects
  you must reach later (controllers, providers, timers), and let the rest go.


COMMON PITFALLS TO AVOID
========================
1. NATIVE LIBRARY NOT FOUND. The most common first failure is a
   `DllNotFoundException` at the first GTK call. Nothing is bundled: each
   module resolves its native libraries by OS-specific name - one library for
   every module except Cairo, which needs two.

       module      Linux                     Windows / macOS
       ----------  ------------------------  -------------------------------
       GLib        libglib-2.0.so.0          libglib-2.0-0.dll
                                             libglib-2.0.0.dylib
       GObject     libgobject-2.0.so.0       libgobject-2.0-0.dll
                                             libgobject-2.0.0.dylib
       Gio         libgio-2.0.so.0           libgio-2.0-0.dll
                                             libgio-2.0.0.dylib
       Gtk         libgtk-4.so.1             libgtk-4-1.dll
       Gdk         libgtk-4.so.1             libgtk-4.1.dylib
       Gsk         libgtk-4.so.1
       GtkSource   libgtksourceview-5.so.0   libgtksourceview-5-0.dll
                                             libgtksourceview-5.0.dylib
       Pango       libpango-1.0.so.0         libpango-1.0-0.dll
                                             libpango-1.0.0.dylib
       PangoCairo  libpangocairo-1.0.so.0    libpangocairo-1.0-0.dll
                                             libpangocairo-1.0.0.dylib
       Cairo       libcairo-gobject.so.2     libcairo-gobject-2.dll
                                             libcairo-gobject.2.dylib
                   libcairo.so.2             libcairo-2.dll
                                             libcairo.2.dylib
       GdkPixbuf   libgdk_pixbuf-2.0.so.0    libgdk_pixbuf-2.0-0.dll
                                             libgdk_pixbuf-2.0.0.dylib
       Graphene    libgraphene-1.0.so.0      libgraphene-1.0-0.dll
                                             libgraphene-1.0.0.dylib
       HarfBuzz    libharfbuzz-gobject.so.0  libharfbuzz-gobject-0.dll
                                             libharfbuzz-gobject.0.dylib

   Gtk, Gdk and Gsk all resolve to the SAME native GTK library.

   Note the Cairo and HarfBuzz entries. Cairo needs BOTH libraries: plain
   libcairo provides the drawing API and libcairo-gobject provides its GObject
   integration, and the module installs a resolver for each. HarfBuzz, by
   contrast, resolves only the `-gobject` variant, not plain libharfbuzz.
   Freetype2 resolves NO library at all (it is type stubs only). On
   Debian-based Linux `sudo apt install libgtk-4-1` provides everything except
   GtkSourceView, which needs `libgtksourceview-5-0`.

2. NO NATIVE LIBRARIES ARE SHIPPED, AND THERE IS NO SELF-CONTAINED MODE.
   `dotnet publish --self-contained` still requires GTK 4 on the target
   machine. Plan your deployment (system package, MSYS2 bundle, Homebrew,
   flatpak) accordingly.

3. GTK IS SINGLE-THREADED. Never touch a widget, buffer or GObject property
   from a thread pool thread. Marshal with
   `RunWithSynchronizationContext` + `await`, or with
   `GLib.Functions.IdleAdd`. There is no `Invoke`/`Dispatcher` equivalent and
   no exception to warn you.

4. `AlertDialog` HAS NO `New()`. Its C constructor is variadic, so it was not
   bound. Create it with `AlertDialog.NewWithProperties([])` and then call
   `SetMessage`/`SetDetail`/`SetButtons`. Every other dialog type has a normal
   `New()`.

5. GTK 4 REMOVED `Add()` AND `ShowAll()`. Use the container's own method
   (`SetChild`, `Append`, `Attach`, `PackStart`, `AppendPage`,
   `SetStartChild`); widgets are visible by default and `Show()`/`Hide()`
   on a widget are legacy — use `SetVisible(bool)`.

6. `Widget.SetParent(Widget)` IS NOT HOW YOU ADD A CHILD to a normal
   container. It exists for implementing a CUSTOM container, and calling it on
   a widget that already has a parent produces native criticals. Use the
   container's API; call `Unparent()` in your custom container's dispose path.

7. `-=` WITH A NEW LAMBDA DOES NOT DISCONNECT. Signal closures are keyed on the
   delegate instance. Store the delegate in a field if you need to detach:

       SignalHandler<Button> handler = (_, _) => DoWork();
       button.OnClicked += handler;
       ...
       button.OnClicked -= handler;       // works
       button.OnClicked -= (_, _) => DoWork();   // silently does nothing

8. THE `sender` OF AN APPLICATION SIGNAL IS `Gio.Application`, not
   `Gtk.Application`, because the signal is declared on the base class. Capture
   your `Gtk.Application` local instead of casting `sender`.

9. `Module.Initialize()` MUST RUN BEFORE THE FIRST NATIVE CALL. Touching
   `Gtk.Application` or `Gio.Application` does it implicitly via a static
   constructor, but a program whose first contact is (say) `GtkSource.Buffer`,
   `GLib.Bytes` or `Gdk.Display` must call `GtkSource.Module.Initialize()` (or
   the appropriate module) itself. `SetCustomDllImportResolver` throws if
   called after initialization.

10. THE Gtk AND GtkSource BINDINGS SHARE ONE ASSEMBLY, and .NET permits only
    one `DllImportResolver` per assembly. `Gtk.Module.Initialize()` therefore
    registers a single CHAINED resolver that serves both libraries;
    `GtkSource.Module.Initialize()` registers none of its own. Consequences:
    a custom resolver set on `Gtk.Module` is consulted first for BOTH
    libraries, and `GtkSource.Module.Initialize()` always initializes Gtk too.

11. `GtkSource.FileLoader`/`FileSaver` EXPOSE ONLY `LoadFinish`/`SaveFinish`.
    The async STARTER functions were not generated, so you cannot drive them
    from C# as an async pair. Read and write documents with ordinary .NET I/O
    (or Gio streams) and `buffer.SetText(text, -1)` /
    `buffer.GetText(start, end, true)`.

12. LANGUAGE AND STYLE-SCHEME IDS ARE RUNTIME DATA. `GetLanguage("c-sharp")`
    and `GetScheme("...")` return null when the installed GtkSourceView data
    does not have them. Always null-check; enumerate `GetLanguageIds()` /
    `GetSchemeIds()` rather than hard-coding.

13. `Gdk.Display.GetDefault()` RETURNS NULL before the display is opened. Any
    display-scoped call (notably `StyleContext.AddProviderForDisplay`) belongs
    in `OnStartup`/`OnActivate` or later, never in `Main` before `Run`.

14. THE LEGACY OBJECT CONSTRUCTORS ARE OBSOLETE. `new Button(...)`,
    `new Window(...)` and friends compile but raise `GirCore1007`. Use
    `Button.New()` / `Window.New()` / `T.NewWithProperties([...])`.

15. A `[Subclass]` TYPE CANNOT BE GENERIC, CANNOT BE NESTED IN A GENERIC TYPE,
    CANNOT BE PRIVATE, AND CANNOT USE CONSTRUCTORS. Initialize in
    `partial void Initialize()`, and expose extra creation options through a
    static factory method that calls `NewWithProperties`. The analyzers
    (GirCore1002/1004/1005/1006/1008) will tell you at compile time.

16. A `[Template]` CLASS NEEDS `[Subclass]` TOO, and its parent must derive
    from `Gtk.Widget` (GirCore2001, GirCore2002). `[Connect]` on a field
    outside a `[Template]` class is silently inert (GirCore2003).

17. `AssemblyResource.Load` USES `Assembly.GetCallingAssembly()`. The .ui file
    must be an `EmbeddedResource` in the SAME assembly as the class that
    declares `[Template<AssemblyResource>(...)]`, and the resource name is the
    full manifest name (default: `<RootNamespace>.<Folder>.<File>.ui`).

18. HANDLES AND OWNERSHIP. `GObject.Object` implements `IDisposable` and wraps
    a native handle; the wrapper holds a toggle reference so the native object
    stays alive while managed code can reach it. Do not dispose widgets you
    have handed to a container — the container owns them. Do dispose the
    short-lived value types you create explicitly (`GObject.Value`,
    `GObject.ConstructArgument`). `Handle.DangerousGetHandle()` is exactly as
    dangerous as it sounds and is not needed by ordinary consumer code. Record
    arguments handed to your callbacks follow a third rule of their own - see
    item 23.

19. NAMESPACE COLLISIONS ARE REAL. Type names repeated across namespaces
    include `Application` (Gtk / Gio), `ListStore` (Gtk / Gio), `Settings`
    (Gtk / Gio), `MountOperation` (Gtk / Gio), `Snapshot` (Gtk / Gdk), `File`
    (Gio / GtkSource), and `Module`, `Functions`, `Constants` and
    `VersionAttribute`, which exist in EVERY namespace. Importing two of these
    namespaces flat produces CS0104 ambiguity. Use
    `using X = CodeBrix.Develop.UI.X;` namespace aliases instead.

20. NULLABLE REFERENCE TYPES ARE ON in this binding, and the annotations are
    accurate: `Label.New(string? str)` really does accept null,
    `LanguageManager.GetLanguage` really can return null. Do not suppress the
    warnings; they encode the C library's contract.

21. A `[Flags]` VALUE OF 0 HAS TWO NAMES.
    `Gio.ApplicationFlags.FlagsNone` and `Gio.ApplicationFlags.DefaultFlags`
    are both 0; they are interchangeable.

22. `TextBuffer.SetText(text, len)` TAKES A LENGTH. Pass `-1` for
    "NUL-terminated / use the whole string". The same `-1` convention applies
    to `Insert`, `InsertAtCursor`, `InsertMarkup` and
    `SearchContext.Replace`/`ReplaceAll`.

23. CALLBACK RECORD ARGUMENTS ARE BORROWED. A transfer-none record handed to
    your delegate belongs to the native caller, not to you. Where the record's
    managed class is IDisposable - typed records (Gtk.TreeIter,
    GObject.Value), opaque typed records (Gtk.TreePath), foreign typed records
    (Cairo.Context) and opaque untyped records that have a free function
    (GLib.Variant) - the binding disposes the wrapper as soon as your delegate
    returns, and using it afterwards throws ObjectDisposedException. Copy what
    you need before returning. Records whose managed class is not IDisposable
    (opaque untyped records without a free function such as
    Gtk.BuildableParseContext or GLib.SequenceIter, and plain untyped records
    such as GLib.Hook) are not disposed for you and stay valid. Full rules,
    with the copy caveat, under CUSTOM DRAWING above.


WHAT THIS PACKAGE DOES NOT DO
=============================
* It does NOT ship, install or bundle the native GTK 4 / GtkSourceView
  libraries, and it never will. They are a machine prerequisite.

* It is NOT a cross-platform abstraction over other toolkits. It is GTK 4 and
  nothing else — no Win32, no WinUI, no Cocoa, no web target. Where you run it
  on Windows or macOS you are running GTK there.

* It does NOT bind libadwaita (`Adw`), GStreamer (`Gst*`), `Rsvg`, `Secret`,
  `GModule` or the platform-specific `GdkWin32`/`GdkX11`/`GdkWayland`
  surfaces. Only the fourteen namespaces listed under KEY NAMESPACES exist.

* It does NOT provide XAML, MVVM, data binding to POCOs, or a designer. UI is
  built in C# or in GTK Builder .ui XML through `[Template]`/`Builder`.

* It does NOT make GTK thread-safe, and it adds no dispatcher beyond the
  main-loop SynchronizationContext and the GLib idle/timeout helpers.

* It does NOT generate async wrappers for the whole GIO async surface. Only
  the hand-written `...Async` methods listed under DIALOGS exist
  (`FileDialog`, `AlertDialog`, `FontDialog`, `FileLauncher`, `UriLauncher`);
  everything else exposes the raw `*Finish` half and expects a
  `Gio.AsyncReadyCallback`.

* It does NOT support GObject subclasses that are generic, private, or nested
  inside generic types, and GObject properties declared in C# are not settable
  through `NewWithProperties` (only properties defined in the C library are).

* `Freetype2` does NOT bind FreeType. It contains the few type stubs (`Bitmap`,
  `Face`, `Library`) other GIR files refer to, has no `Module` and resolves no
  native library.

* It does NOT expose the upstream package names or namespaces. Anything you
  find in gir.core documentation must be namespace-translated before use.


WORKING EXAMPLES ON GITHUB
==========================
The test suite is the executable specification for this binding. Every file
below constructs real GTK objects against the real native libraries.

  Package shape, namespaces and assembly layout
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/PackageSmoke.cs

  Module initialization and the single-threaded rule
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/ModuleInitialization.cs

  Widget construction (Label.New, null arguments)
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/Gtk/ConstructorTest.cs

  Methods that can return null across the interop boundary
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/Gtk/MethodTest.cs

  Generated properties (bool / int / uint / enum round-trips)
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/Gtk/PropertyTests.cs

  Signals: OnNotify, per-instance handlers, property descriptors
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/Gtk/SignalTest.cs

  Interface members surfaced on a widget (Gtk.Editable on Entry)
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/Gtk/InterfaceTest.cs

  GtkSourceView: Buffer text, View.NewWithBuffer, LanguageManager, SetLanguage
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/GtkSource/GtkSourceTest.cs

  Gio actions and GLib.Variant state (SimpleAction.NewStateful, OnActivate)
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/Gio/SimpleActionTest.cs

  The main-loop SynchronizationContext (async resumption, exception handling)
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/GLib/SynchronizationContextTest.cs

  GLib records and collections (Variant, VariantType, SList, PtrArray, Dir,
  error/exception mapping, unowned handles)
    https://github.com/ellisnet/CodeBrix.Develop.UI/tree/main/tests/CodeBrix.Develop.UI.Tests/GLib

  GObject type system (Type, Value, ParamSpec, interfaces, SList extensions)
    https://github.com/ellisnet/CodeBrix.Develop.UI/tree/main/tests/CodeBrix.Develop.UI.Tests/GObject

  Cairo drawing (Context, ImageSurface, Pattern, Matrix, FontFace,
  FontOptions, ScaledFont)
    https://github.com/ellisnet/CodeBrix.Develop.UI/tree/main/tests/CodeBrix.Develop.UI.Tests/Cairo

  Callback record-argument lifetime (the borrowed cairo context of a
  DrawingArea draw function - the executable form of pitfall 23)
    https://github.com/ellisnet/CodeBrix.Develop.UI/blob/main/tests/CodeBrix.Develop.UI.Tests/Gtk/DrawingAreaDrawFuncCallHandlerTests.cs

  All tests:
    https://github.com/ellisnet/CodeBrix.Develop.UI/tree/main/tests

The largest real consumer of this package is the CodeBrix.Develop IDE, which
builds its whole user interface — editor, docking, dialogs, menus — on it:
    https://github.com/ellisnet/CodeBrix.Develop


QUICK REFERENCE CARD
====================
    PackageId ............. CodeBrix.Develop.UI          (no license suffix)
    License ............... MIT
    TFM ................... net10.0 (.NET 10 or later)
    NuGet dependencies .... none
    Runtime prerequisite .. native GTK 4 (+ GtkSourceView 5 if used)
                            Debian: sudo apt install libgtk-4-1
                                    sudo apt install libgtksourceview-5-0

    NAMESPACES
      CodeBrix.Develop.UI.Gtk / .GtkSource / .Gdk / .Gsk / .Gio / .GObject
      CodeBrix.Develop.UI.GLib / .Pango / .PangoCairo / .Cairo / .HarfBuzz
      CodeBrix.Develop.UI.Freetype2 / .GdkPixbuf / .Graphene

    NAMING RULES
      construct ........... T.New(...), T.NewWithXxx(...),
                            T.NewWithProperties(ConstructArgument[])
      call ................ obj.SetFoo(v) / obj.GetFoo()  (direct C calls)
      property ............ obj.Foo = v                   (GObject property)
      descriptor .......... T.FooPropertyDefinition       (Property<T,K>)
      signal .............. obj.OnFoo += (sender, args) => ...
      signal descriptor ... T.FooSignal                   (Signal<...>)
      free function ....... Ns.Functions.Xxx(...)
      C #define ........... Ns.Constants.XXX

    START AN APP
      var app = Application.New("com.example.app",
                                Gio.ApplicationFlags.FlagsNone);
      app.OnActivate += (_, _) => { /* build UI */ };
      return app.RunWithSynchronizationContext(args);

    BUILD A WINDOW
      var w = ApplicationWindow.New(app);
      w.Title = "..."; w.SetDefaultSize(800, 600);
      w.SetChild(child); w.SetTitlebar(HeaderBar.New()); w.Present();

    PARENTING (GTK 4 has no Add()/ShowAll())
      Window/ScrolledWindow/Frame/Overlay .. SetChild(w)
      Box .................................. Append(w) / Prepend(w)
      Grid ................................. Attach(w, col, row, cw, ch)
      Paned ................................ SetStartChild(w)/SetEndChild(w)
      HeaderBar ............................ PackStart(w) / PackEnd(w)
      Notebook ............................. AppendPage(w, tabLabel)
      Stack ................................ AddNamed(w, name)/AddTitled(...)
      visibility ........................... w.SetVisible(bool)

    SOURCE EDITOR
      var b = GtkSource.Buffer.New(null);
      b.SetLanguage(GtkSource.LanguageManager.GetDefault()
                                             .GetLanguage("c-sharp"));
      b.SetStyleScheme(GtkSource.StyleSchemeManager.GetDefault()
                                                   .GetScheme(id));
      b.SetHighlightSyntax(true);  b.SetText(text, -1);
      var v = GtkSource.View.NewWithBuffer(b);
      v.SetShowLineNumbers(true);  v.SetTabWidth(4);  v.SetMonospace(true);

    FILE DIALOG (await needs RunWithSynchronizationContext)
      Gio.File? f = await FileDialog.New().OpenAsync(window);

    CUSTOM GOBJECT TYPE
      [Subclass<Widget>] public partial class T { partial void Initialize(){} }
      var t = T.NewWithProperties([]);

    COMPOSITE TEMPLATE
      [Subclass<ApplicationWindow>] [Template<AssemblyResource>("A.B.X.ui")]
      public partial class X { [Connect("id")] private readonly Button _b; }

    CSS
      var css = CssProvider.New(); css.LoadFromString("...");
      StyleContext.AddProviderForDisplay(Gdk.Display.GetDefault()!, css,
          Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION);
      widget.AddCssClass("name");

    CALLBACK RECORD ARGUMENTS
      Borrowed for the call. The IDisposable ones (Cairo.Context,
      Gtk.TreeIter, Gtk.TreePath, GObject.Value, GLib.Variant) are disposed
      the moment your delegate returns - copy what you need first. See
      pitfall 23.

    THREADING
      GTK is single-threaded. Marshal with await (under
      RunWithSynchronizationContext) or:
      GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_DEFAULT_IDLE,
          () => { /* UI thread */ return GLib.Constants.SOURCE_REMOVE; });

    INITIALIZATION
      Implicit via Gtk.Application / Gio.Application static constructors.
      Otherwise: GtkSource.Module.Initialize();  // cascades through the tree

    DIAGNOSTICS FROM THE SHIPPED ANALYZERS
      GirCore1002/1006 subclass constructors -> Initialize()/factory (warning)
      GirCore1004/1005 subclass may not be generic or nested in one (error)
      GirCore1008      subclass may not be private (error)
      GirCore1007      legacy object constructors are obsolete (warning)
      GirCore1009      SetCustomDllImportResolver is experimental
      GirCore2001/2002 [Template] needs [Subclass] deriving from Widget (error)
      GirCore2003      [Connect] outside a [Template] class (warning)
================================================================================
