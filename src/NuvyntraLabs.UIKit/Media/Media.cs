namespace NuvyntraLabs.UIKit;

public class NVImageEditor : ThemeAwareView
{
    public static readonly BindableProperty CaptionProperty = BindableProperty.Create(nameof(Caption), typeof(string), typeof(NVImageEditor), "Image", propertyChanged: Refresh);
    public static readonly BindableProperty RotationDegreesProperty = BindableProperty.Create(nameof(RotationDegrees), typeof(int), typeof(NVImageEditor), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty CropRectProperty = BindableProperty.Create(nameof(CropRect), typeof(Rect), typeof(NVImageEditor), new Rect(0, 0, 1, 1), propertyChanged: Refresh);
    public static readonly BindableProperty AnnotationsProperty = BindableProperty.Create(nameof(Annotations), typeof(IList<string>), typeof(NVImageEditor), new List<string>(), propertyChanged: Refresh);
    readonly NVInteractiveViewer _viewer = new();
    public NVImageEditor() { Content = _viewer; ApplyTheme(); }
    public string Caption { get => (string)GetValue(CaptionProperty); set => SetValue(CaptionProperty, value); }
    public int RotationDegrees { get => (int)GetValue(RotationDegreesProperty); set => SetValue(RotationDegreesProperty, value); }
    public Rect CropRect { get => (Rect)GetValue(CropRectProperty); set => SetValue(CropRectProperty, value); }
    public IList<string> Annotations { get => (IList<string>)GetValue(AnnotationsProperty); set => SetValue(AnnotationsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVImageEditor)b).ApplyTheme();

    public void Rotate() => RotationDegrees = (RotationDegrees + 90) % 360;

    public void Crop(Rect rect) => CropRect = rect;

    public void Annotate(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            return;
        }

        var next = Annotations?.ToList() ?? [];
        next.Add(note);
        Annotations = next;
    }

    protected override void ApplyTheme()
    {
        var notes = (Annotations ?? []).Count == 0 ? "No notes" : string.Join(" · ", Annotations ?? []);
        var rotate = new NVButton { Text = "Rotate", Variant = NVButtonVariant.Tonal };
        var crop = new NVButton { Text = "Crop", Variant = NVButtonVariant.Outline };
        var mark = new NVButton { Text = "Annotate", Variant = NVButtonVariant.Ghost };
        rotate.Command = new Command(Rotate);
        crop.Command = new Command(() => Crop(new Rect(0.1, 0.1, 0.8, 0.8)));
        mark.Command = new Command(() => Annotate("circle"));
        var canvas = new Grid
        {
            HeightRequest = 180,
            BackgroundColor = NVTheme.Current.Mist
        };
        canvas.Add(new NVIcon { Kind = NVIconKind.Image, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center });
        canvas.Add(new BoxView
        {
            Color = Colors.Transparent,
            BackgroundColor = NVTheme.Current.Accent.WithAlpha(0.12f),
            Margin = new Thickness(180 * (1 - CropRect.Width) / 2)
        });
        canvas.Rotation = RotationDegrees;
        _viewer.Viewport = new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children =
            {
                new NVHeading { Text = Caption, Role = NVTextRole.Body },
                canvas,
                new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { rotate, crop, mark } },
                new NVCaptionText { Text = $"Rotate {RotationDegrees}° · crop {CropRect.Width:0.##}×{CropRect.Height:0.##} · {notes}" }
            }
        };
    }
}

