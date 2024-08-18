import { MuiMarkdown, getOverrides } from "mui-markdown";
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
			overrides={{
				...getOverrides({}),
				h1: {
					component: "h2",
					props: {
						style: {
							padding: "0.2ex 0.5ex",
							background: "#ccc",
							lineHeight: "1.5em",
						},
					},
				},
				h2: {
					component: "h3",
					props: {
						style: {
							padding: "0.2ex 0.5ex",
							borderLeft: "1ex solid #fff5bc",
							borderBottom: "3px double #fff5bc",
							lineHeight: "1.5em",
						},
					},
				},
			}}
		>
			{props.children}
		</MuiMarkdown>
	);
};
/*

h1 {
	padding: 6px 1em;
	border-bottom: 3px double #888;
	line-height: 1em;
}

h2 {
	padding: 0.2ex 0.5ex;
	background: #ccc;
	line-height: 1em;
}

h3 {
	padding: 0.2ex 0.5ex;
	border-left: 1ex solid #fff5bc;
	border-bottom: 3px double #fff5bc;
	line-height: 1em;
}
*/
