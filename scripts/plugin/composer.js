define(function(require, exports, module) {
	var Fiddler = require("fiddler");
	var $	    = require("jquery");
				  require('bootstrapselect');
				  require('bootstraptypeahead');
				  require('beautify');
				  require('common');
	var Storage = require("storage");
	var ConfigKeyHelper = $.storageHelper;
	Fiddler.addResponse(function(e, args) {
		var session = args.session;
		var guid = session.RequestHeaders['Calibur-Composer'];
		var wapper;
		if(guid){
			if(parsed_guid === guid){
				var brush = $.getMode(session.ResponseHeaders['Content-Type']);
				session.GetResponseBodyAsString(function(ssn){
					$.showEditor(cmpsr_resbd,$.vkbeautify[brush](ssn.Return),brush);
				});
			}else if(raw_guid === guid){
				session.GetResponse(function(ssn){
					$.showEditor(cmpsr_resraw,ssn.Return,'text');
				});
			}
		}
	});
	var $cmpsr_type = $('#cmpsr_type');
	var $cmpsr_method = $('#cmpsr_method');
	var $url = $('#cmpsr_requrl');
	var command = {
	    name: 'Run',
	    bindKey: {win: 'Ctrl-R',  mac: 'Command-R'},
	    exec: function(editor) {
	        runComposer();
	    },
	    readOnly: false
	}; 
    var getEditor =function($dom){
    	var editor =$.CreateEditor($dom);
		editor.getSession().setMode("ace/mode/text");
		editor.commands.addCommand(command);
		return editor;
    };
   

    var parsed_guid = '';
    var raw_guid = '';

	var cmpsr_reqraw = getEditor($('#cmpsr_reqraw'));
	var cmpsr_resraw = getEditor($('#cmpsr_resraw'));
	var cmpsr_reqhd = getEditor($('#cmpsr_reqhd'));
	var cmpsr_reqbd = getEditor($('#cmpsr_reqbd'));
	var cmpsr_resbd = getEditor($('#cmpsr_resbd'));

	var runComposer = function () {
		if($('#composerraw').hasClass('active')){
			cmpsr_resraw.setValue('');
    		Fiddler.InjectRaw(cmpsr_reqraw.getValue(),function(guid){
    			raw_guid = guid;
    		});
    	}else{
    		var url = $url.val();
    		var method = $cmpsr_method.selectpicker('val');
    		var contenttype = $cmpsr_type.selectpicker('val');
    		var header = cmpsr_reqhd.getValue();
    		var body = cmpsr_reqbd.getValue();
    		cmpsr_resbd.setValue('');
    		Fiddler.Inject(url,method,contenttype,header,body,function(guid){
    			var item = {
    				url:url,
    				method:method,
    				requesttype:contenttype,
    				requestheader:header,
    				requestbody:body
    			};
    			ConfigKeyHelper.getStorageValueByKey('ComposerParsed',function(settings){
    				if(settings&&settings.length){
    					settings.push(item);
    					ConfigKeyHelper.setStorageValueByKey('ComposerParsed',settings);
    				}else{
    					ConfigKeyHelper.setStorageValueByKey('ComposerParsed',[item]);
    				}
		    	});
    			parsed_guid = guid;
    		});
    	}
    };

    $('.exeInject').on('click',function (e) {
    	runComposer();
    });

	$('#composer-nav a:first').tab('show');
	$('.selectpicker').selectpicker({
		noneSelectedText:'GET',
		style:'btn-sm full-x'
	});
	$.fn.typeahead.Constructor.prototype.blur = function() {
      var that = this;
      setTimeout(function () { that.hide() }, 250);
   };
	
   $url.typeahead({
		source: function(query, process) {
			ConfigKeyHelper.getStorageValueByKey('ComposerParsed',function(settings){
	    		settings&&process(settings);
	    	});
		},
		
		filter:function(item){
			return item.url;
		},

        updater: function (item) {
        	cmpsr_reqbd.setValue(item.requestbody);
        	cmpsr_reqhd.setValue(item.requestheader);
        	$cmpsr_method.selectpicker('val',item.method);
        	$cmpsr_type.selectpicker('val',item.requesttype);
        	cmpsr_resbd.setValue('');
            return item.url;
        }
   });
	$cmpsr_type.on('change',function(e){
		var mode = $(e.target).find('option:selected').attr('mode');
		cmpsr_reqbd.getSession().setMode("ace/mode/"+mode||'text');
		cmpsr_resbd.getSession().setMode("ace/mode/"+mode||'text');
		$.formatEditer(cmpsr_reqbd,mode);
	});
	// [cmpsr_reqbd,cmpsr_resbd].forEach(function(itm){
	// 	itm.on('paste',function(e){
	// 		var mode = $cmpsr_type.find('option:selected').attr('mode');
	// 		window.setTimeout(function(){$.formatEditer(cmpsr_reqbd,mode);},400);
	// 	});
	// });
	exports.editor={
		Url:$url,
		Method:$cmpsr_method,
		ContentType:$cmpsr_type,
		RawRequest:cmpsr_reqraw,
		RawResponse:cmpsr_resraw,
		ParsedHeader:cmpsr_reqhd,
		ParsedRequest:cmpsr_reqbd,
		ParsedResponse:cmpsr_resbd
	};
});