public class NVPdfViewer : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVPdfViewer), "Document.pdf", propertyChanged: Refresh);
    public static readonly BindableProperty PagesProperty = BindableProperty.Create(nameof(Pages), typeof(IList<string>), typeof(NVPdfViewer), new List<string>(), propertyChanged: Refresh);
    public static readonly BindableProperty ZoomProperty = BindableProperty.Create(nameof(Zoom), typeof(double), typeof(NVPdfViewer), 1d, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty QueryProperty = BindableProperty.Create(nameof(Query), typeof(string), typeof(NVPdfViewer), "", propertyChanged: Refresh);
    public static readonly BindableProperty PageIndexProperty = BindableProperty.Create(nameof(PageIndex), typeof(int), typeof(NVPdfViewer), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly VerticalStackLayout _root = new() { Spacing = NVTokens.Space3 };
    readonly NVSearchBar _search = new() { Label = "Find", Placeholder = "Search pages" };
    readonly NVCard _page = new();
    public NVPdfViewer()
    {
        var zoomOut = new NVIconButton { Kind = NVIconKind.Minus, Variant = NVButtonVariant.Tonal };
        var zoomIn = new NVIconButton { Kind = NVIconKind.Plus, Variant = NVButtonVariant.Tonal };
        zoomOut.Command = new Command(ZoomOut);
        zoomIn.Command = new Command(ZoomIn);
        _search.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVTextField.Text))
            {
                Query = _search.Text;
            }
        };
        _root.Children.Add(new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { zoomOut, zoomIn, _search } });
        _root.Children.Add(_page);
        Content = _root;
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public IList<string> Pages { get => (IList<string>)GetValue(PagesProperty); set => SetValue(PagesProperty, value); }
    public double Zoom { get => (double)GetValue(ZoomProperty); set => SetValue(ZoomProperty, Math.Clamp(value, 0.5, 4)); }
    public string Query { get => (string)GetValue(QueryProperty); set => SetValue(QueryProperty, value); }
    public int PageIndex { get => (int)GetValue(PageIndexProperty); set => SetValue(PageIndexProperty, value); }
    public IReadOnlyList<int> SearchHits => Find(Pages, Query);
    public int MatchCount => SearchHits.Count;
    static void Refresh(BindableObject b, object o, object n) => ((NVPdfViewer)b).ApplyTheme();

    public static IReadOnlyList<int> Find(IEnumerable<string>? pages, string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        return (pages ?? [])
            .Select((text, index) => (text, index))
            .Where(item => (item.text ?? "").Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(item => item.index)
            .ToList();
    }

    public void ZoomIn() => Zoom = Math.Min(4, Zoom + 0.25);
    public void ZoomOut() => Zoom = Math.Max(0.5, Zoom - 0.25);

    protected override void ApplyTheme()
    {
        var pages = Pages ?? [];
        var index = pages.Count == 0 ? 0 : Math.Clamp(PageIndex, 0, pages.Count - 1);
        var body = pages.Count == 0
            ? "Drop a PDF or bind Pages to review a document."
            : pages[index];
        if (MatchCount > 0)
        {
            body += $"\n{MatchCount} match(es)";
        }

        _search.Text = Query;
        var count = pages.Count;
        _page.Title = $"{Title}  ·  page {index + 1} / {Math.Max(1, count)}  ·  {Zoom:0.##}×";
        _page.Body = body;
        _page.Scale = Math.Clamp(Zoom, 0.5, 1.5);
    }
}

public class NVDocxViewer : NVPdfViewer
{
    public NVDocxViewer() => Title = "Document.docx";
}

public class NVSpreadsheet : ThemeAwareView
{
    public static readonly BindableProperty CellsProperty = BindableProperty.Create(nameof(Cells), typeof(IList<NVSpreadsheetCell>), typeof(NVSpreadsheet), new List<NVSpreadsheetCell>(), propertyChanged: Refresh);
    readonly NVDataGrid _grid = new();
    public NVSpreadsheet() { Content = _grid; ApplyTheme(); }
    public IList<NVSpreadsheetCell> Cells { get => (IList<NVSpreadsheetCell>)GetValue(CellsProperty); set => SetValue(CellsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVSpreadsheet)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        var cols = (Cells ?? []).Select(c => c.Column).Distinct().OrderBy(c => c).ToList();
        var rows = (Cells ?? []).Select(c => c.Row).Distinct().OrderBy(r => r).ToList();
        _grid.Columns = cols.Select(c => new NVGridColumn { Key = c.ToString(), Header = ((char)('A' + c)).ToString() }).ToList();
        _grid.Rows = rows.Select(r =>
        {
            var dict = new Dictionary<string, object?>();
            foreach (var col in cols)
            {
                dict[col.ToString()] = (Cells ?? []).FirstOrDefault(c => c.Row == r && c.Column == col)?.Value ?? "";
            }
            return (IDictionary<string, object?>)dict;
        }).ToList();
    }
}

