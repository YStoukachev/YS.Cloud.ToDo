@description('Key vault name')
param kvName string

@description('Storage account name')
param storageAccountName string

@description('Resource location')
param location string

@description('Queue name')
param queueName string

@description('Storage account connection string secret name')
param storageAccountConnectionStringSecretName string

@description('Content container name')
param contentContainerName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

resource queueService 'Microsoft.Storage/storageAccounts/queueServices@2022-09-01' = {
  name: 'default'
  parent: storageAccount
}

resource queue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-09-01' = {
  name: queueName
  parent: queueService
}

resource kv 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: kvName
}

resource storageConnectionString 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: kv
  name: storageAccountConnectionStringSecretName
  properties: {
    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccount.listKeys().keys[1].value}'
  }
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2022-09-01' = {
  name: 'default'
  parent: storageAccount
}

resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-09-01' = {
  name: contentContainerName
  parent: blobService
}
