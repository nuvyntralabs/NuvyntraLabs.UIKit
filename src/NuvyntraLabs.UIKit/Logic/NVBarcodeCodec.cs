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
}
