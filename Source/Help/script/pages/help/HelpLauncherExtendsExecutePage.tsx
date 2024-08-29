import { default as markdown } from "bundle-text:./HelpLauncherExtendsExecutePage.md";
import type { FC } from "react";
import { HelpMarkdown } from "../../components/HelpMarkdown";
import type { PageProps } from "../../types/page";

export const HelpLauncherExtendsExecutePage: FC<PageProps> = (
	props: PageProps,
) => {
	return <HelpMarkdown>{markdown}</HelpMarkdown>;
};
