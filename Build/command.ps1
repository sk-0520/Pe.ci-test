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

function Set-Command {
    Param ($command, $envName, $defaultPath)

    if ( ! ( Test-CommandExists $command )) {
        $envValue = env:$envName
        if ( $null -eq $envValue) {
            $env:Path += ";" + [Environment]::ExpandEnvironmentVariables($defaultPath)
        } else {
            $envPath += ";" + [Environment]::ExpandEnvironmentVariables($envValue)
        }
    }
}
