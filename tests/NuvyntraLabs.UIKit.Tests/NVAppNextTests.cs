namespace NuvyntraLabs.UIKit.Tests;

[Collection("UIKit")]
public class NVAppNextTests
{
    public static TheoryData<Type> NextControls =>
    [
        typeof(NVCommandPalette),
        typeof(NVCoachMark),
        typeof(NVContextMenu),
        typeof(NVFileDrop),
        typeof(NVPaywall),
        typeof(NVWhatsNew),
        typeof(NVConsentBanner),
        typeof(NVHeatCalendar)
    ];

    [Theory]
    [MemberData(nameof(NextControls))]
    public void Default_ctor_does_not_throw(Type type)
    {
        var view = (View)Activator.CreateInstance(type)!;
        Assert.NotNull(view);
        NVAccessibility.Name(view, type.Name, "Gallery");
        Assert.Equal(type.Name, AutomationProperties.GetName(view));
    }

    [Theory]
    [MemberData(nameof(NextControls))]
    public void Disabled_and_theme_and_unload(Type type)
    {
        var view = (View)Activator.CreateInstance(type)!;
        view.IsEnabled = false;
        Assert.False(view.IsEnabled);
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Ink, NVTheme.Current.Paper));
        view.IsEnabled = true;
        view.Unloaded += (_, _) => { };
    }

    [Fact]
    public void Palette_filter_is_case_insensitive_and_empty_query_shows_recents()
    {
        var open = new NVCommandItem { Title = "Open file" };
        var theme = new NVCommandItem { Title = "Toggle theme" };
        var recents = new List<NVCommandItem> { theme };

        Assert.Equal(recents, NVCommandPalette.Filter([open, theme], recents, ""));
        Assert.Equal(recents, NVCommandPalette.Filter([open, theme], recents, "   "));
        var hits = NVCommandPalette.Filter([open, theme], recents, "OPEN");
        Assert.Single(hits);
        Assert.Equal("Open file", hits[0].Title);

        var palette = new NVCommandPalette { Commands = [open, theme], Recents = recents, Query = "" };
        Assert.Equal(theme.Title, Assert.Single(palette.VisibleCommands).Title);
        palette.Query = "th";
        Assert.Equal(theme.Title, Assert.Single(palette.VisibleCommands).Title);
    }

    [Fact]
    public void Coach_mark_next_advances_and_last_step_raises_completed()
    {
        var completed = 0;
        var coach = new NVCoachMark
        {
            Steps =
            [
                new NVCoachStep { Title = "One", Body = "Palette" },
                new NVCoachStep { Title = "Two", Body = "Done" }
            ]
        };
        coach.Completed += (_, _) => completed++;
        coach.IsOpen = true;
        Assert.Equal("One", coach.CurrentStep?.Title);
        coach.Next();
        Assert.Equal(1, coach.Index);
        Assert.Equal("Two", coach.CurrentStep?.Title);
        coach.Next();
        Assert.Equal(1, completed);
        Assert.False(coach.IsOpen);
        coach.Next();
        Assert.Equal(2, completed);
    }

    [Fact]
    public void Context_menu_opens_on_command_and_item_command_fires_once()
    {
        var fired = 0;
        var item = new NVMenuAction
        {
            Text = "Share",
            Command = new Command(() => fired++)
        };
        var menu = new NVContextMenu { Items = [item] };
        Assert.False(menu.IsOpen);
        Assert.True(menu.OpenCommand.CanExecute(null));
        menu.OpenCommand.Execute(null);
        Assert.True(menu.IsOpen);

        var button = Assert.IsType<NVButton>(Assert.Single(((VerticalStackLayout)menu.PanelContent!).Children));
        button.Command!.Execute(null);
        button.Command.Execute(null);
        Assert.Equal(1, fired);
        Assert.False(menu.IsOpen);
    }

    [Fact]
    public void File_drop_ignores_empty_and_chips_bind_name_size()
    {
        var drop = new NVFileDrop();
        drop.Attach(null);
        drop.Attach(new NVFileChip { Name = "", Size = 12 });
        Assert.Empty(drop.Files);

        drop.Attach(new NVFileChip { Name = "brief.pdf", Size = 2048 });
        var chip = Assert.Single(drop.Files);
        Assert.Equal("brief.pdf", chip.Name);
        Assert.Equal(2048, chip.Size);
        Assert.Equal("2 KB", NVFileDrop.FormatSize(2048));

        drop.Files = [new NVFileChip { Name = "" }, new NVFileChip { Name = "shot.png", Size = 512 }];
        Assert.Equal("shot.png", Assert.Single(drop.Files).Name);
    }

    [Fact]
    public void Paywall_dismiss_is_noop_when_blocking()
    {
        var wall = new NVPaywall { IsOpen = true, IsBlocking = true, Title = "Plus", Message = "Unlock" };
        wall.Dismiss();
        Assert.True(wall.IsOpen);
        Assert.False(wall.DismissOnScrim);
        wall.IsBlocking = false;
        wall.Dismiss();
        Assert.False(wall.IsOpen);
    }

    [Fact]
    public void Consent_accept_sets_is_accepted()
    {
        var accepted = 0;
        var banner = new NVConsentBanner
        {
            Text = "Privacy copy",
            AcceptCommand = new Command(() => accepted++)
        };
        Assert.False(banner.IsAccepted);
        banner.Accept();
        Assert.True(banner.IsAccepted);
        Assert.Equal(1, accepted);
        Assert.False(banner.IsVisible);
    }

    [Fact]
    public void Heat_calendar_cells_match_month_and_missing_values_are_empty()
    {
        var month = new DateTime(2026, 9, 1);
        var heat = new NVHeatCalendar
        {
            Month = month,
            Values = [new NVHeatDay { Date = new DateTime(2026, 9, 3), Value = 4 }]
        };
        Assert.Equal(30, heat.DayCount);
        Assert.Equal(30, heat.DayCells.Count);
        Assert.Equal(4, heat.ValueFor(new DateTime(2026, 9, 3)));
        Assert.Null(heat.ValueFor(new DateTime(2026, 9, 4)));
        Assert.Null(NVHeatCalendar.Find([], new DateTime(2026, 9, 1)));
    }

    [Fact]
    public void Commands_honor_can_execute()
    {
        var can = false;
        var gated = new Command(() => { }, () => can);
        var palette = new NVCommandPalette
        {
            Commands = [new NVCommandItem { Title = "Gated", Command = gated }],
            Query = "gat"
        };
        var paletteButton = Assert.IsType<NVButton>(Assert.Single(((VerticalStackLayout)((VerticalStackLayout)palette.PanelContent!).Children[1]).Children));
        Assert.False(paletteButton.Command!.CanExecute(null));

        var menu = new NVContextMenu { Items = [new NVMenuAction { Text = "Gated", Command = gated }] };
        var menuButton = Assert.IsType<NVButton>(Assert.Single(((VerticalStackLayout)menu.PanelContent!).Children));
        Assert.False(menuButton.Command!.CanExecute(null));

        var drop = new NVFileDrop { PickCommand = gated };
        drop.IsEnabled = false;
        Assert.False(drop.IsEnabled);
    }
}
