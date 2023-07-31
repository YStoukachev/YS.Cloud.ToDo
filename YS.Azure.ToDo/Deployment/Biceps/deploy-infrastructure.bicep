@description('Resources location')
param location string = 'polandcentral'

@description('Deployment environment')
param env string = 'dev'

var keyValutName = 'ystodokeyvault${env}'
var queueName = 'ys-to-do-queue'
var storageAccountName = 'ysstorageaccount${env}'
var contentContainerName = 'task-content'
var storageAccountConnectionStringSecretName = 'StorageAccountSecret'
var cosmosDbName = 'todo'
var cosmosContainerName = 'tasks'
var cosmosAccountName = 'yscosmosaccount${env}'
var primaryKeySecretName = 'CosmosAccountPrimaryKeySecret'
var sqlDatabaseLogin = 'ys.admin'
var sqlDatabasePassword = 'qwerty123!'
var sqlDatabaseName = 'todo${env}'
var sqlServerName = 'ystodo${env}'
var sqlConnectionSecretName = 'SqlConnectionSecret'
var appInsightName = 'ystodo${env}'
var todoFunctionAppName = 'ystodo${env}'
var todoHostingPlanName = 'ystodohostingplan${env}'
var appInsightLocation = 'eastus'
var apiManagementName = 'ystodoapimanagement${env}'
var publisherEmail = 'y.stukachou@godeltech.com'
var publisherName = 'Yaraslau Stukachou'

module kv 'create-key-vault.bicep' = {
  name: 'deploy-key-vault'
  params: {
    keyVaultName: keyValutName
    location: location
  }
}

module storageAccount 'create-storage-account.bicep' = {
  name: 'deploy-storage-account'
  params: {
    kvName: keyValutName
    contentContainerName: contentContainerName
    location: location
    queueName: queueName
    storageAccountName: storageAccountName
    storageAccountConnectionStringSecretName: storageAccountConnectionStringSecretName
  }
  dependsOn: [
    kv
  ]
}

module cosmosAccount 'create-cosmos-account.bicep' = {
  name: 'deploy-cosmos-account'
  params: {
    kvName: keyValutName
    location: location
    containerName: cosmosContainerName
    databaseName: cosmosDbName
    cosmosAccountName: cosmosAccountName
    primaryKeySecretName: primaryKeySecretName
  }
  dependsOn: [
    kv
  ]
}

module sqlDatabase 'create-sql-database.bicep' = {
  name: 'deploy-sql-database'
  params: {
    kvName: keyValutName
    location: location
    login: sqlDatabaseLogin
    password: sqlDatabasePassword
    sqlDatabaseName: sqlDatabaseName
    sqlServerName: sqlServerName
    connectionStringSecretName: sqlConnectionSecretName
  }
  dependsOn: [
    kv
  ]
}

module appInsight 'create-application-insight.bicep' = {
  name: 'deploy-app-insight'
  params: {
    location: appInsightLocation
    name: appInsightName
  }
}

module todoFunctionApp 'create-todo-function-app.bicep' = {
  name: 'deploy-todo-function-app'
  params: {
    appInsightName: appInsightName
    location: location
    storageAccountName: storageAccountName
    functionAppName: todoFunctionAppName
    hostingPlanName: todoHostingPlanName
    connectionStringSecretName: sqlConnectionSecretName
    contentContainerName: contentContainerName
    cosmosAccountName: cosmosAccountName
    cosmosContainerName: cosmosContainerName
    cosmosDbName: cosmosDbName
    kvName: keyValutName
    primaryKeySecretName: primaryKeySecretName
    storageAccountConnectionStringSecretName: storageAccountConnectionStringSecretName
  }
  dependsOn: [
    storageAccount
    appInsight
  ]
}

module apiManagement 'create-api-management.bicep' = {
  name: 'deploy-api-management'
  params: {
    location: location
    apiManagementName: apiManagementName
    publisherEmail: publisherEmail
    publisherName: publisherName
  }
}
