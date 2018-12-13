--// create table: AppSystems
create table [AppSystems] (
	[Key] text not null,
	[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
	[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
	[Value] text not null,
	[Note] text not null,
	primary key (
		[Key]
	)
)
;

--// create table: LauncherItems
create table [LauncherItems] (
	[LauncherItemId] text not null,
	[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
	[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
	[Name] text not null,
	[Kind] text not null,
	[Command] text not null,
	[CommandOption] text not null,
	[WorkDirectory] text not null,
	[IconPath] text not null,
	[IconIndex] integer not null,
	[IsEnabledCommandLauncher] boolean not null,
	[IsEnabledCustomEnvVar] boolean not null,
	[IsEnabledStandardOutput] boolean not null,
	[IsEnabledStandardInput] boolean not null,
	[Permission] text not null,
	[CredentId] text not null,
	[ExecuteCount] integer not null,
	[LastExecuteTimestamp] datetime not null,
	[Note] text not null,
	primary key (
		[LauncherItemId]
	)
)
;



