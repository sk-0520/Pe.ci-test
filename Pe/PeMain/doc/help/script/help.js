
var defaultLanguageKey = 'ja-JP';

var helpTitle = {
	'ja-JP': ' : Pe ヘルプ'
};

var menuList = [
	{
		name: 'top',
		level: 0,
		title: {
			'ja-JP': 'はじめに'
		}
	},
	{
		name: 'install_uninstall_data',
		level: 1,
		title: {
			'ja-JP': 'インストール・アンインストール・使用データについて'
		}
	},
	{
		name: 'privacy',
		level: 1,
		title: {
			'ja-JP': 'ユーザー情報について'
		}
	},
	{
		name: 'internet',
		level: 1,
		title: {
			'ja-JP': 'インターネット通信とその内容'
		}
	},
	{
		name: 'launcher',
		level: 0,
		title: {
			'ja-JP': 'ランチャー'
		}
	},
	{
		name: 'toolbar',
		level: 1,
		title: {
			'ja-JP': 'ツールバー'
		}
	},
	{
		name: 'command',
		level: 1,
		title: {
			'ja-JP': 'コマンド'
		}
	},
	{
		name: 'execute',
		level: 1,
		title: {
			'ja-JP': '指定して実行'
		}
	},
	{
		name: 'stream',
		level: 1,
		title: {
			'ja-JP': '標準入出力'
		}
	},
	{
		name: 'note',
		level: 0,
		title: {
			'ja-JP': 'ノート'
		}
	},
	{
		name: 'log',
		level: 0,
		title: {
			'ja-JP': 'ログ'
		}
	},
	{
		name: 'notifyarea',
		level: 0,
		title: {
			'ja-JP': '通知領域'
		}
	},
	{
		name: 'clipboard',
		level: 0,
		title: {
			'ja-JP': 'クリップボード'
		}
	},
	{
		name: 'template',
		level: 0,
		title: {
			'ja-JP': 'テンプレート'
		}
	},
	{
		name: 'setting',
		level: 0,
		title: {
			'ja-JP': '設定'
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

		$li.addClass('level-' + menuItem.level);
		var title = menuItem.title[lang];
		var target = menuItem.name + '.' + lang + '.html?' + param;
		if (menuItem.name == page) {
			$li.text(title);
			$('h1').text(title);
			$('title').text(title + helpTitle[lang]);
		} else {
			var $link = $('<a>');
			$link.text(title);
			$link.attr('href', target);
			$li.append($link);
		}

		$ul.append($li);
	}
	$menu.append($ul)
});

