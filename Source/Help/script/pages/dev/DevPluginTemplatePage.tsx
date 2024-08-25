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
	Typography,
} from "@mui/material";
import { type FC, useEffect, useState } from "react";
import { HelpMarkdown } from "../../components/HelpMarkdown";
import type { PageProps } from "../../types/PageProps";

function makeParameter(
	projectDirectory: string,
	pluginId: string,
	pluginName: string,
	projectNamespace: string,
): string {
	const items = {
		ProjectDirectory: projectDirectory,
		PluginId: pluginId,
		PluginName: pluginName,
		DefaultNamespace: projectNamespace,
	};

	const targetItems = Object.entries(items)
		.filter(([_, v]) => 0 < v.trim().length)
	;
	if(targetItems.length !== Object.keys(items).length) {
		return "";
	}

	return targetItems.map(([k, v]) => {
			const key = `-${k}`;
			const value = v.indexOf(" ") !== -1 ? `"${v}"` : v;

			return `${key} ${value}`;
		})
		.join(" ");
}

export const DevPluginTemplatePage: FC<PageProps> = (props: PageProps) => {
	const [projectDirectory, setProjectDirectory] = useState("");
	const [pluginId, setPluginId] = useState("");
	const [pluginName, setPluginName] = useState("");
	const [projectNamespace, setProjectNamespace] = useState("");
	const [parameter, setParameter] = useState("");

	useEffect(() => {
		const result = makeParameter(
			projectDirectory,
			pluginId,
			pluginName,
			projectNamespace,
		);
		if(result) {
			setParameter(`.\\create-project.ps1 ${result}`);
		} else {
			setParameter("");
		}
	}, [projectDirectory, pluginId, pluginName, projectNamespace]);

	return (
		<>
			<HelpMarkdown>{markdown_1}</HelpMarkdown>

			<Stack spacing={2} sx={{ m: 2, width: "25ch" }}>
				<TextField
					label="projectDirectory"
					value={projectDirectory}
					onChange={(ev) => setProjectDirectory(ev.target.value)}
				/>

				<TextField
					label="pluginId"
					value={pluginId}
					onChange={(ev) => setPluginId(ev.target.value)}
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
				<TextField
					label="pluginName"
					value={pluginName}
					onChange={(ev) => setPluginName(ev.target.value)}
				/>

				<TextField
					label="projectNamespace"
					value={projectNamespace}
					onChange={(ev) => setProjectNamespace(ev.target.value)}
				/>
			</Stack>

			<code>{parameter}</code>

			<HelpMarkdown>{markdown_2}</HelpMarkdown>
		</>
	);
};
