
var defaultLanguageKey = 'ja-JP';

var menuList = [
	{
		name: 'top',
		title: {
			'ja-JP': 'トップ'
		}
	}
];



// http://stackoverflow.com/questions/19491336/get-url-parameter-jquery?answertab=votes#tab-top
function getParameter(key)
{
	var sPageURL = decodeURIComponent(window.location.search.substring(1));
	var sURLVariables = sPageURL.split('&');

	for (var i = 0; i < sURLVariables.length; i++) {
		var sParameterName = sURLVariables[i].split('=');

		if (sParameterName[0] === key) {
			return sParameterName[1] === undefined ? true : sParameterName[1];
		}
	}
}

function getLanguageCode()
{
	var lang = getParameter('lang');
	if (!lang || typeof lang !== 'string') {
		return defaultLanguageKey;
	}

	return lang;
}

$(function() {
	var lang = getLanguageCode();
	
	var $menu = $('#main-menu');

	var param = 'lang=' + lang;
	var $ul = $('<ul>');
	for (var i = 0 ; i < menuList.length; i++) {
		var menuItem = menuList[i];

		var $li = $('<li>');
		var $link = $('<a>');

		var title = menuItem.title[lang];
		var target = menuItem.name + '.' + lang + '.html?' + param;

		$link.text(title);
		$link.attr('href', target);

		$li.append($link);
		$ul.append($li);
	}
	$menu.append($ul)
});

