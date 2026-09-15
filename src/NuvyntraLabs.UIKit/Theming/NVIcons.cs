namespace NuvyntraLabs.UIKit;

/// <summary>Stroke-style icon names. <see cref="NVIcon"/> draws the glyph.</summary>
public enum NVIconKind
{
    None,
    Check,
    Close,
    ChevronDown,
    ChevronRight,
    Search,
    Home,
    Menu,
    Settings,
    User,
    Cart,
    Heart,
    Star,
    Bell,
    Calendar,
    Chat,
    Image,
    File,
    Plus,
    Minus,
    Warning,
    Info,
    Mic,
    Play,
    Pause,
    Lock,
    Share,
    Mail,
    Phone,
    Globe,
    Link,
    Camera
}

/// <summary>Maps <see cref="NVIconKind"/> to a compact glyph. Lucide-like, not a vendor font.</summary>
public static class NVIcons
{
    public static string Glyph(NVIconKind kind) =>
        kind switch
        {
            NVIconKind.Check => "✓",
            NVIconKind.Close => "✕",
            NVIconKind.ChevronDown => "▾",
            NVIconKind.ChevronRight => "▸",
            NVIconKind.Search => "⌕",
            NVIconKind.Home => "⌂",
            NVIconKind.Menu => "☰",
            NVIconKind.Settings => "⚙",
            NVIconKind.User => "☺",
            NVIconKind.Cart => "🛒",
            NVIconKind.Heart => "♡",
            NVIconKind.Star => "★",
            NVIconKind.Bell => "⚑",
            NVIconKind.Calendar => "▦",
            NVIconKind.Chat => "💬",
            NVIconKind.Image => "▣",
            NVIconKind.File => "▤",
            NVIconKind.Plus => "+",
            NVIconKind.Minus => "−",
            NVIconKind.Warning => "!",
            NVIconKind.Info => "i",
            NVIconKind.Mic => "◉",
            NVIconKind.Play => "▶",
            NVIconKind.Pause => "❚❚",
            NVIconKind.Lock => "◌",
            NVIconKind.Share => "↗",
            NVIconKind.Mail => "✉",
            NVIconKind.Phone => "☎",
            NVIconKind.Globe => "◎",
            NVIconKind.Link => "⚭",
            NVIconKind.Camera => "📷",
            _ => ""
        };

    public static IReadOnlyList<NVIconKind> All { get; } = Enum.GetValues<NVIconKind>();
}