public class NVChat : ThemeAwareView
{
    public static readonly BindableProperty MessagesProperty = BindableProperty.Create(nameof(Messages), typeof(IList<NVChatMessage>), typeof(NVChat), new List<NVChatMessage>(), propertyChanged: Refresh);
    public static readonly BindableProperty AttachmentsProperty = BindableProperty.Create(nameof(Attachments), typeof(IList<NVFileChip>), typeof(NVChat), new List<NVFileChip>(), propertyChanged: Refresh);
    public static readonly BindableProperty IsStreamingProperty = BindableProperty.Create(nameof(IsStreaming), typeof(bool), typeof(NVChat), false, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };
    public NVChat() { Content = _list; ApplyTheme(); }
    public IList<NVChatMessage> Messages { get => (IList<NVChatMessage>)GetValue(MessagesProperty); set => SetValue(MessagesProperty, value); }
    public IList<NVFileChip> Attachments { get => (IList<NVFileChip>)GetValue(AttachmentsProperty); set => SetValue(AttachmentsProperty, value); }
    public bool IsStreaming { get => (bool)GetValue(IsStreamingProperty); set => SetValue(IsStreamingProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVChat)b).ApplyTheme();

    public void AppendStream(string chunk)
    {
        var messages = Messages?.ToList() ?? [];
        var last = messages.LastOrDefault(item => !item.IsMine);
        if (last is null)
        {
            last = new NVChatMessage { Author = "Lumina" };
            messages.Add(last);
        }

        last.Text += chunk ?? "";
        IsStreaming = true;
        Messages = messages;
    }

    public void FinishStream() => IsStreaming = false;

    public void Attach(string name, long size = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        var next = Attachments?.ToList() ?? [];
        next.Add(new NVFileChip { Name = name, Size = size });
        Attachments = next;
    }

    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var message in Messages ?? [])
        {
            var streaming = IsStreaming && !message.IsMine && ReferenceEquals(message, (Messages ?? []).LastOrDefault(item => !item.IsMine));
            _list.Children.Add(new NVBubble
            {
                Text = message.Text + (streaming ? " ▍" : ""),
                IsMine = message.IsMine
            });
        }

        if (IsStreaming)
        {
            _list.Children.Add(new NVTypingIndicator());
        }

        foreach (var file in Attachments ?? [])
        {
            _list.Children.Add(new NVChip { Text = file.Name });
        }

        _list.Children.Add(new NVComposer());
    }
}

public class NVMarkdownViewer : ThemeAwareView
{
    public static readonly BindableProperty MarkdownProperty = BindableProperty.Create(nameof(Markdown), typeof(string), typeof(NVMarkdownViewer), "# Hello", propertyChanged: Refresh);
    readonly VerticalStackLayout _stack = new() { Spacing = NVTokens.Space2 };
    public NVMarkdownViewer() { Content = _stack; ApplyTheme(); }
    public string Markdown { get => (string)GetValue(MarkdownProperty); set => SetValue(MarkdownProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVMarkdownViewer)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _stack.Children.Clear();
        foreach (var (role, text) in NVMarkdownLogic.Parse(Markdown))
        {
            _stack.Children.Add(new NVHeading { Text = text, Role = role });
        }
    }
}

public class NVRichTextEditor : NVEditor
{
    public NVRichTextEditor() => Label = "Rich text";
}

public class NVMap : ThemeAwareView
{
    public static readonly BindableProperty PlaceProperty = BindableProperty.Create(nameof(Place), typeof(string), typeof(NVMap), "Aurora", propertyChanged: Refresh);
    readonly GraphicsView _canvas = new() { HeightRequest = 180, Drawable = new NVMapDrawable() };
    public NVMap() { Content = _canvas; ApplyTheme(); }
    public string Place { get => (string)GetValue(PlaceProperty); set => SetValue(PlaceProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVMap)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        if (_canvas.Drawable is NVMapDrawable drawable)
        {
            drawable.Place = Place;
        }

        _canvas.Invalidate();
    }
}

