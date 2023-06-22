param (
    $RESOURCE_GROUP_NAME,
    $STORAGE_ACCOUNT_NAME,
    $SERVER_NAME,
    $DATABASE_NAME,
    $LOGIN,
    $PASSWORD,
    $APP_SERVICE_PLAN_NAME,
    $WEBAPP_NAME
)

$REGION="eastus"
$CONTAINER_NAME="image-container"

#Connect to Azure
az login

echo "Creating $RESOURCE_GROUP_NAME in $REGION..."
az group create --name $RESOURCE_GROUP_NAME --location $REGION

echo "Creating storage account $STORAGE_ACCOUNT_NAME..."
az storage account create --resource-group $RESOURCE_GROUP_NAME --name $STORAGE_ACCOUNT_NAME --sku Standard_LRS --allow-blob-public-access true

echo "Getting storage accout key..."
$ACCOUNT_KEY = $(az storage account keys list --resource-group $RESOURCE_GROUP_NAME --account-name $STORAGE_ACCOUNT_NAME --query "[0].value" --output tsv)
echo "access_key: $ACCOUNT_KEY"

#Start-Sleep -Seconds 60

echo "Creating blob container  with container name $CONTAINER_NAME"
echo "az storage container create  --name $CONTAINER_NAME --account-name $STORAGE_ACCOUNT_NAME --account-key $ACCOUNT_KEY --public-access blob"
az storage container create  --name $CONTAINER_NAME --account-name $STORAGE_ACCOUNT_NAME --account-key $ACCOUNT_KEY --public-access blob

##############################################################
echo "Creating SQL Azure Database..."
echo $SERVER_NAME
echo $DATABASE_NAME
echo $LOGIN
echo $PASSWORD

# Specify appropriate IP address values for your environment
# to limit access to the SQL Database server
$LOCALHOST_IP=(Test-Connection -ComputerName $env:COMPUTERNAME -Count 1).IPv4Address.IPAddressToString
$START_IP="0.0.0.0"
$END_IP="0.0.0.0"

echo "Using resource group $RESOURCE_GROUP_NAME with login: $LOGIN, password: $PASSWORD..."
echo "Creating $SERVER_NAME in $REGION..."
az sql server create --name $SERVER_NAME --resource-group $RESOURCE_GROUP_NAME --location $REGION --admin-user $LOGIN --admin-password $PASSWORD

echo "Configuring firewall..."
az sql server firewall-rule create --resource-group $RESOURCE_GROUP_NAME --server $SERVER_NAME --name "AllowAllWindowsAzureIps" --start-ip-address $START_IP --end-ip-address $END_IP
az sql server firewall-rule create --resource-group $RESOURCE_GROUP_NAME --server $SERVER_NAME --name "AllowYourIp" --start-ip-address $LOCALHOST_IP --end-ip-address $LOCALHOST_IP

echo "Creating $DATABASE_NAME on $SERVER_NAME..."
#az sql db create --resource-group $RESOURCE_GROUP_NAME --server $SERVER_NAME --name $DATABASE_NAME --sample-name AdventureWorksLT --edition GeneralPurpose --family Gen5 --capacity 2 --zone-redundant true # zone redundancy is only supported on premium and business critical service tiers
az sql db create --resource-group $RESOURCE_GROUP_NAME --server $SERVER_NAME --name $DATABASE_NAME --edition Basic

###############################################################
echo "Creating AppServicePlan $APP_SERVICE_NAME"
az appservice plan create --name $APP_SERVICE_PLAN_NAME --resource-group $RESOURCE_GROUP_NAME --sku FREE

echo "Creating WebApp $WEBAPP_NAME"
az webapp create --name $WEBAPP_NAME --resource-group $RESOURCE_GROUP_NAME --plan $APP_SERVICE_PLAN_NAME --public-network-access Enabled --runtime "dotnet:6"

#Para exibir a página de exceção detalhada no Serviço de Aplicativo, adicione a ASPNETCORE_ENVIRONMENT configuração do aplicativo ao seu aplicativo
echo "Adding configurations to webapp"
az webapp config appsettings set --name $WEBAPP_NAME --resource-group $RESOURCE_GROUP_NAME --settings ASPNETCORE_ENVIRONMENT="Development"