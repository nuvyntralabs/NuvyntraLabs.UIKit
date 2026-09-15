namespace NuvyntraLabs.UIKit;

/// <summary>0–4 score for <see cref="NVPasswordStrength"/>. No view constructors.</summary>
public static class NVPasswordRules
{
    public static int Score(string? password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return 0;
        }

        var score = 0;
        if (password.Length >= 8)
        {
            score++;
        }

        if (password.Any(char.IsDigit))
        {
            score++;
        }

        if (password.Any(char.IsUpper) && password.Any(char.IsLower))
        {
            score++;
        }

        if (password.Any(c => !char.IsLetterOrDigit(c)))
        {
            score++;
        }

        return score;
    }

    public static string Caption(int score) =>
        score switch
        {
            >= 4 => "Strong",
            3 => "Good",
            2 => "Fair",
            1 => "Weak",
            _ => "Empty"
        };
}