sealed class NVMapDrawable : IDrawable
{
    public string Place { get; set; } = "Aurora";

    public void Draw(ICanvas canvas, RectF dirty)
    {
        canvas.FillColor = NVTheme.Current.Mist;
        canvas.FillRectangle(dirty);
        canvas.FillColor = NVTheme.Current.Accent.WithAlpha(0.18f);
        canvas.FillEllipse(dirty.X + dirty.Width * 0.15f, dirty.Y + dirty.Height * 0.45f, dirty.Width * 0.7f, dirty.Height * 0.4f);
        canvas.StrokeColor = NVTheme.Current.Fog;
        canvas.StrokeSize = 1;
        for (var i = 1; i < 6; i++)
        {
            var x = dirty.X + dirty.Width * i / 6;
            var y = dirty.Y + dirty.Height * i / 6;
            canvas.DrawLine(x, dirty.Y, x, dirty.Bottom);
            canvas.DrawLine(dirty.X, y, dirty.Right, y);
        }

        var pin = new PointF(dirty.Center.X, dirty.Center.Y - 8);
        canvas.FillColor = NVTheme.Current.Accent;
        canvas.FillCircle(pin, 8);
        canvas.FillColor = NVTheme.Current.Ink;
        canvas.FontSize = (float)NVTokens.Type(12);
        canvas.DrawString(Place, dirty.X, dirty.Bottom - 22, dirty.Width, 18, HorizontalAlignment.Center, VerticalAlignment.Center);
    }
}

public class NVMaps : NVMap { }

public class NVAiAssistView : ThemeAwareView
{
    public static readonly BindableProperty PromptProperty = BindableProperty.Create(nameof(Prompt), typeof(string), typeof(NVAiAssistView), "", BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVChat _chat = new();
    readonly NVTextField _field = new() { Label = "Ask Lumina" };
    public NVAiAssistView()
    {
        _field.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVTextField.Text))
            {
                Prompt = _field.Text;
            }
        };
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _chat, _field } };
        ApplyTheme();
    }
    public string Prompt { get => (string)GetValue(PromptProperty); set => SetValue(PromptProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVAiAssistView)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _field.Text = Prompt;
        _chat.Messages = new List<NVChatMessage>
        {
            new() { Author = "Lumina", Text = string.IsNullOrWhiteSpace(Prompt) ? "How can I help?" : $"Thinking about: {Prompt}" }
        };
    }
}

public class NVPromptInput : NVTextField
{
    public NVPromptInput()
    {
        Label = "Prompt";
        Placeholder = "Ask anything";
    }
}

public class NVAIPrompt : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVAIPrompt), "", BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVPromptInput _input = new();
    readonly HorizontalStackLayout _chips = new() { Spacing = NVTokens.Space2 };
    public NVAIPrompt()
    {
        _input.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVTextField.Text))
            {
                Text = _input.Text;
            }
        };
        foreach (var chip in new[] { "Summarize", "Rewrite", "Translate" })
        {
            var pick = chip;
            var view = new NVChip { Text = pick, Kind = NVChipKind.Assist };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => Text = pick + " this";
            view.GestureRecognizers.Add(tap);
            _chips.Children.Add(view);
        }

        var send = new NVButton { Text = "Send", Variant = NVButtonVariant.Filled };
        send.Command = new Command(() => Text = string.IsNullOrWhiteSpace(Text) ? "Hello Lumina" : Text);
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _chips, _input, send } };
        ApplyTheme();
    }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVAIPrompt)b).ApplyTheme();
    protected override void ApplyTheme() => _input.Text = Text;
}

public class NVSmartPasteButton : NVButton
{
    public NVSmartPasteButton()
    {
        Text = "Smart paste";
        Variant = NVButtonVariant.Tonal;
    }
}
