namespace NuvyntraLabs.UIKit.Sample.Pages;

public sealed class PrimitivesPage : CatalogSectionPage
{
    public PrimitivesPage() : base("02  Primitives",
        "Building blocks first. NV-PRI-01 … NV-PRI-12.",
        () =>
        [
            Gallery.Sample(1, "NVSurface", "NV-PRI-01  ·  Themed paper", new NVSurface { Elevation = 1, Content = new NVCaptionText { Text = "Surface" } }),
            Gallery.Sample(2, "NVDivider", "NV-PRI-02  ·  Hairline", new NVDivider()),
            Gallery.Sample(3, "NVIcon", "NV-PRI-03  ·  Star", new NVIcon { Kind = NVIconKind.Star }),
            Gallery.Sample(4, "NVAvatar", "NV-PRI-04  ·  Initials + status", new NVAvatar { Initials = "NV", StatusOn = true }),
            Gallery.Sample(5, "NVBadge", "NV-PRI-05  ·  Count", new NVBadge { Text = "3" }),
            Gallery.Sample(6, "NVSkeleton", "NV-PRI-06  ·  Placeholder", new NVSkeleton()),
            Gallery.Sample(7, "NVEffects", "NV-PRI-07  ·  Tap to flash press", new NVEffects()),
            Gallery.Sample(8, "NVElevation", "NV-PRI-08  ·  Shadow steps 0–3", ElevationDemo()),
            Gallery.Overlay(9, "NVOverlay", "NV-PRI-09  ·  Scrim + panel", new NVOverlay()),
            Gallery.Sample(10, "NVInteractiveViewer", "NV-PRI-10  ·  Zoom / reset", new NVInteractiveViewer { Viewport = new NVCaptionText { Text = "Viewport" } }),
            Gallery.Sample(11, "NVSpacer", "NV-PRI-11  ·  Token gap between the cards", new VerticalStackLayout
            {
                Children =
                {
                    new NVCaptionText { Text = "Above" },
                    new NVSpacer(),
                    new NVCaptionText { Text = "Below" }
                }
            }),
            Gallery.Sample(12, "NVHighlight", "NV-PRI-12  ·  Mention span", new NVHighlight { Text = "aurora" })
        ]) { }

    static View ElevationDemo()
    {
        var row = new HorizontalStackLayout { Spacing = NVTokens.Space2 };
        for (var i = 0; i <= 3; i++)
        {
            row.Children.Add(new NVSurface { Elevation = i, Content = new NVCaptionText { Text = $"E{i}" } });
        }

        return row;
    }
}

public sealed class FeedbackPage : CatalogSectionPage
{
    public FeedbackPage() : base("06  Feedback",
        "Status and interruption. NV-FBK-01 … NV-FBK-10.",
        () =>
        [
            Gallery.Sample(1, "NVProgressBar", "NV-FBK-01  ·  60%", new NVProgressBar { Value = 0.6 }),
            Gallery.Sample(2, "NVCircularProgressBar", "NV-FBK-02  ·  40%", new NVCircularProgressBar { Value = 0.4 }),
            Gallery.Sample(3, "NVStepProgressBar", "NV-FBK-03  ·  Step 2 of 3", new NVStepProgressBar { Index = 1 }),
            Gallery.Sample(4, "NVBusyIndicator", "NV-FBK-04  ·  Spinner", new NVBusyIndicator()),
            Gallery.Sample(5, "NVPullToRefresh", "NV-FBK-05  ·  Pull the list", new NVPullToRefresh()),
            Gallery.Overlay(6, "NVPopup", "NV-FBK-06  ·  Modal popup", new NVPopup()),
            Gallery.Sample(7, "NVToast", "NV-FBK-07  ·  Timed alert", ToastDemo()),
            Gallery.Sample(8, "NVBanner", "NV-FBK-08  ·  Success tone", new NVBanner { Text = "Lumina is ready", Tone = NVBannerTone.Success }),
            Gallery.Sample(9, "NVEmptyView", "NV-FBK-09  ·  Empty reason", new NVEmptyView { Title = "Nothing yet", Reason = NVStatusReason.Empty }),
            Gallery.Sample(10, "NVTooltip", "NV-FBK-10  ·  Hint", new NVTooltip { Text = "Hint" })
        ]) { }

