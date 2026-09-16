namespace NuvyntraLabs.UIKit;

/// <summary>Compact in-call overlay. Host attaches VoipCore.</summary>
public class NVCallBar : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVCallBar), "On a call", propertyChanged: Refresh);
    public static readonly BindableProperty IsMutedProperty = BindableProperty.Create(nameof(IsMuted), typeof(bool), typeof(NVCallBar), false, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty MuteCommandProperty = BindableProperty.Create(nameof(MuteCommand), typeof(ICommand), typeof(NVCallBar));
    public static readonly BindableProperty EndCommandProperty = BindableProperty.Create(nameof(EndCommand), typeof(ICommand), typeof(NVCallBar));

    readonly NVHeading _title = new() { Role = NVTextRole.Body };
    readonly NVButton _mute = new() { Variant = NVButtonVariant.Tonal };
    readonly NVButton _end = new() { Text = "End", Variant = NVButtonVariant.Danger };

    public NVCallBar()
    {
        _mute.Command = HostCommand(() => MuteCommand, () => IsMuted = !IsMuted);
        _end.Command = HostCommand(() => EndCommand);
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space3, Children = { _title, _mute, _end } };
        NVAccessibility.Name(this, "Call bar", "Mute or end the call");
        ApplyTheme();
    }

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public bool IsMuted { get => (bool)GetValue(IsMutedProperty); set => SetValue(IsMutedProperty, value); }
    public ICommand? MuteCommand { get => (ICommand?)GetValue(MuteCommandProperty); set => SetValue(MuteCommandProperty, value); }
    public ICommand? EndCommand { get => (ICommand?)GetValue(EndCommandProperty); set => SetValue(EndCommandProperty, value); }

    static void Refresh(BindableObject b, object o, object n) => ((NVCallBar)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _title.Text = Title;
        _mute.Text = IsMuted ? "Unmute" : "Mute";
        _mute.IsEnabled = IsEnabled && Can(MuteCommand);
        _end.IsEnabled = IsEnabled && Can(EndCommand);
    }

    internal static bool Can(ICommand? command) => command is null || command.CanExecute(null);

    internal Command HostCommand(Func<ICommand?> source, Action? local = null) =>
        new(
            () =>
            {
                if (!IsEnabled)
                {
                    return;
                }

                local?.Invoke();
                var command = source();
                if (command?.CanExecute(null) == true)
                {
                    command.Execute(null);
                }
            },
            () => IsEnabled && Can(source()));
}

/// <summary>Full in-call chrome. Host attaches VoipCore.</summary>
public class NVInCallView : ThemeAwareView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(NVInCallView), "Ada", propertyChanged: Refresh);
    public static readonly BindableProperty ElapsedProperty = BindableProperty.Create(nameof(Elapsed), typeof(string), typeof(NVInCallView), "00:42", propertyChanged: Refresh);
    public static readonly BindableProperty KeypadProperty = BindableProperty.Create(nameof(Keypad), typeof(View), typeof(NVInCallView));
    public static readonly BindableProperty MuteCommandProperty = BindableProperty.Create(nameof(MuteCommand), typeof(ICommand), typeof(NVInCallView));
    public static readonly BindableProperty EndCommandProperty = BindableProperty.Create(nameof(EndCommand), typeof(ICommand), typeof(NVInCallView));

    readonly NVAvatar _avatar = new() { Initials = "AD", StatusOn = true };
    readonly NVHeading _name = new();
    readonly NVCaptionText _elapsed = new();
    readonly ContentView _keypad = new();
    readonly NVCallBar _bar = new();

    public NVInCallView()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, HorizontalOptions = LayoutOptions.Center, Children = { _avatar, _name, _elapsed, _keypad, _bar } };
        NVAccessibility.Name(this, "In-call", "Full call chrome");
        ApplyTheme();
    }

    public string Name { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
    public string Elapsed { get => (string)GetValue(ElapsedProperty); set => SetValue(ElapsedProperty, value); }
    public View? Keypad { get => (View?)GetValue(KeypadProperty); set => SetValue(KeypadProperty, value); }
    public ICommand? MuteCommand { get => (ICommand?)GetValue(MuteCommandProperty); set => SetValue(MuteCommandProperty, value); }
    public ICommand? EndCommand { get => (ICommand?)GetValue(EndCommandProperty); set => SetValue(EndCommandProperty, value); }

    static void Refresh(BindableObject b, object o, object n) => ((NVInCallView)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _name.Text = Name;
        _elapsed.Text = Elapsed;
        _keypad.Content = Keypad;
        _bar.Title = Name;
        _bar.MuteCommand = MuteCommand;
        _bar.EndCommand = EndCommand;
    }
}

