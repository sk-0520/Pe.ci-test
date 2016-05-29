var targetName = 'PE_BROWSER';
var issueLink = 'https://bitbucket.org/sk_0520/pe/issue/';
var revisionLink = 'https://bitbucket.org/sk_0520/pe/commits/';

var changelogTypeMap = {
	'features': '機能',
	'fixes': '修正',
	'developer': '開発',
	'note': 'メモ'
};

function makeIssueLink()
{
	var itemList = document.getElementsByTagName('li');
	for(var i = 0; i < itemList.length; i++) {
		var li = itemList[i];

		var linkElements = li.getElementsByTagName('a');
		if(linkElements.length == 1 && linkElements[0].className == 'rev') {
			var linkElement = linkElements[0];
			var rev = linkElement.innerHTML;
			var link = revisionLink + rev;
			linkElement.setAttribute('target', targetName);
			linkElement.setAttribute('href', link);
		}

		var text = li.innerHTML;
		text = text.replace(/#([0-9]+)/g, "<a href='" + issueLink + "$1' target='" + targetName + "'>#$1</a>");
		li.innerHTML = text;
	}
}