    static View ToastDemo()
    {
        var button = new NVButton { Text = "Show toast", Variant = NVButtonVariant.Tonal };
        button.Command = new Command(async () => await NVToast.ShowAsync("Lumina is ready"));
        return button;
    }
}

public sealed class LayoutPage : CatalogSectionPage
{
    public LayoutPage() : base("07  Layout",
        "Structure and navigation. NV-LAY-01 … NV-LAY-16.",
        () =>
        [
            Gallery.Sample(1, "NVCard", "NV-LAY-01", new NVCard { Title = "Card", Body = "Warm paper" }),
            Gallery.Sample(2, "NVAccordion", "NV-LAY-02", new NVAccordion { Title = "Accordion", IsExpanded = true, Panel = new NVBodyText { Text = "One-open section body." } }),
            Gallery.Sample(3, "NVExpander", "NV-LAY-03", new NVExpander { Title = "Expander", IsExpanded = true, Panel = new NVBodyText { Text = "Single panel body." } }),
            Gallery.Sample(4, "NVTabView", "NV-LAY-04", new NVTabView { Tabs = new List<string> { "One", "Two" } }),
            Gallery.Overlay(5, "NVBottomSheet", "NV-LAY-05  ·  Bottom detent", new NVBottomSheet()),
            Gallery.Sample(6, "NVNavigationDrawer", "NV-LAY-06", new NVNavigationDrawer { Items = new List<string> { "Home", "Settings" } }),
            Gallery.Sample(7, "NVNavigationView", "NV-LAY-07  ·  Adaptive rail", new NVNavigationView()),
            Gallery.Sample(8, "NVDockLayout", "NV-LAY-08", new NVDockLayout()),
            Gallery.Sample(9, "NVWrapLayout", "NV-LAY-09", new NVWrapLayout { Items = new List<string> { "One", "Two", "Three" } }),
            Gallery.Sample(10, "NVGridSplitter", "NV-LAY-10", new NVGridSplitter()),
            Gallery.Sample(11, "NVToolbar", "NV-LAY-11", new NVToolbar { Title = "Toolbar" }),
            Gallery.Sample(12, "NVBackdrop", "NV-LAY-12", new NVBackdrop()),
            Gallery.Sample(13, "NVCarousel", "NV-LAY-13", new NVCarousel { Items = new List<string> { "One", "Two" } }),
            Gallery.Sample(14, "NVParallaxView", "NV-LAY-14", new NVParallaxView { Items = new List<string> { "Header", "Body" } }),
            Gallery.Sample(15, "NVBottomNavigation", "NV-LAY-15", new NVBottomNavigation()),
            Gallery.Sample(16, "NVRadialMenu", "NV-LAY-16", new NVRadialMenu())
        ]) { }
}