/// <summary>Local vs remote + keep / take remote. Host attaches OfflineSync.</summary>
public class NVSyncConflictCard : ThemeAwareView
{
    public static readonly BindableProperty LocalProperty = BindableProperty.Create(nameof(Local), typeof(string), typeof(NVSyncConflictCard), "", propertyChanged: Refresh);
    public static readonly BindableProperty RemoteProperty = BindableProperty.Create(nameof(Remote), typeof(string), typeof(NVSyncConflictCard), "", propertyChanged: Refresh);
    public static readonly BindableProperty KeepCommandProperty = BindableProperty.Create(nameof(KeepCommand), typeof(ICommand), typeof(NVSyncConflictCard));
    public static readonly BindableProperty TakeRemoteCommandProperty = BindableProperty.Create(nameof(TakeRemoteCommand), typeof(ICommand), typeof(NVSyncConflictCard));

    readonly NVCard _local = new() { Title = "Local" };
    readonly NVCard _remote = new() { Title = "Remote" };
    readonly NVButton _keep = new() { Text = "Keep local", Variant = NVButtonVariant.Tonal };
    readonly NVButton _take = new() { Text = "Take remote", Variant = NVButtonVariant.Filled };

    public NVSyncConflictCard()
    {
        _keep.Command = Host();
        _take.Command = Host(remote: true);
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _local, _remote, new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _keep, _take } } } };
        NVAccessibility.Name(this, "Sync conflict", "Keep local or take remote");
        ApplyTheme();
    }

    public string Local { get => (string)GetValue(LocalProperty); set => SetValue(LocalProperty, value); }
    public string Remote { get => (string)GetValue(RemoteProperty); set => SetValue(RemoteProperty, value); }
    public ICommand? KeepCommand { get => (ICommand?)GetValue(KeepCommandProperty); set => SetValue(KeepCommandProperty, value); }
    public ICommand? TakeRemoteCommand { get => (ICommand?)GetValue(TakeRemoteCommandProperty); set => SetValue(TakeRemoteCommandProperty, value); }

    Command Host(bool remote = false) =>
        new(
            () =>
            {
                if (!IsEnabled)
                {
                    return;
                }

                var command = remote ? TakeRemoteCommand : KeepCommand;
                if (command?.CanExecute(null) == true)
                {
                    command.Execute(null);
                }
            },
            () => IsEnabled && NVCallBar.Can(remote ? TakeRemoteCommand : KeepCommand));

    static void Refresh(BindableObject b, object o, object n) => ((NVSyncConflictCard)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _local.Body = Local;
        _remote.Body = Remote;
        _keep.IsEnabled = IsEnabled && NVCallBar.Can(KeepCommand);
        _take.IsEnabled = IsEnabled && NVCallBar.Can(TakeRemoteCommand);
    }
}

/// <summary>File name, bytes, retry. Host attaches SmartUpload.</summary>
public class NVUploadTile : ThemeAwareView
{
    public static readonly BindableProperty FileNameProperty = BindableProperty.Create(nameof(FileName), typeof(string), typeof(NVUploadTile), "file.bin", propertyChanged: Refresh);
    public static readonly BindableProperty BytesProperty = BindableProperty.Create(nameof(Bytes), typeof(long), typeof(NVUploadTile), 0L, propertyChanged: Refresh);
    public static readonly BindableProperty RetryCommandProperty = BindableProperty.Create(nameof(RetryCommand), typeof(ICommand), typeof(NVUploadTile));

    readonly NVBanner _banner = new() { Tone = NVBannerTone.Info };
    readonly NVButton _retry = new() { Text = "Retry", Variant = NVButtonVariant.Ghost };

