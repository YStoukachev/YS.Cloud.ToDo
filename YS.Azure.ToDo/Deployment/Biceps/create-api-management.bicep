@description('Api management name')
param apiManagementName string

@description('Resource location')
param location string

@description('Publisher e-mail')
param publisherEmail string

@description('Publisher name')
param publisherName string

resource apiManagement 'Microsoft.ApiManagement/service@2023-03-01-preview' = {
  name: apiManagementName
  location: location
  sku: {
    name: 'Developer'
    capacity: 1
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
  }
}

// resource apis 'Microsoft.ApiManagement/service/apis@2023-03-01-preview'
