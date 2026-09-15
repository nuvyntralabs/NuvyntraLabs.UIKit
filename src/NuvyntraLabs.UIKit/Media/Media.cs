namespace NuvyntraLabs.UIKit;

public class NVImageEditor : ThemeAwareView
{
    public static readonly BindableProperty CaptionProperty = BindableProperty.Create(nameof(Caption), typeof(string), typeof(NVImageEditor), "Image", propertyChanged: Refresh);
    readonly NVInteractiveViewer _viewer = new();
    public NVImageEditor() { Content = _viewer; ApplyTheme(); }
    public string Caption { get => (string)GetValue(CaptionProperty); set => SetValue(CaptionProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVImageEditor)b).ApplyTheme();
    protected override void ApplyTheme() => _viewer.Viewport = new NVCard { Title = Caption, Body = "Crop · rotate · annotate" };
}

public class NVPdfViewer : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVPdfViewer), "Document.pdf", propertyChanged: Refresh);
    readonly NVCard _card = new();
    public NVPdfViewer() { Content = _card; ApplyTheme(); }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVPdfViewer)b).ApplyTheme();
    protected override void ApplyTheme() { _card.Title = Title; _card.Body = "PDF viewer (kit-level)"; }
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
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };
    public NVChat() { Content = _list; ApplyTheme(); }
    public IList<NVChatMessage> Messages { get => (IList<NVChatMessage>)GetValue(MessagesProperty); set => SetValue(MessagesProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVChat)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var message in Messages ?? [])
        {
            _list.Children.Add(new NVCard
            {
                Title = message.IsMine ? "You" : message.Author,
                Body = message.Text
            });
        }
    }
}

public class NVMarkdownViewer : ThemeAwareView
{
    public static readonly BindableProperty MarkdownProperty = BindableProperty.Create(nameof(Markdown), typeof(string), typeof(NVMarkdownViewer), "# Hello", propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular };
    public NVMarkdownViewer() { Content = _label; ApplyTheme(); }
    public string Markdown { get => (string)GetValue(MarkdownProperty); set => SetValue(MarkdownProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVMarkdownViewer)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _label.Text = Markdown;
        _label.TextColor = NVTheme.Current.Ink;
    }
}

public class NVRichTextEditor : NVEditor
{
    public NVRichTextEditor() => Label = "Rich text";
}

public class NVMap : ThemeAwareView
{
    public static readonly BindableProperty PlaceProperty = BindableProperty.Create(nameof(Place), typeof(string), typeof(NVMap), "Aurora", propertyChanged: Refresh);
    readonly NVCard _card = new();
    public NVMap() { Content = _card; ApplyTheme(); }
    public string Place { get => (string)GetValue(PlaceProperty); set => SetValue(PlaceProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVMap)b).ApplyTheme();
    protected override void ApplyTheme() { _card.Title = Place; _card.Body = "Map surface (kit-level)"; }
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
        _chips.Children.Add(new NVChip { Text = "Summarize" });
        _chips.Children.Add(new NVChip { Text = "Rewrite" });
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _chips, _input, new NVButton { Text = "Send", Variant = NVButtonVariant.Filled } } };
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
