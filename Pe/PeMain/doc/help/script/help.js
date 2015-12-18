
var issuesPageUri = '';
var defaultLanguageKey = 'ja-JP';

var helpLanguage = {
	'ja-JP': { 
		title: ' : Pe ヘルプ',
		outLink: '外部リンク'
	}
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
	},
	{
		name: 'setting-general',
		level: 1,
		title: {
			'ja-JP': '本体'
		}
	},
	{
		name: 'setting-launcher',
		level: 1,
		title: {
			'ja-JP': 'ランチャー'
		}
	},
	{
		name: 'setting-toolbar',
		level: 1,
		title: {
			'ja-JP': 'ツールバー'
		}
	}
];
//----------------------------------------------------------------------


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

function getPageTitle(lang, menuItem) {
	var title = menuItem.title[lang];
	if (title) {
		return title;
	}

	return menuItem.title[defaultLanguageKey];
}

function createMenu(lang, pageName) {
	var $menu = $('#main-menu');

	var param = 'lang=' + lang;
	var $ul = $('<ul>');
	var $top = null;
	for (var i = 0 ; i < menuList.length; i++) {
		var menuItem = menuList[i];

		var $li = $('<li>');

		$li.addClass('item');
		$li.addClass('level-' + menuItem.level);
		
		var title = getPageTitle(lang, menuItem);
		var target = menuItem.name + '.' + lang + '.html?' + param;
		if (menuItem.name == pageName) {
			$top = $li;
			$li.text(title);
			$('h1').text(title);
			$('title').text(title + helpLanguage[lang].title);
			$li.addClass('level-active');
		} else {
			var $link = $('<a>');
			$link.text(title);
			$link.attr('href', target);
			$li.append($link);
		}

		$ul.append($li);
	}
	$menu.append($ul);
	
	if ($top) {
		var y = $top.offset().top - $menu.offset().top;
		$menu.scrollTop(y);
	}
}

function createLink(lang) {
	var param = 'lang=' + lang;

	var $content = $('#content');
	$content.find('.page').each(function() {
		var $page = $(this);
		// TODO: 内部リンク
		var pageName = $page.text();
		for (var i = 0; i < menuList.length; i++) {
			var menuItem = menuList[i];
			if (menuItem.name == pageName) {
				var title = getPageTitle(lang, menuItem);
				var target = menuItem.name + '.' + lang + '.html?' + param;
				var $link = $('<a>');
				$link.text(title);
				$link.attr('href', target);
				$page.empty().append($link);
				break;
			}
		}
	});
	$content.find('.issue').each(function() {
		var $issue = $(this);
		var $link = $('<a>');
		var number = $issue.text();
		var target = issueLink + number;
		$link.text(number);
		$link.attr('href', target);
		$issue.empty().append($link);
	});
}

function setPadding() {
	var $content = $('#content');
	$content.append($('<div>').addClass('padding'));
}

$(function() {
	var lang = getLanguageCode();
	var pageName = getPageName();

	createMenu(lang, pageName);
	createLink(lang);
	setPadding();
});

