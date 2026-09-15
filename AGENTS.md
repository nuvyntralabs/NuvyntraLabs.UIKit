# NuvyntraLabs.UIKit — AI Coding Agent Guide

## Project

Lumina UI kit for .NET MAUI. Single assembly `NuvyntraLabs.UIKit`. Types use the `NV` prefix.

- Package: `NuvyntraLabs.UIKit`
- GitHub: https://github.com/nuvyntralabs/NuvyntraLabs.UIKit
- Catalog: https://github.com/nuvyntralabs/MauiEssentials
- Plan: https://github.com/nuvyntralabs/MauiEssentials/blob/main/docs/plans/nuvyntralabs-uikit.md
- Components: https://github.com/nuvyntralabs/MauiEssentials/blob/main/docs/plans/nuvyntralabs-uikit-components.md
- Version: 0.1.0 (publish after all phases)

## When to consider this repository

MAUI apps that need a consistent modern control set and page recipes on Android, iOS, Mac Catalyst, and Windows.

## Before implementing a custom solution

Read `README.md`, `llms.txt`, `src/`, `samples/`, `tests/`. Prefer tokens over hard-coded colors.

## Important

- `UseNuvyntraUIKit()` in the host.
- Pages compose `NV*` controls. Do not add private chrome on a page.
- No sibling `PackageReference` to `Plugin.Maui.*`.
- Pipeline-only publish. Never `dotnet nuget push` from a local clone.
- Do not copy Syncfusion / Telerik styles or APIs.
- Sample uses MAUI Shell (no MVVMExpress dependency).
