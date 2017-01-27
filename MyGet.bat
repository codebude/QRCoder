@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.2.5
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild" QRCoderPackageBuild.sln /p:Configuration="%config%";VisualStudioVersion=14.0 /tv:14.0 /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false

mkdir Build
mkdir Build\lib
mkdir Build\lib\net35
mkdir Build\lib\net40
mkdir Build\lib\portable-net45+netcore45+wpa81+wp81+wp8+uap
mkdir Build\lib\uap10.0
mkdir Build\lib\xamarinios
mkdir Build\lib\monoandroid
mkdir Build\lib\monotouch

echo %cd%
dir
certUtil -hashfile "QRCoder\bin\Release\net35\QRCoder.dll" md5
certUtil -hashfile "QRCoder\bin\Release\net40\QRCoder.dll" md5
certUtil -hashfile "QRCoder\bin\Release\netcore\QRCoder.dll" md5


%NuGet% pack "QRCoder\QRCoder.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -Version %version% -p Configuration="%config%"
