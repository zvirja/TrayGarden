@echo off
call "%~dp0build.cmd" Publish --configuration Release %*
if errorlevel 1 pause
