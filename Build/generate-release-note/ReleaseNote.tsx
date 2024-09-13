import type { FC } from "react";
import React from "react";
import json from "../../Define/@changelog.json";
import { ChangelogVersion } from "../../Source/Help/script/components/changelog/ChangelogVersion";
import type { ChangelogVersion as ChangelogVersionType } from "../../Source/Help/script/types/changelog";

export const ReleaseNote: FC = () => {
	const changelog = json as ChangelogVersionType;
	return <ChangelogVersion {...changelog} />;
};
