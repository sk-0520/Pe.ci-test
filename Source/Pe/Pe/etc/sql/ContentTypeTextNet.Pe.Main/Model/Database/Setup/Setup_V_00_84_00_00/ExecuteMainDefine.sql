--// drop all tables
PRAGMA writable_schema = 1;

--// drop all tables
delete from sqlite_master where type in ('table', 'index', 'trigger');

--// drop all tables
PRAGMA writable_schema = 0;

--// drop all tables
VACUUM;
--// drop all tables
PRAGMA INTEGRITY_CHECK

--// create table: AppSystems
create table [AppSystems] (
	[Key] text not null,
	[CreatedKind] text not null,
	[CreatedAccount] text not null,
	[CreatedTimestamp] text not null,
	[UpdatedKind] text not null,
	[UpdatedAccount] text not null,
	[UpdatedTimestamp] text not null,
	[UpdatedCount] integer not null,
	[Value] text not null,
	[Note] text not null,
	primary key (
		[Key]
	)
);

--// create table:




