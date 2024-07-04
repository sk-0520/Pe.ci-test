//declare function makeChangelogLink(): void;
// eslint-disable-next-line @typescript-eslint/triple-slash-reference
/// <reference path="./changelog-link.ts" />

/*--------BUILD-EMBEDDED-JSON--------*/

import "./changelog-link";
import changelogsArchives from "../../../Define/changelogs-archive.json";
import changelogs from "../../../Define/changelogs.json";

Array.prototype.push.apply(changelogs, changelogsArchives);

window.addEventListener("load", () => {
	const changelogTypeMap: { [key: string]: string } = {
		features: "機能",
		fixes: "修正",
		developer: "開発",
		note: "メモ",
	};

	const listElement = document.getElementById("list") as HTMLSelectElement;
	listElement.addEventListener("change", (_) => {
		const listOptionElement = document.getElementById(listElement.value);
		if (listOptionElement) {
			listOptionElement.scrollIntoView();
		}
	});

	const changelogElement = document.getElementById("changelogs");

	const changelogGroupFirstItems = new Map<object, string>();
	let prevGroupChangelogGroup: string | undefined = undefined;
	for (const changelog of [...changelogs].reverse()) {
		if ("group" in changelog && typeof changelog.group === "string") {
			prevGroupChangelogGroup = changelog.group;
		}
		// biome-ignore lint/style/noNonNullAssertion: <explanation>
		changelogGroupFirstItems.set(changelog, prevGroupChangelogGroup!);
	}

	let optionGroupElement: HTMLOptGroupElement | undefined = undefined;

	for (const changelog of changelogs) {
		const versionSection = document.createElement("section");
		versionSection.id = changelog.version;

		if (
			optionGroupElement === undefined ||
			changelogGroupFirstItems.get(changelog) !== optionGroupElement.label
		) {
			// biome-ignore lint/style/noNonNullAssertion: <explanation>
			optionGroupElement = document.createElement("optgroup")!;
			// biome-ignore lint/style/noNonNullAssertion: <explanation>
			optionGroupElement.label = changelogGroupFirstItems.get(changelog)!;
			listElement.appendChild(optionGroupElement);
		}
		if (!optionGroupElement) {
			throw new Error("optionGroupElement");
		}

		const listOptionElement = document.createElement("option");
		listOptionElement.value = changelog.version;
		listOptionElement.text = `${changelog.date}, ${changelog.version}`;
		listElement.appendChild(listOptionElement);

		const versionHeader = document.createElement("h2");
		versionHeader.textContent = `${changelog.date}, ${changelog.version}`;
		versionSection.appendChild(versionHeader);

		for (const content of changelog.contents) {
			const contentSection = document.createElement("section");

			const contentHeader = document.createElement("h3");
			contentHeader.className = content.type;
			contentHeader.textContent = changelogTypeMap[content.type];

			contentSection.appendChild(contentHeader);

			const logs = document.createElement("ul");
			for (const log of content.logs) {
				const item = document.createElement("li");
				if ("class" in log && log.class) {
					item.className = log.class;
				}

				const header = document.createElement("span");
				header.className = "header";

				const subject = document.createElement("span");
				subject.textContent = log.subject;
				header.appendChild(subject);

				if ("revision" in log && log.revision) {
					const revision = document.createElement("a");
					revision.className = "revision";
					revision.textContent = log.revision;
					header.appendChild(revision);
				}

				item.appendChild(header);

				if ("comments" in log && log.comments) {
					const comments = document.createElement("ul");
					for (const comment of log.comments) {
						const li = document.createElement("li");
						li.textContent = comment;
						comments.appendChild(li);
					}
					item.appendChild(comments);
				}

				logs.appendChild(item);
			}
			contentSection.appendChild(logs);

			versionSection.appendChild(contentSection);
		}

		// biome-ignore lint/style/noNonNullAssertion: <explanation>
		changelogElement!.appendChild(versionSection);
	}

	makeChangelogLink();
});
