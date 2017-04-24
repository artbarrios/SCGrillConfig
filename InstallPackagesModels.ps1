# This script runs in the Visual Studio Package Manager Console
# to install the NuGet packages required for new AppGenerator apps
# and also to update all currently installed packages to latest versions

# Delete the log file if it exists
$filename=".\InstallPackagesModels.log"
If (Test-Path $filename){ Remove-Item $filename }

# Update all installed packages
Update-Package

# Install all needed packages
Install-Package Newtonsoft.Json
Install-Package System.ComponentModel.Annotations

# Update all installed packages
Update-Package

# Create the log file
echo "InstallPackagesModels run completed." > ".\InstallPackagesModels.log"
