import { Checkbox, TableRow, TextField } from "@mui/material";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import type { TableDefineProps } from "../../types/table";
import type { TableIndex } from "../../utils/table";
import { EditorCell, EditorCheckbox, EditorTextField } from "./editor";

interface InputValues {
	isUnique: boolean;
	name: string;
	columns: string[];
}

interface DatabaseTableIndexProps extends TableIndex, TableDefineProps {}

export const DatabaseTableIndex: FC<DatabaseTableIndexProps> = (
	props: DatabaseTableIndexProps,
) => {
	const { tableDefine, isUnique, name, columns } = props;
	const { control, handleSubmit } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			isUnique: isUnique,
			name: name,
			columns: columns,
		},
	});

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object, unknown, unknown> | undefined,
	): void {
		console.debug(data);
	}

	return (
		<TableRow>
			<EditorCell>
				<Controller
					name="isUnique"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorCheckbox {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="name"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField
							fullWidth
							{...field}
							onBlur={handleSubmit(handleInput)}
						/>
					)}
				/>
			</EditorCell>
			<EditorCell>list</EditorCell>
		</TableRow>
	);
	//return <pre>{JSON.stringify(props)}</pre>;
};
