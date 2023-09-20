The extractorHelper.ps1 and ExtractApiManagement.bat files are designed to be placed in the same directory with each extractorSettings.json file.

The extractorHelper.ps1 file works out the correct paths and launches the extractor executable (ArmTemplates.exe) with the necessary parameters.
The default PowerShell parameter value $extractorBinaryPath should be adjusted for your environment to point to wherever the ArmTemplates.exe
binary is located. This can be a relative or absolute path.

The ExtractApiManagement.bat file is a simple wrapper around the PowerShell script. It is provided for convenience and can be used to run the
extractor with a simple double-click.

In the extractorSettings.json file, the fileFolder setting should be configured as a relative path (such as "./") so that the generated ARM template
files are placed in the correct location relative to the extractorSettings.json file. This is important because each developer downloads the repository
to a different location on their machine.