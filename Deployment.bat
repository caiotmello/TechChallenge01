@ECHO off

powershell.exe -Executionpolicy ByPass -Command "& {.\Scripts\create-az-storage-account-sql-azuredb-webapp.ps1}"
powershell.exe -Executionpolicy ByPass -Command "& {.\Scripts\publish_app.ps1}"

pause

@ECHO on 