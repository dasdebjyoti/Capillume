param(
	[Parameter(Mandatory = $true)]
	[string]$InstallerPath
)

$projectFile = Join-Path $PSScriptRoot '..\Capillume.csproj'
[xml]$project = Get-Content -Raw $projectFile
$version = @($project.Project.PropertyGroup | ForEach-Object { $_.Version } | Where-Object { $_ })[0]

if (-not $version) {
	throw "The application version could not be found in '$projectFile'."
}

$destination = Join-Path (Split-Path -Parent $InstallerPath) "CapillumeInstaller_$version.msi"
Move-Item -LiteralPath $InstallerPath -Destination $destination -Force
