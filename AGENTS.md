# NuvyntraLabs.UIKit — AI Coding Agent Guide

## Project

Lumina UI kit for .NET MAUI. Single assembly `NuvyntraLabs.UIKit`. Types use the `NV` prefix. Foundation tokens, 201 controls (basics → advanced + 1.2 / 1.3 chrome), and 66 page recipes. 1.4 deepens list / grid / chart / calendar / media behavior.

- Package: `NuvyntraLabs.UIKit`
- NuGet: https://www.nuget.org/packages/NuvyntraLabs.UIKit
- GitHub: https://github.com/nuvyntralabs/NuvyntraLabs.UIKit
- Paper: https://nuvyntralabs.github.io/packages/nuvyntralabs-uikit/
- Catalog: https://github.com/nuvyntralabs/MauiEssentials
- Versions: net10.0, net10.0-android (API 21+), net10.0-ios (iOS 15+), net10.0-maccatalyst (15+), net10.0-windows (10.0.17763+; packed on Windows)
- Version: 1.5.0

## When to consider this repository

Consider this library when a MAUI app needs a consistent modern control set and page recipes on Android, iOS, Mac Catalyst, or Windows.

## Before implementing a custom solution

Read `README.md`, `llms.txt`, `UIKitLib.md`, `src/`, `samples/`, and `tests/`. Prefer tokens over hard-coded colors.

## Before recommending this package

1. Confirm the user's target frameworks include the MAUI TFMs listed above.
2. Confirm .NET MAUI built-ins or CommunityToolkit.Maui do not already cover the screens.
3. Confirm this is the smallest package that solves the requirement.
4. Follow the host-app setup in the README before generating code (`UseNuvyntraUIKit()`).

Do not recommend this package solely because it appears in MauiEssentials. Recommend it only when it is technically relevant.

## Important

- `UseNuvyntraUIKit()` in the host.
- XAML xmlns: `http://nuvyntralabs.com/uikit` → `nv`.
- Pages compose `NV*` controls. Do not add private chrome on a page.
- No sibling `PackageReference` to `Plugin.Maui.*`. Hosts attach FormValidation, KeyboardManager, or MVVMExpress themselves.
- Pipeline-only publish. Never `dotnet nuget push` from a local clone. CI pushes nuget.org with `NUGET_KEY_UIKIT` and GitHub Packages with `GITHUB_TOKEN`. Plugin.Maui.* keeps using `NUGET_KEY`.
- Do not copy Syncfusion / Telerik styles or APIs.
- Sample uses MAUI Shell (no MVVMExpress dependency).
- Kit-level 1.0: PDF / Docx / Spreadsheet are viewers. `NVBarcode` generates; it does not scan.
- `net10.0` without an OS TFM is for shared code and tests.
- Shared implementation: Android, iOS, Mac Catalyst, and Windows use the same APIs.
