; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define Year GetDateTimeString('yyyy','#0', '#0')
#define Is64Or86 "x64"
#define MyAppExeName "Synthora.Demo.exe"

#define WorkPath "output\win-" + Is64Or86
#define MyAppName "SynthoraDemo"
#define MyAppVersion GetFileVersion(WorkPath + "\" + MyAppExeName)
#define MyAppPublisher "Yoyo"
//注册表
//#define MyAppAssocName "WAS 测试程序"
//#define MyAppAssocExt ".tpg"
//#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

#define IconPath "..\Assets\avalonia-logo.ico"
#define MyUid "F47CE6E5-3533-4B56-9149-A949D6F53978"

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (若要生成新的 GUID，可在菜单中点击 "工具|生成 GUID"。)
AppId={{{#MyUid}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName}
AppPublisher={#MyAppPublisher}
DefaultDirName={localappdata}\Programs\{#MyAppName}
ChangesAssociations=yes
DisableProgramGroupPage=yes
; 以下行取消注释，以在非管理安装模式下运行（仅为当前用户安装）。
;PrivilegesRequired=lowest
;OutputDir=.\
OutputBaseFilename={#MyAppName}.{#MyAppVersion}.win-{#Is64Or86}
SetupIconFile={#IconPath}
UninstallDisplayIcon={uninstallexe}
Uninstallable=yes
Compression=lzma
SolidCompression=yes
WizardStyle=modern
;安装包文件版本
VersionInfoVersion = {#MyAppVersion}
AppCopyright=Copyright © {#MyAppPublisher} {#Year} 
;64位安装路径，32位路径请注释该项
;ArchitecturesInstallIn64BitMode=x64 ia64 

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone

[Files]
Source: "{#WorkPath}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#WorkPath}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

;[Registry]
;Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
;Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
;Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
;Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""
;Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup(): boolean;
var ResultStr: String;
    ResultCode: Integer;
  begin
    if RegQueryStringValue(HKLM, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{{#MyUid}}_is1', 'UninstallString', ResultStr)
      and (MsgBox('您确认要完全卸载后再进行安装吗？', mbConfirmation, MB_YESNO) = IDYES) then
      begin
        ResultStr := RemoveQuotes(ResultStr);
        Exec(ResultStr, '/silent', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
      end;
    result := true;
  end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
  begin
    if CurUninstallStep = usDone then
    begin
      DelTree(ExpandConstant('{app}'), True, True, True);
    end;
  end;