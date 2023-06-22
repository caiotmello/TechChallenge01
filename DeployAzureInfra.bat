@ECHO off

set RESOURCE_GROUP_NAME="rg-tech001"
set STORAGE_ACCOUNT_NAME="tech001"
set SERVER_NAME="azure-sqlserver-tech001"
set DATABASE_NAME="tech001db"
set LOGIN="azureuser"
set PASSWORD="Admin1234!@#$"
set APP_SERVICE_PLAN_NAME="asp-tech001"
set WEBAPP_NAME="caiomello-techc001"

powershell.exe -Executionpolicy ByPass -Command "& {.\Scripts\create-az-storage-account-sql-azuredb-webapp.ps1 '%RESOURCE_GROUP_NAME%' '%STORAGE_ACCOUNT_NAME%' '%SERVER_NAME%' '%DATABASE_NAME%' '%LOGIN%' '%PASSWORD%' '%APP_SERVICE_PLAN_NAME%' '%WEBAPP_NAME%'}"

pause

@ECHO on 