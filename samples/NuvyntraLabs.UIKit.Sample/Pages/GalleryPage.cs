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
            Gallery.Sample(7, "NVEffects", "NV-PRI-07  ·  Press host", new NVEffects()),
            Gallery.Sample(8, "NVElevation", "NV-PRI-08  ·  Applied on the surface above", new NVCaptionText { Text = "See NVSurface elevation 1" }),
            Gallery.Sample(9, "NVOverlay", "NV-PRI-09  ·  Scrim host (closed)", new NVOverlay { IsOpen = false }),
            Gallery.Sample(10, "NVInteractiveViewer", "NV-PRI-10  ·  Zoom / pan", new NVInteractiveViewer { Viewport = new NVCaptionText { Text = "Viewport" } }),
            Gallery.Sample(11, "NVSpacer", "NV-PRI-11  ·  Token gap", new NVSpacer()),
            Gallery.Sample(12, "NVHighlight", "NV-PRI-12  ·  Mention span", new NVHighlight { Text = "aurora" })
        ]) { }
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
            Gallery.Sample(5, "NVPullToRefresh", "NV-FBK-05  ·  RefreshView chrome", new NVPullToRefresh()),
            Gallery.Sample(6, "NVPopup", "NV-FBK-06  ·  Closed overlay", new NVPopup { IsOpen = false }),
            Gallery.Sample(7, "NVToast", "NV-FBK-07  ·  Use NVToast.ShowAsync from a button", new NVCaptionText { Text = "Timed alert helper" }),
            Gallery.Sample(8, "NVBanner", "NV-FBK-08  ·  Success tone", new NVBanner { Text = "Lumina is ready", Tone = NVBannerTone.Success }),
            Gallery.Sample(9, "NVEmptyView", "NV-FBK-09  ·  Empty reason", new NVEmptyView { Title = "Nothing yet", Reason = NVStatusReason.Empty }),
            Gallery.Sample(10, "NVTooltip", "NV-FBK-10  ·  Hint", new NVTooltip { Text = "Hint" })
        ]) { }
}

public sealed class LayoutPage : CatalogSectionPage
{
    public LayoutPage() : base("07  Layout",
        "Structure and navigation. NV-LAY-01 … NV-LAY-16.",
        () =>
        [
            Gallery.Sample(1, "NVCard", "NV-LAY-01", new NVCard { Title = "Card", Body = "Warm paper" }),
            Gallery.Sample(2, "NVAccordion", "NV-LAY-02", new NVAccordion { Title = "Accordion" }),
            Gallery.Sample(3, "NVExpander", "NV-LAY-03", new NVExpander { Title = "Expander" }),
            Gallery.Sample(4, "NVTabView", "NV-LAY-04", new NVTabView { Tabs = new List<string> { "One", "Two" } }),
            Gallery.Sample(5, "NVBottomSheet", "NV-LAY-05  ·  Closed", new NVBottomSheet { IsOpen = false }),
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
            Gallery.Sample(1, "NVCollectionView", "NV-DAT-01", new NVCollectionView { Items = [new NVListItem { Title = "Row", Subtitle = "Lumina" }] }),
            Gallery.Sample(2, "NVDataGrid", "NV-DAT-02", new NVDataGrid
            {
                Columns = [new NVGridColumn { Header = "Name", Binding = "name" }],
                Rows = [new Dictionary<string, object?> { ["name"] = "Aurora" }]
            }),
            Gallery.Sample(3, "NVTreeDataGrid", "NV-DAT-03", new NVTreeDataGrid
            {
                Columns = [new NVGridColumn { Header = "Node", Binding = "name" }],
                Rows = [new Dictionary<string, object?> { ["name"] = "Root" }]
            }),
            Gallery.Sample(4, "NVTreeView", "NV-DAT-04", new NVTreeView { Roots = [new NVTreeNode { Title = "Root", IsExpanded = true }] }),
            Gallery.Sample(5, "NVDataForm", "NV-DAT-05", new NVDataForm { Fields = [NVFormField.For("Title", typeof(string), "Kit")] }),
            Gallery.Sample(6, "NVDataPager", "NV-DAT-06  ·  42 items, page size 10", new NVDataPager { TotalCount = 42, PageSize = 10 }),
            Gallery.Sample(7, "NVKanban", "NV-DAT-07", new NVKanban { Columns = [new NVKanbanColumn { Title = "Todo" }] })
        ]) { }
}

