import { default as markdown_1 } from "bundle-text:./DevPluginTemplatePage_1.md";
import { default as markdown_2 } from "bundle-text:./DevPluginTemplatePage_2.md";
import ApiIcon from "@mui/icons-material/Api";
import { IconButton, InputAdornment, Stack, TextField } from "@mui/material";
import { type FC, type MouseEvent, useEffect, useState } from "react";
import { HelpMarkdown } from "../../components/HelpMarkdown";
import type { PageProps } from "../../types/page";
import { generatePluginId, makeParameter } from "../../utils/plugin";

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
		if (result) {
			setParameter(`.\\create-project.ps1 ${result}`);
		} else {
			setParameter("");
		}
	}, [projectDirectory, pluginId, pluginName, projectNamespace]);

	async function handleGeneratePluginIdClick(event: MouseEvent) {
		const id = await generatePluginId();
		setPluginId(id);
	}

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
								<IconButton
									title="自動生成"
									edge="end"
									color="primary"
									onClick={handleGeneratePluginIdClick}
								>
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
