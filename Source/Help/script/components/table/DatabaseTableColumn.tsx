import {
	Box,
	Checkbox,
	ListItem,
	ListSubheader,
	MenuItem,
	Select,
	TableRow,
	TextField,
} from "@mui/material";
import { useAtom } from "jotai";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import { TableDefinesAtom } from "../../stores/TableStore";
import type { TableDefineProps } from "../../types/table";
import type { TableColumn } from "../../utils/table";
import {
	EditorCell,
	EditorCheckbox,
	EditorSelect,
	EditorTextField,
} from "./editor";

const DatabaseTypeMap = new Map([
	// 通常
	["integer", "integer"],
	["real", "real"],
	["text", "text"],
	["blob", "blob"],
	// 意味だけ
	["datetime", "text"],
	["boolean", "integer"],
]) as ReadonlyMap<string, string>;

const ClrMap = new Map([
	["integer", ["System.Int64"]],
	["real", ["System.Decimal", "System.Single", "System.Double"]],
	[
		"text",
		["System.String", "System.Guid", "System.Version", "System.TimeSpan"],
	],
	["blob", ["System.Byte[]"]],
	["datetime", ["System.DateTime", "System.String"]],
	["boolean", ["System.Boolean", "System.Int64"]],
]) as ReadonlyMap<string, ReadonlyArray<string>>;

interface InputValues {
	isPrimary: boolean;
	notNull: boolean;
	foreignKeyTable: string;
	foreignKeyColumn: string;
	logicalName: string;
	logicalType: string;
	physicalName: string;
	cliType: string;
	checkConstraints: string;
	comment: string;
}

interface DatabaseTableColumnProps extends TableColumn, TableDefineProps {}

export const DatabaseTableColumn: FC<DatabaseTableColumnProps> = (
	props: DatabaseTableColumnProps,
) => {
	const {
		tableDefine,
		isPrimary,
		notNull,
		foreignKey,
		logical,
		physicalName,
		cliType,
		checkConstraints,
		comment,
	} = props;
	const [_, setTableDefines] = useAtom(TableDefinesAtom);
	const { control, handleSubmit } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			isPrimary: isPrimary,
			notNull: notNull,
			foreignKeyTable: foreignKey?.table ?? "",
			foreignKeyColumn: foreignKey?.column ?? "",
			logicalName: logical.name,
			logicalType: logical.type,
			physicalName: physicalName,
			cliType: cliType,
			checkConstraints: checkConstraints,
			comment: comment,
		},
	});

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object>,
	): void {
		// setTableDefines((state) => {
		// 	const index = state.indexOf(tableDefine);
		// 	const current = state[index];
		// 	return [...state];
		// });
		console.debug(data);
	}

	return (
		<TableRow>
			<EditorCell>
				<Controller
					name="isPrimary"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorCheckbox {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="notNull"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorCheckbox {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Box>
					<Controller
						name="foreignKeyTable"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorSelect
								{...field}
								sx={{ fontSize: "80%" }}
								onBlur={handleSubmit(handleInput)}
							/>
						)}
					/>
					<Controller
						name="foreignKeyColumn"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorSelect
								{...field}
								sx={{ fontSize: "80%" }}
								onBlur={handleSubmit(handleInput)}
							/>
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
				<EditorTextField />
			</EditorCell>
			<EditorCell>
				<Controller
					name="cliType"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorSelect {...field} onBlur={handleSubmit(handleInput)}>
							<MenuItem value="System.String">string</MenuItem>
							<MenuItem value="System.Int64">long</MenuItem>
							<MenuItem value="System.Decimal">decimal</MenuItem>
							<MenuItem value="System.Byte[]">byte[]</MenuItem>
							<MenuItem value="System.Boolean">bool</MenuItem>
							<MenuItem value="System.Single">float</MenuItem>
							<MenuItem value="System.Double">double</MenuItem>
							<MenuItem value="System.Guid">Guid</MenuItem>
							<MenuItem value="System.DateTime">DateTime</MenuItem>
							<MenuItem value="System.Version">Version</MenuItem>
							<MenuItem value="System.TimeSpan">TimeSpan</MenuItem>
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
