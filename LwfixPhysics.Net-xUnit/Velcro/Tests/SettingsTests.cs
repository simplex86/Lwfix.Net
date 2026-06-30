using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro;
using SimplexLab.LwfixPhysics.Velcro.Collision.Filtering;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Tests;

/// <summary>
/// Additional coverage for the global <see cref="Velcro.Settings"/> class:
/// friction / restitution mixing laws and default fixture categories.
/// </summary>
public class SettingsTests
{
    [Fact]
    public void MixFriction_GeometricMean()
    {
        // MixFriction(f1, f2) = sqrt(f1 * f2)
        Fixed32 a = (Fixed32)0.4;
        Fixed32 b = (Fixed32)0.9;
        Fixed32 expected = Fixed32.Sqrt(a * b);
        Fixed32 actual = Settings.MixFriction(a, b);
        TestHelper.AssertApprox(expected, actual, (Fixed32)1e-2);
    }

    [Fact]
    public void MixFriction_Symmetric()
    {
        Fixed32 a = (Fixed32)0.4;
        Fixed32 b = (Fixed32)0.9;
        Assert.Equal(Settings.MixFriction(a, b), Settings.MixFriction(b, a));
    }

    [Fact]
    public void MixRestitution_PicksLargerValue()
    {
        Assert.Equal((Fixed32)0.5, Settings.MixRestitution((Fixed32)0.5, (Fixed32)0.2));
        Assert.Equal((Fixed32)0.9, Settings.MixRestitution((Fixed32)0.5, (Fixed32)0.9));
    }

    [Fact]
    public void MixRestitutionThreshold_PicksSmallerValue()
    {
        Assert.Equal((Fixed32)0.2, Settings.MixRestitutionThreshold((Fixed32)0.5, (Fixed32)0.2));
        Assert.Equal((Fixed32)0.1, Settings.MixRestitutionThreshold((Fixed32)0.1, (Fixed32)0.9));
    }

    [Fact]
    public void DefaultFilter_HasExpectedDefaults()
    {
        Filter filter = new Filter();
        Assert.Equal(Settings.DefaultCollisionGroup, filter.Group);
        Assert.Equal(Settings.DefaultFixtureCollisionCategories, filter.Category);
        Assert.Equal(Settings.DefaultFixtureCollidesWith, filter.CategoryMask);
    }

    [Fact]
    public void Filter_Constructor_SetsExplicitValues()
    {
        Filter f = new Filter(7, Category.Cat3, Category.Cat1 | Category.Cat2);
        Assert.Equal(7, f.Group);
        Assert.Equal(Category.Cat3, f.Category);
        Assert.Equal(Category.Cat1 | Category.Cat2, f.CategoryMask);
    }

    [Fact]
    public void PolygonRadius_IsLinearSlopDoubled()
    {
        Assert.Equal((Fixed32)2.0 * Settings.LinearSlop, Settings.PolygonRadius);
    }

    [Fact]
    public void Category_All_IsAllBitsSet()
    {
        Assert.Equal(Category.All, (Category)int.MaxValue);
    }

    [Fact]
    public void Category_None_IsZero()
    {
        Assert.Equal(Category.None, (Category)0);
    }
}
