#Requires -Version 5.1
<#
.SYNOPSIS
    MachineryManagerEnterprise - Codebase Snapshot Generator v3
.DESCRIPTION
    Scans project directory, captures tree structure, concatenates source files
    into a single text file for AI consumption.

    EXCLUDES: All markdown (.md), .gitignore, build artifacts, dependencies,
    hidden folders, IDE folders, client libraries, the script itself, and binaries.
.PARAMETER SourcePath
    Root directory. Defaults to current directory.
.PARAMETER OutputPath
    Output file path. Defaults to ./MME_Codebase_Snapshot.txt
.PARAMETER MaxFileSizeKB
    Max file size in KB. Default: 500.
.PARAMETER ExcludeBlazorTemplates
    If set, excludes scaffolded Blazor template files (Counter, Weather, Home, etc.)
#>
[CmdletBinding()]
param(
    [string]$SourcePath = ".",
    [string]$OutputPath = ".\MME_Codebase_Snapshot.txt",
    [int]$MaxFileSizeKB = 500,
    [switch]$ExcludeBlazorTemplates
)

# =============================================================================
# EXCLUDED DIRECTORIES
# =============================================================================
$ExcludedDirectories = @(
    '.git', '.github', '.vs', '.vscode', '.idea', '.azure', '.devcontainer',
    'bin', 'obj', 'packages', 'publish', 'out', 'release', 'releases',
    'debug', 'x64', 'x86', 'arm64', 'artifacts', '.artifacts',
    'node_modules', 'dist', 'build', 'bower_components', 'jspm_packages',
    'docs-english', 'docs-farsi', 'docs',
    'tools', 'test-results', 'coverage', '.coverage', 'TestResults',
    'lib',
    '.angular', '.next', '.nuxt', '.svelte-kit', '.cache', '.temp', 'tmp'
)

# =============================================================================
# INCLUDED FILE EXTENSIONS
# =============================================================================
$IncludedExtensions = @(
    '.cs', '.cshtml', '.razor',
    '.csproj', '.sln', '.slnx', '.props', '.targets', '.csproj.user',
    '.json', '.xml', '.config', '.ini', '.yml', '.yaml',
    '.http', '.rest',
    '.txt', '.env', '.env.example',
    '.gitattributes', '.editorconfig',
    'Dockerfile', '.dockerignore', 'docker-compose.yml',
    '.sh', '.ps1', '.bat', '.cmd'
)

# =============================================================================
# EXCLUDED FILE EXTENSIONS
# =============================================================================
$ExcludedExtensions = @(
    '.md', '.markdown', '.mdx'
)

# =============================================================================
# EXCLUDED FILE NAMES (exact match)
# =============================================================================
$ExcludedFileNames = @(
    'MME_AI_Reference.md',
    'MME_Codebase_Snapshot.txt',
    '.gitignore',
    'Generate-CodebaseSnapshot.ps1',   # <-- Exclude the script itself
    'package-lock.json', 'yarn.lock', 'pnpm-lock.yaml', 'project.lock.json',
    'packages.config',
    'Thumbs.db', 'Desktop.ini', '.DS_Store'
)

# =============================================================================
# BLAZOR TEMPLATE FILES (scaffolded by dotnet new blazor)
# =============================================================================
$BlazorTemplateFiles = @(
    'Counter.razor',
    'Weather.razor',
    'Home.razor',
    'NavMenu.razor',
    'MainLayout.razor',
    'App.razor',
    'Routes.razor',
    'ReconnectModal.razor',
    'Error.razor',
    'NotFound.razor'
)

# =============================================================================
# BINARY EXTENSIONS
# =============================================================================
$BinaryExtensions = @(
    '.exe', '.dll', '.pdb', '.so', '.dylib', '.lib', '.a', '.obj', '.o',
    '.zip', '.rar', '.7z', '.tar', '.gz', '.bz2', '.xz',
    '.jpg', '.jpeg', '.png', '.gif', '.bmp', '.svg', '.ico', '.webp', '.tiff',
    '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.ppt', '.pptx',
    '.mp3', '.mp4', '.avi', '.mov', '.wav', '.ogg', '.flv', '.wmv',
    '.ttf', '.otf', '.woff', '.woff2', '.eot',
    '.db', '.sqlite', '.sqlite3', '.db-journal',
    '.nupkg', '.snupkg'
)

# =============================================================================
# FUNCTIONS
# =============================================================================

