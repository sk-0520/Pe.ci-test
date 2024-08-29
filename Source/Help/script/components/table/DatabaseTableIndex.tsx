import { Checkbox, TableCell, TableRow, TextField } from "@mui/material";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import type { TableDefineProps } from "../../types/table";
import type { TableIndex } from "../../utils/table";

interface InputValues {
	isUnique: boolean;
	name: boolean;
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
			<TableCell>
				<Controller
					name="isUnique"
					control={control}
					render={({ field, formState: { errors } }) => (
						<Checkbox {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</TableCell>
			<TableCell>
				<Controller
					name="name"
					control={control}
					render={({ field, formState: { errors } }) => (
						<TextField
							fullWidth
							{...field}
							onBlur={handleSubmit(handleInput)}
						/>
					)}
				/>
			</TableCell>
			<TableCell>list</TableCell>
		</TableRow>
	);
	//return <pre>{JSON.stringify(props)}</pre>;
};
