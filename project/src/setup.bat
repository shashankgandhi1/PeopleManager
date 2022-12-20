@echo off

IF exist ..\bin\ (echo directory already exists && C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc /out:..\bin\Program.exe *.cs) ELSE (mkdir ..\bin\ && C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc /out:..\bin\Program.exe *.cs)

if %errorlevel% neq 0 (
	exit /b %errorlevel%
)

rem cls
rem start ..\bin\Program.exe
