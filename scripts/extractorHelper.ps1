[CmdletBinding()]
param (
    [string]$settingsFilePath = 'extractorSettings.template.json',
    [string]$extractorBinaryPath = '../../tools/azure-api-management-devops-resource-kit/ApiManagementExtractor.exe',
    [string[]] $apis = @('device-meter-api-v10', 'game-play-api-v10', 'metrics-api-v10', 'game-api-v10')
)

$absScriptDirectoryPath = $PSScriptRoot
$absSettingsFilePath = Convert-Path -LiteralPath (Join-Path -Path $absScriptDirectoryPath -ChildPath $settingsFilePath)
$absExtractorBinaryPath = Convert-Path -LiteralPath (Join-Path -Path $absScriptDirectoryPath -ChildPath $extractorBinaryPath)

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

$template = Get-Content $settingsFilePath

foreach($api in $apis){ 
    $tempFile = New-TemporaryFile
    try {
        $template -replace '{{apiName}}', $api | Set-Content $tempFile
        & $absExtractorBinaryPath extract --extractorConfig $tempFile
    }
    catch{
        Write-Error "Failed to extract $api"
    }
    finally{
        Remove-Item $tempFile
    }
    
}
