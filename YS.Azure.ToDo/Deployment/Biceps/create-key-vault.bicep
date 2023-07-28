@description('Key vault name')
param keyVaultName string

@description('Key vault location')
param location string

@description('Specifies whether the key vault is a standard vault or a premium vault.')
@allowed([
  'standard'
  'premium'
])
param skuName string = 'standard'

@description('Tenant id')
param tenantId string = subscription().tenantId

resource kv 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: keyVaultName
  location: location
  properties: {
    tenantId: tenantId
    sku: {
      name: skuName
      family: 'A'
    }
    accessPolicies: [
      {
        tenantId: tenantId
        objectId: tenantId
        permissions: {
          secrets: [
            'list'
          ]
          keys: [
            'list'
          ]
          storage: [
            'list'
          ]
          certificates: [
            'list'
          ]
        }
      }
    ]
  }
}

output kvName string = keyVaultName
