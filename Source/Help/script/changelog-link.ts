const targetName = 'PE_BROWSER';
const issueLink = 'https://github.com/sk-0520/Pe/issues/';
const revisionLink = 'https://github.com/sk-0520/Pe/commit/';

// 0.84.0 以降はもうちっとまともにする


function makeChangelogLink()
{
	makeAutoLink();
	makeIssueLink();
}
// import 周りを理解していないマン参上！
window.makeChangelogLink = makeChangelogLink;

function makeAutoLink()
{
	const itemList = document.getElementsByTagName('li');
	for (let i = 0; i < itemList.length; i++) {
		const li = itemList[i];
		li.innerHTML = li.innerHTML.replace(/((http|https|ftp):\/\/[\w?=&./\-;#~%-]+(?![\w\s?&./;#~%"=-]*>))/g, '<a href="$1" href="' + targetName + '">$1</a>'); //'"
	}
}

function makeIssueLink()
{
	const itemList = document.getElementsByTagName('li');
	for (let i = 0; i < itemList.length; i++) {
		const li = itemList[i];

		const linkElements = li.getElementsByTagName('a');
		if (linkElements.length == 1 && linkElements[0].className == 'revision') {
			const linkElement = linkElements[0];
			const rev = linkElement.innerHTML;
			const link = revisionLink + rev;
			linkElement.setAttribute('target', targetName);
			linkElement.setAttribute('href', link);
		}

		let text = li.innerHTML;
		text = text.replace(/#([0-9]+)/g, "<a href='" + issueLink + "$1' target='" + targetName + "'>#$1</a>");
		li.innerHTML = text;
	}
}


