# This script runs in the Visual Studio Package Manager Console
# to install the NuGet packages required for new AppGenerator apps
# and also to update all currently installed packages to latest versions

# Delete the log file if it exists
$filename=".\InstallPackages.log"
If (Test-Path $filename){ Remove-Item $filename }

# Update all installed packages
Update-Package

# Install all needed packages
Install-Package Microsoft.SqlServer.Compact
Install-Package EntityFramework.SqlServerCompact
Install-Package jQuery.Fullcalendar
Install-Package jQuery.UI.Combined
Install-Package Moment.js
Install-Package bootstrap-select
Install-Package Elmah.Mvc
Install-Package Microsoft.AspNet.WebApi.Core
Install-Package Microsoft.AspNet.WebApi.WebHost

# Update all installed packages
Update-Package

# Create the log file
echo "InstallPackages run completed." > ".\InstallPackages.log"
