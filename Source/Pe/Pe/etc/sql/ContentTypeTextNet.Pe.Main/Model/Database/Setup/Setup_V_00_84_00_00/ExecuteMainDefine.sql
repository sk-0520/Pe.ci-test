--==
--// create table: AppSystems
create table [AppSystems] (
	[AppSystemsKey] text not null,
	[CreatedKind] text not null,
	[CreatedAccount] text not null,
	[CreatedTimestamp] text not null,
	[UpdatedKind] text not null,
	[UpdatedAccount] text not null,
	[UpdatedTimestamp] text not null,
	[UpdatedCount] integer not null,
	[AppSystemsValue] text not null,
	[AppSystemsNote] text not null,
	primary key (
		[AppSystemsKey]
	)
);

--// create table:




