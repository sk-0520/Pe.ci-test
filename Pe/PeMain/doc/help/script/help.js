
var titleTail = ' Pe';
var defaultLanguageKey = 'ja-JP';

var menuList = [
	{
		name: 'help',
		level: 0,
		title: {
			'ja-JP': 'ヘルプ'
		}
	}
];



// http://stackoverflow.com/questions/19491336/get-url-parameter-jquery?answertab=votes#tab-top
function getParameter(key)
{
	var pageUri = decodeURIComponent(window.location.search.substring(1));
	var params = pageUri.split('&');

	for (var i = 0; i < params.length; i++) {
		var paramNames = params[i].split('=');

		if (paramNames[0] === key) {
			return paramNames[1] === undefined ? true : paramNames[1];
		}
	}
}

function getLanguageCode() {
	var lang = getParameter('lang');
	if (!lang || typeof lang !== 'string') {
		return defaultLanguageKey;
	}

	return lang;
}

function getPageName() {
	var pageUri = location.pathname.substring(1)
	var index = pageUri.lastIndexOf('/');
	var fileName = pageUri.substring(index + 1);
	return fileName.split('.')[0];
}

$(function() {
	var lang = getLanguageCode();
	var page = getPageName();

	var $menu = $('#main-menu');

	var param = 'lang=' + lang;
	var $ul = $('<ul>');
	for (var i = 0 ; i < menuList.length; i++) {
		var menuItem = menuList[i];

		var $li = $('<li>');
		var $link = $('<a>');

		$li.addClass('level-' + menuItem.level);
		var title = menuItem.title[lang];
		var target = menuItem.name + '.' + lang + '.html?' + param;
		if (menuItem.name == page) {
			$('h1').text(title);
			$('title').text(title + titleTail);
		}

		$link.text(title);
		$link.attr('href', target);

		$li.append($link);
		$ul.append($li);
	}
	$menu.append($ul)
});