public sealed class DataPage : CatalogSectionPage
{
    public DataPage() : base("08  Data",
        "Lists and tables. NV-DAT-01 … NV-DAT-07.",
        () =>
        [
            Gallery.Sample(1, "NVCollectionView", "NV-DAT-01  ·  Virtualized, swipe, multi-select, grouped", new NVCollectionView
            {
                Items = [new NVListItem { Title = "Aurora", Subtitle = "Ink" }, new NVListItem { Title = "Paper", Subtitle = "Mist" }],
                LayoutMode = NVLayoutMode.List,
                SelectionMode = NVSelectionKind.Multiple,
                AllowSwipe = true,
                Grouped = true
            }),
            Gallery.Sample(2, "NVDataGrid", "NV-DAT-02  ·  Sort / filter / freeze / page", new NVDataGrid
            {
                Columns =
                [
                    new NVGridColumn { Header = "Name", Binding = "name", Frozen = true },
                    new NVGridColumn { Header = "N", Binding = "n" }
                ],
                Rows =
                [
                    new Dictionary<string, object?> { ["name"] = "Cora", ["n"] = 3 },
                    new Dictionary<string, object?> { ["name"] = "Ada", ["n"] = 1 },
                    new Dictionary<string, object?> { ["name"] = "Bea", ["n"] = 2 }
                ],
                Filter = "",
                FrozenColumnCount = 1,
                PageSize = 2
            }),
            Gallery.Sample(3, "NVTreeDataGrid", "NV-DAT-03  ·  Expand-once", new NVTreeDataGrid
            {
                Columns = [new NVGridColumn { Header = "Node", Binding = "name" }],
                Roots = [new NVTreeNode { Title = "Root", IsExpanded = true, Children = { new NVTreeNode { Title = "Leaf" } } }]
            }),
            Gallery.Sample(4, "NVTreeView", "NV-DAT-04  ·  Tap to expand", new NVTreeView
            {
                Roots = [new NVTreeNode { Title = "Root", IsExpanded = true, Children = { new NVTreeNode { Title = "Leaf A" }, new NVTreeNode { Title = "Leaf B" } } }]
            }),
            Gallery.Sample(5, "NVDataForm", "NV-DAT-05", new NVDataForm { Fields = [NVFormField.For("Title", typeof(string), "Kit")] }),
            Gallery.Sample(6, "NVDataPager", "NV-DAT-06  ·  42 items, page size 10", new NVDataPager { TotalCount = 42, PageSize = 10 }),
            Gallery.Sample(7, "NVKanban", "NV-DAT-07", KanbanDemo())
        ]) { }

    static View KanbanDemo()
    {
        var todo = new NVKanbanColumn { Title = "Todo" };
        todo.Cards.Add(new NVListItem { Title = "Tokens", Subtitle = "Paper" });
        var doing = new NVKanbanColumn { Title = "Doing" };
        doing.Cards.Add(new NVListItem { Title = "Gallery", Subtitle = "Seed demos" });
        return new NVKanban { Columns = [todo, doing] };
    }
}

public sealed class ChartsPage : CatalogSectionPage
{
    public ChartsPage() : base("09  Charts",
        "Visualization. NV-VIZ-01 … NV-VIZ-07.",
        () =>
        [
            Gallery.Sample(1, "NVChart", "NV-VIZ-01  ·  Drawn series (empty is safe)", new NVChart
            {
                Series =
                [
                    new NVChartSeries
                    {
                        Title = "Week",
                        Kind = NVChartSeriesKind.Bar,
                        Points = [new NVChartPoint { Category = "Mon", Value = 3 }, new NVChartPoint { Category = "Tue", Value = 8 }]
                    },
                    new NVChartSeries
                    {
                        Title = "Trend",
                        Kind = NVChartSeriesKind.Line,
                        Points = [new NVChartPoint { Category = "Mon", Value = 2 }, new NVChartPoint { Category = "Tue", Value = 6 }]
                    }
                ]
            }),
            Gallery.Sample(2, "NVRadialGauge", "NV-VIZ-02", new NVRadialGauge { Value = 70 }),
            Gallery.Sample(3, "NVLinearGauge", "NV-VIZ-03", new NVLinearGauge()),
            Gallery.Sample(4, "NVDigitalGauge", "NV-VIZ-04", new NVDigitalGauge { Value = 128 }),
            Gallery.Sample(5, "NVMap", "NV-VIZ-05  ·  Host supplies tiles", new NVMap { Place = "Aurora" }),
            Gallery.Sample(6, "NVBarcode", "NV-VIZ-06  ·  Drawn Code128 / QR", new NVBarcode { Value = "NUVEXA", Format = NVBarcodeFormat.Qr }),
            Gallery.Sample(7, "NVTreeMap", "NV-VIZ-07", new NVTreeMap { Nodes = [new NVTreeMapNode { Title = "A", Value = 8 }, new NVTreeMapNode { Title = "B", Value = 3 }] })
        ]) { }
}

