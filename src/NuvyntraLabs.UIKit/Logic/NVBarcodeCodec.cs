namespace NuvyntraLabs.UIKit;

/// <summary>Encode/decode for generated barcodes (not camera scan).</summary>
public static class NVBarcodeCodec
{
    public static string Encode(string payload, NVBarcodeFormat format)
    {
        ArgumentNullException.ThrowIfNull(payload);
        var bytes = System.Text.Encoding.UTF8.GetBytes(payload);
        return $"{format}:{Convert.ToBase64String(bytes)}";
    }

    public static string Decode(string encoded)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encoded);
        var split = encoded.Split(':', 2);
        if (split.Length != 2)
        {
            throw new FormatException("Encoded barcode must be format:payload.");
        }

        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(split[1]));
    }

    public static NVBarcodeFormat FormatOf(string encoded)
    {
        var head = encoded.Split(':', 2)[0];
        return Enum.Parse<NVBarcodeFormat>(head);
    }

    /// <summary>Code128-style bar widths for drawing (generate-only).</summary>
    public static IReadOnlyList<int> Code128Bars(string payload)
    {
        var bars = new List<int> { 2, 1, 1, 2, 3, 2 };
        foreach (var ch in payload ?? "")
        {
            bars.Add(1 + (ch % 3));
            bars.Add(1);
        }

        bars.AddRange([2, 3, 3, 1, 1, 2, 1]);
        return bars;
    }

    /// <summary>Version-1 QR matrix with finder patterns + payload modules.</summary>
    public static bool[,] QrMatrix(string payload)
    {
        const int n = 21;
        var matrix = new bool[n, n];
        void Finder(int row, int col)
        {
            for (var r = 0; r < 7; r++)
            {
                for (var c = 0; c < 7; c++)
                {
                    var edge = r is 0 or 6 || c is 0 or 6;
                    var core = r is >= 2 and <= 4 && c is >= 2 and <= 4;
                    matrix[row + r, col + c] = edge || core;
                }
            }
        }

        Finder(0, 0);
        Finder(0, n - 7);
        Finder(n - 7, 0);
        var bytes = System.Text.Encoding.UTF8.GetBytes(payload ?? "");
        if (bytes.Length == 0)
        {
            bytes = [0];
        }

        var i = 0;
        for (var r = 0; r < n; r++)
        {
            for (var c = 0; c < n; c++)
            {
                if (InFinder(r, c, n))
                {
                    continue;
                }

                var b = bytes[i % bytes.Length];
                matrix[r, c] = ((b >> (i % 8)) & 1) == 1;
                i++;
            }
        }

        return matrix;
    }

    static bool InFinder(int row, int col, int n) =>
        row < 7 && col < 7 || row < 7 && col >= n - 7 || row >= n - 7 && col < 7;
}
