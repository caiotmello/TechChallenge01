param (
    $resource_group,
    $webapp_name
)

$DateTime = Get-Date
'Horario Inicial: ' + $DateTime

$dirPublish = "publicacao"

# Força a exclusão de uma pasta para publicação criada anteriormente
if (Test-Path $dirPublish) {
    Remove-Item -Recurse -Force $dirPublish
}

# Realiza a publicação
dotnet publish -c release -o $dirPublish

# Remove um arquivo compactado com a publicação (caso o mesmo tenha sido
# criado anteriormente)
$arqPublish = "publicacao.zip"
if (Test-Path $arqPublish) {
    Remove-Item -Force $arqPublish
}

# Efetua a compressão da pasta com a publicação
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($dirPublish, $arqPublish)

# Efetua o deployment utilizando o arquivo compactado (.zip)
az webapp deployment source config-zip --name $webapp_name --resource-group $resource_group --src $arqPublish

$DateTime = Get-Date
'Horario Final: ' + $DateTime