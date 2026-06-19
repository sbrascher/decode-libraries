[CmdletBinding()]
param (
    [Parameter(Mandatory = $true, HelpMessage = "Please enter your NuGet API Key.")]
    [string]$ApiKey,

    [Parameter(Mandatory = $false)]
    [string]$Version = "1.0.3",

    [Parameter(Mandatory = $false)]
    [string]$Source = "https://api.nuget.org/v3/index.json"
)

$ErrorActionPreference = "Stop"

Write-Host "--------------------------------------------------" -ForegroundColor Cyan
Write-Host "Starting NuGet Publishing Process for version $Version" -ForegroundColor Cyan
Write-Host "--------------------------------------------------" -ForegroundColor Cyan

# 1. Clean previous packages in nupkg folder to avoid pushing old versions
$NugetFolder = Join-Path $PSScriptRoot "nupkg"
if (Test-Path $NugetFolder) {
    Write-Host "Cleaning existing packages in $NugetFolder..." -ForegroundColor Yellow
    Remove-Item (Join-Path $NugetFolder "*") -Force -ErrorAction SilentlyContinue
} else {
    New-Item -ItemType Directory -Path $NugetFolder | Out-Null
}

# 2. Build and Pack the solution
$SolutionPath = Join-Path $PSScriptRoot "src\Decode.sln"
Write-Host "Building and packing solution: $SolutionPath..." -ForegroundColor Yellow
dotnet pack $SolutionPath -c Release -o $NugetFolder

# 3. Push packages to NuGet
Write-Host "Pushing packages to NuGet source: $Source..." -ForegroundColor Yellow
$Packages = Get-ChildItem -Path $NugetFolder -Filter "*.nupkg"

if ($Packages.Count -eq 0) {
    Write-Error "No .nupkg files found in $NugetFolder. Packing might have failed."
}

foreach ($Package in $Packages) {
    # Skip symbols packages in the main loop since dotnet nuget push handles them automatically
    if ($Package.Name.EndsWith(".symbols.nupkg") -or $Package.Name.EndsWith(".snupkg")) {
        continue
    }
    
    Write-Host "Pushing package: $($Package.Name)..." -ForegroundColor Green
    dotnet nuget push $Package.FullName --api-key $ApiKey --source $Source --skip-duplicate
}

Write-Host "--------------------------------------------------" -ForegroundColor Cyan
Write-Host "NuGet Publishing Process Completed Successfully!" -ForegroundColor Cyan
Write-Host "--------------------------------------------------" -ForegroundColor Cyan
