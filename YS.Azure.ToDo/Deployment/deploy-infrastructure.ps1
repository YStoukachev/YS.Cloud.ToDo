param ($Location, $ResourceGroup, $Env)

Write-Output 'LOGIN TO AZURE...'
az login

az group create `
  --name $ResourceGroup `
  --location $Location

az deployment group create `
  --name 'deploying-infrastructure' `
  --resource-group $ResourceGroup `
  --template-file './Biceps/deploy-infrastructure.bicep' `
  --parameters `
  location=$Location `
  env=$Env
