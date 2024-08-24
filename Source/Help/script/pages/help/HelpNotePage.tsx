import { default as markdown } from "bundle-text:./HelpNotePage.md";
import type { FC } from "react";
import { HelpMarkdown } from "../../components/HelpMarkdown";
import type { PageProps } from "../../types/PageProps";

export const HelpNotePage: FC<PageProps> = (props: PageProps) => {
	return <HelpMarkdown>{markdown}</HelpMarkdown>;
};
