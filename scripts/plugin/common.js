define(function(require, exports, module) {
	var EditSession = require("ace/edit_session").EditSession;
	var UndoManager = require("ace/undomanager").UndoManager;
	var Editor = require("ace/editor").Editor;
	var Renderer = require("ace/virtual_renderer").VirtualRenderer;

	var $ = require("jquery");

	var re_html = /^text\/(.+?)?html/;
	var re_js = /^(text|application)\/(.+?)?(json|javascript)/;
	var re_css = /^text\/css/;
	var re_xml = /^(text|application)\/(.+?)?xml/;
	var re_text = /^text\//;

	$.getMode = function(contentType){
		if(!contentType)return "text";
		if(re_html.test(contentType))return "html";
		if(re_js.test(contentType))return "javascript";
		if(re_xml.test(contentType))return "xml";
		if(re_css.test(contentType))return "css";
		if(re_text.test(contentType))return "text";
		return "text";
	};


	$.showEditor = function(editor,content,mode){
		editor.getSession().setMode("ace/mode/" + mode);
		editor.setValue(content);
		editor.selection.moveCursorTo(0,0)
	};

	$.statusbar = function(content,mode){
		mode = mode||'success';
		$('#statusbar')
			.removeClass('alert-success alert-info alert-warning alert-danger')
			.addClass('alert-'+mode)
			.html(content);
	};
	$.CreateEditor = function($dom){
		var editor = new Editor(new Renderer($dom.get(0)));
		editor.getSession().setUndoManager(new UndoManager());
		editor.setTheme("ace/theme/xcode");
		editor.commands.addCommand({
		    name: 'FullScreen',
		    bindKey: {win: 'F11',  mac: 'F11'},
		    exec: function(ieditor) {
		    	if(!$dom.hasClass('full-screen')){
		    		$wrap = $('<div>').attr('id',$dom.attr('id')+'-editor');
		    		$dom.addClass('full-screen').wrap($wrap);
		    		$('body').append($dom.detach());
		    	}else{
		    		$('#'+$dom.attr('id')+'-editor')
			    		.after($dom.removeClass('full-screen'))
			    		.remove();
		    	}
		    	ieditor.resize(true);
		    	ieditor.focus();
		    },
		    readOnly: false
		});
		return editor;
	};
});
