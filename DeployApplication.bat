@ECHO off

set RESOURCE_GROUP_NAME=rg-tech01
set WEBAPP_NAME=caiomello-techc01

powershell.exe -Executionpolicy ByPass -Command "& {.\Scripts\publish_app.ps1 '%RESOURCE_GROUP_NAME%' '%WEBAPP_NAME%'}"
powershell.exe -Executionpolicy ByPass -Command "& {.\Scripts\create-tables.ps1}"

pause

@ECHO on 