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

    static readonly Color LightWarn = Color.FromArgb("#8A5A10");
    static readonly Color LightOk = Color.FromArgb("#1F6B3A");
    NVThemeMode _mode = NVThemeMode.System;
    Color _accent = DefaultAccent;
    NVDensity _density = NVDensity.Comfortable;
    double _typeScale = 1;
    FlowDirection _flowDirection = FlowDirection.MatchParent;

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

    /// <summary>Type size multiplier. Clamped to 0.8–2.0. Default 1.0 so existing hosts are unchanged.</summary>
    public double TypeScale
    {
        get => _typeScale;
        set
        {
            var next = Math.Clamp(value, 0.8, 2.0);
            if (Math.Abs(_typeScale - next) < 0.0001)
            {
                return;
            }

            _typeScale = next;
            RaiseChanged();
        }
    }

    /// <summary>Default <see cref="FlowDirection.MatchParent"/> so playground apps keep the page direction.</summary>
    public FlowDirection FlowDirection
    {
        get => _flowDirection;
        set
        {
            if (_flowDirection == value)
            {
                return;
            }

            _flowDirection = value;
            RaiseChanged();
        }
    }

    public bool IsDark => ResolveDark();

    public bool IsRtl => _flowDirection == FlowDirection.RightToLeft;

    public Color Paper => IsDark ? DarkPaper : LightPaper;
    public Color Surface => IsDark ? DarkSurface : LightSurface;
    public Color Ink => IsDark ? DarkInk : LightInk;
    public Color Mist => IsDark ? DarkMist : LightMist;
    public Color Fog => IsDark ? DarkFog : LightFog;
    public Color Muted => IsDark ? DarkMuted : LightMuted;
    public Color Danger => IsDark ? DarkDanger : LightDanger;
    public Color Warn => IsDark ? DarkWarn : LightWarn;
    public Color Ok => IsDark ? DarkOk : LightOk;

    /// <summary>Text on <see cref="Accent"/> that meets 4.5:1. White on aurora fails, so light uses ink and dark uses paper.</summary>
    public Color OnAccent => On(Accent);

    public Color On(Color fill)
    {
        if (NVAccessibility.BodyContrastOk(Colors.White, fill))
        {
            return Colors.White;
        }

        var ink = IsDark ? Paper : Ink;
        return NVAccessibility.BodyContrastOk(ink, fill) ? ink : Paper;
    }

    /// <summary>Reset to Lumina defaults (warm paper, aurora accent, 100% type, match-parent flow).</summary>
    public void UseLumina()
    {
        _mode = NVThemeMode.System;
        _accent = DefaultAccent;
        _density = NVDensity.Comfortable;
        _typeScale = 1;
        _flowDirection = FlowDirection.MatchParent;
        RaiseChanged();
    }

    public void SetMode(NVThemeMode mode) => Mode = mode;

    public void SetAccent(Color accent) => Accent = accent;

    public void SetTypeScale(double scale) => TypeScale = scale;

    public void SetFlowDirection(FlowDirection direction) => FlowDirection = direction;

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
