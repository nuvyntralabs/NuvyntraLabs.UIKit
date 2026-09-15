# NuvyntraLabs.UIKit

[![NuGet](https://img.shields.io/nuget/v/NuvyntraLabs.UIKit.svg?label=NuGet)](https://www.nuget.org/packages/NuvyntraLabs.UIKit)

**Version:** 1.0.0

Lumina-themed UI kit for **.NET MAUI** on **Android**, **iOS**, **Mac Catalyst**, and **Windows**. One library: `NV*` controls, tokens, and page recipes.

This is a **UI library**, not a MauiEssentials runtime plugin. Publishing stays pipeline-only — never `dotnet nuget push` from a local clone.

## Install

```bash
dotnet add package NuvyntraLabs.UIKit
```

```csharp
builder
    .UseMauiApp<App>()
    .UseNuvyntraUIKit();
```

```xml
xmlns:nv="http://nuvyntralabs.com/uikit"

<nv:NVCheckBox Text="Accept terms" IsChecked="{Binding Accept}" />
<nv:NVRadioButton GroupName="Plan" Text="Monthly" />
<nv:NVTextField Label="Email" Text="{Binding Email}" />
<nv:NVButton Text="Continue" Variant="Filled" Command="{Binding Submit}" />
<nv:NVSignInView />
```

## 1.0 catalog

177 controls + 57 page recipes + helpers `NVRadioGroup` and `NVFormField`. Basics through advanced — one kit for a typical mobile app.

| Layer | Types |
| --- | --- |
| Foundation | `NVTheme`, `NVTokens`, `NVTypography`, `NVIcons`, `NVMotion`, `NVDensity`, `NVVisualState`, `NVAccessibility` |
| Basics | `NVHeading`, `NVListTile`, `NVAppScaffold`, `NVFloatingActionButton`, `NVDialog`, `NVImage`, … |
| Forms | `NVTextField`, `NVEmailField`, `NVPasswordStrength`, `NVPinPad`, `NVDateRangePicker`, … |
| Feedback / nav | `NVBanner`, `NVEmptyView`, `NVTabView`, `NVBottomSheet`, `NVNavigationView`, … |
| Data / viz | `NVDataGrid`, `NVChart`, `NVCalendar`, `NVKanban`, `NVGantt`, … |
| Social / media | `NVChat`, `NVComposer`, `NVVideoPlayer`, `NVWebView`, `NVPdfViewer`, … |
| Advanced | `NVMasterDetail`, `NVLockPad`, `NVBiometricGate`, `NVWizard`, `NVTicket`, … |
| Pages | `NVSignInView` … `NVOrderSummaryView` |

XAML + property list: [UIKitLib.md](UIKitLib.md). Full IDs: [nuvyntralabs-uikit-components.md](https://github.com/nuvyntralabs/MauiEssentials/blob/main/docs/plans/nuvyntralabs-uikit-components.md).

Kit-level 1.0: working Lumina APIs, not Telerik/Syncfusion parity. PDF / Docx / Spreadsheet are viewers. `NVBarcode` generates; it does not scan. Compose [FormValidation](https://www.nuget.org/packages/Plugin.Maui.FormValidation), [KeyboardManager](https://www.nuget.org/packages/Plugin.Maui.KeyboardManager), and [MVVMExpress](https://www.nuget.org/packages/Plugin.Maui.MVVMExpress.Core) at the host — this library does not PackageReference them.

## Sample

`samples/NuvyntraLabs.UIKit.Sample` — MAUI Shell flyout: Theme through Media, plus Basics, Advanced, and every page recipe.

## Platforms

`net10.0`, `net10.0-android` (API 21+), `net10.0-ios` (15+), `net10.0-maccatalyst` (15+), `net10.0-windows10.0.19041.0`.

## License

MIT. Font: Outfit (OFL). Look is original (Lumina) — not a Syncfusion or Telerik theme.
