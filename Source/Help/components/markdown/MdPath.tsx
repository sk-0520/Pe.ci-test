import ExtensionOutlinedIcon from "@mui/icons-material/ExtensionOutlined";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import InsertDriveFileOutlinedIcon from "@mui/icons-material/InsertDriveFileOutlined";
import { type SxProps, type Theme, Typography } from "@mui/material";
import type { FC } from "react";

const FileRegex = /.+\.\w+$/;

type PathType = "dir" | "file" | "plugin";

const IconStyle: SxProps<Theme> = {
	display: "inline-flex",
	verticalAlign: "middle",
};

function getIcon(type: PathType) {
	switch (type) {
		case "dir":
			return <FolderOutlinedIcon sx={IconStyle} />;

		case "file":
			return <InsertDriveFileOutlinedIcon sx={IconStyle} />;

		case "plugin":
			return <ExtensionOutlinedIcon sx={IconStyle} />;
	}
}

interface MdPathProps {
	type?: PathType;
	children: string;
}

export const MdPath: FC<MdPathProps> = (props: MdPathProps) => {
	const { type, children } = props;

	let usingType = type;
	if (!usingType) {
		if (1 <= children.length) {
			const child = children[0];
			// @ts-expect-error ts(2345) ガチでこの辺何してんのか分からん(配列？ 文字列？)
			if (child.endsWith(".dll")) {
				usingType = "plugin";
				// @ts-expect-error ts(2345)
			} else if (FileRegex.test(child)) {
				usingType = "file";
			}
		}
		if (!usingType) {
			usingType = "dir";
		}
	}

	const icon = getIcon(usingType);

	return (
		<>
			{icon}
			<Typography
				component="span"
				sx={{
					fontFamily: "Consolas, monospace",
					marginLeft: "0.5ch",
				}}
			>
				{children}
			</Typography>
		</>
	);
};
