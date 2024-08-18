import type { FC } from "react";
import {HelpMarkdown} from "../../components/HelpMarkdown"
import type { PageProps } from "../../types/PageProps";

export const HelpIndexPage: FC<PageProps> = (props: PageProps) => {
	return <HelpMarkdown>{markdown}</HelpMarkdown>;
};

const markdown = /* markdown */ `

# asd

`;
