@echo off

IF "%GO_SDK_4%" == "" GOTO NOPATH
   :YESPATH
   @ECHO The GO_SDK environment variable was detected.
   xcopy "%GO_SDK_4%\bin\win32d\GoSdk.dll" "%CD%\bin\x86\Debug\GoSdk.dll*"
   xcopy "%GO_SDK_4%\bin\win32d\GoSdkNet.dll" "%CD%\bin\x86\Debug\GoSdkNet.dll*"	
   xcopy "%GO_SDK_4%\bin\win32\GoSdk.dll" "%CD%\bin\x86\Release\GoSdk.dll*"
   xcopy "%GO_SDK_4%\bin\win32\GoSdkNet.dll" "%CD%\bin\x86\Release\GoSdkNet.dll*"
   xcopy "%GO_SDK_4%\bin\win32d\kApi.dll" "%CD%\bin\x86\Debug\kApi.dll*"
   xcopy "%GO_SDK_4%\bin\win32d\kApiNet.dll" "%CD%\bin\x86\Debug\kApiNet.dll*"
   xcopy "%GO_SDK_4%\bin\win32\kApi.dll" "%CD%\bin\x86\Release\kApi.dll*"
   xcopy "%GO_SDK_4%\bin\win32\kApiNet.dll" "%CD%\bin\x86\Release\kApiNet.dll*"
   xcopy "%GO_SDK_4%\bin\win64d\GoSdk.dll" "%CD%\bin\x64\Debug\GoSdk.dll*"
   xcopy "%GO_SDK_4%\bin\win64d\GoSdkNet.dll" "%CD%\bin\x64\Debug\GoSdkNet.dll*"
   xcopy "%GO_SDK_4%\bin\win64\GoSdk.dll" "%CD%\bin\x64\Release\GoSdk.dll*"
   xcopy "%GO_SDK_4%\bin\win64\GoSdkNet.dll" "%CD%\bin\x64\Release\GoSdkNet.dll*"
   xcopy "%GO_SDK_4%\bin\win64d\kApi.dll" "%CD%\bin\x64\Debug\kApi.dll*"
   xcopy "%GO_SDK_4%\bin\win64d\kApiNet.dll" "%CD%\bin\x64\Debug\kApiNet.dll*"
   xcopy "%GO_SDK_4%\bin\win64\kApi.dll" "%CD%\bin\x64\Release\kApi.dll*"
   xcopy "%GO_SDK_4%\bin\win64\kApiNet.dll" "%CD%\bin\x64\Release\kApiNet.dll*"
   GOTO END
   :NOPATH
   @ECHO The GO_SDK_4 environment variable was NOT detected.Please set the ENV Variable GO_SDK_4 to point to the Go SDK Directory.
   GOTO END
   :END

pause