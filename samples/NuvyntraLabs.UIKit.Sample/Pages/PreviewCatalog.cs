namespace NuvyntraLabs.UIKit.Sample.Pages;

/// <summary>Every catalog type as a demo view. Gallery pages compose these; capture exports them.</summary>
static class PreviewCatalog
{
    public static IReadOnlyList<(string Name, View Demo)> All()
    {
        var items = new List<(string, View)>();
        foreach (var type in NVCatalog.Foundation)
        {
            items.Add((type.Name, FoundationDemo(type.Name)));
        }

        foreach (var type in NVCatalog.Controls)
        {
            if (!typeof(View).IsAssignableFrom(type) || type.IsAbstract)
            {
                items.Add((type.Name, Caption(type.Name == nameof(NVElevation)
                    ? "Applied on NVSurface elevation"
                    : "Helper — not a view")));
                continue;
            }

            try
            {
                if (Activator.CreateInstance(type) is View view)
                {
                    items.Add((type.Name, Seed(type.Name, view)));
                }
            }
            catch (Exception ex)
            {
                items.Add((type.Name, Caption(ex.GetBaseException().Message)));
            }
        }

        items.Add((nameof(NVRadioGroup), new NVRadioGroup
        {
            GroupName = "plan",
            Children =
            {
                new NVRadioButton { Text = "Monthly", IsChecked = true },
                new NVRadioButton { Text = "Yearly" }
            }
        }));
        items.Add((nameof(NVFormField), new NVDataForm { Fields = [NVFormField.For("Title", typeof(string), "Kit")] }));
        items.Add((nameof(NVToast), Caption("Use NVToast.ShowAsync from a button")));

        foreach (var type in NVCatalog.Pages)
        {
            try
            {
                if (Activator.CreateInstance(type) is View view)
                {
                    items.Add((type.Name, view));
                }
            }
            catch (Exception ex)
            {
                items.Add((type.Name, Caption(ex.GetBaseException().Message)));
            }
        }

        return items;
    }

    static View Seed(string name, View view)
    {
        switch (view)
        {
            case NVButton button when string.IsNullOrWhiteSpace(button.Text) && button is not NVIconButton:
                button.Text = "Continue";
                break;
            case NVHeading heading when string.IsNullOrWhiteSpace(heading.Text):
                heading.Text = heading is NVCaptionText
                    ? "Muted supporting line"
                    : heading is NVBodyText
                        ? "One kit for a typical mobile screen."
                        : "Lumina";
                break;
            case NVCard card when string.IsNullOrWhiteSpace(card.Title):
                card.Title = "Card";
                card.Body = "Warm paper";
                break;
            case NVTextField field when string.IsNullOrWhiteSpace(field.Text) && string.IsNullOrWhiteSpace(field.Placeholder):
                field.Label = string.IsNullOrWhiteSpace(field.Label) ? "Label" : field.Label;
                field.Placeholder = "Type here";
                break;
            case NVChip chip when string.IsNullOrWhiteSpace(chip.Text):
                chip.Text = "Filter";
                break;
            case NVBadge badge when string.IsNullOrWhiteSpace(badge.Text) && !badge.Dot:
                badge.Text = "3";
                break;
            case NVCheckBox check when string.IsNullOrWhiteSpace(check.Text):
                check.Text = "Accept terms";
                check.IsChecked = true;
                break;
            case NVBanner banner when string.IsNullOrWhiteSpace(banner.Text):
                banner.Text = "Lumina is ready";
                banner.Tone = NVBannerTone.Success;
                break;
            case NVCopyable copy when string.IsNullOrWhiteSpace(copy.Text):
                copy.Text = "NUV-2048";
                break;
            case NVQuote quote when string.IsNullOrWhiteSpace(quote.Text):
                quote.Text = "Warm paper, aurora accent.";
                break;
            case NVBubble bubble when string.IsNullOrWhiteSpace(bubble.Text):
                bubble.Text = "Hello";
                bubble.IsMine = true;
                break;
            case NVSelectionBar bar when bar.Count == 0:
                bar.Count = 2;
                break;
            case NVCurrencyLabel money when money.Amount == 0 && name != nameof(NVPriceTag):
                money.Amount = 86;
                break;
            case NVPriceTag price when price.Amount == 0:
                price.Amount = 42;
                break;
            case NVCartBar cart when cart.Total == 0:
                cart.Total = 86;
                break;
            case NVProfileHeader profile when profile.Name == "Niladri":
                profile.Name = "Studio";
                break;
            case NVFileDrop drop when drop.Files.Count == 0:
                drop.Attach(new NVFileChip { Name = "brief.pdf", Size = 2048 });
                break;
            case NVHeatCalendar heat when heat.Values.Count == 0:
                heat.Month = new DateTime(2026, 9, 1);
                heat.Values = [new NVHeatDay { Date = new DateTime(2026, 9, 3), Value = 4 }];
                break;
            case NVConsentBanner consent when string.IsNullOrWhiteSpace(consent.Text):
                consent.Text = "We use cookies to keep Lumina useful.";
                break;
            case NVJsonTree json when string.IsNullOrWhiteSpace(json.Json):
                json.Json = """{"kit":"Lumina"}""";
                break;
            case NVUploadTile upload when upload.Bytes == 0:
                upload.FileName = "brief.pdf";
                upload.Bytes = 2048;
                break;
            case NVKanban board when board.Columns.Count == 0:
                var todo = new NVKanbanColumn { Title = "Todo" };
                todo.Cards.Add(new NVListItem { Title = "Tokens" });
                board.Columns = [todo];
                break;
            case NVChart chart when chart.IsEmpty:
                chart.Series =
                [
                    new NVChartSeries
                    {
                        Title = "Week",
                        Kind = NVChartSeriesKind.Bar,
                        Points = [new NVChartPoint { Category = "Mon", Value = 3 }, new NVChartPoint { Category = "Tue", Value = 8 }]
                    }
                ];
                break;
            case NVTreeView tree when (tree.Roots?.Count ?? 0) == 0:
                tree.Roots = [new NVTreeNode { Title = "Root", IsExpanded = true, Children = { new NVTreeNode { Title = "Leaf" } } }];
                break;
            case NVPdfViewer pdf when (pdf.Pages?.Count ?? 0) == 0:
                pdf.Pages = ["Lumina spec", "Tokens and density"];
                break;
            case OverlayHost overlay when !overlay.IsOpen:
                overlay.IsOpen = true;
                break;
        }

        return view;
    }

