define(function(require, exports, module) {
	var EditSession = require("ace/edit_session").EditSession;
	var UndoManager = require("ace/undomanager").UndoManager;
	var Editor = require("ace/editor").Editor;
	var Renderer = require("ace/virtual_renderer").VirtualRenderer;

	var Fiddler = require("fiddler");
	var Utils = require("utils");
	var $ = require("jquery");
	require("format");
	var $ssnpanel = require('sessionpanel').SessionPanel;

	var showHeader=function(oheaders,wapper){
		var headers = JSON.stringify(oheaders);
		Utils.FormatMessage(headers).promise.then(function(msg){
			show_editor(wapper,msg.Result,'javascript');
		});
	};

	var show_editor = function(editor,content,mode){
		editor.getSession().setMode("ace/mode/" + mode);
		editor.setValue(content);
		editor.selection.moveCursorTo(0,0)
	};
	
	var hastext = function(contentType){
		return !!getBrush(contentType);
	};

	var re_html = /^text\/(.+?)?html/;
	var re_js = /^(text|application)\/(.+?)?(json|javascript)/;
	var re_css = /^text\/css/;
	var re_xml = /^(text|application)\/(.+?)?xml/;
	var re_text = /^text\//;

	var getBrush = function(contentType){
		if(!contentType)return "text";
		if(re_html.test(contentType))return "html";
		if(re_js.test(contentType))return "javascript";
		if(re_xml.test(contentType))return "xml";
		if(re_css.test(contentType))return "css";
		if(re_text.test(contentType))return "text";
		return "text";
	};

	var shownStatus = {
		'session':null,
		'req-nav':null,
		'res-nav':null,

		statistics:function(wapper){
			showHeader(this.session,wapper);
		},

		reqheader:function(wapper){
			showHeader(this.session.RequestHeaders,wapper);
		},
		resheader:function(wapper){
			showHeader(this.session.ResponseHeaders,wapper);
		},

		reqtext:function(wapper){
			if(hastext(this.session.RequestHeaders['Content-Type'])){
				this.session.GetRequestBodyAsString(function(msg){
					show_editor(wapper,msg.Result.Return,'text');
				});
			}else{
				show_editor(wapper,"Empty",'text');
			}
		},
		restext:function(wapper){
			if(hastext(this.session.ResponseHeaders['Content-Type'])){
				this.session.GetResponseBodyAsString(function(msg){
					show_editor(wapper,msg.Result.Return,'text');
				});
			}else{
				show_editor(wapper,"Empty",'text');
			}
		},

		reqhex:function(wapper){
			show_editor(wapper,"Empty",'text');
		},
		reshex:function(wapper){
			show_editor(wapper,"Empty",'text');
		},
		
		resweb:function(wapper){
			var responseType = this.session.ResponseHeaders['Content-Type'];
			if(responseType&&responseType!=='application/octet-stream'){
				this.session.GetResponseBodyAsBase64(function(msg){
					responseType = responseType.split(';')[0];
					var src = 'data:'+responseType+';base64,'+msg.Result.Return;
					wapper.html($('<iframe class="full-body" src='+src+' />'));
				});
			}
		},

		reqform:function(wapper){
			showHeader(this.session.UrlParam,wapper);
		},

		reqraw:function(wapper){
			this.session.GetRequest(function(msg){
				show_editor(wapper,msg.Result.Return,'text');
			});
		},
		resraw:function(wapper){
			this.session.GetResponse(function(msg){
				show_editor(wapper,msg.Result.Return,'text');
			});
		},

		reqformat:function(wapper){
			var brush = getBrush(this.session.RequestHeaders['Content-Type']);
			this.session.GetRequestBodyAsString(function(msg){
				show_editor(wapper,$.format(msg.Result.Return,{method: brush}),brush);
			});
		},
		resformat:function(wapper){
			var brush = getBrush(this.session.ResponseHeaders['Content-Type']);
			this.session.GetResponseBodyAsString(function(msg){
				var fmt = $.format(msg.Result.Return,{method: brush});
				show_editor(wapper,fmt,brush);
			});
		},

		showSession:function(){
			var self = this;
			if(self['session']&&self['req-nav']&&self['res-nav']){
				shownStatus[self['req-nav'].attr('id')](self['req-nav'].data('editor'));
				shownStatus[self['res-nav'].attr('id')](self['res-nav'].data('editor'));
			}
		}
	};
	
	var addSession= (function(){
		var wapper = $('.mmg-bodyWrapper');
		return function(session){
			$ssnpanel.addRow(session, $ssnpanel.rowsLength());
			if (Math.abs(wapper.height() + wapper[0].scrollTop - wapper[0].scrollHeight) < 25) {
				wapper.scrollTop(wapper[0].scrollHeight);
			}
		};
	}());

	var beforeRequest = function(e, args) {
		addSession(args.session);
	}

	var beforeResponse = function(e, args) {
		var session = args.session;
		args.callback=function(){
			$ssnpanel.updateRow(session,$.data($ssnpanel,'id_row')[session.Id]);
		};
	};
	$ssnpanel.on('cellSelected', function(e, item, rowIndex, colIndex) {
		shownStatus.session = item;
		shownStatus.showSession();
	}).on('loadSuccess', function(e, data) {
		Fiddler.addRequest(beforeRequest);
		Fiddler.addResponse(beforeResponse);
	}).on('rowInserted', function(e, item, index, $tr) {
		$.data($ssnpanel,'id_row')[item.Id] = $tr;
	}).on('rowRemoved', function(e, item, index) {
		delete $.data($ssnpanel,'id_row')[item.Id];
	}).load();

	$('#req-nav,#res-nav').on('show.bs.tab', function (e) {
		var $el = $(e.target.hash);
		if(!$el.data('editor')){
			if(!$el.attr('no-ace')){
				var editor = new Editor(new Renderer($el.get(0)));
				editor.getSession().setUndoManager(new UndoManager());
				//editor.setTheme("ace/theme/idle_fingers");
				$el.data('editor',editor)
			}else{
				$el.data('editor',$el)
			}
		}
    	shownStatus[e.delegateTarget.id] = $(e.target.hash);
    }).on('shown.bs.tab', function (e) {
    	shownStatus.showSession();
    });
    $('#req-nav a:first,#res-nav a:first').tab('show');
});
