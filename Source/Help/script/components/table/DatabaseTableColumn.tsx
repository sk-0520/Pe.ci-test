import { Box, Checkbox, Select, TableRow, TextField } from "@mui/material";
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
							<EditorSelect {...field} onBlur={handleSubmit(handleInput)} />
						)}
					/>
					<Controller
						name="foreignKeyColumn"
						control={control}
						render={({ field, formState: { errors } }) => (
							<EditorSelect {...field} onBlur={handleSubmit(handleInput)} />
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
						<EditorSelect {...field} onBlur={handleSubmit(handleInput)} />
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
						<EditorSelect {...field} onBlur={handleSubmit(handleInput)} />
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
