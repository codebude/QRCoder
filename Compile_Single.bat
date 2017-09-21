"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild" QRCoder\QRCoder.csproj /p:Configuration="Release";VisualStudioVersion=14.0 /tv:14.0 /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false /t:Rebuild
"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild" QRCoder\QRCoder.NET40.csproj /p:Configuration="Release";VisualStudioVersion=14.0 /tv:14.0 /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false /t:Rebuild
"C:\Program Files (x86)\MSBuild\14.0\bin\msbuild" QRCoder\QRCoderProject.Portable.csproj /p:Configuration="Release";VisualStudioVersion=14.0 /tv:14.0 /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false /t:Rebuild
dotnet build /p:Configuration="Release" QRCoder\QRCoder.NETCore20.csproj
pause
