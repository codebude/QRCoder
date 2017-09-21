echo Working dir: %cd%

certUtil -hashfile "QRCoder\bin\Release\net35\QRCoder.dll" md5
certUtil -hashfile "QRCoder\bin\Release\net40\QRCoder.dll" md5
certUtil -hashfile "QRCoder\bin\Release\netcore\QRCoder.dll" md5
certUtil -hashfile "QRCoder\bin\Release\netstandard2.0\QRCoder.dll" md5

powershell -Command "[Reflection.Assembly]::ReflectionOnlyLoadFrom(\"%cd%\QRCoder\bin\Release\net35\QRCoder.dll\").ImageRuntimeVersion"
powershell -Command "[Reflection.Assembly]::ReflectionOnlyLoadFrom(\"%cd%\QRCoder\bin\Release\net40\QRCoder.dll\").ImageRuntimeVersion"
powershell -Command "[Reflection.Assembly]::ReflectionOnlyLoadFrom(\"%cd%\QRCoder\bin\Release\netcore\QRCoder.dll\").ImageRuntimeVersion"
powershell -Command "[Reflection.Assembly]::ReflectionOnlyLoadFrom(\"%cd%\QRCoder\bin\Release\netstandard2.0\QRCoder.dll\").ImageRuntimeVersion"

pause