public sealed class CalendarPage : CatalogSectionPage
{
    public CalendarPage() : base("10  Calendar",
        "Dates and appointments. NV-CAL-01 … NV-CAL-02.",
        () =>
        [
            Gallery.Sample(1, "NVCalendar", "NV-CAL-01  ·  Month nav + multi-select", new NVCalendar { AllowMultiple = true, Month = DateTime.Today }),
            Gallery.Sample(2, "NVScheduler", "NV-CAL-02  ·  Recurrence cap + agenda", new NVScheduler
            {
                RecurrenceCap = 5,
                Appointments =
                [
                    new NVAppointment
                    {
                        Title = "Standup",
                        Start = DateTime.Today.AddHours(9),
                        End = DateTime.Today.AddHours(9.5),
                        Recurrence = "DAILY;COUNT=8"
                    }
                ]
            })
        ]) { }
}

public sealed class MediaPage : CatalogSectionPage
{
    public MediaPage() : base("11  Media",
        "Documents, chat, and AI chrome. NV-MED-01 … NV-MED-10.",
        () =>
        [
            Gallery.Sample(1, "NVImageEditor", "NV-MED-01  ·  Rotate / crop / annotate", new NVImageEditor { Caption = "Edit", RotationDegrees = 90, Annotations = ["circle"] }),
            Gallery.Sample(2, "NVRichTextEditor", "NV-MED-02", new NVRichTextEditor()),
            Gallery.Sample(3, "NVMarkdownViewer", "NV-MED-03", new NVMarkdownViewer { Markdown = "# Lumina" }),
            Gallery.Sample(4, "NVPdfViewer", "NV-MED-04  ·  Zoom + search", new NVPdfViewer { Title = "Spec.pdf", Pages = ["Lumina spec", "Tokens and density"], Query = "spec", Zoom = 1.25 }),
            Gallery.Sample(5, "NVChat", "NV-MED-05  ·  Stream + attach", new NVChat
            {
                IsStreaming = true,
                Messages = [new NVChatMessage { Author = "Lumina", Text = "Hello" }],
                Attachments = [new NVFileChip { Name = "shot.png", Size = 12 }]
            }),
            Gallery.Sample(6, "NVAIPrompt", "NV-MED-06", new NVAIPrompt()),
            Gallery.Sample(7, "NVSmartPasteButton", "NV-MED-07", new NVSmartPasteButton()),
            Gallery.Sample(8, "NVDocxViewer", "NV-MED-08  ·  Viewer", new NVDocxViewer { Title = "Brief.docx", Pages = ["Heading 1", "Body copy for the kit-level viewer."] }),
            Gallery.Sample(9, "NVSpreadsheet", "NV-MED-09  ·  Cell grid", new NVSpreadsheet { Cells = [new NVSpreadsheetCell { Row = 0, Column = 0, Text = "A1" }] }),
            Gallery.Sample(10, "NVPromptInput", "NV-MED-10", new NVPromptInput())
        ]) { }
}

