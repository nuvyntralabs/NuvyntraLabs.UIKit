namespace NuvyntraLabs.UIKit;

/// <summary>⌘K / spotlight. Empty query shows recents; otherwise case-insensitive title filter.</summary>
public class NVCommandPalette : OverlayHost
{
    public static readonly BindableProperty QueryProperty = BindableProperty.Create(nameof(Query), typeof(string), typeof(NVCommandPalette), "", BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty CommandsProperty = BindableProperty.Create(nameof(Commands), typeof(IList<NVCommandItem>), typeof(NVCommandPalette), new List<NVCommandItem>(), propertyChanged: Refresh);
    public static readonly BindableProperty RecentsProperty = BindableProperty.Create(nameof(Recents), typeof(IList<NVCommandItem>), typeof(NVCommandPalette), new List<NVCommandItem>(), propertyChanged: Refresh);

    readonly NVSearchBar _search = new() { Label = "Search commands" };
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };
    readonly NVEmptyView _empty = new() { Title = "No commands", Reason = NVStatusReason.Empty };

    public NVCommandPalette()
    {
        Placement = OverlayPlacement.Top;
        _search.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVTextField.Text))
            {
                Query = _search.Text;
            }
        };
        PanelContent = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _search, _list, _empty } };
        NVAccessibility.Name(this, "Command palette", "Filter and run commands");
        ApplyTheme();
    }

    public string Query { get => (string)GetValue(QueryProperty); set => SetValue(QueryProperty, value); }
    public IList<NVCommandItem> Commands { get => (IList<NVCommandItem>)GetValue(CommandsProperty); set => SetValue(CommandsProperty, value); }
    public IList<NVCommandItem> Recents { get => (IList<NVCommandItem>)GetValue(RecentsProperty); set => SetValue(RecentsProperty, value); }
    public IReadOnlyList<NVCommandItem> VisibleCommands { get; private set; } = [];

    public static bool MatchesOpenShortcut(string key)
    {
        var normalized = key.Replace(" ", "", StringComparison.Ordinal).ToLowerInvariant();
        return normalized is "control+k" or "ctrl+k" or "cmd+k" or "command+k" or "meta+k";
    }

    /// <summary>Ctrl/Cmd+K toggles the palette. Escape uses <see cref="OverlayHost.TryHandleKey"/>.</summary>
    public bool TryHandleShortcut(string key)
    {
        if (MatchesOpenShortcut(key))
        {
            IsOpen = !IsOpen;
            return true;
        }

        return TryHandleKey(key);
    }

    public static IReadOnlyList<NVCommandItem> Filter(IEnumerable<NVCommandItem>? commands, IEnumerable<NVCommandItem>? recents, string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return (recents ?? []).ToList();
        }

        return (commands ?? [])
            .Where(item => (item.Title ?? "").Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVCommandPalette)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        base.ApplyTheme();
        _search.Text = Query;
        VisibleCommands = Filter(Commands, Recents, Query);
        _list.Children.Clear();
        foreach (var item in VisibleCommands)
        {
            var button = new NVButton { Text = item.Title, Variant = NVButtonVariant.Ghost, Command = item.Command };
            button.IsEnabled = IsEnabled;
            button.Command = new Command(
                () =>
                {
                    if (!IsEnabled)
                    {
                        return;
                    }

                    item.Command?.Execute(null);
                    IsOpen = false;
                },
                () => IsEnabled && (item.Command is null || item.Command.CanExecute(null)));
            _list.Children.Add(button);
        }

        _empty.IsVisible = VisibleCommands.Count == 0;
        _list.IsVisible = VisibleCommands.Count > 0;
    }
}

