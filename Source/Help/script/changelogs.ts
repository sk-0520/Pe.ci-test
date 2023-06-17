//declare function makeChangelogLink(): void;
// eslint-disable-next-line @typescript-eslint/triple-slash-reference
/// <reference path="./changelog-link.ts" />

/*--------BUILD-EMBEDDED-JSON--------*/

import './changelog-link'
import changelogs from '../../../Define/changelogs.json';
import changelogsArchives from '../../../Define/changelogs-archive.json';

Array.prototype.push.apply(changelogs, changelogsArchives);

window.addEventListener('load', () => {
	const changelogTypeMap: { [key: string]: string } = {
		'features': '機能',
		'fixes': '修正',
		'developer': '開発',
		'note': 'メモ'
	};

	const listElement = document.getElementById('list') as HTMLSelectElement;
	listElement.addEventListener("change", _ => {
		const listOptionElement = document.getElementById(listElement.value);
		if(listOptionElement) {
			listOptionElement.scrollIntoView();
		}
	});

	const changelogElement = document.getElementById('changelogs')!;
	for (const changelog of changelogs) {
		const versionSection = document.createElement('section');
		versionSection.id = changelog.version;

		const listOptionElement = document.createElement("option");
		listOptionElement.value = changelog.version
		listOptionElement.text = `${changelog['date']}, ${changelog['version']}`;
		listElement.appendChild(listOptionElement)

		const versionHeader = document.createElement('h2');
		versionHeader.textContent = `${changelog['date']}, ${changelog['version']}`;
		versionSection.appendChild(versionHeader);

		for (const content of changelog['contents']) {
			const contentSection = document.createElement('section');

			const contentHeader = document.createElement('h3');
			contentHeader.className = content['type'];
			contentHeader.textContent = changelogTypeMap[content['type']];

			contentSection.appendChild(contentHeader);

			const logs = document.createElement('ul');
			for (const log of content['logs']) {
				const item = document.createElement('li');
				if ('class' in log) {
					item.className = log['class']!;
				}

				const header = document.createElement('span');
				header.className = 'header';

				const subject = document.createElement('span');
				subject.textContent = log['subject'];
				header.appendChild(subject);


				if ('revision' in log) {
					if (log['revision']) {
						const revision = document.createElement('a');
						revision.className = 'revision';
						revision.textContent = log['revision']!;
						header.appendChild(revision);
					}
				}


				item.appendChild(header)

				if ('comments' in log) {
					const comments = document.createElement('ul');
					for (const comment of log['comments']!) {
						const li = document.createElement('li');
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

		changelogElement.appendChild(versionSection);
	}

	makeChangelogLink();
});
