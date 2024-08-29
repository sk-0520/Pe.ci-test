import { TextField } from "@mui/material";
import { useAtom } from "jotai";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import { TableDefinesAtom } from "../../stores/TableStore";
import type { TableDefineProps } from "../../types/table";

interface InputForm {
	name: string;
}

interface DatabaseTableNameProps extends TableDefineProps {
	name: string;
}

export const DatabaseTableName: FC<DatabaseTableNameProps> = (
	props: DatabaseTableNameProps,
) => {
	const { name, tableDefine } = props;
	const [_, setTableDefines] = useAtom(TableDefinesAtom);
	const { control, handleSubmit } = useForm<InputForm>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			name: name,
		},
	});

	function handleInput(
		data: InputForm,
		event?: BaseSyntheticEvent<object>,
	): void {
		setTableDefines((state) => {
			const index = state.indexOf(tableDefine);
			const current = state[index];
			current.name = data.name;
			return [...state];
		});
	}

	return (
		<Controller
			name="name"
			control={control}
			render={({ field, formState: { errors } }) => (
				<TextField
					label="テーブル名"
					{...field}
					onBlur={handleSubmit(handleInput)}
				/>
			)}
		/>
	);
};