function Test-ShouldIncludeFile {
    param([System.IO.FileInfo]$File)
    $ext = $File.Extension.ToLower()
    $name = $File.Name

    if ($BinaryExtensions -contains $ext) { return $false }
    if ($ExcludedExtensions -contains $ext) { return $false }
    if ($ExcludedFileNames -contains $name) { return $false }
    if ($ExcludeBlazorTemplates -and ($BlazorTemplateFiles -contains $name)) { return $false }
    if ($IncludedExtensions -contains $ext) { return $true }
    if ([string]::IsNullOrEmpty($ext)) { return $true }
    return $false
}

function Test-ShouldExcludeDirectory {
    param([System.IO.DirectoryInfo]$Dir)
    $n = $Dir.Name
    if ($n.StartsWith('.')) { return $true }
    foreach ($ex in $ExcludedDirectories) {
        if ($n -eq $ex) { return $true }
    }
    return $false
}

function Get-DirectoryTree {
    param([string]$Path, [string]$Prefix = "")
    $items = Get-ChildItem -Path $Path -Force -ErrorAction SilentlyContinue
    $dirs = @($items | Where-Object { $_.PSIsContainer } | Sort-Object Name | Where-Object { -not (Test-ShouldExcludeDirectory -Dir $_) })
    $files = @($items | Where-Object { -not $_.PSIsContainer } | Sort-Object Name | Where-Object { Test-ShouldIncludeFile -File $_ })
    $all = $dirs + $files
    $count = $all.Count
    $result = @()
    for ($i = 0; $i -lt $count; $i++) {
        $item = $all[$i]
        $isLast = ($i -eq $count - 1)
        $conn = if ($isLast) { "+-- " } else { "|-- " }
        $childPref = if ($isLast) { "    " } else { "|   " }
        $result += "$Prefix$conn$($item.Name)"
        if ($item.PSIsContainer) {
            $result += Get-DirectoryTree -Path $item.FullName -Prefix "$Prefix$childPref"
        }
    }
    return $result
}

# =============================================================================
# MAIN
# =============================================================================

$Stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$SourcePath = Resolve-Path $SourcePath
$OutputPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutputPath)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  MME Codebase Snapshot Generator v3" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Source:  $SourcePath" -ForegroundColor Gray
Write-Host "Output:  $OutputPath" -ForegroundColor Gray
Write-Host "MaxFileSize: ${MaxFileSizeKB} KB" -ForegroundColor Gray
if ($ExcludeBlazorTemplates) {
    Write-Host "ExcludeBlazorTemplates: YES" -ForegroundColor Yellow
}
Write-Host ""

"" | Set-Content -Path $OutputPath -Encoding UTF8 -Force

$Header = @"
================================================================================
  MACHINERYMANAGERENTERPRISE - CODEBASE SNAPSHOT
  Generated: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
  Source: $SourcePath
  Purpose: Complete source code reference for AI-assisted development
  Excludes: .md, .gitignore, this script, build artifacts, binaries, IDE,
            dependencies, client libs (wwwroot/lib), docs
  $(if ($ExcludeBlazorTemplates) { "  Also excludes: Blazor template files (Counter, Weather, Home, etc.)" })
================================================================================

"@
Add-Content -Path $OutputPath -Value $Header -Encoding UTF8

# SECTION 1: Directory Tree
Write-Host "[1/3] Building directory tree..." -ForegroundColor Yellow
Add-Content -Path $OutputPath -Value "# SECTION 1: PROJECT STRUCTURE (Directory Tree)" -Encoding UTF8
Add-Content -Path $OutputPath -Value "================================================================================" -Encoding UTF8
Add-Content -Path $OutputPath -Value "" -Encoding UTF8

$rootDir = Get-Item $SourcePath
$treeLines = @($rootDir.Name)
$treeLines += Get-DirectoryTree -Path $SourcePath
foreach ($line in $treeLines) { Add-Content -Path $OutputPath -Value $line -Encoding UTF8 }
Add-Content -Path $OutputPath -Value "" -Encoding UTF8
Add-Content -Path $OutputPath -Value "================================================================================" -Encoding UTF8
Add-Content -Path $OutputPath -Value "" -Encoding UTF8

# SECTION 2: File Contents
Write-Host "[2/3] Scanning and collecting file contents..." -ForegroundColor Yellow
Add-Content -Path $OutputPath -Value "# SECTION 2: SOURCE FILE CONTENTS" -Encoding UTF8
Add-Content -Path $OutputPath -Value "================================================================================" -Encoding UTF8
Add-Content -Path $OutputPath -Value "" -Encoding UTF8