/// <summary>Spotlight hole + title/body/next on a real control.</summary>
public class NVCoachMark : OverlayHost
{
    public static readonly BindableProperty StepsProperty = BindableProperty.Create(nameof(Steps), typeof(IList<NVCoachStep>), typeof(NVCoachMark), new List<NVCoachStep>(), propertyChanged: Refresh);
    public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(NVCoachMark), 0, BindingMode.TwoWay, propertyChanged: Refresh);

    readonly NVHeading _title = new();
    readonly NVBodyText _body = new();
    readonly NVButton _next = new() { Text = "Next", Variant = NVButtonVariant.Filled };

    public NVCoachMark()
    {
        Placement = OverlayPlacement.Center;
        _next.Command = new Command(Next);
        PanelContent = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _title, _body, _next } };
        NVAccessibility.Name(this, "Coach mark", "Guided step");
        ApplyTheme();
    }

    public event EventHandler? Completed;

    public IList<NVCoachStep> Steps { get => (IList<NVCoachStep>)GetValue(StepsProperty); set => SetValue(StepsProperty, value); }
    public int Index { get => (int)GetValue(IndexProperty); set => SetValue(IndexProperty, value); }
    public NVCoachStep? CurrentStep { get; private set; }

    public void Next()
    {
        if (!IsEnabled)
        {
            return;
        }

        var steps = Steps ?? [];
        if (steps.Count == 0 || Index >= steps.Count - 1)
        {
            Completed?.Invoke(this, EventArgs.Empty);
            IsOpen = false;
            return;
        }

        Index++;
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVCoachMark)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        base.ApplyTheme();
        var steps = Steps ?? [];
        var i = steps.Count == 0 ? 0 : Math.Clamp(Index, 0, steps.Count - 1);
        CurrentStep = steps.Count == 0 ? null : steps[i];
        _title.Text = CurrentStep?.Title ?? "";
        _body.Text = CurrentStep?.Body ?? "";
        _next.Text = steps.Count == 0 || i >= steps.Count - 1 ? "Done" : "Next";
        _next.IsEnabled = IsEnabled;
    }
}

/// <summary>Long-press / right-click items. <see cref="NVMenu"/> stays a drop-down button.</summary>
public class NVContextMenu : OverlayHost
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<NVMenuAction>), typeof(NVContextMenu), new List<NVMenuAction>(), propertyChanged: Refresh);

    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };

    public NVContextMenu()
    {
        Placement = OverlayPlacement.Start;
        OpenCommand = new Command(Open);
        PanelContent = _list;
        NVAccessibility.Name(this, "Context menu", "Actions for the current item");
        ApplyTheme();
    }

    public IList<NVMenuAction> Items { get => (IList<NVMenuAction>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    public ICommand OpenCommand { get; }

    public void Open()
    {
        if (IsEnabled)
        {
            IsOpen = true;
        }
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVContextMenu)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        base.ApplyTheme();
        _list.Children.Clear();
        foreach (var item in Items ?? [])
        {
            var fired = false;
            var button = new NVButton
            {
                Text = item.Text,
                Variant = NVButtonVariant.Ghost,
                Command = new Command(
                    () =>
                    {
                        if (!IsEnabled || fired)
                        {
                            return;
                        }

                        fired = true;
                        if (item.Command?.CanExecute(null) == true)
                        {
                            item.Command.Execute(null);
                        }

                        IsOpen = false;
                    },
                    () => IsEnabled && (item.Command is null || item.Command.CanExecute(null)))
            };
            _list.Children.Add(button);
        }
    }
}

