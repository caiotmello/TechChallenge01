@ECHO off

powershell.exe -Executionpolicy ByPass -Command "& {.\Scripts\publish_app.ps1}"

pause

@ECHO on 