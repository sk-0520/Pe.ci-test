import type { FC } from "react";
import React from "react";
import { Pages } from "../../pages";
import { SideMenuItem } from "./SideMenuItem";

export const SideMenu: FC = () => {
	return (
		<ul>
			{Pages.map((a) => (
				<SideMenuItem key={a.key} page={a} />
			))}
		</ul>
	);
};
