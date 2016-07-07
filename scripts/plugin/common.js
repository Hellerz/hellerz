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

	//根据请求头的Content-Type判断报文数据格式
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

	//展示状态栏（底部）
	$.statusbar = function(content,mode){
		mode = mode||'success';
		$('#statusbar')
			.css({opacity: 1})
			.removeClass('alert-success alert-info alert-warning alert-danger bold')
			.addClass('alert-'+mode)
			.html(content).addClass('bold').animate({ 
			    opacity: 0.5
			}, 3000 );
	};

	//展示模态框
	var popup = $('#popup');
	$.showPopup = function(title,body,footer){
		popup.find('.modal-title').html(title);
		popup.find('.modal-body').html(body);
		popup.find('.modal-footer').html(footer);
		popup.modal();
		return popup;
	}

	//展示通知栏（顶部）
	$.notifybar = function(content,mode,indecator,callback){
		if($.isFunction(mode)){
			callback = mode;
			indecator = Math.random().toString();
			
		}else if($.isFunction(indecator)){
			callback = indecator;
			indecator = Math.random().toString();
		}
		if($('#'+indecator).length){
			return ;
		}
		mode = mode||'success';
		var notibar = $('<div>').attr('id',indecator)
		.addClass('notifybar navbar navbar-fixed-top alert-'+mode)
		.html(content)
		.on('click',function(e){
			var result = callback&&callback(e);
			if(result === true||result===undefined){
				notibar.remove();
			}
		}).appendTo($('body'));
	};

	//创建ACE编辑器
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
