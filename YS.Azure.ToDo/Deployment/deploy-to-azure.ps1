Write-Output 'STARTING DEPLOYMENT PROCESS'

$Location = 'polandcentral'
$ResourceGroup = 'ToDoResourceGroupDev'
$FunctionAppName = 'ystododev'
$Env = 'dev'

$DeployInfrastructureScriptPath = './deploy-infrastructure.ps1'
$DeployFuncCodeScriptPath = './deploy-func-code.ps1'

& $DeployInfrastructureScriptPath -Location $Location -ResourceGroup $ResourceGroup -Env $Env
& $DeployFuncCodeScriptPath -Location $Location -ResourceGroup $ResourceGroup -FunctionAppName $FunctionAppName

Read-Host -Prompt 'Press any key to continue...'