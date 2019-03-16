--// TABLE: LauncherItemIcons
create table LauncherItemIcons(
[LauncherItemId] text not null,
[IconScale] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Image] blob,
primary key (
	LauncherItemId,
	IconScale
)
)
;

--// TABLE: NoteFiles
create table NoteFiles(
[NoteFileId] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Content] blob,
primary key (
	NoteFileId
)
)
;

