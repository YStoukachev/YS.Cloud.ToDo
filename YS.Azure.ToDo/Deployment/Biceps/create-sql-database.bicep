@description('Key vault name')
param kvName string

@description('Resource location')
param location string

@description('Sql server name')
param sqlServerName string

@description('Sql database name')
param sqlDatabaseName string

@description('Sql server administrator login')
param login string

@secure()
@description('Sql server administrator password')
param password string

@description('Sql DB connection string secret name')
param connectionStringSecretName string

resource sqlServer 'Microsoft.Sql/servers@2022-11-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: login
    administratorLoginPassword: password
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2022-11-01-preview' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

resource kv 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: kvName
}

resource connectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: kv
  name: connectionStringSecretName
  properties: {
    value: 'Server=tcp:${sqlServerName}.database.windows.net,1433;Initial Catalog=${sqlDatabaseName};Persist Security Info=False;User ID=${login};Password=${password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
  }
}
