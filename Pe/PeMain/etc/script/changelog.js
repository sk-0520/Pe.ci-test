var targetName = 'PE_BROWSER';
var issueLink = 'https://bitbucket.org/sk_0520/pe/issue/';

window.onload = function()
{
	var itemList = document.getElementsByTagName('li');
	for(var i = 0; i < itemList.length; i++) {
		var li = itemList[i];
		var text = li.innerHTML;
		
		text = text.replace(/#([0-9]+)/g, "<a href='" + issueLink + "$1' target='" + targetName + "'>#$1</a>");
		li.innerHTML = text;
	}
}