/// <summary>Drop well + attach chips. Host supplies pick / bytes.</summary>
public class NVFileDrop : ThemeAwareView
{
    public static readonly BindableProperty FilesProperty = BindableProperty.Create(nameof(Files), typeof(IList<NVFileChip>), typeof(NVFileDrop), new List<NVFileChip>(), BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty PickCommandProperty = BindableProperty.Create(nameof(PickCommand), typeof(ICommand), typeof(NVFileDrop));

    readonly NVBodyText _hint = new() { Text = "Drop files or attach" };
    readonly NVButton _pick = new() { Text = "Attach", Variant = NVButtonVariant.Tonal };
    readonly HorizontalStackLayout _chips = new() { Spacing = NVTokens.Space2 };

    public NVFileDrop()
    {
        _pick.Command = new Command(
            () =>
            {
                if (IsEnabled && PickCommand?.CanExecute(null) == true)
                {
                    PickCommand.Execute(null);
                }
            },
            () => IsEnabled && (PickCommand is null || PickCommand.CanExecute(null)));
        Content = new Border
        {
            Padding = NVTokens.Space4,
            StrokeThickness = 1,
            Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _hint, _pick, _chips } }
        };
        NVAccessibility.Name(this, "File drop", "Attach files");
        ApplyTheme();
    }

    public IList<NVFileChip> Files { get => (IList<NVFileChip>)GetValue(FilesProperty); set => SetValue(FilesProperty, value); }
    public ICommand? PickCommand { get => (ICommand?)GetValue(PickCommandProperty); set => SetValue(PickCommandProperty, value); }

    public static IList<NVFileChip> Sanitize(IEnumerable<NVFileChip>? files) =>
        (files ?? []).Where(file => !string.IsNullOrWhiteSpace(file.Name)).ToList();

    public static string FormatSize(long bytes) =>
        bytes < 1024 ? $"{bytes} B" : $"{bytes / 1024d:0.#} KB";

    public void Attach(NVFileChip? file)
    {
        if (file is null || string.IsNullOrWhiteSpace(file.Name))
        {
            return;
        }

        var next = Sanitize(Files);
        next.Add(file);
        Files = next;
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVFileDrop)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        var clean = Sanitize(Files);
        if (!ReferenceEquals(clean, Files) && (Files is null || clean.Count != Files.Count))
        {
            Files = clean;
            return;
        }

        _chips.Children.Clear();
        foreach (var file in clean)
        {
            _chips.Children.Add(new NVChip { Text = $"{file.Name} · {FormatSize(file.Size)}", Kind = NVChipKind.Input });
        }

        _pick.IsEnabled = IsEnabled && (PickCommand is null || PickCommand.CanExecute(null));
        if (Content is Border border)
        {
            border.BackgroundColor = NVTheme.Current.Surface;
            border.Stroke = NVTheme.Current.Fog;
            border.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusMedium };
        }
    }
}

/// <summary>Blocking / sheet gate. Slots for plan tiles. Dismiss is a no-op when <see cref="IsBlocking"/>.</summary>
public class NVPaywall : OverlayHost
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVPaywall), "Go further", propertyChanged: Refresh);
    public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(NVPaywall), "Unlock Lumina Plus", propertyChanged: Refresh);
    public static readonly BindableProperty IsBlockingProperty = BindableProperty.Create(nameof(IsBlocking), typeof(bool), typeof(NVPaywall), false, propertyChanged: Refresh);
    public static readonly BindableProperty PlansProperty = BindableProperty.Create(nameof(Plans), typeof(View), typeof(NVPaywall), null, propertyChanged: Refresh);

    readonly NVHeading _title = new();
    readonly NVBodyText _body = new();
    readonly ContentView _plans = new();
    readonly NVButton _dismiss = new() { Text = "Not now", Variant = NVButtonVariant.Ghost };

    public NVPaywall()
    {
        Placement = OverlayPlacement.Center;
        _dismiss.Command = new Command(Dismiss);
        PanelContent = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _title, _body, _plans, _dismiss } };
        NVAccessibility.Name(this, "Paywall", "Choose a plan");
        ApplyTheme();
    }

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Message { get => (string)GetValue(MessageProperty); set => SetValue(MessageProperty, value); }
    public bool IsBlocking { get => (bool)GetValue(IsBlockingProperty); set => SetValue(IsBlockingProperty, value); }
    public View? Plans { get => (View?)GetValue(PlansProperty); set => SetValue(PlansProperty, value); }

    public void Dismiss()
    {
        if (IsBlocking || !IsEnabled)
        {
            return;
        }

        IsOpen = false;
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVPaywall)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        DismissOnScrim = !IsBlocking;
        DismissOnEscape = !IsBlocking;
        base.ApplyTheme();
        _title.Text = Title;
        _body.Text = Message;
        _plans.Content = Plans;
        _dismiss.IsVisible = !IsBlocking;
        _dismiss.IsEnabled = IsEnabled && !IsBlocking;
    }
}

