import { Typography, type TypographyProps, useTheme } from "@mui/material";
import { MuiMarkdown, defaultOverrides } from "mui-markdown";
import { Highlight, themes } from "prism-react-renderer";
import type { FC } from "react";
import { MdInline } from "./markdown/MdInline";
import { MdLink } from "./markdown/MdLink";
import { MdPath } from "./markdown/MdPath";
import { renderAlert } from "./markdown/alert";

type HelpMarkdownProps = {
	children: string;
};

export const HelpMarkdown: FC<HelpMarkdownProps> = (
	props: HelpMarkdownProps,
) => {
	const { children } = props;
	const theme = useTheme();

	return (
		<MuiMarkdown
			Highlight={Highlight}
			themes={themes}
			prismTheme={themes.github}
			options={{
				overrides: {
					...defaultOverrides,
					h1: {
						component: Typography,
						props: {
							variant: "h2",
							sx: {
								padding: "0.2ex 0.5ex",
								marginBlock: "1rem",
								background: theme.palette.primary.light,
								color: theme.palette.primary.contrastText,
								fontSize: "18pt",
								fontWeight: "bold",
								lineHeight: "1.5em",
							},
						} satisfies TypographyProps,
					},
					h2: {
						component: Typography,
						props: {
							variant: "h3",
							sx: {
								padding: "0.2ex 0.5ex",
								marginBlock: "1rem",
								borderLeft: `1ex solid ${theme.palette.primary.light}`,
								borderBottom: `3px double ${theme.palette.primary.light}`,
								fontSize: "16pt",
								fontWeight: "bold",
								lineHeight: "1.25em",
							},
						} satisfies TypographyProps,
					},
					p: {
						component: Typography,
						props: {
							variant: "body1",
							sx: {
								marginBlock: "0.5em",
							},
						} satisfies TypographyProps,
					},
					code: {
						component: Typography,
						props: {
							variant: "body1",
							sx: {
								display: "inline-block",
								background: "#efefef",
								padding: "1px 1ch",
								margin: "1px",
								fontFamily: "Consolas, monospace",
								borderRadius: "4px",
							},
						} satisfies TypographyProps,
					},
					MdLink,
					MdPath,
					MdInline,
				},
				renderRule: (next, node, renderChildren, state) => {
					const child = renderAlert(node, renderChildren, state);
					if (child) {
						return child;
					}

					return next();
				},
			}}
		>
			{children}
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
