param location string = resourceGroup().location
param appServicePlanName string
param appName string
param keyVaultName string
param appSettings array = []
param logAnalyticsWorkspaceId string

module appInsights '../telemetry/app-insights.bicep' = {
  name: '${appName}AppInsightsDeployment'
  params: {
    name: 'app-insights-${appName}'
    location: location
    logAnalyticsWorkspaceId: logAnalyticsWorkspaceId
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  kind: 'linux'
  location: location
  name: appServicePlanName
  properties: {
    reserved: true
  }
  sku: {
    name: 'F1'
  }
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: appName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      appSettings: concat(
        [
          {
            name: 'keyVaultName'
            value: keyVaultName
          }
          {
            name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
            value: appInsights.outputs.instrumentationKey
          }
          {
            name: 'APPINSIGHTS_CONNECTIONSTRING'
            value: appInsights.outputs.connectionString
          }
        ],
        appSettings
      )
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource webAppConfig 'Microsoft.Web/sites/config@2023-12-01' = {
  parent: webApp
  name: 'web'
  properties: {
    scmType: 'GitHub'
  }
}

output appServiceId string = webApp.id
output principalId string = webApp.identity.principalId
output url string = 'https://${webApp.properties.defaultHostName}'
