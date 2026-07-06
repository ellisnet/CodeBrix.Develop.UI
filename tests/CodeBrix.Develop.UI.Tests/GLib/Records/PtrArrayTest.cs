using System.Runtime.InteropServices;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.GLib.Tests; //was previously: GLib.Tests;

[Trait("Category", "UnitTest")]
public class PtrArrayTest
{
    [Fact]
    public void CanCreateNewPtrArray()
    {
        var ptrArray = PtrArray.New();
        ptrArray.Len.Should().Be(0);
    }

    [Fact]
    public void CanManipulatePtrArray()
    {
        Internal.NonNullableUtf8StringOwnedHandle[] valueArr = [
            Internal.NonNullableUtf8StringOwnedHandle.Create("0 - Zero"),
            Internal.NonNullableUtf8StringOwnedHandle.Create("2 - Two"),
            Internal.NonNullableUtf8StringOwnedHandle.Create("1 - One")
        ];
        var ptrArray = PtrArray.New();
        foreach (var value in valueArr)
        {
            PtrArray.Add(ptrArray, value.DangerousGetHandle());
        }

        ptrArray.Len.Should().Be(3);

        PtrArray.Find(ptrArray, valueArr[1].DangerousGetHandle(), out var index).Should().BeTrue();
        index.Should().Be(1);

        // sort string values
        PtrArray.SortValues(ptrArray, (a, b) => Marshal.PtrToStringUTF8(a)!.CompareTo(Marshal.PtrToStringUTF8(b)!));

        PtrArray.Find(ptrArray, valueArr[1].DangerousGetHandle(), out var index2).Should().BeTrue();
        // item "2 - Two" should be moved to the end after sorting
        index2.Should().Be(2);
    }
}