    public NVUploadTile()
    {
        _retry.Command = new Command(
            () =>
            {
                if (IsEnabled && RetryCommand?.CanExecute(null) == true)
                {
                    RetryCommand.Execute(null);
                }
            },
            () => IsEnabled && NVCallBar.Can(RetryCommand));
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _banner, _retry } };
        NVAccessibility.Name(this, "Upload tile", "Retry an upload");
        ApplyTheme();
    }

    public string FileName { get => (string)GetValue(FileNameProperty); set => SetValue(FileNameProperty, value); }
    public long Bytes { get => (long)GetValue(BytesProperty); set => SetValue(BytesProperty, value); }
    public ICommand? RetryCommand { get => (ICommand?)GetValue(RetryCommandProperty); set => SetValue(RetryCommandProperty, value); }

    static void Refresh(BindableObject b, object o, object n) => ((NVUploadTile)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _banner.Text = $"{FileName} · {NVFileDrop.FormatSize(Bytes)}";
        _retry.IsEnabled = IsEnabled && NVCallBar.Can(RetryCommand);
    }
}

/// <summary>Nearby device list + connect. Host attaches BluetoothManager.</summary>
public class NVDeviceSheet : OverlayHost
{
    public static readonly BindableProperty DevicesProperty = BindableProperty.Create(nameof(Devices), typeof(IList<NVDeviceItem>), typeof(NVDeviceSheet), new List<NVDeviceItem>(), propertyChanged: Refresh);
    public static readonly BindableProperty ConnectCommandProperty = BindableProperty.Create(nameof(ConnectCommand), typeof(ICommand), typeof(NVDeviceSheet));

    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };

    public NVDeviceSheet()
    {
        Placement = OverlayPlacement.Bottom;
        PanelContent = _list;
        NVAccessibility.Name(this, "Device sheet", "Connect a nearby device");
        ApplyTheme();
    }

    public IList<NVDeviceItem> Devices { get => (IList<NVDeviceItem>)GetValue(DevicesProperty); set => SetValue(DevicesProperty, value); }
    public ICommand? ConnectCommand { get => (ICommand?)GetValue(ConnectCommandProperty); set => SetValue(ConnectCommandProperty, value); }

    static void Refresh(BindableObject b, object o, object n) => ((NVDeviceSheet)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        base.ApplyTheme();
        _list.Children.Clear();
        foreach (var device in Devices ?? [])
        {
            var item = device;
            _list.Children.Add(new NVButton
            {
                Text = item.IsConnected ? $"{item.Name} · connected" : item.Name,
                Variant = NVButtonVariant.Ghost,
                Command = new Command(
                    () =>
                    {
                        if (IsEnabled && ConnectCommand?.CanExecute(item) == true)
                        {
                            ConnectCommand.Execute(item);
                            IsOpen = false;
                        }
                    },
                    () => IsEnabled && (ConnectCommand is null || ConnectCommand.CanExecute(item)))
            });
        }

        if ((Devices ?? []).Count == 0)
        {
            _list.Children.Add(new NVEmptyView { Title = "No devices", Reason = NVStatusReason.Empty });
        }
    }
}

/// <summary>Page image slot + print / share. Host attaches Printing / SharePlus.</summary>
public class NVPrintPreview : ThemeAwareView
{
    public static readonly BindableProperty PageProperty = BindableProperty.Create(nameof(Page), typeof(View), typeof(NVPrintPreview), propertyChanged: Refresh);
    public static readonly BindableProperty PrintCommandProperty = BindableProperty.Create(nameof(PrintCommand), typeof(ICommand), typeof(NVPrintPreview));
    public static readonly BindableProperty ShareCommandProperty = BindableProperty.Create(nameof(ShareCommand), typeof(ICommand), typeof(NVPrintPreview));

    readonly ContentView _page = new();
    readonly NVButton _print = new() { Text = "Print", Variant = NVButtonVariant.Filled };
    readonly NVButton _share = new() { Text = "Share", Variant = NVButtonVariant.Ghost };

