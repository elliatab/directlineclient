@echo off
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
set errorlevel=0
erase /s *.nupkg
msbuild /property:Configuration=release DirectLineClient.csproj
%HOMEPATH%\nuget.exe pack DirectLineClient.csproj -Prop Configuration=Release

