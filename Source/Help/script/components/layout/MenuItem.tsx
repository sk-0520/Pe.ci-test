import type { FC } from "react";
import React from "react";
import type { PageElement } from "../../pages";

type MenuItemProps = {
	page: PageElement;
};

export const MenuItem: FC<MenuItemProps> = (props: MenuItemProps) => {
	const { page } = props;
	return (
		<li>
			{page.title}
			{page.nodes && 0 < page.nodes.length && (
				<ul>
					{page.nodes.map((a) => (
						<MenuItem key={a.key} page={a} />
					))}
				</ul>
			)}
		</li>
	);
};
