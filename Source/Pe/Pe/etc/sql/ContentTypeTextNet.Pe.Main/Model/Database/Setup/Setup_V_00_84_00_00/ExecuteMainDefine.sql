--// create table: AppSystems
create table [AppSystems] (
	[Key] text not null,
	[CreatedKind] text not null,
	[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
	[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
	[Value] text not null,
	[Note] text not null,
	primary key (
		[Key]
	)
);

--// create table:




