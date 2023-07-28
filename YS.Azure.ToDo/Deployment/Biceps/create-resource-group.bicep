@description('Resource group name')
param resourceGroupName string

@description('Resource location')
param location string

targetScope = 'subscription'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: resourceGroupName
  location: location
}