$allFiles = Get-ChildItem -Path $SourcePath -Recurse -File -Force -ErrorAction SilentlyContinue | Where-Object {
    $parent = $_.DirectoryName
    $shouldExclude = $false
    $pathParts = $parent -split '\\'
    foreach ($part in $pathParts) {
        if ($part.StartsWith('.')) {
            $shouldExclude = $true
            break
        }
        foreach ($exDir in $ExcludedDirectories) {
            if ($part -eq $exDir) {
                $shouldExclude = $true
                break
            }
        }
        if ($shouldExclude) { break }
    }
    -not $shouldExclude
} | Where-Object { Test-ShouldIncludeFile -File $_ } | Sort-Object FullName

$totalFiles = $allFiles.Count
$processedFiles = 0
$skippedFiles = 0
$totalSize = 0

foreach ($file in $allFiles) {
    $processedFiles++
    $relativePath = $file.FullName.Substring($SourcePath.Length).TrimStart('\','/')
    $sizeKB = [math]::Round($file.Length / 1KB, 2)
    $totalSize += $file.Length

    if ($processedFiles % 50 -eq 0 -or $processedFiles -eq 1 -or $processedFiles -eq $totalFiles) {
        $pct = [math]::Round(($processedFiles / $totalFiles) * 100, 1)
        Write-Host "      Progress: $processedFiles / $totalFiles ($pct%) - $relativePath" -ForegroundColor DarkGray
    }

    $fileHeader = "`n================================================================================`nFILE: $relativePath`nSIZE: $sizeKB KB`n================================================================================"
    Add-Content -Path $OutputPath -Value $fileHeader -Encoding UTF8

    if ($sizeKB -gt $MaxFileSizeKB) {
        Add-Content -Path $OutputPath -Value "[FILE TOO LARGE - $sizeKB KB exceeds ${MaxFileSizeKB} KB limit. Skipped.]" -Encoding UTF8
        $skippedFiles++
        continue
    }

    try {
        $reader = [System.IO.StreamReader]::new($file.FullName, $true)
        $content = $reader.ReadToEnd()
        $reader.Close()
        if ([string]::IsNullOrWhiteSpace($content)) {
            Add-Content -Path $OutputPath -Value "[EMPTY FILE]" -Encoding UTF8
        } else {
            Add-Content -Path $OutputPath -Value $content -Encoding UTF8
        }
    } catch {
        Add-Content -Path $OutputPath -Value "[ERROR READING FILE: $_]" -Encoding UTF8
        $skippedFiles++
    }
}

# SECTION 3: Summary
Write-Host "[3/3] Finalizing snapshot..." -ForegroundColor Yellow
$totalSizeMB = [math]::Round($totalSize / 1MB, 2)
$outputSizeMB = [math]::Round((Get-Item $OutputPath).Length / 1MB, 2)
$Stopwatch.Stop()

$Summary = @"

================================================================================
# SECTION 3: SNAPSHOT SUMMARY
================================================================================

Generation Time     : $($Stopwatch.Elapsed.ToString())
Total Files Scanned : $totalFiles
Files Included      : $($totalFiles - $skippedFiles)
Files Skipped       : $skippedFiles
Total Source Size   : $totalSizeMB MB
Snapshot File Size  : $outputSizeMB MB
Output Path         : $OutputPath

Excluded Directories: $($ExcludedDirectories -join ', ')
Excluded Extensions : $($ExcludedExtensions -join ', ')
Excluded File Names : $($ExcludedFileNames -join ', ')
$(if ($ExcludeBlazorTemplates) { "Excluded Templates  : $($BlazorTemplateFiles -join ', ')" })
Max File Size       : ${MaxFileSizeKB} KB

================================================================================
END OF SNAPSHOT
================================================================================
"@
Add-Content -Path $OutputPath -Value $Summary -Encoding UTF8

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  SNAPSHOT COMPLETE!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Files processed : $totalFiles" -ForegroundColor White
Write-Host "Files included  : $($totalFiles - $skippedFiles)" -ForegroundColor White
Write-Host "Files skipped   : $skippedFiles" -ForegroundColor $(if ($skippedFiles -gt 0) { 'Yellow' } else { 'White' })
Write-Host "Source size     : $totalSizeMB MB" -ForegroundColor White
Write-Host "Snapshot size   : $outputSizeMB MB" -ForegroundColor White
Write-Host "Time elapsed    : $($Stopwatch.Elapsed.ToString())" -ForegroundColor White
Write-Host ""
Write-Host "Output file:" -ForegroundColor Cyan
Write-Host "  $OutputPath" -ForegroundColor Cyan
Write-Host ""
Write-Host "Usage: Upload this file along with MME_AI_Reference.md to any AI model." -ForegroundColor Gray
Write-Host ""
