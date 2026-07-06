using System;
using SilverAssertions;
using CodeBrix.Develop.UI.GLib;
using Xunit;

namespace CodeBrix.Develop.UI.GObject.Tests; //was previously: GObject.Tests;

[Trait("Category", "UnitTest")]
public class SListTest : Test
{
    [Fact]
    public void CanEnumerateSListObjects()
    {
        // Test enumerable for GLib.SList with GObject.Object stored as a pointer
        var valueArr = new[]
        {
            BindingGroup.New(),
            BindingGroup.New(),
            BindingGroup.New()
        };

        // Create zeroed SList handle to pass to "Append"
        // TODO: Change to "new SList(null)" once https://github.com/gircore/gir.core/issues/1318 is merged
        var index = 0;
        var slist = new SList(new GLib.Internal.SListOwnedHandle(IntPtr.Zero));
        foreach (var value in valueArr)
        {
            slist = SList.Append(slist, value.Handle.DangerousGetHandle());
            index++;
        }

        SList.Length(slist).Should().Be((uint) valueArr.Length);

        index = 0;
        foreach (var value in slist.AsObjects<BindingGroup>())
        {
            var origHandle = valueArr[index].Handle.DangerousGetHandle();
            value.Handle.DangerousGetHandle().Should().Be(origHandle);
            index++;
        }
    }

    [Fact]
    public void CanEnumerateSListBoxedRecords()
    {
        // Test enumerable for GLib.SList with GObject.Object stored as a pointer
        var valueArr = new[]
        {
            new Value(1234),
            new Value(46.19531),
            new Value("Hello World"),
        };

        // Create zeroed SList handle to pass to "Append"
        // TODO: Change to "new SList(null)" once https://github.com/gircore/gir.core/issues/1318 is merged
        var index = 0;
        var slist = new SList(new GLib.Internal.SListOwnedHandle(IntPtr.Zero));
        foreach (var value in valueArr)
        {
            slist = SList.Append(slist, value.Handle.DangerousGetHandle());
            index++;
        }

        SList.Length(slist).Should().Be((uint) valueArr.Length);

        index = 0;
        foreach (var value in slist.AsBoxed<Value>())
        {
            value.Extract().Should().Be(valueArr[index].Extract());
            index++;
        }
    }
}
