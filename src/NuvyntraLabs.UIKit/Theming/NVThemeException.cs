namespace NuvyntraLabs.UIKit;

/// <summary>Thrown when a Lumina token or theme operation is invalid.</summary>
public sealed class NVThemeException : Exception
{
    public NVThemeException(string message)
        : base(message)
    {
    }
}
