import { Box, ListSubheader, MenuItem, TableRow, Typography } from "@mui/material";
import { useAtomValue } from "jotai";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import { WorkTablesAtom, useWorkColumn } from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import { getElement } from "../../utils/access";
import {
	ClrMap,
	type ClrTypeFullName,
	ClrTypeFullNames,
	ClrTypeMap,
	CommonColumnNames,
	CommonCreatedColumnNames,
	CommonUpdatedColumnNames,
	type ForeignKey,
	type Sqlite3Type,
	SqliteTypeMap,
	type WorkColumn,
	type WorkForeignKey,
	type WorkTable,
} from "../../utils/table";
import {
	EditorCell,
	EditorCheckbox,
	EditorSelect,
	EditorTextField,
} from "./editor";

interface InputValues {
	isPrimary: boolean;
	notNull: boolean;
	foreignKey: string;
	logicalName: string;
	logicalType: Sqlite3Type;
	physicalName: string;
	clrType: ClrTypeFullName;
	checkConstraints: string;
	comment: string;
}

interface ForeignKeyItem<Type extends "Table" | "Column", Data> {
	type: Type;
	data: Data;
}
type ForeignKeyTableItem = ForeignKeyItem<"Table", WorkTable>;
type ForeignKeyColumnItem = ForeignKeyItem<"Column", WorkColumn> & {
	table: WorkTable;
};

interface DatabaseTableColumnProps extends TableBaseProps {
	columnId: string;
}

export const DatabaseTableColumn: FC<DatabaseTableColumnProps> = (
	props: DatabaseTableColumnProps,
) => {
	const { tableId, columnId } = props;
	const { workColumn, updateWorkColumn } = useWorkColumn(tableId, columnId);
	const {
		id,
		isPrimary,
		notNull,
		foreignKeyId,
		logical,
		physicalName,
		clrType,
		checkConstraints,
		comment,
	} = workColumn;

	const workTables = useAtomValue(WorkTablesAtom);

	const foreignTables = workTables.filter((a) => a.id !== tableId);
	const foreignTableColumns = foreignTables.flatMap((a) => [
		{
			type: "Table",
			data: a,
		} satisfies ForeignKeyTableItem,
		...a.columns.items
			.filter((b) => !CommonColumnNames.includes(b.physicalName))
			.map(
				(b) =>
					({
						type: "Column",
						data: b,
						table: a,
					}) satisfies ForeignKeyColumnItem,
			),
	]);

	const foreignTable = foreignKeyId
		? foreignTables.find((a) => a.id === foreignKeyId.tableId)
		: undefined;
	const foreignColumn =
		foreignTable && foreignKeyId
			? foreignTable.columns.items.find((a) => a.id === foreignKeyId.columnId)
			: undefined;

	const { control, handleSubmit, watch, setValue } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			isPrimary: isPrimary,
			notNull: notNull,
			foreignKey:
				foreignTable && foreignColumn
					? `${foreignTable.id}.${foreignColumn.id}`
					: "",
			logicalName: logical.name,
			logicalType: logical.type,
			physicalName: physicalName,
			clrType: clrType,
			checkConstraints: checkConstraints,
			comment: comment,
		},
	});

	const isCommonCreatedColumn = CommonCreatedColumnNames.includes(
		watch("physicalName"),
	);
	const isCommonUpdatedColumn = CommonUpdatedColumnNames.includes(
		watch("physicalName"),
	);
	const isCommonColumn = isCommonCreatedColumn || isCommonUpdatedColumn;

	// この辺データ構造全くわからんわ
	const physicalType = getElement(SqliteTypeMap, watch("logicalType"));
	const selectableClrTypes = getElement(ClrMap, physicalType);
	console.debug(clrType);
	console.debug(selectableClrTypes);
	if (!selectableClrTypes.includes(clrType)) {
		const value = selectableClrTypes[0];
		setValue("clrType", value as ClrTypeFullName);
	}

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object>,
	): void {
		let foreignKey: ForeignKey | undefined = undefined;
		let foreignKeyId: WorkForeignKey | undefined = undefined;
		if (data.foreignKey) {
			const [foreignKeyTableId, foreignKeyColumnId] =
				data.foreignKey.split(".");
			foreignKeyId = {
				tableId: foreignKeyTableId,
				columnId: foreignKeyColumnId,
			};
			const foreignKeyTable = foreignTables.find(
				(a) => a.id === foreignKeyTableId,
			);
			if (foreignKeyTable) {
				const foreignKeyColumn = foreignKeyTable.columns.items.find(
					(a) => a.id === foreignKeyColumnId,
				);
				if (foreignKeyColumn) {
					foreignKey = {
						table: foreignKeyTable.define.tableName,
						column: foreignKeyColumn.logical.name,
					};
				}
			}
			if (!foreignKey) {
				foreignKeyId = undefined;
			}
		}

		updateWorkColumn({
			id: id,
			isPrimary: data.isPrimary,
			notNull: data.notNull,
			foreignKey: foreignKey,
			foreignKeyId: foreignKeyId,
			logical: {
				name: data.logicalName,
				type: data.logicalType,
			},
			physicalName: data.physicalName,
			clrType: data.clrType,
			checkConstraints: data.checkConstraints,
			comment: data.comment,
		});
	}

	return (
		<TableRow
			sx={{
				background: isCommonColumn ? "lightgray" : undefined,
			}}
		>
			<EditorCell>
				<Controller
					name="isPrimary"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorCheckbox
							checked={field.value}
							{...field}
							onBlur={handleSubmit(handleInput)}
						/>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="notNull"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorCheckbox
							checked={field.value}
							{...field}
							onBlur={handleSubmit(handleInput)}
						/>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Box>
					<Controller
						name="foreignKey"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorSelect
								{...field}
								sx={{ fontSize: "80%" }}
								onBlur={handleSubmit(handleInput)}
							>
								<MenuItem value="">{"未設定"}</MenuItem>

								{foreignTableColumns.map((a) => {
									switch (a.type) {
										case "Table":
											return (
												<ListSubheader key={a.data.id}>
													{a.data.define.tableName}
												</ListSubheader>
											);

										case "Column":
											return (
												<MenuItem
													key={`${a.table.id}.${a.data.id}`}
													value={`${a.table.id}.${a.data.id}`}
												>
													{a.data.physicalName}
												</MenuItem>
											);
									}
								})}
							</EditorSelect>
						)}
					/>
				</Box>
			</EditorCell>
			<EditorCell>
				<Controller
					name="logicalName"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="physicalName"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="logicalType"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorSelect {...field} onBlur={handleSubmit(handleInput)}>
							<ListSubheader>SQLite3</ListSubheader>
							<MenuItem value="integer">integer</MenuItem>
							<MenuItem value="real">real</MenuItem>
							<MenuItem value="text">text</MenuItem>
							<MenuItem value="blob">blob</MenuItem>
							<ListSubheader>affinity</ListSubheader>
							<MenuItem value="datetime">datetime</MenuItem>
							<MenuItem value="boolean">boolean</MenuItem>
						</EditorSelect>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Typography>{physicalType}</Typography>
			</EditorCell>
			<EditorCell>
				<Controller
					name="clrType"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorSelect {...field} onBlur={handleSubmit(handleInput)}>
							{ClrTypeFullNames.map((a) => {
								return (
									<MenuItem
										key={a}
										value={a}
										disabled={!selectableClrTypes.includes(a)}
									>
										{ClrTypeMap.get(a)}
									</MenuItem>
								);
							})}
						</EditorSelect>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="checkConstraints"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="comment"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>delete</EditorCell>
		</TableRow>
	);
};
