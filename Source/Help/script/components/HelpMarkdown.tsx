import { MuiMarkdown } from "mui-markdown";
import { Highlight, themes } from "prism-react-renderer";
import type { FC } from "react";

type HelpMarkdownProps = {
	children: string;
};

export const HelpMarkdown: FC<HelpMarkdownProps> = (
	props: HelpMarkdownProps,
) => {
	return (
		<MuiMarkdown
			Highlight={Highlight}
			themes={themes}
			prismTheme={themes.github}
		>
			{props.children}
		</MuiMarkdown>
	);
};
