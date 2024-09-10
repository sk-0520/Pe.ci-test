import type { FC } from "react";
import type { PageKey } from "../../pages";
import { PageLink } from "../PageLink";

interface MdLinkProps {
	page: PageKey;
}

export const MdLink: FC<MdLinkProps> = (props: MdLinkProps) => {
	const { page } = props;

	return <PageLink page={page} />;
};
