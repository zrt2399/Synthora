#!/bin/zsh

cd "$(dirname "$0")/.."

# 设置目标平台 Runtime Identifier
RID="osx-arm64"
BuildDir="mac-build"
OutputDir="artifacts"
DMG_PROJECT="$BuildDir/package.dmgcanvas"
DMG_OUTPUT="$BuildDir/$OutputDir/SynthoraDemoInstaller.dmg"

# 1. 发布应用
dotnet publish -t:BundleApp \
  -p:RuntimeIdentifier=$RID \
  -p:Configuration=Release \
  -p:SelfContained=true \
  -p:PublishDir=$BuildDir/$OutputDir/$RID

# 2. 执行 DMG Canvas 打包
echo "🛠	生成 DMG 中..."
"/Applications/DMG Canvas.app/Contents/Resources/dmgcanvas" "$DMG_PROJECT" "$DMG_OUTPUT"

# 3. 打开dmg文件目录
open -R "$DMG_OUTPUT"