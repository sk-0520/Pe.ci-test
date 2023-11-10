@{
    IncludeDefaultRules = $false
    Rules = @{
        PSAvoidLongLines = @{
            Enable = $false
            MaximumLineLength = 40
        }
        UseBOMForUnicodeEncodedFile = @{
            Enable = $true
        }
    }
    ExcludeRules = @()
}
