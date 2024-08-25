import { default as markdown_1 } from "bundle-text:./DevPluginTemplatePage_1.md";
import { default as markdown_2 } from "bundle-text:./DevPluginTemplatePage_2.md";
import ApiIcon from "@mui/icons-material/Api";
import {
	Box,
	Button,
	FormControl,
	IconButton,
	Input,
	InputAdornment,
	InputLabel,
	Stack,
	TextField,
} from "@mui/material";
import type { FC } from "react";
import { Controller, type SubmitHandler, useForm } from "react-hook-form";
import { HelpMarkdown } from "../../components/HelpMarkdown";
import type { PageProps } from "../../types/PageProps";

type InputParameter = {
	projectDirectory: string;
	pluginId: string;
	pluginName: string;
	projectNamespace: string;
};

export const DevPluginTemplatePage: FC<PageProps> = (props: PageProps) => {
	const {
		control,
		handleSubmit,
		watch,
		formState: { errors },
	} = useForm<InputParameter>();

	const onSubmit: SubmitHandler<InputParameter> = (data) => console.log(data);

	return (
		<>
			<HelpMarkdown>{markdown_1}</HelpMarkdown>

			<FormControl onSubmit={handleSubmit(onSubmit)}>
				<Stack spacing={2} sx={{ m: 2, width: "25ch" }}>
					<Controller
						name="projectDirectory"
						control={control}
						render={({ field, formState: { errors } }) => (
							<TextField {...field} label="projectDirectory" />
						)}
					/>
					<Controller
						name="pluginId"
						control={control}
						render={({ field, formState: { errors } }) => (
							<TextField
								{...field}
								label="pluginId"
								InputProps={{
									endAdornment: (
										<InputAdornment position="end">
											<IconButton title="自動生成" edge="end" color="primary">
												<ApiIcon />
											</IconButton>
										</InputAdornment>
									),
								}}
							/>
						)}
					/>
					<Controller
						name="pluginName"
						control={control}
						render={({ field, formState: { errors } }) => (
							<TextField {...field} label="pluginName" />
						)}
					/>
					<Controller
						name="projectNamespace"
						control={control}
						render={({ field, formState: { errors } }) => (
							<TextField {...field} label="projectNamespace" />
						)}
					/>
				</Stack>
			</FormControl>

			<HelpMarkdown>{markdown_2}</HelpMarkdown>
		</>
	);
};
