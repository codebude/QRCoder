@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.2.3
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild" QRCoderPackageBuild.sln /p:Configuration="%config%";VisualStudioVersion=14.0 /tv:14.0 /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false


mkdir Build
mkdir Build\lib
mkdir Build\lib\net40
mkdir Build\lib\portable-net45+netcore45+wpa81
mkdir Build\lib\xamarinios
mkdir Build\lib\monoandroid
mkdir Build\lib\monotouch

%NuGet% pack "QRCoder\QRCoder.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -Version %version% -p Configuration="%config%"