public sealed class BasicsPage : CatalogSectionPage
{
    public BasicsPage() : base("05  Basics",
        "Everyday mobile chrome, in build order: type → rows → overlays → fields.",
        () =>
        [
            Gallery.Chapter("Type and image"),
            Gallery.Sample(1, "NVHeading", "Display / title role", new NVHeading { Text = "Lumina", Role = NVTextRole.Display }),
            Gallery.Sample(2, "NVBodyText", "Body copy", new NVBodyText { Text = "One kit for a typical mobile screen." }),
            Gallery.Sample(3, "NVCaptionText", "Caption", new NVCaptionText { Text = "Muted supporting line" }),
            Gallery.Sample(4, "NVImage", "Image + caption slot", new NVImage { Caption = "Placeholder" }),
            Gallery.Sample(5, "NVSafeArea", "Inset wrapper", new NVSafeArea { Child = new NVCaptionText { Text = "Padded child" } }),
            Gallery.Chapter("Chrome"),
            Gallery.Sample(6, "NVSectionHeader", "Section label", new NVSectionHeader { Text = "Account" }),
            Gallery.Sample(7, "NVFormSection", "Labeled field group", FormSectionDemo()),
            Gallery.Sample(8, "NVFloatingActionButton", "Primary plus action", new NVFloatingActionButton()),
            Gallery.Sample(9, "NVDotIndicator", "Page dots, index 2 of 4", new NVDotIndicator { Count = 4, Index = 1 }),
            Gallery.Sample(10, "NVAppScaffold", "Toolbar + body + FAB + tabs", new NVAppScaffold { Title = "Scaffold", Body = new NVCaptionText { Text = "Body" } }),
            Gallery.Chapter("Rows"),
            Gallery.Sample(11, "NVListTile", "Leading · title · trailing", new NVListTile { Title = "Home", Subtitle = "Default destination", Kind = NVIconKind.Home }),
            Gallery.Sample(12, "NVSettingsTile", "Label + switch", new NVSettingsTile { Text = "Notifications", IsOn = true }),
            Gallery.Sample(13, "NVChipGroup", "Choice chips", new NVChipGroup { Items = new List<string> { "All", "New" } }),
            Gallery.Sample(14, "NVCheckList", "Multi-select rows", new NVCheckList { Items = new List<string> { "One", "Two" } }),
            Gallery.Sample(15, "NVGroupedList", "Sectioned rows", new NVGroupedList { Groups = new List<string> { "A", "B" } }),
            Gallery.Sample(16, "NVIndexBar", "A–Z jump list", new NVIndexBar()),
            Gallery.Sample(17, "NVSwipeTile", "Leading / trailing actions", new NVSwipeTile()),
            Gallery.Sample(18, "NVSelectionBar", "2 selected", new NVSelectionBar { Count = 2 }),
            Gallery.Sample(19, "NVSkeletonList", "Placeholder rows", new NVSkeletonList()),
            Gallery.Sample(20, "NVInfiniteFooter", "Loading more", new NVInfiniteFooter()),
            Gallery.Chapter("Overlays"),
            Gallery.Overlay(21, "NVDialog", "Confirm", new NVDialog { Title = "Confirm", Message = "Continue with Lumina?" }),
            Gallery.Overlay(22, "NVActionSheet", "Actions", new NVActionSheet()),
            Gallery.Sample(23, "NVMenu", "Overflow menu", new NVMenu { Text = "Menu", Items = new List<string> { "Edit", "Share" } })
        ]) { }

    static View FormSectionDemo()
    {
        var section = new NVFormSection { Title = "Profile" };
        section.Add(new NVTextField { Label = "Name", Text = "Ada" });
        return section;
    }
}

