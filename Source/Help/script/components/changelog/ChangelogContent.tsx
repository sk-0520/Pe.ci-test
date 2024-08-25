import type { FC } from "react";
import type { Changelog } from "../../types/changelog";

type ChangelogContentProps = {
	changelog: Changelog;
};

export const ChangelogContent: FC<ChangelogContentProps> = (
	props: ChangelogContentProps,
) => {
	return <>{JSON.stringify(props.changelog)}</>;
};
