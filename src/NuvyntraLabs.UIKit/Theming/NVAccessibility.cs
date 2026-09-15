namespace NuvyntraLabs.UIKit;

/// <summary>Automation and contrast helpers.</summary>
public static class NVAccessibility
{
    public static void Name(BindableObject view, string name, string? hint = null)
    {
        AutomationProperties.SetIsInAccessibleTree(view, true);
        AutomationProperties.SetName(view, name);
        if (!string.IsNullOrWhiteSpace(hint))
        {
            AutomationProperties.SetHelpText(view, hint);
        }
    }

    public static bool BodyContrastOk(Color text, Color background)
    {
        return ContrastRatio(text, background) >= 4.5;
    }

    public static double ContrastRatio(Color a, Color b)
    {
        var l1 = RelativeLuminance(a);
        var l2 = RelativeLuminance(b);
        var lighter = Math.Max(l1, l2);
        var darker = Math.Min(l1, l2);
        return (lighter + 0.05) / (darker + 0.05);
    }

    static double RelativeLuminance(Color color)
    {
        static double Channel(double c)
        {
            c = c <= 0.03928 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
            return c;
        }

        return 0.2126 * Channel(color.Red) + 0.7152 * Channel(color.Green) + 0.0722 * Channel(color.Blue);
    }
}
