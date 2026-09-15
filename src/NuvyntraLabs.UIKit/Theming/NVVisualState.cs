namespace NuvyntraLabs.UIKit;

/// <summary>Shared interaction states. Styles read these instead of inventing new names.</summary>
public enum NVVisualStateKind
{
    Rest,
    Hover,
    Press,
    Focus,
    Disabled,
    Error
}

/// <summary>Named interaction states. Styles read these instead of inventing new names.</summary>
public static class NVVisualState
{
    public const string Rest = nameof(NVVisualStateKind.Rest);
    public const string Hover = nameof(NVVisualStateKind.Hover);
    public const string Press = nameof(NVVisualStateKind.Press);
    public const string Focus = nameof(NVVisualStateKind.Focus);
    public const string Disabled = nameof(NVVisualStateKind.Disabled);
    public const string Error = nameof(NVVisualStateKind.Error);
}
