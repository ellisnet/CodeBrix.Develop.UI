using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.GObject.Tests.Classes; //was previously: GObject.Tests.Classes;

[Trait("Category", "UnitTest")]
public class ParamSpecTest : Test
{
    [Fact]
    public void CanCreateBooleanParamSpec()
    {
        var pspec = new ParamSpecBoolean(
            name: "test",
            nick: "test",
            blurb: "test",
            defaultValue: false,
            flags: ParamFlags.Writable
        );
        pspec.Handle.Should().NotBe(default);
    }
}
