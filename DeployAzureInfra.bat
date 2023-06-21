@ECHO off

powershell.exe -Executionpolicy ByPass -Command "& {.\Scripts\create-az-storage-account-sql-azuredb-webapp.ps1}"

pause
@ECHO on 