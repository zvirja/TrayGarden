@echo off
call "%~dp0build.cmd" Publish --configuration Release %*
if errorlevel 1 (
  pause
  exit /b 1
)
start "" /d "%~dp0.artifacts" "%~dp0.artifacts\TrayGarden.exe"
