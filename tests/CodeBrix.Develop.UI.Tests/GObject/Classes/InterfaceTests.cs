using System;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.GObject.Tests.Classes; //was previously: GObject.Tests.Classes;

[Trait("Category", "UnitTest")]
public class InterfaceTests : Test
{
    [Fact]
    public void InterfaceShouldImplementIDisposable()
    {
        typeof(TypePlugin).Should().Implement<IDisposable>();
    }
}
