[CmdletBinding()]
param (
    [string]$settingsFileName = 'ExtractorSettings.Global.json'
)

# Determine the absolute path to the settings file based on the script's location
$absScriptDirectoryPath = $PSScriptRoot
$absSettingsFileName = Convert-Path -LiteralPath (Join-Path -Path $absScriptDirectoryPath -ChildPath $settingsFileName)

Write-Host
Write-Host "Note:"
Write-Host "  If the extractor fails with a 404 error, your CLI subscription context is likely wrong or expired."
Write-Host "  View your subscription context with the command 'az account show'."
Write-Host "  Set it with the command 'az account set --subscription <subscriptionId>'."

Write-Host
Write-Host "================================"
Write-Host "Extracting APIM Global Resources"
Write-Host "================================"
Write-Host

dotnet tool exec ApiManagementExtractor --yes -- extract --extractorConfig $absSettingsFileName