public sealed class ChartsPage : CatalogSectionPage
{
    public ChartsPage() : base("09  Charts",
        "Visualization. NV-VIZ-01 … NV-VIZ-07.",
        () =>
        [
            Gallery.Sample(1, "NVChart", "NV-VIZ-01  ·  Bar series", new NVChart
            {
                Series =
                [
                    new NVChartSeries
                    {
                        Title = "Week",
                        Kind = NVChartSeriesKind.Bar,
                        Points = [new NVChartPoint { Category = "Mon", Value = 3 }, new NVChartPoint { Category = "Tue", Value = 8 }]
                    }
                ]
            }),
            Gallery.Sample(2, "NVRadialGauge", "NV-VIZ-02", new NVRadialGauge { Value = 70 }),
            Gallery.Sample(3, "NVLinearGauge", "NV-VIZ-03", new NVLinearGauge()),
            Gallery.Sample(4, "NVDigitalGauge", "NV-VIZ-04", new NVDigitalGauge { Value = 128 }),
            Gallery.Sample(5, "NVMap", "NV-VIZ-05  ·  Host supplies tiles", new NVMap { Place = "Aurora" }),
            Gallery.Sample(6, "NVBarcode", "NV-VIZ-06  ·  Generate only", new NVBarcode { Value = "NUVEXA" }),
            Gallery.Sample(7, "NVTreeMap", "NV-VIZ-07", new NVTreeMap { Nodes = [new NVTreeMapNode { Title = "A", Value = 8 }, new NVTreeMapNode { Title = "B", Value = 3 }] })
        ]) { }
}

