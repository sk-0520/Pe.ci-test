
function Initialize-Scm_Git {
    if ( ! ( Test-CommandExists git )) {
        if ( $env:GIT_PATH -eq $null ) {
            $env:Path += ";" + [Environment]::ExpandEnvironmentVariables("%PROGRAMFILES%\git\bin")
        } else {
            $envPath += ";" + [Environment]::ExpandEnvironmentVariables($env:GIT_PATH)
        }
    }

    Install-Module posh-git -Scope CurrentUser -Force
    git --version
}

function Get-ChangedScm_Git {
    return Get-GitStatus
}


