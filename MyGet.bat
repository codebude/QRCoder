@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.3.4
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

echo Working dir: %cd%

echo Create template folders

mkdir Build
mkdir Build\lib
mkdir Build\lib\net35
mkdir Build\lib\net40
mkdir Build\lib\netcore
mkdir Build\lib\netstandard2.0

echo Compile single projects

"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild" QRCoder\QRCoder.csproj /p:Configuration="%config%";VisualStudioVersion=14.0 /tv:14.0 /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false /t:Rebuild
copy "QRCoder\bin\%config%\net35\*.dll" "Build\lib\net35"
del /F /S /Q "QRCoder\bin"
del /F /S /Q "QRCoder\obj"

"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild" QRCoder\QRCoder.NET40.csproj /p:Configuration="%config%";VisualStudioVersion=14.0 /tv:14.0 /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false /t:Rebuild
copy "QRCoder\bin\%config%\net40\*.dll" "Build\lib\net40"
del /F /S /Q "QRCoder\bin"
del /F /S /Q "QRCoder\obj"

"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild" QRCoder\QRCoderProject.Portable.csproj /p:Configuration="%config%";VisualStudioVersion=14.0 /tv:14.0 /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false /t:Rebuild
copy "QRCoder\bin\%config%\netcore\*.dll" "Build\lib\netcore"
del /F /S /Q "QRCoder\bin"
del /F /S /Q "QRCoder\obj"

dotnet build /p:Configuration="%config%" QRCoder\QRCoder.NETCore20.csproj
copy "QRCoder\bin\%config%\netstandard2.0\*.dll" "Build\lib\netstandard2.0"
del /F /S /Q "QRCoder\bin"
del /F /S /Q "QRCoder\obj"


echo Assembly information

powershell -Command "[Reflection.Assembly]::ReflectionOnlyLoadFrom(\"%cd%\Build\lib\net35\QRCoder.dll\").ImageRuntimeVersion"
certUtil -hashfile "Build\lib\net35\QRCoder.dll" md5

powershell -Command "[Reflection.Assembly]::ReflectionOnlyLoadFrom(\"%cd%\Build\lib\net40\QRCoder.dll\").ImageRuntimeVersion"
certUtil -hashfile "Build\lib\net40\QRCoder.dll" md5

powershell -Command "[Reflection.Assembly]::ReflectionOnlyLoadFrom(\"%cd%\Build\lib\netcore\QRCoder.dll\").ImageRuntimeVersion"
certUtil -hashfile "Build\lib\netcore\QRCoder.dll" md5

powershell -Command "[Reflection.Assembly]::ReflectionOnlyLoadFrom(\"%cd%\Build\lib\netstandard2.0\QRCoder.dll\").ImageRuntimeVersion"
certUtil -hashfile "Build\lib\netstandard2.0\QRCoder.dll" md5

call %NuGet% pack "QRCoder.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -Version %version% -p Configuration="%config%"
