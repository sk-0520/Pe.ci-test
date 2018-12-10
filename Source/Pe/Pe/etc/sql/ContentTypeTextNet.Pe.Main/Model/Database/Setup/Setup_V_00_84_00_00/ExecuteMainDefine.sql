--// create table AppKeyValue
create table [AppKeyValue] (
	[AppKeyValueKey] text not null,
	[CreatedKind] text not null,
	[CreatedAccount] text not null,
	[CreatedTimestamp] text not null,
	[UpdatedKind] text not null,
	[UpdatedAccount] text not null,
	[UpdatedTimestamp] text not null,
	[UpdatedCount] integer not null,
	[AppKeyValueValue] text not null,
	[AppKeyValueNote] text not null,
	constraint [PK_AppKeyValueMaster] primary key (
		[AppKeyValueKey]
	)
);




