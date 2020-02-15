function Test-CommandExists {
    Param ($command)

    $oldPreference = $ErrorActionPreference

    $ErrorActionPreference = 'stop'

    try {
        if (Get-Command $command) {
            return $true
        }
    }
    catch {
        return $false
    }
    finally {
        $ErrorActionPreference = $oldPreference
    }
}

function Set-Command($command, $envName, $defaultPath) {

    if ( ! ( Test-CommandExists $command )) {
        #$envValue = env:$envName
        $envValue = Get-ChildItem env: | Where-Object {$_.Name -match $envName} | Select-Object -Property Value -First 1
        if ( $null -eq $envValue) {
            $env:Path += ";" + [Environment]::ExpandEnvironmentVariables($defaultPath)
        } else {
            $envPath += ";" + [Environment]::ExpandEnvironmentVariables($envValue)
        }
    }
}

function TestAliasExists([string] $alias) {
    $oldPreference = $ErrorActionPreference

    $ErrorActionPreference = 'stop'

    try {
        if (Get-Alias $alias) {
            return $true
        }
    }
    catch {
        return $false
    }
    finally {
        $ErrorActionPreference = $oldPreference
    }
}
