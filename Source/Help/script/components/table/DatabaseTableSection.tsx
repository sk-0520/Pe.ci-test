import { Typography } from "@mui/material";
import React, { type ReactNode, type FC } from "react";

interface TableSectionProps {
	title: string;
	children: ReactNode;
}

export const DatabaseTableSection: FC<TableSectionProps> = (props) => {
	const { title, children } = props;

	return (
		<>
			<Typography
				variant="h2"
				sx={{
					fontSize: "16pt",
					marginTop: "1em",
				}}
			>
				{title}
			</Typography>
			{children}
		</>
	);
};