public sealed class AdvancedPage : CatalogSectionPage
{
    public AdvancedPage() : base("12  Advanced",
        "Product patterns after the basics: commerce, social, media chrome, then gates.",
        () =>
        [
            Gallery.Chapter("Fields and pickers"),
            Gallery.Sample(1, "NVEmailField", "Email", new NVEmailField()),
            Gallery.Sample(2, "NVPhoneField", "Phone mask", new NVPhoneField()),
            Gallery.Sample(3, "NVPasswordField", "Secret", new NVPasswordField()),
            Gallery.Sample(4, "NVPasswordStrength", "Score 4 / 4", new NVPasswordStrength { Password = "Longenough1!" }),
            Gallery.Sample(5, "NVQuantityStepper", "Qty stepper", new NVQuantityStepper()),
            Gallery.Sample(6, "NVDateRangePicker", "From / to", new NVDateRangePicker()),
            Gallery.Sample(7, "NVMonthYearPicker", "Month", new NVMonthYearPicker()),
            Gallery.Sample(8, "NVFilterBar", "Quick filters", new NVFilterBar()),
            Gallery.Sample(9, "NVTagInput", "Tags", new NVTagInput()),
            Gallery.Sample(10, "NVPinPad", "4-digit PIN", new NVPinPad()),
            Gallery.Sample(11, "NVCopyable", "Copy token", new NVCopyable { Text = "NUV-2048" }),
            Gallery.Sample(12, "NVLink", "Inline link", new NVLink { Text = "Learn more" }),
            Gallery.Sample(13, "NVCountryPicker", "Country", new NVCountryPicker()),
            Gallery.Sample(14, "NVLanguagePicker", "Language", new NVLanguagePicker()),
            Gallery.Sample(15, "NVThemePicker", "Light / dark / system", new NVThemePicker()),
            Gallery.Chapter("Commerce and booking"),
            Gallery.Sample(16, "NVCurrencyLabel", "Currency", new NVCurrencyLabel { Amount = 86 }),
            Gallery.Sample(17, "NVCountdown", "Timer", new NVCountdown { Seconds = 90 }),
            Gallery.Sample(18, "NVQuote", "Pull quote", new NVQuote { Text = "Warm paper, aurora accent." }),
            Gallery.Sample(19, "NVCodeBlock", "Mono snippet", new NVCodeBlock { Code = "UseNuvyntraUIKit();" }),
            Gallery.Sample(20, "NVBulletList", "Bullets", new NVBulletList { Items = new List<string> { "Tokens", "Controls", "Recipes" } }),
            Gallery.Sample(21, "NVStatCard", "KPI", new NVStatCard { Label = "Orders", Value = "128" }),
            Gallery.Sample(22, "NVTimeline", "Activity", new NVTimeline { Items = [new NVTimelineItem { Title = "Packed", Detail = "Warehouse" }] }),
            Gallery.Sample(23, "NVWizard", "Account → Address → Pay", new NVWizard()),
            Gallery.Sample(24, "NVStickyBar", "Sticky continue", new NVStickyBar { Text = "Continue" }),
            Gallery.Sample(25, "NVCartBar", "Sticky checkout", new NVCartBar { Total = 86 }),
            Gallery.Sample(26, "NVPriceTag", "Price", new NVPriceTag { Amount = 42 }),
            Gallery.Sample(27, "NVVariantPicker", "S / M / L", new NVVariantPicker()),
            Gallery.Sample(28, "NVCouponField", "Coupon", new NVCouponField()),
            Gallery.Sample(29, "NVTicket", "Pass + barcode", new NVTicket { Title = "Boarding pass", Code = "NUV-2048" }),
            Gallery.Sample(30, "NVTimeSlotPicker", "Slots", new NVTimeSlotPicker()),
            Gallery.Sample(31, "NVSeatPicker", "Seats", new NVSeatPicker()),
            Gallery.Chapter("Social"),
            Gallery.Sample(32, "NVProfileHeader", "Avatar + follow", new NVProfileHeader { Name = "Studio" }),
            Gallery.Sample(33, "NVFeedCard", "Post + reactions", new NVFeedCard { Title = "Ship", Body = "Left the hub." }),
            Gallery.Sample(34, "NVComposer", "Message + send", new NVComposer()),
            Gallery.Sample(35, "NVBubble", "Chat bubble", new NVBubble { Text = "Hello", IsMine = true }),
            Gallery.Sample(36, "NVTypingIndicator", "Typing…", new NVTypingIndicator()),
            Gallery.Sample(37, "NVStoryRing", "Story avatar", new NVStoryRing()),
            Gallery.Sample(38, "NVReactionBar", "Heart / chat / share", new NVReactionBar()),
            Gallery.Sample(39, "NVNotificationRow", "Bell row", new NVNotificationRow()),
            Gallery.Sample(40, "NVContactTile", "Presence row", new NVContactTile()),
            Gallery.Chapter("Media chrome"),
            Gallery.Sample(41, "NVImageGallery", "Thumb grid", new NVImageGallery()),
            Gallery.Overlay(42, "NVLightbox", "Image overlay", new NVLightbox()),
            Gallery.Sample(43, "NVVideoPlayer", "Host supplies decode", new NVVideoPlayer()),
            Gallery.Sample(44, "NVAudioPlayer", "Scrub + play", new NVAudioPlayer()),
            Gallery.Sample(45, "NVWebView", "https only", new NVWebView()),
            Gallery.Sample(46, "NVVoiceNote", "Mic + wave", new NVVoiceNote()),
            Gallery.Sample(47, "NVWaveform", "Bars", new NVWaveform()),
            Gallery.Sample(48, "NVBeforeAfter", "Compare", new NVBeforeAfter()),
            Gallery.Chapter("Gates"),
            Gallery.Sample(49, "NVMasterDetail", "Two-pane", new NVMasterDetail()),
            Gallery.Sample(50, "NVRetryView", "Couldn’t load", new NVRetryView()),
            Gallery.Sample(51, "NVOfflineBanner", "Offline", new NVOfflineBanner()),
            Gallery.Sample(52, "NVPermissionCard", "Host attaches PermissionFlow", new NVPermissionCard { Title = "Location" }),
            Gallery.Sample(53, "NVForceUpdate", "Store prompt", new NVForceUpdate()),
            Gallery.Sample(54, "NVLockPad", "PIN unlock", new NVLockPad()),
            Gallery.Sample(55, "NVBiometricGate", "Host attaches Biometric", new NVBiometricGate()),
            Gallery.Sample(56, "NVDashboardGrid", "KPI wrap", new NVDashboardGrid()),
            Gallery.Sample(57, "NVGantt", "Plan timeline", new NVGantt()),
            Gallery.Sample(58, "NVOrgChart", "Tree", new NVOrgChart())
        ]) { }
}

