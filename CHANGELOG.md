# Changelog

## 1.4.0

- Deepen existing 1.0 types (no new catalog names)
- `NVCollectionView`: MAUI virtualization, `LayoutMode`, swipe, select, group
- `NVDataGrid` / `NVTreeDataGrid`: sort cycle, filter, freeze, edit commit/cancel, page, expand-once
- `NVChart`: one `GraphicsView`; every `NVChartSeriesKind` draws; empty series is safe
- `NVBarcode`: drawn Code128 / QR (still generate-only)
- `NVCalendar` / `NVScheduler`: month nav, multi-select, recurrence cap, agenda
- `NVPdfViewer` / `NVImageEditor` / `NVChat`: zoom/search, rotate/crop/annotate, stream + attach
- Tests: T-GRD / T-VIZ / T-CAL / T-LST in `NVDeepenTests`

## 1.3.0

- App: `NVSpeedDial`, `NVSubscriptionCard`, `NVEmojiPicker`
- Data: `NVPivotGrid`, `NVPropertyGrid`, `NVJsonTree`
- Media: `NVDiffView`, `NVCodeEditor`
- Host chrome (slots only): `NVCallBar`, `NVInCallView`, `NVSyncConflictCard`, `NVUploadTile`, `NVDeviceSheet`, `NVPrintPreview`, `NVNfcPrompt`, `NVReviewPrompt`
- Recipes: `NVConflictResolveView`, `NVCallView`, `NVAddressFormView`
- Catalog is now 201 controls + 66 page recipes

## 1.2.0

- App chrome: `NVCommandPalette`, `NVCoachMark`, `NVContextMenu`, `NVFileDrop`, `NVPaywall`, `NVWhatsNew`, `NVConsentBanner`
- Viz: `NVHeatCalendar` (day cells, not a chart series)
- Recipes: `NVInvoiceView`, `NVReceiptView`, `NVCompareView`, `NVStoreLocatorView`, `NVSubscriptionView`, `NVWhatsNewView`
- Catalog is now 185 controls + 63 page recipes. Gallery section **14 Next**. Tests cover T-APP / T-VIZ-08 / T-PG-50…55

## 1.0.0

- Full Lumina catalog: foundation tokens, 177 `NV*` controls (basics → advanced), 57 page recipes, catalog tests, and a Shell gallery
- PDF / Docx / Sheet are viewers. Barcode is generate-only. Video / WebView are chrome, not engines
- CI publishes nupkg + snupkg to nuget.org and GitHub Packages (pipeline-only; never `dotnet nuget push` from a local clone)

## 0.1.0

Phase 0: Lumina tokens, `NVTheme`, `NVSurface`, `NVButton`, `NVCheckBox`, `NVRadioButton`, `NVRadioGroup`, `NVTextField`, sample gallery (theme / actions / inputs), unit tests.
