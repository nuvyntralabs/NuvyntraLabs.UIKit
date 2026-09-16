namespace NuvyntraLabs.UIKit;

/// <summary>Unified / side-by-side text.</summary>
public class NVDiffView : ThemeAwareView
{
    public static readonly BindableProperty LeftProperty = BindableProperty.Create(nameof(Left), typeof(string), typeof(NVDiffView), "", propertyChanged: Refresh);
    public static readonly BindableProperty RightProperty = BindableProperty.Create(nameof(Right), typeof(string), typeof(NVDiffView), "", propertyChanged: Refresh);
    public static readonly BindableProperty ModeProperty = BindableProperty.Create(nameof(Mode), typeof(NVDiffMode), typeof(NVDiffView), NVDiffMode.Unified, propertyChanged: Refresh);

    readonly VerticalStackLayout _root = new() { Spacing = NVTokens.Space2 };

    public NVDiffView()
    {
        Content = _root;
        NVAccessibility.Name(this, "Diff view", "Compare text");
        ApplyTheme();
    }

    public string Left { get => (string)GetValue(LeftProperty); set => SetValue(LeftProperty, value); }
    public string Right { get => (string)GetValue(RightProperty); set => SetValue(RightProperty, value); }
    public NVDiffMode Mode { get => (NVDiffMode)GetValue(ModeProperty); set => SetValue(ModeProperty, value); }
    public IReadOnlyList<string> Lines { get; private set; } = [];

    public static IReadOnlyList<string> Unified(string? left, string? right)
    {
        var a = Split(left);
        var b = Split(right);
        var n = Math.Max(a.Length, b.Length);
        var lines = new List<string>(n);
        for (var i = 0; i < n; i++)
        {
            var l = i < a.Length ? a[i] : "";
            var r = i < b.Length ? b[i] : "";
            if (l == r)
            {
                lines.Add($"  {l}");
            }
            else
            {
                if (l.Length > 0)
                {
                    lines.Add($"- {l}");
                }

                if (r.Length > 0)
                {
                    lines.Add($"+ {r}");
                }
            }
        }

        return lines;
    }

    static string[] Split(string? text) =>
        (text ?? "").Replace("\r\n", "\n").Split('\n');

    static void Refresh(BindableObject b, object o, object n) => ((NVDiffView)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _root.Children.Clear();
        if (Mode == NVDiffMode.SideBySide)
        {
            _root.Children.Add(new HorizontalStackLayout
            {
                Spacing = NVTokens.Space3,
                Children =
                {
                    new NVCodeBlock { Code = Left },
                    new NVCodeBlock { Code = Right }
                }
            });
            Lines = Unified(Left, Right);
            return;
        }

        Lines = Unified(Left, Right);
        _root.Children.Add(new NVCodeBlock { Code = string.Join('\n', Lines) });
    }
}

/// <summary>Editable code. <see cref="NVCodeBlock"/> stays display-only.</summary>
public class NVCodeEditor : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVCodeEditor), "UseNuvyntraUIKit();", BindingMode.TwoWay, propertyChanged: Refresh);

    readonly NVEditor _editor = new() { Label = "Code" };

    public NVCodeEditor()
    {
        _editor.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVEditor.Text))
            {
                Text = _editor.Text;
            }
        };
        Content = _editor;
        NVAccessibility.Name(this, "Code editor", "Edit source");
        ApplyTheme();
    }

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

    static void Refresh(BindableObject b, object o, object n) => ((NVCodeEditor)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _editor.Text = Text;
        _editor.IsEnabled = IsEnabled;
    }
}