public sealed class CalendarPage : CatalogSectionPage
{
    public CalendarPage() : base("10  Calendar",
        "Dates and appointments. NV-CAL-01 … NV-CAL-02.",
        () =>
        [
            Gallery.Sample(1, "NVCalendar", "NV-CAL-01  ·  Month grid", new NVCalendar()),
            Gallery.Sample(2, "NVScheduler", "NV-CAL-02  ·  Agenda", new NVScheduler
            {
                Appointments =
                [
                    new NVAppointment { Title = "Standup", Start = DateTime.Today.AddHours(9), End = DateTime.Today.AddHours(9.5) }
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
            Gallery.Sample(1, "NVImageEditor", "NV-MED-01", new NVImageEditor { Caption = "Edit" }),
            Gallery.Sample(2, "NVRichTextEditor", "NV-MED-02", new NVRichTextEditor()),
            Gallery.Sample(3, "NVMarkdownViewer", "NV-MED-03", new NVMarkdownViewer { Markdown = "# Lumina" }),
            Gallery.Sample(4, "NVPdfViewer", "NV-MED-04  ·  Viewer, not an engine", new NVPdfViewer { Title = "Spec.pdf" }),
            Gallery.Sample(5, "NVChat", "NV-MED-05", new NVChat { Messages = [new NVChatMessage { Author = "Lumina", Text = "Hello" }] }),
            Gallery.Sample(6, "NVAIPrompt", "NV-MED-06", new NVAIPrompt()),
            Gallery.Sample(7, "NVSmartPasteButton", "NV-MED-07", new NVSmartPasteButton()),
            Gallery.Sample(8, "NVDocxViewer", "NV-MED-08  ·  Viewer", new NVDocxViewer()),
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
            Gallery.Chapter("Overlays"),
            Gallery.Sample(15, "NVDialog", "Confirm (closed)", new NVDialog { Title = "Confirm", Message = "Continue?", IsOpen = false }),
            Gallery.Sample(16, "NVActionSheet", "Sheet (closed)", new NVActionSheet { IsOpen = false }),
            Gallery.Sample(17, "NVMenu", "Overflow menu", new NVMenu { Text = "Menu", Items = new List<string> { "Edit", "Share" } })
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
            Gallery.Sample(5, "NVDateRangePicker", "From / to", new NVDateRangePicker()),
            Gallery.Sample(6, "NVFilterBar", "Quick filters", new NVFilterBar()),
            Gallery.Sample(7, "NVPinPad", "4-digit PIN", new NVPinPad()),
            Gallery.Chapter("Commerce and booking"),
            Gallery.Sample(8, "NVStatCard", "KPI", new NVStatCard { Label = "Orders", Value = "128" }),
            Gallery.Sample(9, "NVTimeline", "Activity", new NVTimeline { Items = [new NVTimelineItem { Title = "Packed", Detail = "Warehouse" }] }),
            Gallery.Sample(10, "NVWizard", "Account → Address → Pay", new NVWizard()),
            Gallery.Sample(11, "NVCartBar", "Sticky checkout", new NVCartBar { Total = 86 }),
            Gallery.Sample(12, "NVTicket", "Pass + barcode", new NVTicket { Title = "Boarding pass", Code = "NUV-2048" }),
            Gallery.Sample(13, "NVTimeSlotPicker", "Slots", new NVTimeSlotPicker()),
            Gallery.Sample(14, "NVSeatPicker", "Seats", new NVSeatPicker()),
            Gallery.Chapter("Social"),
            Gallery.Sample(15, "NVProfileHeader", "Avatar + follow", new NVProfileHeader { Name = "Studio" }),
            Gallery.Sample(16, "NVFeedCard", "Post + reactions", new NVFeedCard { Title = "Ship", Body = "Left the hub." }),
            Gallery.Sample(17, "NVComposer", "Message + send", new NVComposer()),
            Gallery.Sample(18, "NVBubble", "Chat bubble", new NVBubble { Text = "Hello", IsMine = true }),
            Gallery.Chapter("Media chrome"),
            Gallery.Sample(19, "NVImageGallery", "Thumb grid", new NVImageGallery()),
            Gallery.Sample(20, "NVVideoPlayer", "Host supplies decode", new NVVideoPlayer()),
            Gallery.Sample(21, "NVAudioPlayer", "Scrub + play", new NVAudioPlayer()),
            Gallery.Sample(22, "NVWebView", "https only", new NVWebView()),
            Gallery.Chapter("Gates"),
            Gallery.Sample(23, "NVMasterDetail", "Two-pane", new NVMasterDetail()),
            Gallery.Sample(24, "NVLockPad", "PIN unlock", new NVLockPad()),
            Gallery.Sample(25, "NVBiometricGate", "Host attaches Biometric", new NVBiometricGate()),
            Gallery.Sample(26, "NVGantt", "Plan timeline", new NVGantt()),
            Gallery.Sample(27, "NVOrgChart", "Tree", new NVOrgChart())
        ]) { }
}

public sealed class RecipesPage : CatalogSectionPage
{
    public RecipesPage() : base("13  Pages",
        "57 recipes in catalog order. Auth → commerce → content → social → files → system.",
        Build) { }

    static IEnumerable<View> Build()
    {
        var groups = new (string Title, int Start, int Count)[]
        {
            ("1. Auth and onboarding", 0, 8),
            ("2. Commerce", 8, 12),
            ("3. Content and about", 20, 8),
            ("4. Chat, social, profile", 28, 7),
            ("5. Lists, files, media", 35, 6),
            ("6. Empty, error, settings", 41, 8),
            ("7. Product extras", 49, 8)
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
                yield return Gallery.Sample(index + 1, type.Name, $"NV-PG-{(index + 1):00}", demo);
            }
        }
    }
}
