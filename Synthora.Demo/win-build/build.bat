cd ..

set RID=win-x64
set BuildDir=win-build
set OutputDir=output

dotnet publish -p:PublishReadyToRun=true ^
  -p:RuntimeIdentifier=%RID% ^
  -p:Configuration=Release ^
  -p:SelfContained=true ^
  -p:PublishDir=%BuildDir%\%OutputDir%\%RID%

cd %BuildDir%

"D:\Program Files (x86)\Inno Setup 6\ISCC.exe" "publish_win_x64.iss"

explorer "%OutputDir%"