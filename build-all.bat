@echo off
setlocal EnableDelayedExpansion
SET MSBUILD_CONFIG=Release
SET PLATFORM="Any CPU"
SET VCVARS_PLATFORM=x86_amd64

Echo Choose Visual Studio version to compile with (must be installed):
Echo.
Echo 1. Visual Studio 2015
Echo 2. Visual Studio 2017
Echo.
SET QUERY=""
SET /P QUERY=Choose compiler (1 or 2): 
IF /I "%QUERY%"=="1" (
	call .\set-env-2015.bat
	GOTO BUILD
)
IF /I "%QUERY%"=="2" (
	call .\set-env-2017.bat
	GOTO BUILD
)
IF /I "%QUERY%" NEQ "1" (
	IF /I "%QUERY%" NEQ "2" (
		Echo Invalid choice.
		Echo.
		GOTO CHOOSE
	)
)

GOTO BUILD

:BUILD
ECHO --------------------------------
Echo Building Ablaze...
Echo.
msbuild.exe /m /p:Configuration=%MSBUILD_CONFIG% /p:Platform=%PLATFORM% Ablaze.sln
cd Tools
msbuild.exe /m /p:Configuration=%MSBUILD_CONFIG% /p:Platform=%PLATFORM% Ablaze.Tools.sln
cd ..
cd Demos\Particles\AForge.Net
msbuild.exe /m /p:Configuration=%MSBUILD_CONFIG% /p:Platform=%PLATFORM% AForge.Net.sln
cd ..\..
msbuild.exe /m /p:Configuration=%MSBUILD_CONFIG% /p:Platform=%PLATFORM% Ablaze.Demos.sln
cd ..\..\..
pause