public sealed class RecipesPage : CatalogSectionPage
{
    public RecipesPage() : base("13  Pages",
        "66 recipes in catalog order. Auth → commerce → content → social → files → system → next.",
        Build) { }

    static IEnumerable<View> Build()
    {
        var groups = new (string Title, int Start, int Count, int? IdStart)[]
        {
            ("1. Auth and onboarding", 0, 8, 1),
            ("2. Commerce", 8, 12, 9),
            ("3. Content and about", 20, 8, 21),
            ("4. Chat, social, profile", 28, 7, 29),
            ("5. Lists, files, media", 35, 6, 36),
            ("6. Empty, error, settings", 41, 8, 42),
            ("7. Product extras", 49, 8, null),
            ("8. Next (1.2)", 57, 6, 50),
            ("9. Next (1.3)", 63, 3, 56)
        };

        var pages = NVCatalog.Pages;
        foreach (var group in groups)
        {
            yield return Gallery.Chapter(group.Title);
            for (var i = 0; i < group.Count; i++)
            {
                var index = group.Start + i;
                var type = pages[index];
                var demo = (View)(Activator.CreateInstance(type) ?? new NVAboutView());
                var note = group.IdStart is int id
                    ? $"NV-PG-{(id + i):00}"
                    : type.Name;
                yield return Gallery.Sample(index + 1, type.Name, note,
                    new NVExpander { Title = "Open recipe", IsExpanded = index == 0, Panel = demo });
            }
        }
    }
}

