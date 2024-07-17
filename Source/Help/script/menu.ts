function applyCurrentStrong() {
	const pathName = window.location.pathname;
	const fileName = pathName.substring(pathName.lastIndexOf("/") + 1);
	const elements = document.querySelectorAll("#menu nav li a");

	for (const element of elements) {
		const hrefValue = element.getAttribute("href");
		if (!hrefValue) {
			throw "実装ミス";
		}
		const hrefFileName = hrefValue.substring(hrefValue.lastIndexOf("/") + 1);
		if (fileName === hrefFileName) {
			element.classList.add("current");
			element.scrollIntoView();
			break;
		}
	}
}

applyCurrentStrong();
