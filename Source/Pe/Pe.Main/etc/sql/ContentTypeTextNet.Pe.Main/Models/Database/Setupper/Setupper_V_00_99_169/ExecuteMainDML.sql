--// insert: AppProxySetting
insert into
	[AppProxySetting]
	(
		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion],
		[UpdatedTimestamp],
		[UpdatedAccount],
		[UpdatedProgramName],
		[UpdatedProgramVersion],
		[UpdatedCount],
		[ProxyIsEnabled],
		[ProxyUrl],
		[CredentialIsEnabled],
		[CredentialUser],
		[CredentialPassword]
	)
	values
	(
/* CreatedTimestamp       */ @CreatedTimestamp,
/* CreatedAccount         */ @CreatedAccount,
/* CreatedProgramName     */ @CreatedProgramName,
/* CreatedProgramVersion  */ @CreatedProgramVersion,
/* UpdatedTimestamp       */ @UpdatedTimestamp,
/* UpdatedAccount         */ @UpdatedAccount,
/* UpdatedProgramName     */ @UpdatedProgramName,
/* UpdatedProgramVersion  */ @UpdatedProgramVersion,
/* UpdatedCount           */ 0,
/* ProxyIsEnabled         */ false,
/* ProxyUrl               */ '',
/* CredentialIsEnabled    */ false,
/* CredentialUser         */ '',
/* CredentialPassword     */ ''
	)
;
