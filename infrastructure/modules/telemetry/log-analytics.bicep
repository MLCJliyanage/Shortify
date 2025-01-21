param name string
param location string

resource logAnalyticsWorkspace 'microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: name
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
}

output id string = logAnalyticsWorkspace.id
