import { default as markdown_1 } from "bundle-text:./DevPluginTemplatePage_1.md";
import { default as markdown_2 } from "bundle-text:./DevPluginTemplatePage_2.md";
import type { FC } from "react";
import { HelpMarkdown } from "../../components/HelpMarkdown";
import type { PageProps } from "../../types/PageProps";

export const DevPluginTemplatePage: FC<PageProps> = (props: PageProps) => {
	return (
		<>
			<HelpMarkdown>{markdown_1}</HelpMarkdown>
			<HelpMarkdown>{markdown_2}</HelpMarkdown>
		</>
	);
};