    static View FoundationDemo(string name) => name switch
    {
        nameof(NVTheme) => new HorizontalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children =
            {
                new NVButton { Text = "Light", Variant = NVButtonVariant.Outline },
                new NVButton { Text = "Dark", Variant = NVButtonVariant.Outline },
                new NVButton { Text = "System", Variant = NVButtonVariant.Tonal }
            }
        },
        nameof(NVTokens) => new HorizontalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children =
            {
                Swatch(NVTheme.Current.Paper),
                Swatch(NVTheme.Current.Surface),
                Swatch(NVTheme.Current.Accent),
                Swatch(NVTheme.Current.Ink),
                Swatch(NVTheme.Current.Danger)
            }
        },
        nameof(NVTypography) => new VerticalStackLayout
        {
            Spacing = NVTokens.Space1,
            Children =
            {
                new NVHeading { Text = "Display", Role = NVTextRole.Display },
                new NVHeading { Text = "Title", Role = NVTextRole.Title },
                new NVBodyText { Text = "Body" },
                new NVCaptionText { Text = "Caption" }
            }
        },
        nameof(NVIcons) => new HorizontalStackLayout
        {
            Spacing = NVTokens.Space3,
            Children =
            {
                new NVIcon { Kind = NVIconKind.Home },
                new NVIcon { Kind = NVIconKind.Search },
                new NVIcon { Kind = NVIconKind.Settings },
                new NVIcon { Kind = NVIconKind.Star }
            }
        },
        nameof(NVMotion) => Caption($"Normal duration {NVMotion.Normal} ms"),
        nameof(NVDensity) => Caption(NVTheme.Current.Density.ToString()),
        nameof(NVVisualState) => Caption(string.Join(" · ", new[] { NVVisualState.Rest, NVVisualState.Press, NVVisualState.Error })),
        nameof(NVAccessibility) => Caption(NVAccessibility.BodyContrastOk(NVTheme.Current.Ink, NVTheme.Current.Paper) ? "Pass ≥ 4.5:1" : "Fail"),
        _ => Caption(name)
    };

    static View Caption(string text) => new NVCaptionText { Text = text };

    static View Swatch(Color color) => new BoxView
    {
        Color = color,
        WidthRequest = 36,
        HeightRequest = 36,
        CornerRadius = 8
    };
}
