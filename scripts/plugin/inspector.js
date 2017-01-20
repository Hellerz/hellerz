define(function(require, exports, module) {

	var Fiddler = require("fiddler");
	var Calibur = require("calibur");
	var Utils = require("utils");
	var $ = require("jquery");
			require('common');
			require('beautify');
	var $ssnpanel = require('sessionpanel').SessionPanel;

	var showHeader=function(oheaders,wapper){
		$.showEditor(wapper,$.vkbeautify['json'](JSON.stringify(oheaders)),'json');
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
				this.session.GetRequestBodyAsString(function(ssn){
					$.showEditor(wapper,ssn.Return,'text');
				});
			}else{
				$.showEditor(wapper,"Empty",'text');
			}
		},
		restext:function(wapper){
			if(hastext(this.session.ResponseHeaders['Content-Type'])){
				this.session.GetResponseBodyAsString(function(ssn){
					$.showEditor(wapper,ssn.Return,'text');
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
				this.session.GetResponseBodyAsBase64(function(ssn){
					responseType = responseType.split(';')[0];
					var src = 'data:'+responseType+';base64,'+ssn.Return;
					wapper.html($('<iframe class="full-body" src='+src+' />'));
				});
			}
		},

		reqform:function(wapper){
			showHeader(this.session.UrlParam,wapper);
		},

		reqraw:function(wapper){
			this.session.GetRequest(function(ssn){
				$.showEditor(wapper,ssn.Return,'text');
			});
		},



		resraw:function(wapper){
			this.session.GetResponse(function(ssn){
				$.showEditor(wapper,ssn.Return,'text');
			});
		},

		reqformat:function(wapper){
			var brush = $.getMode(this.session.RequestHeaders['Content-Type']);
			this.session.GetRequestBodyAsString(function(ssn){
				$.showEditor(wapper,$.vkbeautify[brush](ssn.Return),brush);
			});
		},
		resformat:function(wapper){
			var brush = $.getMode(this.session.ResponseHeaders['Content-Type']);
			this.session.GetResponseBodyAsString(function(ssn){
				var fmt = $.vkbeautify[brush](ssn.Return);
				$.showEditor(wapper,fmt,brush);
			});
		},

		urlqrcode:function(wapper){
			wapper.html($('<div>').addClass('qrcode').qrcode(Calibur.UTF16to8(this.session.FullUrl)));
		},

		showSession:function($ul){
			var self = this;
			if(self['session']&&$ul){
				var $nav = self[$ul.attr('id')];
				shownStatus[$nav.attr('id')]($nav.data('editor'));
			}else if(self['session']&&self['req-nav']&&self['res-nav']){
				shownStatus[self['req-nav'].attr('id')](self['req-nav'].data('editor'));
				shownStatus[self['res-nav'].attr('id')](self['res-nav'].data('editor'));
			}
		}
	};
	
	var beforeRequest = function(e, args) {
		$ssnpanel.addRow(args.session, $ssnpanel.rowsLength());
	}

	var beforeResponse = function(e, args) {
		var session = args.session;
		args.callback=function(){
			//session 完成时触发，在Grid中填充StatusCode
			$ssnpanel.updateRow(session,$.data($ssnpanel,'id_row')[session.Id]);
		};
	};
	
	$ssnpanel.on('selected', function(e, $trs) {
		shownStatus.session = $trs.data('item');;
		shownStatus.showSession();
	}).on('loadSuccess', function(e, data) {
		Fiddler.addRequest(beforeRequest);
		Fiddler.addResponse(beforeResponse);
	}).on('rowInserted', function(e, item, index, $tr) {
		$.data($ssnpanel,'id_row')[item.Id] = $tr;
	}).on('rowsRemoved', function(e, items, indexs) {
		for (var i = items.length - 1; i >= 0; i--) {
			items[i]&&(delete $.data($ssnpanel,'id_row')[items[i].Id]);
		};
	}).load();
	$ssnpanel.$body.on('dblclick','td',function(e){
	    $('#navFun a[href="#inspectors"]').tab('show');
	    shownStatus.session =  $(this).parents('tr').data('item');
		shownStatus.showSession();
	});
	$('#req-nav,#res-nav').on('show.bs.tab', function (e) {
		var $el = $(e.target.hash);
		if(!$el.data('editor')){
			if(!$el.attr('no-ace')){//no-ace ----> 无需用ace编辑器展示的tab
				$el.data('editor',$.CreateEditor($el))
			}else{
				$el.data('editor',$el)
			}
		}
    	shownStatus[e.delegateTarget.id] = $(e.target.hash);
    }).on('shown.bs.tab', function (e) {
    	shownStatus.showSession($(e.delegateTarget));
    });
    $('#req-nav a:first,#res-nav a:first').tab('show');

});