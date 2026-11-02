@echo off
call "%~dp0build.cmd" Publish --configuration Release %*
if %errorlevel% neq 0 (
  pause
  exit /b 1
)
start "" /d "%~dp0.artifacts" "%~dp0.artifacts\TrayGarden.Host.exe"
