@{
	IncludeDefaultRules = $false
	Rules = @{
		PSAvoidLongLines = @{
			Enable = $false
		}

		PSPlaceOpenBrace = @{
			Enable = $true
			IgnoreOneLineBlock = $true
			NoEmptyLineBefore = $true
			NewLineAfter = $true
		}

	}
	ExcludeRules = @(
		'PSUseBOMForUnicodeEncodedFile'
		'PSReviewUnusedParameter'
	)
}
