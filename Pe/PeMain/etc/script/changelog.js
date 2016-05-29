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

//-----------------------------------------------------------------------------
// 更新履歴出力・表示で統一使用するためのがんばり

function is(type, obj) {
	var clas = Object.prototype.toString.call(obj).slice(8, -1);
	return obj !== undefined && obj !== null && clas === type;
}

function Element(tagName)
{
	if(!tagName) {
		throw "tagName is null";
	}
	
	this.tagName = tagName;
	this.nodes = [];
	this.attributes = {};

	this.setAttibute = function(name, value) {
		this.attributes[name] = value;
	}
	this.append = function(node) {
		this.nodes.push(node);
	}
	this.toString = function() {
		var attr = [];
		for(var key in this.attributes) {
			if(is('String', this.attributes[key])) {
				attr.push(key + '=' + this.attributes[key]);
			} else if(is('Object', this.attributes[key])) {
				attr.push(key + '=' + this.attributes[key].toString());
			}
			//attr.push(key + '=' + this.attributes[key]);
		}
		
		var result = [];
		result.push('<' + this.tagName + ' ' + attr.join(' ') + '>');

		for(var i = 0; i < this.nodes.length; i++) {
			result.push(this.nodes[i].toString());
		}
		
		result.push('</' + this.tagName + '>');

		return result.join('');
	}
}

/**
主処理。
*/
function makeChangeLogBlock(changelog)
{
	var title = makeChangeLogTitle(changelog);
	var contents = makeChangeLogContents(changelog['contents']);

	return {
		'title': title,
		'contents': contents
	};
}

function makeChangeLogTitle(changelog)
{
	var version = changelog['version'];
	var date    = changelog['date'];
	var title = new Element('h2');
	title.append(version + ', ' + date);
	
	if(changelog.isRc) {
		title.append(' ');
		
		var rc = new Element('em');
		rc.append('RC版');
		
		title.append(rc);
	}

	return title;
}

function makeChangeLogContents(contents)
{
	var parent = new Element('dl')
	parent.setAttibute('class', 'changelog');
	var hasContent = false;

	for(var i = 0; i < contents.length; i++) {
		var content = makeChangeLogContent(contents[i]);
		if(content.body != null) {
			hasContent = true;
			parent.append(content.head);
			parent.append(content.body);
		}
	}

	return hasContent ? parent: null;
}

function makeChangeLogContent(content)
{
	var result = {
		head: null,
		body: null
	};
	var type = content['type'];
	var dt = new Element('dt');
	dt.setAttibute('class', type);
	dt.append(changelogTypeMap[type]);
	result.head = dt;

	if(content['logs']) {
		var hasBody = false;
		var body = new Element('ul');
		for(var i = 0; i < content['logs'].length; i++) {
			var log = content['logs'][i];
			if(log['subject']) {
				hasBody = true;
				var subject = new Element('li');
				subject.append(convertMessage(log['subject']));
				if(log['class']) {
					subject.setAttibute('class', log['class']);
				}
				
				if(log['revision']) {
					var rev = new Element('a');
					rev.setAttibute('class', 'rev');
					rev.append(log['revision']);
					
					subject.append(rev);
				}
				
				if(log['comments']) {
					var hasComment = false;
					var comments = new Element('ul');
					comments.setAttibute('class', 'comment');
					for(var j = 0; j < log['comments'].length; j++) {
						if(log['comments'][j]) {
							hasComment = true;
							var comment = new Element('li')
							comment.append(convertMessage(log['comments'][j]));
							
							comments.append(comment);
						}
					}
					if(hasComment) {
						subject.append(comments);
					}
				}
				body.append(subject);
				hasBody = true;
			}
		}
		if(hasBody) {
			result.body = body;
		}
	}

	return result;
}

function convertMessage(s)
{
	switch(s.slice('-1')) {
		case '!':
		case '！':
		case '?':
		case '？':
		case ')':
		case '）':
		case '…':
		case '。':
			return s;

		default:
			return s + '。';
	}
}


