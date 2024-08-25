import type { FC } from "react";
import { ChangelogContent } from "../../components/changelog/ChangelogContent";
import type { PageProps } from "../../types/PageProps";
import { getChangelogs } from "../../utils/changelog";

const changelogs = getChangelogs();

export const HelpChangelogPage: FC<PageProps> = (props: PageProps) => {
	return changelogs.map((a, i) => (
		<ChangelogContent key={`${i}-${a}`} changelog={a} />
	));
};
