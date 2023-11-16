Param(
	[Parameter(mandatory = $true)][string] $AssemblyDirectory,
	[switch] $ProductMode
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

Write-Verbose "AssemblyDirectory = $AssemblyDirectory"
Write-Verbose "ProductMode = $ProductMode"

if ($ProductMode) {
	$removeTargets = @(
		Get-ChildItem -Path $AssemblyDirectory -File -Filter '*.ilk' -Recurse
		Get-ChildItem -Path $AssemblyDirectory -File -Filter '*.iobj' -Recurse
		Get-ChildItem -Path $AssemblyDirectory -File -Filter '*.ipdb' -Recurse
		Get-ChildItem -Path $AssemblyDirectory -File -Filter '*.pdb' -Recurse
	) | Select-Object -ExpandProperty FullName

	foreach ($removeTarget in $removeTargets) {
		Write-Information "remove: $removeTarget"
		Remove-Item $removeTarget -Recurse -Force
	}
}
