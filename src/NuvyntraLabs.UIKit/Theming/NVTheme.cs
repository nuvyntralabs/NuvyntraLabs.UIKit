namespace NuvyntraLabs.UIKit;

/// <summary>Runtime Lumina theme. Controls read colors from <see cref="Current"/>.</summary>
public sealed class NVTheme
{
    static readonly Color LightPaper = Color.FromArgb("#F6F1E8");
    static readonly Color LightSurface = Color.FromArgb("#FFFCF7");
    static readonly Color LightInk = Color.FromArgb("#1C1916");
    static readonly Color LightMist = Color.FromArgb("#E8E0D4");
    static readonly Color LightFog = Color.FromArgb("#C9C0B3");
    static readonly Color LightMuted = Color.FromArgb("#6F675C");
    static readonly Color DefaultAccent = Color.FromArgb("#1FA87A");
    static readonly Color LightDanger = Color.FromArgb("#C23B2E");
    static readonly Color LightWarn = Color.FromArgb("#C4841D");
    static readonly Color LightOk = Color.FromArgb("#2F8A4E");

    static readonly Color DarkPaper = Color.FromArgb("#161411");
    static readonly Color DarkSurface = Color.FromArgb("#1F1B17");
    static readonly Color DarkInk = Color.FromArgb("#F3EDE3");
    static readonly Color DarkMist = Color.FromArgb("#2A261F");
    static readonly Color DarkFog = Color.FromArgb("#3D372E");
    static readonly Color DarkMuted = Color.FromArgb("#B4AA9C");
    static readonly Color DarkDanger = Color.FromArgb("#E86A5E");
    static readonly Color DarkWarn = Color.FromArgb("#E0A54A");
    static readonly Color DarkOk = Color.FromArgb("#5FBF7A");

    public static NVTheme Current { get; } = new();

    NVThemeMode _mode = NVThemeMode.System;
    Color _accent = DefaultAccent;
    NVDensity _density = NVDensity.Comfortable;

    NVTheme()
    {
    }

    public event EventHandler? Changed;

    public NVThemeMode Mode
    {
        get => _mode;
        set
        {
            if (_mode == value)
            {
                return;
            }

            _mode = value;
            RaiseChanged();
        }
    }

    public Color Accent
    {
        get => _accent;
        set
        {
            _accent = value ?? throw new ArgumentNullException(nameof(value));
            RaiseChanged();
        }
    }

    public NVDensity Density
    {
        get => _density;
        set
        {
            if (_density == value)
            {
                return;
            }

            _density = value;
            RaiseChanged();
        }
    }

    public bool IsDark => ResolveDark();

    public Color Paper => IsDark ? DarkPaper : LightPaper;
    public Color Surface => IsDark ? DarkSurface : LightSurface;
    public Color Ink => IsDark ? DarkInk : LightInk;
    public Color Mist => IsDark ? DarkMist : LightMist;
    public Color Fog => IsDark ? DarkFog : LightFog;
    public Color Muted => IsDark ? DarkMuted : LightMuted;
    public Color Danger => IsDark ? DarkDanger : LightDanger;
    public Color Warn => IsDark ? DarkWarn : LightWarn;
    public Color Ok => IsDark ? DarkOk : LightOk;
    public Color OnAccent => Colors.White;

    /// <summary>Reset to Lumina defaults (warm paper, aurora accent).</summary>
    public void UseLumina()
    {
        _mode = NVThemeMode.System;
        _accent = DefaultAccent;
        _density = NVDensity.Comfortable;
        RaiseChanged();
    }

    public void SetMode(NVThemeMode mode) => Mode = mode;

    public void SetAccent(Color accent) => Accent = accent;

    public Color Token(string key) =>
        key switch
        {
            "paper" => Paper,
            "surface" => Surface,
            "ink" => Ink,
            "mist" => Mist,
            "fog" => Fog,
            "muted" => Muted,
            "accent" => Accent,
            "danger" => Danger,
            "warn" => Warn,
            "ok" => Ok,
            "onAccent" => OnAccent,
            _ => throw new NVThemeException($"Unknown token '{key}'.")
        };

    bool ResolveDark()
    {
        if (_mode == NVThemeMode.Dark)
        {
            return true;
        }

        if (_mode == NVThemeMode.Light)
        {
            return false;
        }

        return Application.Current?.RequestedTheme == AppTheme.Dark;
    }

    void RaiseChanged() => Changed?.Invoke(this, EventArgs.Empty);
}
