import { Divider, Link, Typography } from "@mui/material";
import type { FC } from "react";
import React from "react";
import json from "../../Define/@changelog.json";
import { ChangelogVersion } from "../Help/script/components/changelog/ChangelogVersion";
import type { ChangelogVersion as ChangelogVersionType } from "../Help/script/types/changelog";

export const ReleaseNote: FC = () => {
	const changelog = json as ChangelogVersionType;
	return (
		<>
			<ChangelogVersion {...changelog} />
			<Divider />
			<Typography>
				手動でアップデートする場合は
				<Link
					href="https://github.com/sk-0520/Pe/releases/latest/"
					target="_blank"
				>
					こちら
				</Link>
				からモジュールをダウンロードし、既存 Pe を上書きしてください。
			</Typography>
		</>
	);
};
