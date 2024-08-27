import {
	ListSubheader,
	MenuItem,
	Select,
	type SelectChangeEvent,
	useTheme,
} from "@mui/material";
import { type FC, type ReactNode, useMemo, useState } from "react";
import type { ChangelogVersion, Changelogs } from "../../types/changelog";

function getChangelogGroup(
	changelogs: Changelogs,
): Array<ChangelogVersion | string> {
	const groupItems = new Map<object, string>();

	let prevGroup: string | undefined = undefined;
	for (const changelog of [...changelogs].reverse()) {
		if (changelog.group) {
			prevGroup = changelog.group;
		}
		// biome-ignore lint/style/noNonNullAssertion: 最初の項目は麗しき Forms
		groupItems.set(changelog, prevGroup!);
	}

	prevGroup = undefined;
	const result: Array<ChangelogVersion | string> = [];
	for (const changelog of changelogs) {
		const group = groupItems.get(changelog);
		if (group !== prevGroup) {
			prevGroup = group;
			// biome-ignore lint/style/noNonNullAssertion: undefinedじゃなけりゃグループなんや
			result.push(group!);
		}
		result.push(changelog);
	}

	return result;
}

interface ChangelogVersionSelectorProps {
	changelogs: Changelogs;
}

export const ChangelogVersionSelector: FC<ChangelogVersionSelectorProps> = (
	props: ChangelogVersionSelectorProps,
) => {
	const { changelogs } = props;
	const [selectedValue, setSelectedValue] = useState(changelogs[0].version);
	const theme = useTheme();
	const items = useMemo(() => getChangelogGroup(changelogs), [changelogs]);

	function handleVersionChange(
		event: SelectChangeEvent,
		child: ReactNode,
	): void {
		setSelectedValue(event.target.value);

		const targetElement = document.getElementById(event.target.value);
		if (targetElement) {
			window.scrollTo({ top: targetElement.offsetTop - 64 });
		}
	}

	return (
		<Select
			size="small"
			label="SH"
			value={selectedValue}
			onChange={handleVersionChange}
			sx={{
				color: theme.palette.primary.contrastText,
			}}
		>
			{items.map((a) =>
				typeof a === "string" ? (
					<ListSubheader key={`group-${a}`}>{a}</ListSubheader>
				) : (
					<MenuItem key={`version-${a.version}`} value={a.version}>
						{a.date}, {a.version}
					</MenuItem>
				),
			)}
		</Select>
	);
};
