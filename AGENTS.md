# Synthora Agents Guide

## Overview
Synthora is an Avalonia UI theme and control styles library. The main `Synthora` project produces the NuGet package and targets both `.NET 8.0` and `.NET 10.0`. The repository also contains a `Demo/` solution area with shared demo UI code plus desktop and Android hosts used for validation and packaging.

## Core Projects
* **Synthora**: The main UI library project containing Avalonia themes, resources, attached helpers, overlays, converters, and related infrastructure.
* **Demo/Synthora.Demo**: Shared demo application code, views, view models, styles, assets, and sample data.
* **Demo/Synthora.Demo.Desktop**: Desktop host for the demo application and packaging scripts.
* **Demo/Synthora.Demo.Android**: Android host for the demo application.

## Key Instructions for Agents

1. **Framework & Technologies**:
   - Main library: `.NET 8.0` and `.NET 10.0`
   - Demo shared project: `.NET 10.0`
   - Demo Android host: `.NET 10.0-android`
   - Avalonia UI
   - C# with nullable reference types enabled
2. **Project Structure**:
   - `Synthora/Controls/Themes/`: Control theme definitions.
   - `Synthora/Accents/`: Accent palettes and shared color resources.
   - `Synthora/Strings/`: Localized or shared string resources.
   - `Synthora/ThemeDensitys/`: Density-related theme resources.
   - `Synthora/Attaches/`: Attached behaviors and properties.
   - `Synthora/Input/`, `Synthora/Converters/`, `Synthora/Events/`, `Synthora/Extensions/`, `Synthora/Overlays/`, `Synthora/Resources/`, `Synthora/Utils/`: Supporting library infrastructure used by the theme package.
   - `Demo/Synthora.Demo/Views/` and `Demo/Synthora.Demo/ViewModels/`: Demo pages and their view models.
   - `Demo/Synthora.Demo/Styles/` and `Demo/Synthora.Demo/Assets/`: Demo-only styles and assets.
3. **Versioning & Dependencies**:
   - Shared package versions are centralized in `Directory.Build.props`.
4. **Coding Standards**:
   - Write clean, strictly typed, idiomatic C#.
   - Keep nullable reference types enabled.
   - Follow standard Avalonia control theme architecture and existing repo patterns.
   - For Boolean state naming conventions, prefer `VerbNoun` (for example `AutoScrollToEnd`) or standard state flags, without unnecessary `Is` prefixes unless indicating a simple boolean observer mode.

## NativeAOT Requirements

* Avoid reflection whenever possible.
* Avoid dynamic code generation.
* Avoid runtime type discovery.

---

**CRITICAL REQUIREMENT:**
All modifications or code generations must compile successfully before completing the task. Always run a build to verify compilation when editing multiple files or critical control structures.
Do not add a newline character at the end of the edited file.