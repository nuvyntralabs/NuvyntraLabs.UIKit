namespace NuvyntraLabs.UIKit;

static class NVMarkdownLogic
{
    public static IReadOnlyList<(NVTextRole Role, string Text)> Parse(string? markdown)
    {
        var blocks = new List<(NVTextRole, string)>();
        foreach (var raw in (markdown ?? "").Replace("\r", "").Split('\n'))
        {
            var line = raw.TrimEnd();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("# ", StringComparison.Ordinal))
            {
                blocks.Add((NVTextRole.Display, line[2..].Trim()));
            }
            else if (line.StartsWith("## ", StringComparison.Ordinal))
            {
                blocks.Add((NVTextRole.Title, line[3..].Trim()));
            }
            else if (line.StartsWith("- ", StringComparison.Ordinal) || line.StartsWith("* ", StringComparison.Ordinal))
            {
                blocks.Add((NVTextRole.Body, "• " + line[2..].Trim()));
            }
            else
            {
                blocks.Add((NVTextRole.Body, line.Trim()));
            }
        }

        return blocks;
    }
}
