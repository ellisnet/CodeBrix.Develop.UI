using System;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Develop.UI.Gtk.Tests; //was previously: Gtk.Tests;

[Trait("Category", "SystemTest")]
public class PropertyTests : Test
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void TestBoolProperty(bool value)
    {
        var window = Window.New();
        window.Resizable = value;

        window.Resizable.Should().Be(value);
    }

    [Theory]
    [InlineData(500)]
    public void TestIntegerProperty(int value)
    {
        var window = Window.New();
        window.DefaultWidth = value;

        window.DefaultWidth.Should().Be(value);
    }

    [Theory]
    [InlineData(7u)]
    public void TestUnsignedIntegerProperty(uint value)
    {
        var spinButton = SpinButton.NewWithProperties([]);
        spinButton.Digits = value;

        spinButton.Digits.Should().Be(value);
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(0.1)]
    [InlineData(0.9)]
    public void TestDoubleProperty(double value)
    {
        var window = Window.New();
        window.Opacity = value;

        //TODO: It lookls like double values are very unprecise?
        Math.Round(window.Opacity, 2).Should().Be(value);
    }

    [Theory]
    [InlineData("abc", "def")]
    [InlineData("öö", "ß")]
    public void TestStringArray(string value1, string value2)
    {
        var aboutDialog = AboutDialog.New();
        aboutDialog.Artists = [value1, value2];

        aboutDialog.Artists[0].Should().Be(value1);
        aboutDialog.Artists[1].Should().Be(value2);
    }

    [Theory]
    [InlineData(License.MitX11)]
    [InlineData(License.Agpl30)]
    public void TestEnum(License windowPosition)
    {
        var aboutDialog = AboutDialog.New();
        aboutDialog.LicenseType = windowPosition;

        aboutDialog.LicenseType.Should().Be(windowPosition);
    }
}
