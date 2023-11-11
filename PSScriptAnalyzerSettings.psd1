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

		PSUseConsistentIndentation = @{
			Enable = $true
			Kind = 'tab'
			PipelineIndentation = 'IncreaseIndentationForFirstPipeline'
			IndentationSize = 4
		}

	}
	ExcludeRules = @(
		'PSUseBOMForUnicodeEncodedFile'
		'PSReviewUnusedParameter'
	)
}
