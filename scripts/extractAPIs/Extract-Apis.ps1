[CmdletBinding()]
param (
    [string]$settingsFileName = 'ExtractorSettings.Api.Template.json',
    [string]$apisToExtractFileName = 'ApisToExtract.txt'
)

# Determine the absolute path to the settings file and APIs to extract file based on the script's location
$absScriptDirectoryPath = $PSScriptRoot
$absSettingsFilePath = Convert-Path -LiteralPath (Join-Path -Path $absScriptDirectoryPath -ChildPath $settingsFileName)
$absApisToExtractFilePath = Convert-Path -LiteralPath (Join-Path -Path $absScriptDirectoryPath -ChildPath $apisToExtractFileName)

# Load APIs to extract from file
$apisToExtract = Get-Content -Path $absApisToExtractFilePath | Where-Object { $_.Trim() -ne '' }

Write-Host
Write-Host "Note:"
Write-Host "  If the extractor fails with a 404 error, your CLI subscription context is likely wrong or expired."
Write-Host "  View your subscription context with the command 'az account show'."
Write-Host "  Set it with the command 'az account set --subscription <subscriptionId>'."

Write-Host
Write-Host "APIs to Extract:"
for ($i = 0; $i -lt $apisToExtract.Count; $i++) {
    Write-Host "  $($i + 1). $($apisToExtract[$i])"
}

$template = Get-Content $absSettingsFilePath

foreach($api in $apisToExtract){ 
    $tempFile = New-TemporaryFile
    try {
        $message = "Extracting API: $api"
        $separator = "=" * $message.Length
        Write-Host
        Write-Host $separator
        Write-Host $message
        Write-Host $separator
        Write-Host

        $template -replace '{{apiName}}', $api | Set-Content $tempFile
        dotnet tool exec ApiManagementExtractor --yes -- extract --extractorConfig $tempFile
    }
    catch{
        Write-Error "Failed to extract $api"
    }
    finally{
        Remove-Item $tempFile
    }
}