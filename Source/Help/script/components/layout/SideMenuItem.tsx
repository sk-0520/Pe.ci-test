import type { FC } from "react";
import React from "react";
import type { PageElement } from "../../pages";

type SideMenuItemProps = {
	page: PageElement;
};

export const SideMenuItem: FC<SideMenuItemProps> = (props: SideMenuItemProps) => {
	const { page } = props;
	return (
		<li>
			{page.title}
			{page.nodes && 0 < page.nodes.length && (
				<ul>
					{page.nodes.map((a) => (
						<SideMenuItem key={a.key} page={a} />
					))}
				</ul>
			)}
		</li>
	);
};
