define(function(require, exports, module) {
	var EditSession = require("ace/edit_session").EditSession;
	var UndoManager = require("ace/undomanager").UndoManager;
	var Editor = require("ace/editor").Editor;
	var Renderer = require("ace/virtual_renderer").VirtualRenderer;

	var Fiddler = require("fiddler");
	var Calibur = require("calibur");
	var Utils = require("utils");
	var $ = require("jquery");
			require('common');
			require("format");
	var $ssnpanel = require('sessionpanel').SessionPanel;

	var showHeader=function(oheaders,wapper){
		var headers = JSON.stringify(oheaders);
		Utils.FormatMessage(headers).promise.then(function(msg){
			$.showEditor(wapper,msg.Result,'javascript');
		});
	};
	
	var hastext = function(contentType){
		return !! $.getMode(contentType);
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
					$.showEditor(wapper,msg.Result.Return,'text');
				});
			}else{
				$.showEditor(wapper,"Empty",'text');
			}
		},
		restext:function(wapper){
			if(hastext(this.session.ResponseHeaders['Content-Type'])){
				this.session.GetResponseBodyAsString(function(msg){
					$.showEditor(wapper,msg.Result.Return,'text');
				});
			}else{
				$.showEditor(wapper,"Empty",'text');
			}
		},

		reqhex:function(wapper){
			$.showEditor(wapper,"Empty",'text');
		},
		reshex:function(wapper){
			$.showEditor(wapper,"Empty",'text');
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
				$.showEditor(wapper,msg.Result.Return,'text');
			});
		},



		resraw:function(wapper){
			this.session.GetResponse(function(msg){
				$.showEditor(wapper,msg.Result.Return,'text');
			});
		},

		reqformat:function(wapper){
			var brush = $.getMode(this.session.RequestHeaders['Content-Type']);
			this.session.GetRequestBodyAsString(function(msg){
				$.showEditor(wapper,$.format(msg.Result.Return,{method: brush}),brush);
			});
		},
		resformat:function(wapper){
			var brush = $.getMode(this.session.ResponseHeaders['Content-Type']);
			this.session.GetResponseBodyAsString(function(msg){
				var fmt = $.format(msg.Result.Return,{method: brush});
				$.showEditor(wapper,fmt,brush);
			});
		},

		urlqrcode:function(wapper){
			wapper.html($('<div>').addClass('qrcode').qrcode(Calibur.UTF16to8(this.session.FullUrl)));
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
				$el.data('editor',$.CreateEditor($el))
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
