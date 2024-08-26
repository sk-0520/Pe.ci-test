import { Link } from "@mui/material";
import type { FC } from "react";

const revisionLink = "https://github.com/sk-0520/Pe/commit/";

interface ChangelogRevisionProps {
	revision: string;
}

export const ChangelogRevision: FC<ChangelogRevisionProps> = (
	props: ChangelogRevisionProps,
) => {
	const { revision } = props;

	return (
		<Link
			sx={{
				verticalAlign: "middle",
				display: "inline-block",
				margin: "0 1ex",
				border: "1px solid #888",
				borderRadius: "4px",
				padding: "1px",
				fontSize: "70%",
				lineHeight: "1em",
				width: "8ch",
				whiteSpace: "nowrap",
				overflow: "hidden",
				fontFamily: "Consolas",
				textOverflow: "ellipsis",
				textDecoration: "none",
			}}
			href={revisionLink + revision}
		>
			{revision}
		</Link>
	);
};