public sealed class NextPage : CatalogSectionPage
{
    public NextPage() : base("14  Next",
        "1.2 / 1.3 app chrome, data, media, and host slots.",
        () =>
        [
            Gallery.Chapter("App chrome"),
            Gallery.Overlay(1, "NVCommandPalette", "NV-APP-01  ·  Filter + recents", new NVCommandPalette
            {
                Recents = [new NVCommandItem { Title = "Toggle theme" }],
                Commands = [new NVCommandItem { Title = "Open file" }, new NVCommandItem { Title = "Toggle theme" }]
            }),
            Gallery.Overlay(2, "NVCoachMark", "NV-APP-02  ·  Spotlight step", new NVCoachMark
            {
                Steps = [new NVCoachStep { Title = "Palette", Body = "Press ⌘K" }]
            }),
            Gallery.Overlay(3, "NVContextMenu", "NV-APP-03  ·  Actions", new NVContextMenu
            {
                Items = [new NVMenuAction { Text = "Share" }, new NVMenuAction { Text = "Delete" }]
            }),
            Gallery.Sample(4, "NVFileDrop", "NV-APP-05  ·  Attach chips", FileDropDemo()),
            Gallery.Overlay(5, "NVWhatsNew", "NV-APP-06  ·  Version notes", new NVWhatsNew
            {
                VersionTitle = "1.2.0",
                Items = new List<string> { "Command palette", "Coach marks" }
            }),
            Gallery.Sample(6, "NVConsentBanner", "NV-APP-07  ·  Accept / manage", new NVConsentBanner()),
            Gallery.Overlay(7, "NVPaywall", "NV-APP-08  ·  Plan gate", new NVPaywall
            {
                Title = "Lumina Plus",
                Message = "Unlock coach marks",
                Plans = new NVCard { Title = "Monthly", Body = "$8" }
            }),
            Gallery.Chapter("Viz"),
            Gallery.Sample(8, "NVHeatCalendar", "NV-VIZ-08  ·  September habit cells", new NVHeatCalendar
            {
                Month = new DateTime(2026, 9, 1),
                Values =
                [
                    new NVHeatDay { Date = new DateTime(2026, 9, 3), Value = 2 },
                    new NVHeatDay { Date = new DateTime(2026, 9, 8), Value = 5 },
                    new NVHeatDay { Date = new DateTime(2026, 9, 16), Value = 3 }
                ]
            }),
            Gallery.Chapter("App chrome 1.3"),
            Gallery.Sample(9, "NVSpeedDial", "NV-APP-04  ·  FAB fan-out", new NVSpeedDial
            {
                Actions =
                [
                    new NVSpeedDialAction { Text = "Compose" },
                    new NVSpeedDialAction { Text = "Scan" }
                ]
            }),
            Gallery.Sample(10, "NVSubscriptionCard", "NV-APP-09  ·  Plan tile", new NVSubscriptionCard
            {
                Name = "Yearly",
                Price = "$72",
                Features = new List<string> { "Palette", "Coach", "Heat map" }
            }),
            Gallery.Sample(11, "NVEmojiPicker", "NV-APP-10  ·  Glyph grid", new NVEmojiPicker()),
            Gallery.Chapter("Data plus"),
            Gallery.Sample(12, "NVPivotGrid", "NV-DAT-08  ·  Q1 / Q2", new NVPivotGrid
            {
                Facts =
                [
                    new NVPivotFact { Row = "Ada", Column = "Q1", Value = 4 },
                    new NVPivotFact { Row = "Lin", Column = "Q2", Value = 7 }
                ]
            }),
            Gallery.Sample(13, "NVPropertyGrid", "NV-DAT-09  ·  Inspect", new NVPropertyGrid
            {
                Items = [new NVPropertyItem { Name = "Title", Value = "Lumina", Kind = NVFormFieldKind.Text }]
            }),
            Gallery.Sample(14, "NVJsonTree", "NV-DAT-10  ·  Expand JSON", new NVJsonTree { Json = """{"kit":"Lumina","ok":true}""" }),
            Gallery.Chapter("Media plus"),
            Gallery.Sample(15, "NVDiffView", "NV-MED-11  ·  Unified", new NVDiffView { Left = "paper", Right = "aurora" }),
            Gallery.Sample(16, "NVCodeEditor", "NV-MED-12  ·  Editable", new NVCodeEditor { Text = "UseNuvyntraUIKit();" }),
            Gallery.Chapter("Host chrome"),
            Gallery.Sample(17, "NVCallBar", "NV-HST-01  ·  Mute / end", new NVCallBar()),
            Gallery.Sample(18, "NVInCallView", "NV-HST-02  ·  Full chrome", new NVInCallView { Name = "Ada", Elapsed = "00:42" }),
            Gallery.Sample(19, "NVSyncConflictCard", "NV-HST-03  ·  Local vs remote", new NVSyncConflictCard { Local = "Draft", Remote = "Cloud" }),
            Gallery.Sample(20, "NVUploadTile", "NV-HST-04  ·  Retry", new NVUploadTile { FileName = "brief.pdf", Bytes = 2048 }),
            Gallery.Overlay(21, "NVDeviceSheet", "NV-HST-05  ·  Nearby devices", new NVDeviceSheet
            {
                Devices = [new NVDeviceItem { Name = "Studio buds" }]
            }),
            Gallery.Sample(22, "NVPrintPreview", "NV-HST-06  ·  Page slot", new NVPrintPreview { Page = new NVCaptionText { Text = "Invoice" } }),
            Gallery.Sample(23, "NVNfcPrompt", "NV-HST-07  ·  Hold near", new NVNfcPrompt()),
            Gallery.Overlay(24, "NVReviewPrompt", "NV-HST-08  ·  Stars + review", new NVReviewPrompt { Rating = 4 })
        ]) { }

    static View FileDropDemo()
    {
        var drop = new NVFileDrop();
        drop.Attach(new NVFileChip { Name = "brief.pdf", Size = 2048 });
        return drop;
    }
}
