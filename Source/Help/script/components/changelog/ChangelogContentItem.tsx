import {
	Box,
	Link,
	List,
	ListItem,
	ListItemText,
	Typography,
} from "@mui/material";
import type { FC } from "react";
import type * as changelog from "../../types/changelog";
import { ChangelogReplaceLink } from "./ChangelogReplaceLink";

interface ChangelogContentItemProps extends changelog.ChangelogContentItem {}

export const ChangelogContentItem: FC<ChangelogContentItemProps> = (
	props: ChangelogContentItemProps,
) => {
	const { revision, subject, comments } = props;
	const type = props.class;

	return (
		<>
			<Typography variant="body1" component="span">
				{type && (
					<Typography variant="overline" component="span">
						[{type}]
					</Typography>
				)}
				<ChangelogReplaceLink>{subject}</ChangelogReplaceLink>
				{revision && <Link>{revision}</Link>}
			</Typography>
			{comments && (
				<List>
					{comments.map((a, i) => (
						// biome-ignore lint/suspicious/noArrayIndexKey: キーがねぇ
						<ListItem key={i}>
							<ChangelogReplaceLink>{a}</ChangelogReplaceLink>
						</ListItem>
					))}
				</List>
			)}
		</>
	);
};

/*
	*.compatibility {
		color: #f00;
		&::before {
			content: "[互換性]";
		}
	}
	*.notice:before {
		content: "[事前通知]";
		color: #f00;
	}
	*.nuget:before {
		content: "[NuGet]";
		color: #0a0;
	}
	*.myget:before {
		content: "[myget]";
		color: #0a0;
	}
	*.plugin-compatibility:before {
		content: "[プラグイン互換性]";
		color: #f74;
	}
*/