    public NVPrintPreview()
    {
        _print.Command = Bind(() => PrintCommand);
        _share.Command = Bind(() => ShareCommand);
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _page, new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _print, _share } } } };
        NVAccessibility.Name(this, "Print preview", "Print or share the page");
        ApplyTheme();
    }

    public View? Page { get => (View?)GetValue(PageProperty); set => SetValue(PageProperty, value); }
    public ICommand? PrintCommand { get => (ICommand?)GetValue(PrintCommandProperty); set => SetValue(PrintCommandProperty, value); }
    public ICommand? ShareCommand { get => (ICommand?)GetValue(ShareCommandProperty); set => SetValue(ShareCommandProperty, value); }

    Command Bind(Func<ICommand?> source) =>
        new(
            () =>
            {
                if (IsEnabled && source()?.CanExecute(null) == true)
                {
                    source()!.Execute(null);
                }
            },
            () => IsEnabled && NVCallBar.Can(source()));

    static void Refresh(BindableObject b, object o, object n) => ((NVPrintPreview)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _page.Content = Page ?? new NVCaptionText { Text = "Page slot" };
        _print.IsEnabled = IsEnabled && NVCallBar.Can(PrintCommand);
        _share.IsEnabled = IsEnabled && NVCallBar.Can(ShareCommand);
    }
}

/// <summary>Hold-near artwork + status. Host attaches NfcPlus.</summary>
public class NVNfcPrompt : ThemeAwareView
{
    public static readonly BindableProperty StatusProperty = BindableProperty.Create(nameof(Status), typeof(string), typeof(NVNfcPrompt), "Hold near the tag", propertyChanged: Refresh);

    readonly NVIcon _icon = new() { Kind = NVIconKind.Phone };
    readonly NVHeading _status = new() { Role = NVTextRole.Body };

    public NVNfcPrompt()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, HorizontalOptions = LayoutOptions.Center, Children = { _icon, _status } };
        NVAccessibility.Name(this, "NFC prompt", "Hold near a tag");
        ApplyTheme();
    }

    public string Status { get => (string)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

    static void Refresh(BindableObject b, object o, object n) => ((NVNfcPrompt)b).ApplyTheme();

    protected override void ApplyTheme() => _status.Text = Status;
}

/// <summary>Stars + not now / review. Host attaches AppReview.</summary>
public class NVReviewPrompt : OverlayHost
{
    public static readonly BindableProperty RatingProperty = BindableProperty.Create(nameof(Rating), typeof(int), typeof(NVReviewPrompt), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty NotNowCommandProperty = BindableProperty.Create(nameof(NotNowCommand), typeof(ICommand), typeof(NVReviewPrompt));
    public static readonly BindableProperty ReviewCommandProperty = BindableProperty.Create(nameof(ReviewCommand), typeof(ICommand), typeof(NVReviewPrompt));

    readonly NVHeading _title = new() { Text = "Enjoying Lumina?" };
    readonly NVRating _stars = new();
    readonly NVButton _later = new() { Text = "Not now", Variant = NVButtonVariant.Ghost };
    readonly NVButton _review = new() { Text = "Review", Variant = NVButtonVariant.Filled };

    public NVReviewPrompt()
    {
        Placement = OverlayPlacement.Center;
        _stars.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVRating.Value))
            {
                Rating = _stars.Value;
            }
        };
        _later.Command = Bind(() => NotNowCommand, close: true);
        _review.Command = Bind(() => ReviewCommand, close: true);
        PanelContent = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _title, _stars, new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _later, _review } } } };
        NVAccessibility.Name(this, "Review prompt", "Rate the app");
        ApplyTheme();
    }

    public int Rating { get => (int)GetValue(RatingProperty); set => SetValue(RatingProperty, value); }
    public ICommand? NotNowCommand { get => (ICommand?)GetValue(NotNowCommandProperty); set => SetValue(NotNowCommandProperty, value); }
    public ICommand? ReviewCommand { get => (ICommand?)GetValue(ReviewCommandProperty); set => SetValue(ReviewCommandProperty, value); }

    Command Bind(Func<ICommand?> source, bool close) =>
        new(
            () =>
            {
                if (!IsEnabled)
                {
                    return;
                }

                if (source()?.CanExecute(null) == true)
                {
                    source()!.Execute(null);
                }

                if (close)
                {
                    IsOpen = false;
                }
            },
            () => IsEnabled && NVCallBar.Can(source()));

    static void Refresh(BindableObject b, object o, object n) => ((NVReviewPrompt)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        base.ApplyTheme();
        _stars.Value = Rating;
        _later.IsEnabled = IsEnabled && NVCallBar.Can(NotNowCommand);
        _review.IsEnabled = IsEnabled && NVCallBar.Can(ReviewCommand);
    }
}
