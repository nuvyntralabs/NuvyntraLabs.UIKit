namespace NuvyntraLabs.UIKit.Tests;

[Collection("UIKit")]
public class NVTokensTests
{
    [Fact]
    public void Space_scale_is_complete()
    {
        Assert.Equal(4, NVTokens.Space1);
        Assert.Equal(8, NVTokens.Space2);
        Assert.Equal(12, NVTokens.Space3);
        Assert.Equal(16, NVTokens.Space4);
        Assert.Equal(24, NVTokens.Space5);
        Assert.Equal(32, NVTokens.Space6);
    }

    [Theory]
    [InlineData(NVDensity.Compact, 12)]
    [InlineData(NVDensity.Comfortable, 16)]
    [InlineData(NVDensity.Spacious, 20)]
    public void Space_scales_by_density(NVDensity density, double expected)
    {
        Assert.Equal(expected, NVTokens.Space(NVTokens.Space4, density));
    }

    [Fact]
    public void Motion_zeroes_when_reduced()
    {
        Assert.Equal(0u, NVTokens.Motion(NVTokens.MotionNormal, reduceMotion: true));
        Assert.Equal(NVTokens.MotionNormal, NVTokens.Motion(NVTokens.MotionNormal, reduceMotion: false));
    }
}
