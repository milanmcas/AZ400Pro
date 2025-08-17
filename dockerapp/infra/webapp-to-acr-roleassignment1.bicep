@description('Set the ACR Pull Role Definition ID')
param acrPullRoleDefinitionID string = '7f951dda-4ed3-4680-a7ca-43fe172d538d'

@description('ACR Name')
param acrName string = 'crxtssaqm3zmud2'

@description('Web App Name')
param webAppName string = 'app-xtssaqm3zmud2'

resource acr 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' existing = {
  name: acrName
}

resource webApp 'Microsoft.Web/sites@2022-03-01' existing = {
  name: webAppName
}

@description('Generate a unique GUID to use as name for the role assignment')
// ✅ Use only compile-time known strings
var roleAssignmentName = guid(acrName, webAppName, acrPullRoleDefinitionID)

resource webAppToAcrRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: roleAssignmentName
  scope: acr
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', acrPullRoleDefinitionID)
    principalId: webApp.identity.principalId
  }
}
