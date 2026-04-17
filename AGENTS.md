# 核心模块 (AGENTS)

本文档概述了 Synthora 项目的核心模块和组件，它们是构建整个样式库的基石。

## 1. 主题核心 (Theme Core)

-   **`SynthoraTheme.axaml` / `SynthoraTheme.axaml.cs`**:
    这是 Synthora 样式库的入口点。它负责整合所有样式、资源和主题配置。当用户在 `App.axaml` 中引入 `<SynthoraTheme />` 时，实际上就是加载了这个核心文件。它管理着整个应用的主题切换逻辑和基础样式。

## 2. 控件 (Controls)

-   **路径**: `/Controls`
-   **职责**: 定义项目的自定义控件。每个控件都包含了其特定的逻辑和功能。这些控件是 Synthora 提供的核心用户界面元素。

## 3. 样式 (Styles)

-   **路径**: `/Controls/Themes`
-   **职责**: 为控件提供默认的视觉样式和模板 (`ControlTemplate`)。样式与控件分离，使得用户可以方便地覆盖或自定义控件的外观，而无需修改控件本身的逻辑。

## 4. 颜色主题 (Accents)

-   **路径**: `/Accents`
-   **职责**: 定义了不同的颜色主题，如亮色主题 (`Light`) 和暗色主题 (`Dark`)。这些文件包含了画刷 (`Brush`) 和颜色 (`Color`) 资源，供整个样式库引用。`SynthoraTheme` 通过加载不同的颜色主题来实现明暗模式的切换。

## 5. 附加属性与行为 (Attached Properties & Behaviors)

-   **路径**: `/Attaches`
-   **职责**: 提供附加属性，用于向现有控件添加额外的功能或行为，而无需创建子类。例如，可以创建一个附加属性来为 `Button` 添加一个特定的图标。

## 6. 值转换器 (Converters)

-   **路径**: `/Converters`
-   **职责**: 提供数据绑定时所需的值转换逻辑。例如，将布尔值 `true` 转换为 `Visibility.Visible`。

## 7. 扩展方法 (Extensions)

-   **路径**: `/Extensions`
-   **职责**: 提供一系列静态扩展方法，以简化常见的编码任务，增强框架原有类的功能。

## 8. 本地化 (Localization)

-   **路径**: `/Strings`
-   **职责**: 存放用于本地化的字符串资源。这使得 Synthora 能够支持多语言界面。

## Mandatory Rule

After generating or modifying code, you MUST ensure:

1. The project builds successfully
2. No compilation errors exist
3. Fix all errors before finishing