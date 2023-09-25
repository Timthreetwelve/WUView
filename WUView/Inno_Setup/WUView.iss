; -----------------------------------------------------------------------------
; Windows Update Viewer (WUView)
; -----------------------------------------------------------------------------

#define MyAppName "Windows Update Viewer"
#define MyAppExeName "WUView.exe"
#define MyCompanyName "T_K"
#define MyPublisherName "Tim Kennedy"
#define CurrentYear GetDateTimeString('yyyy', '/', ':')
#define MyCopyright "Copyright (C) " + CurrentYear + " Tim Kennedy"
#define MyAppNameNoSpaces StringChange(MyAppName, " ", "")
#define MyDateTimeString GetDateTimeString('yyyy/mm/dd hh:nn:ss', '/', ':')

#define BaseDir "D:\Visual Studio\Source\Prod\WUView\WUView"
#define MySourceDir BaseDir + "\bin\Publish"
#define MySetupIcon BaseDir + "\Images\UV.ico"
#define MyAppVersion GetStringFileInfo(MySourceDir + "\" + MyAppExeName, "FileVersion")
#define MyInstallerFilename MyAppNameNoSpaces + "_" + MyAppVersion + "_Setup"
#define MyOutputDir "D:\InnoSetup\Output"
#define MyLargeImage "D:\InnoSetup\Images\WizardImageWUV2.bmp"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
;---------------------------------------------
AppId={{3A152885-8378-4FDE-AFCC-85D096B16A1D}

;---------------------------------------------
; Uncomment the following line to run in non administrative install mode (install for current user only.)
; Installs in %localappdata%\Programs\ instead of \Program Files(x86)
;---------------------------------------------
PrivilegesRequired=lowest
;---------------------------------------------

AllowNoIcons=yes
AppCopyright={#MyCopyright}
AppName={#MyAppName}
AppPublisher={#MyPublisherName}
AppSupportURL=https://github.com/Timthreetwelve/WUView
AppVerName={#MyAppName} {#MyAppVersion}
AppVersion={#MyAppVersion}
Compression=lzma
DefaultDirName={autopf}\{#MyCompanyName}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableDirPage=yes
DisableProgramGroupPage=yes
DisableReadyMemo=no
DisableStartupPrompt=Yes
DisableWelcomePage=no
OutputBaseFilename={#MyInstallerFilename}
OutputDir={#MyOutputDir}
OutputManifestFile={#MyAppName}_{#MyAppVersion}_FileList.txt
SetupIconFile={#MySetupIcon}
SetupLogging=yes
ShowLanguageDialog=yes
SolidCompression=no
SourceDir={#MySourceDir}
UninstallDisplayIcon={app}\{#MyAppExeName}
VersionInfoVersion={#MyAppVersion}
WizardImageFile={#MyLargeImage}
WizardSizePercent=100,100
WizardStyle=modern

[Languages]
; https://jrsoftware.org/ishelp/index.php?topic=languagessection
Name: "en"; MessagesFile: "d:\Visual Studio\Source\Prod\Installer_Languages\Catalan.isl"
Name: "nl"; MessagesFile: "d:\Visual Studio\Source\Prod\Installer_Languages\Default.isl"
Name: "es"; MessagesFile: "d:\Visual Studio\Source\Prod\Installer_Languages\Dutch.isl"
Name: "it"; MessagesFile: "d:\Visual Studio\Source\Prod\Installer_Languages\French.isl"
Name: "fr"; MessagesFile: "d:\Visual Studio\Source\Prod\Installer_Languages\German.isl"
Name: "de"; MessagesFile: "d:\Visual Studio\Source\Prod\Installer_Languages\Italian.isl"
Name: "ca"; MessagesFile: "d:\Visual Studio\Source\Prod\Installer_Languages\Spanish.isl"

[LangOptions]
; https://jrsoftware.org/ishelp/index.php?topic=langoptionssection
DialogFontSize=9
DialogFontName="Segoe UI"
WelcomeFontSize=14
WelcomeFontName="Segoe UI"

[Messages]
; Custom messages have been moved to the *.isl files
;
;WelcomeLabel1=[name] Setup
;WelcomeLabel2=This will install [name/ver] on your computer. \
%n%nIt is recommended that you close all other applications before continuing. \
%n%n%nNote that [name] requires .NET 6.%n
;FinishedHeadingLabel=Setup has Completed.
;FinishedLabel=Setup has finished installing [name] on your computer.

[Files]
Source: "{#MySourceDir}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MySourceDir}\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "{#MySourceDir}\*.json"; Excludes: "usersettings.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MySourceDir}\ReadMe.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MySourceDir}\License.txt"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[InstallDelete]
; Delete these files & folders from previous installs
Type: filesandordirs; Name: "{group}"
Type: files; Name: "{app}\Nlog.config"
Type: files; Name: "{app}\Newtonsoft.Json.dll"

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"

[Registry]
Root: HKCU; Subkey: "Software\{#MyCompanyName}"; Flags: uninsdeletekeyifempty
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Copyright"; ValueData: "{#MyCopyright}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Install Date"; ValueData: "{#MyDateTimeString}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Version"; ValueData: "{#MyAppVersion}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Install Folder"; ValueData: "{autopf}\{#MyCompanyName}\{#MyAppName}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Installer Language"; ValueData:"{language}" ;Flags: uninsdeletekey
; Delete this key from previous installs
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: none; ValueName: "Edition"; Flags: uninsdeletekey deletevalue

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent unchecked shellexec
Filename: "{app}\ReadMe.txt"; Description: "{cm:ViewReadme}"; Flags: nowait postinstall skipifsilent unchecked shellexec

[UninstallRun]
Filename: "{sys}\taskkill.exe"; Parameters: "/im {#MyAppExeName} /t /f"; RunOnceId: "DelService"; Flags: runhidden skipifdoesntexist

[UninstallDelete]
Type: files; Name: "{app}\*.log"

[ThirdParty]
CompileLogFile=D:\InnoSetup\Logs\log.txt

; -----------------------------------------------------------------------------
; Code section follows
; -----------------------------------------------------------------------------
[Code]
// function used to check if app is currently running
function IsAppRunning(const FileName : string): Boolean;
var
    FSWbemLocator: Variant;
    FWMIService   : Variant;
    FWbemObjectSet: Variant;
begin
    Result := false;
    FSWbemLocator := CreateOleObject('WBEMScripting.SWBEMLocator');
    FWMIService := FSWbemLocator.ConnectServer('', 'root\CIMV2', '', '');
    FWbemObjectSet :=
      FWMIService.ExecQuery(
        Format('SELECT Name FROM Win32_Process Where Name="%s"', [FileName]));
    Result := (FWbemObjectSet.Count > 0);
    FWbemObjectSet := Unassigned;
    FWMIService := Unassigned;
    FSWbemLocator := Unassigned;
end;

// Checks if app is running, if so, displays msgbox asking to close running app
function InitializeSetup(): Boolean;
var
  Answer: Integer;
  ThisApp: String;
begin
  Result := true;
  ThisApp := ExpandConstant('{#MyAppExeName}');
  while IsAppRunning(ThisApp) do
  begin
        Answer := MsgBox(ThisApp + ' ' + CustomMessage('AppIsRunning'), mbError, MB_OKCANCEL);
    If Answer = IDCANCEL then
    begin
      Result := false;
      Exit;
    end;
  end;
end;

// Copies setup log to app folder
procedure CurStepChanged(CurStep: TSetupStep);
var
  logfilepathname, newfilepathname: string;
begin
    if CurStep = ssDone then
    begin
      logfilepathname := ExpandConstant('{log}');
      newfilepathname := ExpandConstant('{app}\') + 'Setup_Log.txt';
      Log('Setup log file copied to: ' + newfilepathname);
      FileCopy(logfilepathname, newfilepathname, False);
   end;
end;

// Uninstall
procedure CurUninstallStepChanged (CurUninstallStep: TUninstallStep);
var
  mres : integer;
begin
    case CurUninstallStep of
      usPostUninstall:
        begin
          mres := MsgBox(CustomMessage('DeleteConfigFiles'), mbConfirmation, MB_YESNO or MB_DEFBUTTON2)
          if mres = IDYES then
          begin
            DelTree(ExpandConstant('{app}\*.json'), False, True, False);
            DelTree(ExpandConstant('{app}'), True, True, True);
          end;
       end;
   end;
end;