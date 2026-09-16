# NuvyntraLabs.UIKit — control reference

[![NuGet](https://img.shields.io/nuget/v/NuvyntraLabs.UIKit.svg?label=NuGet)](https://www.nuget.org/packages/NuvyntraLabs.UIKit)

- NuGet: https://www.nuget.org/packages/NuvyntraLabs.UIKit
- GitHub: https://github.com/nuvyntralabs/NuvyntraLabs.UIKit
- Docs: https://nuvyntralabs.github.io/packages/nuvyntralabs-uikit/
- Catalog: [MauiEssentials](https://github.com/nuvyntralabs/MauiEssentials)

Package: `NuvyntraLabs.UIKit` · Prefix: `NV` · Version: `1.4.0`

Human + agent overview: [README.md](README.md). IDs: [nuvyntralabs-uikit-components.md](https://github.com/nuvyntralabs/MauiEssentials/blob/main/docs/plans/nuvyntralabs-uikit-components.md).

```xml
xmlns:nv="http://nuvyntralabs.com/uikit"
```

```csharp
builder.UseMauiApp<App>().UseNuvyntraUIKit();
```

Every `NV*` view also inherits MAUI `ContentView` / `VisualElement` members (`IsVisible`, `IsEnabled`, `Margin`, `HorizontalOptions`, `BindingContext`, …). Those are **framework** properties and are not repeated below.

**custom** = a property invented for this kit (Lumina API). Unmarked names match familiar MAUI / forms usage (`Text`, `Command`, `IsChecked`, `Items`).

Enums used as property types:

| Enum | Values |
| --- | --- |
| `NVButtonVariant` | `Filled`, `Tonal`, `Outline`, `Ghost`, `Danger` |
| `NVIconKind` | `None`, `Check`, `Close`, `ChevronDown`, `ChevronRight`, `Search`, `Home`, `Menu`, `Settings`, `User`, `Cart`, `Heart`, `Star`, `Bell`, `Calendar`, `Chat`, `Image`, `File`, `Plus`, `Minus`, `Warning`, `Info`, `Mic`, `Play`, `Pause`, `Lock`, `Share`, `Mail`, `Phone`, `Globe`, `Link`, `Camera` |
| `NVChipKind` | `Filter`, `Input`, `Assist`, `Choice` |
| `NVBannerTone` | `Info`, `Success`, `Warning`, `Danger` |
| `NVStatusReason` | `Empty`, `Offline`, `EmptyCart`, `NoPhotos`, `NoVideos`, `NoTasks`, `LocationDenied`, `PaymentFailed`, `NoCredits`, `Generic` |
| `NVTextRole` | `Display`, `Title`, `Body`, `Caption` |
| `NVLayoutMode` | `List`, `Tile`, `Card` |
| `NVSelectionKind` | `None`, `Single`, `Multiple` |
| `NVSortDirection` | `None`, `Ascending`, `Descending` |
| `NVFormFieldKind` | `Text`, `Number`, `Boolean`, `Date`, `Enum` |
| `NVChartSeriesKind` | `Line`, `Spline`, `Area`, `Bar`, `Column`, `Pie`, `Donut`, `Scatter`, `Bubble`, `Candle`, `Ohlc`, `Funnel`, `Pyramid`, `Polar`, `Radar`, `Sunburst`, `Spark` |
| `NVBarcodeFormat` | `Code128`, `Qr` |
| `NVThemeMode` | `Light`, `Dark`, `System` |
| `NVDensity` | `Compact`, `Comfortable`, `Spacious` |
| `OverlayPlacement` | `Center`, `Bottom`, `Top`, `Start`, `End` |
| `NVDiffMode` | `Unified`, `SideBySide` |

---

## How to read a row

```xml
<nv:NVButton Text="Continue" Variant="Filled" Command="{Binding Submit}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `Variant` | `NVButtonVariant` | `Filled` | **custom** |

---

## Foundation (not XAML views)

These are static / singleton helpers. Register fonts and Lumina with `UseNuvyntraUIKit()`.

| Type | Note | Members |
| --- | --- | --- |
| `NVTheme` | Runtime light / dark + accent. | `Current`, `Mode` **custom**, `Accent`, `Density` **custom**, `IsDark`, `Paper`, `Surface`, `Ink`, `Mist`, `Fog`, `Muted`, `Danger`, `Warn`, `Ok`, `OnAccent`, `UseLumina()`, `SetMode()`, `SetAccent()`, `Token(key)` |
| `NVTokens` | Space, radius, type size, motion ms, Outfit font names. | `Space1`…`Space6`, `RadiusSmall` / `Medium` / `Large`, `DisplaySize`…`CaptionSize`, `MotionFast` / `Normal` / `Slow`, `FontRegular`, `FontSemiBold`, `Space()`, `Motion()` |
| `NVTypography` | Factories for themed `Label`s. | `Display`, `Title`, `Body`, `Label`, `Caption`, `Mono` |
| `NVIcons` | Glyph map for `NVIconKind`. | `Glyph(kind)`, `All` |
| `NVMotion` | Durations that honor reduce-motion. | `ReduceMotion` **custom**, `Fast`, `Normal`, `Slow` |
| `NVDensity` | Enum only. | See table above |
| `NVVisualState` | Named states for styles. | `Rest`, `Hover`, `Press`, `Focus`, `Disabled`, `Error` **custom** |
| `NVAccessibility` | Automation + contrast. | `Name(view, name, hint)`, `BodyContrastOk`, `ContrastRatio` |

```xml
<!-- Theme is not a control. Bind colors from code or NVTheme.Current. -->
```

---

## Primitives

### NVSurface

Themed paper panel. Use instead of a raw `Border`.

```xml
<nv:NVSurface Elevation="1">
    <Label Text="Child" />
</nv:NVSurface>
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Elevation` | `int` | `1` | **custom** — 0–5 shadow step |
| `Content` | `View?` | | Inner child (hides `ContentView.Content`) |

### NVDivider

Hairline rule.

```xml
<nv:NVDivider IsVertical="False" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `IsVertical` | `bool` | `false` | |

### NVIcon

Stroke glyph from `NVIconKind`.

```xml
<nv:NVIcon Kind="Star" Spin="False" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Kind` | `NVIconKind` | `None` | **custom** |
| `Spin` | `bool` | `false` | **custom** |

### NVAvatar

Initials or image with an online pip.

```xml
<nv:NVAvatar Initials="NV" StatusOn="True" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Initials` | `string` | `"NV"` | **custom** |
| `ImageSource` | `ImageSource?` | | |
| `StatusOn` | `bool` | `false` | **custom** |

### NVBadge

Count or dot.

```xml
<nv:NVBadge Text="3" Dot="False" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `Dot` | `bool` | `false` | **custom** |

### NVSkeleton

Placeholder bar.

```xml
<nv:NVSkeleton />
```

No extra properties.

### NVEffects

Press / highlight host. Set `Target` to wrap a child.

```xml
<nv:NVEffects />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Target` | `View?` | | **custom** |

### NVElevation

Static helper, not a view.

```csharp
NVElevation.Apply(view, level: 2);
```

### NVOverlay

Scrim + panel. Inherits overlay host.

```xml
<nv:NVOverlay IsOpen="False" Placement="Center" DismissOnScrim="True" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `IsOpen` | `bool` | `false` | **custom** |
| `DismissOnScrim` | `bool` | `true` | **custom** |
| `Placement` | `OverlayPlacement` | `Center` | **custom** |
| `PanelContent` | `View?` | | **custom** |

### NVInteractiveViewer

Zoom / pan host.

```xml
<nv:NVInteractiveViewer />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Viewport` | `View?` | | **custom** |

### NVSpacer

Token-sized gap.

```xml
<nv:NVSpacer Size="16" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Size` | `double` | `NVTokens.Space4` | **custom** |

### NVHighlight

Search / mention span.

```xml
<nv:NVHighlight Text="aurora" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |

---

## Actions

### NVButton

Primary action. Variants are styles, not extra types.

```xml
<nv:NVButton Text="Continue" Variant="Filled" Command="{Binding Submit}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `Variant` | `NVButtonVariant` | `Filled` | **custom** |
| `Command` | `ICommand?` | | |
| `CommandParameter` | `object?` | | |

### NVIconButton

Ghost icon button. Inherits `NVButton`.

```xml
<nv:NVIconButton Kind="Plus" Variant="Ghost" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Kind` | `NVIconKind` | `Plus` | **custom** |
| *(plus `NVButton`)* | | | |

### NVToggleButton

On / off that looks like a button.

```xml
<nv:NVToggleButton Text="Bold" IsOn="False" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `IsOn` | `bool` | `false` | **custom** |

### NVDropDownButton

Opens an action sheet of strings.

```xml
<nv:NVDropDownButton Text="Menu" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `"Menu"` | |
| `Items` | `IList<string>` | empty | |

### NVCheckBox

Label + box. `IsChecked` is `bool?` so indeterminate is allowed.

```xml
<nv:NVCheckBox Text="Accept terms" IsChecked="True" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `IsChecked` | `bool?` | `false` | nullable is **custom** vs MAUI `CheckBox` |

### NVRadioButton

Use `GroupName` or wrap in `NVRadioGroup`.

```xml
<nv:NVRadioButton GroupName="Plan" Text="Monthly" IsChecked="True" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `IsChecked` | `bool` | `false` | |
| `GroupName` | `string` | `""` | |
| `Value` | `object?` | | |

### NVRadioGroup

Applies one group name to child radios.

```xml
<nv:NVRadioGroup GroupName="Plan">
    <nv:NVRadioButton Text="Monthly" />
    <nv:NVRadioButton Text="Yearly" />
</nv:NVRadioGroup>
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `GroupName` | `string` | `"nv-radio"` | |

### NVSwitch

Switch + label.

```xml
<nv:NVSwitch Text="Aurora" IsOn="True" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `IsOn` | `bool` | `false` | **custom** (MAUI uses `IsToggled`) |

### NVChip

Selectable chip.

```xml
<nv:NVChip Text="Filter" Kind="Filter" IsSelected="False" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `Kind` | `NVChipKind` | `Filter` | **custom** |
| `IsSelected` | `bool` | `false` | |

### NVSegmentedControl

Equal segments.

```xml
<nv:NVSegmentedControl SelectedIndex="0" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Items` | `IList<string>` | `A`, `B` | |
| `SelectedIndex` | `int` | `0` | |

### NVSpeechToTextButton

Mic icon. Host supplies the recognizer.

```xml
<nv:NVSpeechToTextButton />
```

Inherits `NVIconButton` (`Kind=Mic`, `Variant=Tonal`).

---

## Inputs

### NVTextField

Labeled field with helper and error slots (FieldChrome).

```xml
<nv:NVTextField Label="Email" Text="{Binding Email}" Helper="Never shared" Error="{Binding EmailError}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `Label` | `string` | `""` | **custom** floating caption |
| `Helper` | `string` | `""` | **custom** |
| `Error` | `string` | `""` | **custom** |
| `Placeholder` | `string` | `""` | |
| `IsPassword` | `bool` | `false` | |
| `HasError` | `bool` | computed | **custom** |

### NVEditor

Multi-line FieldChrome.

```xml
<nv:NVEditor Label="Notes" Text="{Binding Notes}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `Label` | `string` | `""` | **custom** |

### NVSearchBar

`NVTextField` with Search defaults.

```xml
<nv:NVSearchBar />
```

### NVMaskedEntry

Pattern mask on `NVTextField`.

```xml
<nv:NVMaskedEntry Label="Card" Mask="0000 0000 0000 0000" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Mask` | `string` | `"0000"` | **custom** |
| *(plus `NVTextField`)* | | | |

### NVNumericEntry

Culture number on `NVTextField`.

```xml
<nv:NVNumericEntry Label="Amount" Value="12" Minimum="0" Maximum="100" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Value` | `double` | `0` | |
| `Minimum` | `double` | `MinValue` | |
| `Maximum` | `double` | `MaxValue` | |

### NVNumericUpDown

Inc / dec around `NVNumericEntry`.

```xml
<nv:NVNumericUpDown Value="3" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Value` | `double` | `0` | |

### NVOtpInput

One cell per digit.

```xml
<nv:NVOtpInput Length="6" Code="{Binding Otp}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Length` | `int` | `6` | **custom** |
| `Code` | `string` | `""` | **custom** |

### NVAutoComplete

`NVTextField` plus suggestion list. `Filter()` / `FilterSuggestions()` are code helpers.

```xml
<nv:NVAutoComplete Label="City" Text="{Binding City}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Suggestions` | `IList<string>` | empty | **custom** |

### NVComboBox / NVPicker / NVTemplatedPicker

Closed list. `NVPicker` and `NVTemplatedPicker` inherit `NVComboBox`.

```xml
<nv:NVComboBox Label="Plan" SelectedItem="{Binding Plan}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Label` | `string` | `"Choice"` | **custom** (CLR, not bindable) |
| `Items` | `IList<string>` | empty | |
| `SelectedItem` | `string` | `""` | |

### NVDatePicker / NVTimePicker / NVDateTimePicker / NVTimeSpanPicker / NVMonthYearPicker

```xml
<nv:NVDatePicker Label="Start" Date="{Binding Start}" />
<nv:NVTimePicker Label="At" Time="{Binding At}" />
<nv:NVDateTimePicker Value="{Binding When}" />
<nv:NVTimeSpanPicker Duration="{Binding Wait}" />
```

| Control | Property | Type | |
| --- | --- | --- | --- |
| `NVDatePicker` | `Label` **custom**, `Date` | `string`, `DateTime` | |
| `NVTimePicker` | `Label` **custom**, `Time` | `string`, `TimeSpan` | |
| `NVDateTimePicker` | `Value` | `DateTime` | |
| `NVTimeSpanPicker` | `Duration` **custom**, `Value` | `TimeSpan`, `double` minutes | |
| `NVMonthYearPicker` | same as date, `Label=Month` | | |

### NVColorPicker

RGB sliders + swatch.

```xml
<nv:NVColorPicker Color="{Binding Accent}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Color` | `Color` | Teal | |

### NVSlider / NVCircularSlider

```xml
<nv:NVSlider Value="40" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Value` | `double` | `0` | |

### NVRangeSlider / NVRangeSelector

```xml
<nv:NVRangeSlider Start="20" End="80" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Start` | `double` | `20` | **custom** |
| `End` | `double` | `80` | **custom** |

### NVSignaturePad

Tap marks a stroke. `Export()` returns a token when `HasStroke`.

```xml
<nv:NVSignaturePad HasStroke="False" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `HasStroke` | `bool` | `false` | **custom** |

### NVRating

1–5 stars.

```xml
<nv:NVRating Value="4" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Value` | `int` | `0` | |

---

## Feedback

### NVProgressBar / NVCircularProgressBar / NVLinearGauge

```xml
<nv:NVProgressBar Value="0.6" />
<nv:NVCircularProgressBar Value="0.4" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Value` | `double` | `0` / `0.4` | 0–1 |

`NVLinearGauge` inherits progress (`Value` default `0.42`).

### NVStepProgressBar

```xml
<nv:NVStepProgressBar Index="1" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Steps` | `IList<string>` | One, Two, Three | **custom** |
| `Index` | `int` | `0` | **custom** |

### NVBusyIndicator

```xml
<nv:NVBusyIndicator />
```

### NVPullToRefresh

```xml
<nv:NVPullToRefresh IsRefreshing="{Binding Busy}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `IsRefreshing` | `bool` | `false` | |

### NVPopup / NVBottomSheet / NVDialog / NVActionSheet / NVLightbox

Overlay hosts. Same `IsOpen`, `Placement`, `DismissOnScrim`, `PanelContent` as `NVOverlay` unless noted.

```xml
<nv:NVDialog Title="Confirm" Message="Continue?" IsOpen="False" />
<nv:NVActionSheet IsOpen="False" />
<nv:NVBottomSheet Placement="Bottom" IsOpen="False" />
```

| Extra | Type | | |
| --- | --- | --- | --- |
| `NVDialog.Title` / `Message` | `string` | **custom** | |
| `NVActionSheet.Items` | `IList<string>` | | |

### NVToast

Static helper, not a view.

```csharp
await NVToast.ShowAsync("Saved");
```

### NVBanner / NVAlert / NVOfflineBanner

```xml
<nv:NVBanner Text="Ready" Tone="Success" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `Tone` | `NVBannerTone` | `Info` | **custom** |

`NVAlert` defaults `Tone=Warning`. `NVOfflineBanner` sets warning copy.

### NVEmptyView / NVRetryView

```xml
<nv:NVEmptyView Title="Nothing yet" Reason="Empty" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Title` | `string` | `"Nothing here"` | |
| `Reason` | `NVStatusReason` | `Empty` | **custom** |

### NVTooltip

```xml
<nv:NVTooltip Text="Hint" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |

---

## Layout and navigation

### NVCard

```xml
<nv:NVCard Title="Lamp" Body="Warm paper" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Title` | `string` | `""` | |
| `Body` | `string` | `""` | **custom** |

### NVExpander / NVAccordion

```xml
<nv:NVExpander Title="Section" IsExpanded="False" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Title` | `string` | `"Section"` | |
| `IsExpanded` | `bool` | `false` | |

### NVTabView / NVBottomNavigation

```xml
<nv:NVTabView SelectedIndex="0" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Tabs` | `IList<string>` | One, Two | **custom** |
| `SelectedIndex` | `int` | `0` | |

### NVNavigationDrawer / NVSideDrawer / NVNavigationView

```xml
<nv:NVNavigationDrawer />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Items` | `IList<string>` | Home, Settings | |

`NVNavigationView` composes toolbar + drawer (no extra bindables).

### NVToolbar / NVAppBar

```xml
<nv:NVToolbar Title="Lumina" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Title` | `string` | `"Lumina"` | |

### NVBreadcrumb / NVStepper / NVCarousel / NVSlideView / NVParallaxView / NVRadialMenu / NVWrapLayout

```xml
<nv:NVCarousel Index="0" />
<nv:NVWrapLayout />
```

| Control | Properties | |
| --- | --- | --- |
| `NVBreadcrumb` | `Items` | |
| `NVStepper` | `Steps` **custom**, `Index` **custom** | |
| `NVCarousel` (+ aliases) | `Items`, `Index` | |
| `NVRadialMenu` | `Items` | |
| `NVWrapLayout` | `Items` | |

### NVDockLayout / NVBackdrop / NVGridSplitter

Chrome compositions. No extra bindables.

```xml
<nv:NVDockLayout />
<nv:NVGridSplitter />
```

---

## Data

### NVCollectionView / NVListView / NVCardsView

```xml
<nv:NVCollectionView LayoutMode="List" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Items` | `IList<NVListItem>` | empty | |
| `LayoutMode` | `NVLayoutMode` | `List` | **custom** |
| `SelectionMode` | `NVSelectionKind` | `None` | **custom** |
| `SelectedItems` | `IList<NVListItem>` | empty | |
| `Grouped` | `bool` | `false` | first-letter groups |
| `AllowSwipe` | `bool` | `false` | |

`NVListItem`: `Title`, `Subtitle`, `Detail`, `Icon`. `NVListGroup`: `Name` + items. Hosted in MAUI `CollectionView` (virtualized).

### NVDataPager

```xml
<nv:NVDataPager TotalCount="42" PageSize="10" PageIndex="0" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `TotalCount` | `int` | `0` | **custom** |
| `PageSize` | `int` | `10` | **custom** |
| `PageIndex` | `int` | `0` | **custom** (0-based) |
| `PageCount` | `int` | computed | **custom** |

### NVDataGrid / NVTreeDataGrid

```xml
<nv:NVDataGrid />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Columns` | `IList<NVGridColumn>` | | `Header`, `Binding`, `Key` **custom** alias, `Sortable`, `Frozen` |
| `Rows` | `IList<IDictionary<string, object?>>` | | **custom** |
| `Filter` | `string` | `""` | case-insensitive cell match |
| `SortKey` / `SortDirection` | `string` / `NVSortDirection` | none | header tap cycles none → asc → desc |
| `PageSize` / `PageIndex` | `int` | `0` / `0` | `0` page size shows all; pager when `> 0` |
| `FrozenColumnCount` | `int` | `0` | plus per-column `Frozen` |
| `VisibleRows` | computed | | filtered / sorted / paged |
| `NVTreeDataGrid.Roots` | `IList<NVTreeNode>` | | flatten + `Expand` once |

### NVTreeView / NVOrgChart

```xml
<nv:NVTreeView />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Roots` | `IList<NVTreeNode>` | | **custom** — `Title`, `IsExpanded`, `IsChecked`, `Children` |

### NVDataForm

```xml
<nv:NVDataForm />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Fields` | `IList<NVFormField>` | | **custom** — `Name`, `Label`, `Kind`, `Value`, `Error`, `For(name, type)` |

### NVKanban

```xml
<nv:NVKanban />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Columns` | `IList<NVKanbanColumn>` | | **custom** — `Title`, `Cards` |

---

## Visualization and calendar

### NVChart / NVSparkline

```xml
<nv:NVChart />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Series` | `IList<NVChartSeries>` | | **custom** — one `GraphicsView`; empty series does not throw |

### NVGauge / NVRadialGauge / NVDigitalGauge

```xml
<nv:NVRadialGauge Value="70" />
<nv:NVDigitalGauge Value="128" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Value` | `double` / `int` | `42` | |

### NVBarcode

Generate only (not camera scan).

```xml
<nv:NVBarcode Value="NUVEXA" Format="Code128" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Value` | `string` | `"NUVEXA"` | |
| `Format` | `NVBarcodeFormat` | `Code128` | **custom** — drawn Code128 bars or 21×21 QR |

### NVTreeMap / NVHeatMap / NVMap / NVMaps

```xml
<nv:NVTreeMap />
<nv:NVMap Place="Aurora" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `NVTreeMap.Nodes` | `IList<NVTreeMapNode>` | | **custom** — `Title`, `Value` / `Weight` |
| `NVMap.Place` | `string` | `"Aurora"` | **custom** |

### NVCalendar / NVScheduler

```xml
<nv:NVCalendar Month="{Binding Month}" SelectedDate="{Binding Day}" />
<nv:NVScheduler />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Month` | `DateTime` | today | **custom** — `NextMonth` / `PreviousMonth` |
| `SelectedDate` | `DateTime?` | | last tap |
| `SelectedDates` / `AllowMultiple` | `IList<DateTime>` / `bool` | empty / `false` | |
| `Appointments` | `IList<NVAppointment>` | | **custom** — `Title`, `Start`, `End`, `Recurrence` |
| `AgendaDate` / `RecurrenceCap` | `DateTime?` / `int` | / `64` | `Agenda` expands then filters |

---

## Media, chat, AI

### NVImageEditor / NVPdfViewer / NVDocxViewer / NVMarkdownViewer / NVRichTextEditor / NVPromptInput

```xml
<nv:NVImageEditor Caption="Edit" />
<nv:NVPdfViewer Title="Spec.pdf" />
<nv:NVMarkdownViewer Markdown="# Hello" />
<nv:NVPromptInput Text="{Binding Prompt}" />
```

| Control | Properties | |
| --- | --- | --- |
| `NVImageEditor` | `Caption`, `RotationDegrees`, `CropRect`, `Annotations` — `Rotate` / `Crop` / `Annotate` | |
| `NVPdfViewer` / `NVDocxViewer` | `Title`, `Pages`, `Zoom`, `Query`, `PageIndex`, `MatchCount` | |
| `NVMarkdownViewer` | `Markdown` **custom** | |
| `NVRichTextEditor` | `NVEditor` (`Label=Rich text`) | |
| `NVPromptInput` | `NVTextField` defaults | |

### NVSpreadsheet

```xml
<nv:NVSpreadsheet />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `Cells` | `IList<NVSpreadsheetCell>` | **custom** — `Row`, `Column`, `Text` / `Value` | |

### NVChat / NVAIPrompt / NVAiAssistView / NVSmartPasteButton

```xml
<nv:NVChat />
<nv:NVAIPrompt Text="{Binding Prompt}" />
<nv:NVSmartPasteButton />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `NVChat.Messages` | `IList<NVChatMessage>` | **custom** — `Author`, `Text`, `IsMine`, `At` | |
| `NVChat.Attachments` / `IsStreaming` | `IList<NVFileChip>` / `bool` | `AppendStream` / `Attach` | |
| `NVAIPrompt.Text` | `string` | | |
| `NVAiAssistView.Prompt` | `string` | **custom** | |

---

## Basics (everyday mobile)

### NVHeading / NVBodyText / NVCaptionText

```xml
<nv:NVHeading Text="Lumina" Role="Display" />
<nv:NVBodyText Text="Body copy" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | |
| `Role` | `NVTextRole` | `Title` (`Body` / `Caption` on subclasses) | **custom** |

### NVImage / NVSafeArea

```xml
<nv:NVImage Caption="Photo" />
<nv:NVSafeArea />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `Source` | `ImageSource?` | | |
| `Caption` | `string` | **custom** | |
| `Child` | `View?` | **custom** on `NVSafeArea` | |

### NVSectionHeader / NVFormSection

```xml
<nv:NVSectionHeader Text="Account" />
<nv:NVFormSection Title="Profile" />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `Text` / `Title` | `string` | **custom** | |
| `Add(View)` | method | **custom** | |

### NVFloatingActionButton / NVDotIndicator / NVAppScaffold

```xml
<nv:NVFloatingActionButton />
<nv:NVDotIndicator Count="4" Index="1" />
<nv:NVAppScaffold Title="Home" ShowFab="True" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Count` / `Index` | `int` | `3` / `0` | **custom** |
| `Title` | `string` | `"Lumina"` | |
| `ShowFab` | `bool` | `true` | **custom** |
| `Body` | `View?` | | **custom** |

### NVMenu

Inherits `NVDropDownButton` (`Text`, `Items`).

### NVListTile / NVSwipeTile / NVNotificationRow / NVContactTile

```xml
<nv:NVListTile Title="Home" Subtitle="Default" Kind="Home" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Title` | `string` | | |
| `Subtitle` | `string` | | **custom** |
| `Kind` | `NVIconKind` | `ChevronRight` | **custom** leading icon |

### NVSettingsTile

```xml
<nv:NVSettingsTile Text="Notifications" IsOn="True" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | `"Setting"` | |
| `IsOn` | `bool` | `false` | **custom** |

### NVChipGroup / NVCheckList / NVGroupedList / NVFilterBar / NVTagInput / NVTimeSlotPicker

```xml
<nv:NVChipGroup />
<nv:NVCheckList />
<nv:NVGroupedList />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `Items` or `Groups` | `IList<string>` | `Groups` is **custom** | |

### NVIndexBar / NVSkeletonList / NVInfiniteFooter / NVReactionBar / NVTypingIndicator / NVStoryRing

Chrome with no extra bindables (fixed demo content).

```xml
<nv:NVIndexBar />
<nv:NVSkeletonList />
```

### NVSelectionBar

```xml
<nv:NVSelectionBar Count="3" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Count` | `int` | `0` | **custom** |

---

## Forms plus

### NVEmailField / NVPhoneField / NVPasswordField / NVCouponField / NVQuantityStepper

Specialized `NVTextField` / `NVMaskedEntry` / `NVNumericUpDown`. No new property names beyond inherited + `NVPhoneField.Mask`.

```xml
<nv:NVEmailField Text="{Binding Email}" />
<nv:NVPasswordField Text="{Binding Secret}" />
<nv:NVQuantityStepper Value="1" />
```

### NVPasswordStrength

```xml
<nv:NVPasswordStrength Password="{Binding Secret}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Password` | `string` | `""` | **custom** |
| `Score` | `int` | computed 0–4 | **custom** |

### NVDateRangePicker

```xml
<nv:NVDateRangePicker Start="{Binding From}" End="{Binding To}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Start` | `DateTime` | today | **custom** |
| `End` | `DateTime` | today+2 | **custom** |

### NVPinPad / NVLockPad

```xml
<nv:NVPinPad Code="{Binding Pin}" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Code` | `string` | `""` | **custom** |

`NVLockPad` composes heading + pad (no extra bindables).

### NVCopyable / NVLink

```xml
<nv:NVCopyable Text="NUV-1042" />
<nv:NVLink Text="Learn more" />
```

| Property | Type | Default | |
| --- | --- | --- | --- |
| `Text` | `string` | | |

### NVCountryPicker / NVLanguagePicker / NVThemePicker

`NVComboBox` / `NVSegmentedControl` with preset `Items`. `NVThemePicker` writes `NVTheme.Current.SetMode` from `SelectedIndex`.

```xml
<nv:NVThemePicker />
```

---

## Patterns (commerce / content)

### NVCurrencyLabel / NVPriceTag / NVCountdown / NVQuote / NVCodeBlock / NVBulletList / NVStatCard

```xml
<nv:NVCurrencyLabel Amount="42" />
<nv:NVCountdown Seconds="90" />
<nv:NVQuote Text="Warm paper." />
<nv:NVCodeBlock Code="UseNuvyntraUIKit();" />
<nv:NVStatCard Label="Orders" Value="128" />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `Amount` | `double` | **custom** | |
| `Seconds` | `int` | **custom** | |
| `Text` / `Code` / `Items` | | | |
| `Label` + `Value` on `NVStatCard` | `string` | **custom** pair | |

### NVTimeline / NVGantt / NVWizard / NVStickyBar / NVCartBar / NVTicket / NVSeatPicker / NVVariantPicker

```xml
<nv:NVWizard Index="0" />
<nv:NVCartBar Total="86" />
<nv:NVTicket Title="Pass" Code="NUV-2048" />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `NVTimeline.Items` | `IList<NVTimelineItem>` | **custom** — `Title`, `Detail`, `At` | |
| `NVWizard.Steps` / `Index` | | **custom** | |
| `NVStickyBar.Text` | `string` | | |
| `NVCartBar.Total` | `double` | **custom** | |
| `NVTicket.Title` / `Code` | `string` | **custom** | |
| `NVSeatPicker.Seats` | `IList<NVSeatCell>` | **custom** — `Label`, `Taken` | |

---

## Social and extra media

### NVProfileHeader / NVFeedCard / NVComposer / NVBubble

```xml
<nv:NVProfileHeader Name="Studio" />
<nv:NVFeedCard Title="Ship" Body="Left the hub." />
<nv:NVComposer Text="{Binding Draft}" />
<nv:NVBubble Text="Hello" IsMine="True" />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `Name` | `string` | **custom** | |
| `Title` / `Body` | `string` | | |
| `Text` | `string` | | |
| `IsMine` | `bool` | **custom** | |

### NVImageGallery / NVVideoPlayer / NVAudioPlayer / NVWebView / NVVoiceNote / NVWaveform / NVBeforeAfter

```xml
<nv:NVImageGallery />
<nv:NVWebView Url="https://nuvyntralabs.github.io/" />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `NVImageGallery.Items` | `IList<string>` | | |
| `NVWebView.Url` | `string` | **custom** — http(s) only | |

Video / audio / voice are chrome (host supplies decode / capture).

---

## Advanced

### NVMasterDetail / NVForceUpdate / NVBiometricGate / NVDashboardGrid

Compositions. `NVPermissionCard` has `Title`.

```xml
<nv:NVMasterDetail />
<nv:NVPermissionCard Title="Location" />
<nv:NVBiometricGate />
```

| Property | Type | | |
| --- | --- | --- | --- |
| `NVPermissionCard.Title` | `string` | **custom** | |

---

## Next (1.2 app chrome)

```xml
<nv:NVCommandPalette Query="{Binding Query}" IsOpen="{Binding PaletteOpen}" />
<nv:NVConsentBanner Text="We use cookies to keep Lumina useful." IsAccepted="{Binding Accepted}" />
<nv:NVHeatCalendar Month="{Binding Month}" />
```

| Type | Property | Type | Default | |
| --- | --- | --- | --- | --- |
| `NVCommandPalette` | `Query` | `string` | `""` | **custom** |
| | `Commands` / `Recents` | `IList<NVCommandItem>` | empty | **custom** |
| `NVCoachMark` | `Steps` | `IList<NVCoachStep>` | empty | **custom** |
| | `Index` | `int` | `0` | |
| `NVContextMenu` | `Items` | `IList<NVMenuAction>` | empty | **custom** |
| | `OpenCommand` | `ICommand` | open | **custom** |
| `NVFileDrop` | `Files` | `IList<NVFileChip>` | empty | **custom** |
| | `PickCommand` | `ICommand` | | **custom** |
| `NVPaywall` | `Title` / `Message` | `string` | | |
| | `IsBlocking` | `bool` | `false` | **custom** |
| | `Plans` | `View` | | **custom** |
| `NVWhatsNew` | `VersionTitle` | `string` | `"What's new"` | **custom** |
| | `Items` | `IList<string>` | empty | |
| `NVConsentBanner` | `Text` | `string` | privacy copy | |
| | `IsAccepted` | `bool` | `false` | **custom** |
| | `AcceptCommand` / `ManageCommand` | `ICommand` | | **custom** |
| `NVHeatCalendar` | `Month` | `DateTime` | today | **custom** |
| | `Values` | `IList<NVHeatDay>` | empty | **custom** |

Empty palette query shows `Recents`. Filter is case-insensitive. Paywall `Dismiss` is a no-op when `IsBlocking`. File drop ignores chips with an empty `Name`. Heat calendar cells equal days in `Month`; missing values draw empty.

### 1.3 types

| Type | Property | Type | |
| --- | --- | --- | --- |
| `NVSpeedDial` | `Actions` / `IsOpen` | `IList<NVSpeedDialAction>` / `bool` | **custom** |
| `NVSubscriptionCard` | `Name` / `Price` / `Features` / `CtaText` / `CtaCommand` | | **custom** |
| `NVEmojiPicker` | `Query` / `Glyphs` / `Selected` | | **custom** |
| `NVPivotGrid` | `Facts` | `IList<NVPivotFact>` | **custom** |
| `NVPropertyGrid` | `Items` | `IList<NVPropertyItem>` | **custom** |
| `NVJsonTree` | `Json` | `string` | **custom** |
| `NVDiffView` | `Left` / `Right` / `Mode` | `string` / `NVDiffMode` | **custom** |
| `NVCodeEditor` | `Text` | `string` | |
| `NVCallBar` | `Title` / `IsMuted` / `MuteCommand` / `EndCommand` | | **custom** |
| `NVInCallView` | `Name` / `Elapsed` / `Keypad` | | **custom** |
| `NVSyncConflictCard` | `Local` / `Remote` / `KeepCommand` / `TakeRemoteCommand` | | **custom** |
| `NVUploadTile` | `FileName` / `Bytes` / `RetryCommand` | | **custom** |
| `NVDeviceSheet` | `Devices` / `ConnectCommand` | | **custom** |
| `NVPrintPreview` | `Page` / `PrintCommand` / `ShareCommand` | | **custom** |
| `NVNfcPrompt` | `Status` | `string` | **custom** |
| `NVReviewPrompt` | `Rating` / `NotNowCommand` / `ReviewCommand` | | **custom** |

Host chrome types construct with null commands. The library does not `PackageReference` `Plugin.Maui.*`.

---

## Page recipes (`NV*View`)

Content compositions over the controls above. They do **not** add new bindable names; they set titles and seed demo children.

```xml
<nv:NVSignInView />
<nv:NVDashboardView />
<nv:NVOrderSummaryView />
```

| Group | Types |
| --- | --- |
| Auth | `NVSignInView`, `NVSignUpView`, `NVForgotPasswordView`, `NVResetPasswordView`, `NVSocialSignInView`, `NVTabbedAuthView`, `NVProfileSetupView`, `NVWalkthroughView` |
| Commerce | `NVCategoryView`, `NVCatalogView`, `NVProductHomeView`, `NVProductDetailView`, `NVCartView`, `NVWishlistView`, `NVCheckoutView`, `NVCardPaymentView`, `NVSavedCardsView`, `NVPaymentResultView`, `NVOrdersView`, `NVOrderHistoryView` |
| Content | `NVArticleFeedView`, `NVArticleDetailView`, `NVMyArticlesView`, `NVReviewView`, `NVContactView`, `NVAboutView`, `NVFaqView`, `NVBookmarksView` |
| Social | `NVInboxView`, `NVConversationView`, `NVSocialProfileView`, `NVPeopleListView`, `NVAuthorProfileView`, `NVHealthProfileView`, `NVChatProfileView` |
| Files | `NVNavigationHubView`, `NVMediaLibraryView`, `NVPlaylistView`, `NVFileExplorerView`, `NVDocumentsView`, `NVSuggestionsView` |
| System | `NVStatusView`, `NVSettingsView`, `NVHelpView`, `NVNotificationsView`, `NVDeliveryTrackView`, `NVAddressBookView`, `NVBookingView`, `NVDashboardView` |
| Extras | `NVPinLockView`, `NVForceUpdateView`, `NVSearchResultsView`, `NVFilterSheetView`, `NVMediaPlayerView`, `NVSplitInboxView`, `NVOnboardingPermissionsView`, `NVOrderSummaryView` |
| Next (1.2) | `NVInvoiceView`, `NVReceiptView`, `NVCompareView`, `NVStoreLocatorView`, `NVSubscriptionView`, `NVWhatsNewView` |
| Next (1.3) | `NVConflictResolveView`, `NVCallView`, `NVAddressFormView` |

---

## Custom property index

Kit-invented names (use these instead of guessing MAUI equivalents):

| Name | Used on |
| --- | --- |
| `Variant` | `NVButton`, `NVIconButton` |
| `Kind` | `NVIcon`, `NVIconButton`, `NVChip`, `NVListTile` |
| `Tone` | `NVBanner` |
| `Role` | `NVHeading` |
| `Reason` | `NVEmptyView` |
| `LayoutMode` | `NVCollectionView` |
| `Elevation` | `NVSurface` |
| `StatusOn` | `NVAvatar` |
| `Spin` | `NVIcon` |
| `Dot` | `NVBadge` |
| `Label` / `Helper` / `Error` / `HasError` | FieldChrome inputs |
| `Mask` | `NVMaskedEntry` |
| `Code` / `Length` | OTP, PIN, ticket, code block |
| `HasStroke` | `NVSignaturePad` |
| `IsOn` | `NVSwitch`, `NVToggleButton`, `NVSettingsTile` |
| `IsSelected` | `NVChip` |
| `ShowFab` / `Body` | `NVAppScaffold` |
| `IsOpen` / `Placement` / `DismissOnScrim` / `PanelContent` | overlays |
| `Viewport` / `Target` / `Child` / `Size` | viewer, effects, safe area, spacer |
| `Start` / `End` | range slider, date range |
| `Duration` | `NVTimeSpanPicker` |
| `Format` | `NVBarcode` |
| `Place` | `NVMap` |
| `Password` / `Score` | `NVPasswordStrength` |
| `Amount` / `Seconds` / `Total` | currency, countdown, cart |
| `Groups` / `Roots` / `Series` / `Nodes` / `Cells` / `Messages` / `Appointments` / `Fields` / `Columns` / `Rows` / `Seats` | data surfaces |
| `Month` / `SelectedDate` | `NVCalendar` |
| `Query` / `Commands` / `Recents` | `NVCommandPalette` |
| `Steps` / `Index` | `NVCoachMark` |
| `IsBlocking` / `Plans` | `NVPaywall` |
| `VersionTitle` | `NVWhatsNew` |
| `IsAccepted` | `NVConsentBanner` |
| `Files` / `PickCommand` | `NVFileDrop` |
| `Values` | `NVHeatCalendar` |
| `Caption` | image / image editor |
| `Markdown` / `Url` / `Prompt` | markdown, web, AI |
| `Name` / `IsMine` / `Subtitle` | profile, bubble, list tile |
| `Steps` / `Index` / `Count` / `Tabs` | stepper, dots, wizard, selection, tabs |
| `Initials` | `NVAvatar` |

---

## Out of scope in this kit

PDF / Word / Excel **engines**, camera barcode **scan**, paid map tiles, and real biometrics. Those stay host plugins (Printing, MediaPipeline, GeoLocator, Biometric, FormValidation, KeyboardManager). This library does not `PackageReference` `Plugin.Maui.*`.
