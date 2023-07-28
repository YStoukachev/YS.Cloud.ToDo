@description('Key vault name')
param kvName string

@description('Cosmos account name')
param cosmosAccountName string

@description('Resource location')
param location string

@description('Database name')
param databaseName string

@description('Container name')
param containerName string

@description('Cosmos account primary key secret name')
param primaryKeySecretName string

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: cosmosAccountName
  location: location
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        isZoneRedundant: false
        failoverPriority: 0
        locationName: location
      }
    ]
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  parent: cosmosAccount
  name: databaseName
  location: location
  properties: {
    resource: {
      id: databaseName
    }
  }
}

resource container 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  parent: database
  name: containerName
  location: location
  properties: {
    resource: {
      id: containerName
      partitionKey: {
        paths: [ '/id' ]
        kind: 'Hash'
      }
    }
  }
}

resource kv 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: kvName
}

resource primaryKeySecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: kv
  name: primaryKeySecretName
  properties: {
    value: cosmosAccount.listKeys().primaryMasterKey
  }
}
