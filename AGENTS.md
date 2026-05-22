# Synthora Agents Guide

## Overview
Synthora is an Avalonia UI theme and control styles library component targeting .NET 10.0. The `Synthora` project produces a NuGet package with styling definitions, resources (accents, theme files, strings, density styles), and component setups. `Synthora.Demo` provides a test/demo application.

## Core Projects
* **Synthora**: The main UI library project containing Avalonia XAML themes and behaviors.
* **Synthora.Demo**: The demonstration application.

## Key Instructions for Agents

1. **Framework & Technologies**: 
   - .NET 10.0
   - Avalonia UI
   - C# 14.0 (derived from .NET 10 targeting)
2. **Project Structure**:
   - `Synthora/Controls/Themes/`: Control style definitions.
   - `Synthora/Accents/`: Color palette and accent resources.
   - `Synthora/DensityStyles/`: Layout density adjustments.
   - `Synthora/Attaches/`: Attached behaviors/properties (e.g., `ScrollViewerAttach.cs`).
3. **Coding Standards**:
   - Write clean, strictly typed, idiomatic C#.
   - Use nullable reference types (`<Nullable>enable</Nullable>`).
   - Follow standard UI control theme architecture for Avalonia.
   - For Boolean state naming conventions, prefer `VerbNoun` (e.g., `AutoScrollToEnd`) or standard state flags, without unnecessary `Is` prefixes unless indicating a simple boolean observer mode.

---

**CRITICAL REQUIREMENT:**
All modifications or code generations must compile successfully before completing the task. Always run a build to verify compilation when editing multiple files or critical control structures.
Do not add a newline character at the end of the edited file.