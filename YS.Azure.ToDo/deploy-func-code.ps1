# Init variables

$ResourceGroup = 'ToDoResourceGroup'
$Location = 'polandcentral'
$FunctionAppName = 'ystodo'
$SubscriptionName = 'Azure subscription 1'
$PublishFoldier = 'bin/Release/net6.0/publish'
$PublishZip = 'publish.zip'


# Create publish zip file

Write-Output 'CREATING RELEASE...'
dotnet publish -c Release
Write-Output 'RELEASE SUCCESSFULLY CREATED'

if (Test-Path $PublishZip) {
    Write-Output 'CLEARING OLD ZIP RELEASE...'
    Remove-Item $PublishZip
}

Write-Output 'CREATING NEW ZIP RELEASE...'
Add-Type -assembly 'system.io.compression.filesystem'
[io.compression.zipfile]::CreateFromDirectory($PublishFoldier, $PublishZip)


# Login to azure

Write-Output 'LOGIN TO AZURE...'
az login


#Deploy zip to function app

Write-Output 'DEPLOYING ZIP RELEASE TO FUNCTION APP...'
az functionapp deployment source config-zip `
-g $ResourceGroup -n $FunctionAppName --src $PublishZip
Write-Output 'DEPLOYING SUCCESSFULLY FINISED'

# Update function appsettings

#az functionapp config appsettings set `
#-g $ResourceGroup -n $FunctionAppName `
#--settings 'AzureWebJobsStorage=UseDevelopmentStorage=true'

Read-Host -Prompt 'Press any key to continue...'
