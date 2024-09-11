import { Typography } from "@mui/material";
import type { FC } from "react";

interface MdInlineProps {
	kind: "key" | "ui";
	children: string;
}

export const MdInline: FC<MdInlineProps> = (props: MdInlineProps) => {
	const { kind, children } = props;

	switch (kind) {
		case "key":
			return (
				<Typography
					component="kbd"
					sx={{
						fontFamily: "Consolas, monospace",
						minWidth: "2ex",
						padding: "0 0.5em",
						margin: "0.3em 0.5ex",
						background: "#efeff2",
						borderRadius: "4px",
						borderTop: "1px solid #f5f5f5",
						boxShadow:
							"0 0 25px #e8e8e8 inset, 0 1px 0 #c3c3c3, 0 2px 0 #c9c9c9, 0 2px 3px #333333",
						color: "#555",
						textShadow: "0 1px 0 #f5f5f5",
						textAlign: "center",
						display: "inline-block",
					}}
				>
					{children}
				</Typography>
			);

		case "ui":
			return (
				<Typography
					component="dfn"
					sx={{
						border: "1px solid #707030",
						background: "#fffbcd",
						margin: "2px 4px",
						padding: "2px 4px",
						fontFamily: "Consolas,monospace",
						display: "inline-block",
					}}
				>
					{children}
				</Typography>
			);
	}
};