/// <summary>Version title + bullet list + dismiss.</summary>
public class NVWhatsNew : OverlayHost
{
    public static readonly BindableProperty VersionTitleProperty = BindableProperty.Create(nameof(VersionTitle), typeof(string), typeof(NVWhatsNew), "What's new", propertyChanged: Refresh);
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVWhatsNew), new List<string>(), propertyChanged: Refresh);

    readonly NVHeading _title = new();
    readonly NVBulletList _bullets = new();
    readonly NVButton _dismiss = new() { Text = "Got it", Variant = NVButtonVariant.Filled };

    public NVWhatsNew()
    {
        Placement = OverlayPlacement.Center;
        _dismiss.Command = new Command(Dismiss);
        PanelContent = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _title, _bullets, _dismiss } };
        NVAccessibility.Name(this, "What's new", "Release notes");
        ApplyTheme();
    }

    public string VersionTitle { get => (string)GetValue(VersionTitleProperty); set => SetValue(VersionTitleProperty, value); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }

    public void Dismiss()
    {
        if (IsEnabled)
        {
            IsOpen = false;
        }
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVWhatsNew)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        base.ApplyTheme();
        _title.Text = VersionTitle;
        _bullets.Items = Items ?? [];
        _dismiss.IsEnabled = IsEnabled;
    }
}

/// <summary>Privacy copy + accept / manage. Accept sets <see cref="IsAccepted"/>.</summary>
public class NVConsentBanner : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVConsentBanner), "We use cookies to keep Lumina useful.", propertyChanged: Refresh);
    public static readonly BindableProperty IsAcceptedProperty = BindableProperty.Create(nameof(IsAccepted), typeof(bool), typeof(NVConsentBanner), false, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty AcceptCommandProperty = BindableProperty.Create(nameof(AcceptCommand), typeof(ICommand), typeof(NVConsentBanner));
    public static readonly BindableProperty ManageCommandProperty = BindableProperty.Create(nameof(ManageCommand), typeof(ICommand), typeof(NVConsentBanner));

    readonly NVBanner _banner = new() { Tone = NVBannerTone.Info };
    readonly NVButton _accept = new() { Text = "Accept", Variant = NVButtonVariant.Filled };
    readonly NVButton _manage = new() { Text = "Manage", Variant = NVButtonVariant.Ghost };

    public NVConsentBanner()
    {
        _accept.Command = new Command(Accept, () => IsEnabled && (AcceptCommand is null || AcceptCommand.CanExecute(null)));
        _manage.Command = new Command(
            () =>
            {
                if (IsEnabled && ManageCommand?.CanExecute(null) == true)
                {
                    ManageCommand.Execute(null);
                }
            },
            () => IsEnabled && (ManageCommand is null || ManageCommand.CanExecute(null)));
        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children = { _banner, new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _accept, _manage } } }
        };
        NVAccessibility.Name(this, "Consent banner", "Privacy choices");
        ApplyTheme();
    }

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public bool IsAccepted { get => (bool)GetValue(IsAcceptedProperty); set => SetValue(IsAcceptedProperty, value); }
    public ICommand? AcceptCommand { get => (ICommand?)GetValue(AcceptCommandProperty); set => SetValue(AcceptCommandProperty, value); }
    public ICommand? ManageCommand { get => (ICommand?)GetValue(ManageCommandProperty); set => SetValue(ManageCommandProperty, value); }

    public void Accept()
    {
        if (!IsEnabled)
        {
            return;
        }

        IsAccepted = true;
        if (AcceptCommand?.CanExecute(null) == true)
        {
            AcceptCommand.Execute(null);
        }
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVConsentBanner)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _banner.Text = Text;
        _accept.IsEnabled = IsEnabled && !IsAccepted && (AcceptCommand is null || AcceptCommand.CanExecute(null));
        _manage.IsEnabled = IsEnabled && (ManageCommand is null || ManageCommand.CanExecute(null));
        IsVisible = !IsAccepted;
    }
}
