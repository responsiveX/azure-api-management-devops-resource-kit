[CmdletBinding()]
param (
    [string]$settingsFilePath = 'extractorSettings.json',
    [string]$extractorBinaryPath = '../../../azure-api-management-devops-resource-kit/ArmTemplates.exe')

$absScriptDirectoryPath = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Path)
$absSettingsFilePath = [System.IO.Path]::GetFullPath($settingsFilePath)
$absExtractorBinaryPath = [System.IO.Path]::GetFullPath($extractorBinaryPath)

Write-Host
Write-Host "Note:"
Write-Host "  If the extractor fails with a 404 error, your CLI subscription context is likely wrong."
Write-Host "  View your subscription context with the command 'az account show'."
Write-Host "  Set it with the command 'az account set --subscription <subscriptionId>'."

Write-Host
Write-Host "Script Directory = $absScriptDirectoryPath"
Write-Host "Settings File    = $absSettingsFilePath"
Write-Host "Extractor Binary = $absExtractorBinaryPath"
Write-Host
Write-Host "======================"
Write-Host "Launching Extractor..."
Write-Host "======================"
Write-Host

& $absExtractorBinaryPath extract --extractorConfig $absSettingsFilePath
