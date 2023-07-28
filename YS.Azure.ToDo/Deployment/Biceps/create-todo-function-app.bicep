@description('Resources location')
param location string

@description('Hosting plan name')
param hostingPlanName string

@description('Storage account name')
param storageAccountName string

@description('Function application name')
param functionAppName string

@description('Application insight name')
param appInsightName string

@description('')
param kvName string

@description('')
param storageAccountConnectionStringSecretName string

@description('')
param contentContainerName string

@description('')
param cosmosAccountName string

@description('')
param primaryKeySecretName string

@description('')
param connectionStringSecretName string

@description('')
param cosmosDbName string

@description('')
param cosmosContainerName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
}

resource appInsight 'Microsoft.Insights/components@2020-02-02' existing = {
  name: appInsightName
}

resource hostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsight.properties.InstrumentationKey
        }
        {
          name: 'AzureWebJobsSecretStorageType'
          value: 'files'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'Logging:LogLevel:Default'
          value: 'Information'
        }
        {
          name: 'Logging:LogLevel:Microsoft.AspNetCore'
          value: 'Warning'
        }
        {
          name: 'BlobStorageOptions:ConnectionString'
          value: '@Microsoft.KeyVault(VaultName=${kvName};SecretName=${storageAccountConnectionStringSecretName})'
        }
        {
          name: 'BlobStorageOptions:BlobContainerName'
          value: contentContainerName
        }
        {
          name: 'CosmosDbOptions:AccountEndpoint'
          value: 'https://${cosmosAccountName}.documents.azure.com:443/'
        }
        {
          name: 'CosmosDbOptions:AuthKey'
          value: '@Microsoft.KeyVault(VaultName=${kvName};SecretName=${primaryKeySecretName})'
        }
        {
          name: 'SqlDatabaseOptions:ConnectionString'
          value: '@Microsoft.KeyVault(VaultName=${kvName};SecretName=${connectionStringSecretName})'
        }
        {
          name: 'ToDoOptions:CosmosDbName'
          value: cosmosDbName
        }
        {
          name: 'ToDoOptions:CosmosContainerName'
          value: cosmosContainerName
        }
        {
          name: 'ToDoOptions:BlobNameTemplate'
          value: '{0}/{1}.{2}'
        }
      ]
    }
  }
}

resource kv 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: kvName
}

resource keyVaultPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2023-02-01' = {
  parent: kv
  name: 'add'
  properties: {
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: functionApp.identity.principalId
        permissions: {
          certificates: [
            'all'
          ]
          keys: [
            'all'
          ]
          secrets: [
            'all'
          ]
          storage: [
            'all'
          ]
        }
      }
    ]
  }
}
