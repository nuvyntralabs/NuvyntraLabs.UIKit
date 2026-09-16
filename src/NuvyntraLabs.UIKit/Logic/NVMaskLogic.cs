namespace NuvyntraLabs.UIKit;

static class NVMaskLogic
{
    public static string Apply(string? mask, string? raw)
    {
        if (string.IsNullOrEmpty(mask))
        {
            return raw ?? "";
        }

        var chars = (raw ?? "").Where(char.IsLetterOrDigit).ToArray();
        if (chars.Length == 0)
        {
            return "";
        }

        var i = 0;
        var built = new System.Text.StringBuilder();
        foreach (var slot in mask)
        {
            if (IsSlot(slot))
            {
                if (i >= chars.Length)
                {
                    break;
                }

                built.Append(chars[i++]);
            }
            else if (i < chars.Length)
            {
                built.Append(slot);
            }
        }

        return built.ToString();
    }

    static bool IsSlot(char mask) => mask is '0' or '9' or '#' or 'A' or 'a';
}
