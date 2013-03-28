
if (document.location.href.match(/http:\/\/playstarfleetextreme\.com\/options/i) != null) {
	optionsJsNavAlign();
	optionsJsIndexOwn();
}

if (document.location.href.match(/http:\/\/playstarfleetextreme\.com\/galaxy/i) != null) {
	doAuth();
	injectJs();
}



function injectJs() {
	var d = document;
	var scr = d.createElement('script');
	scr.type = "text/javascript";
	scr.src = 'http://x1.astrospy.co.uk/x2script/x2base.js';
	d.getElementsByTagName('head')[0].appendChild(scr);
}

function optionsJsNavAlign() {
	var d = document;
	var scr = d.createElement('script');
	scr.type = "text/javascript";
	scr.src = 'http://x1.astrospy.co.uk/x2script/x2options_navAlign.js';
	d.getElementsByTagName('head')[0].appendChild(scr);
}

function optionsJsIndexOwn() {
	var d = document;
	var scr = d.createElement('script');
	scr.type = "text/javascript";
	scr.src = 'http://x1.astrospy.co.uk/x2script/x2options_indexOwn.js';
	d.getElementsByTagName('head')[0].appendChild(scr);
}

function doAuth() {
	var d = document;
	var scr = d.createElement('script');
	scr.type = "text/javascript";
	scr.src = 'http://x1.astrospy.co.uk/x2script/x2Auth.js';
	d.getElementsByTagName('head')[0].appendChild(scr);
}