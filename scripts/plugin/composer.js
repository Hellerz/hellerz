define(function(require, exports, module) {
	var Fiddler = require("fiddler");
	var $	    = require("jquery");
				  require('bootstrapselect');
				  require('bootstraptypeahead');
				  require('format');
				  require('common');

	Fiddler.addResponse(function(e, args) {
		var session = args.session;
		var guid = session.RequestHeaders['Calibur-Composer'];
		var wapper;
		if(guid){
			if(parsed_guid === guid){
				var brush = $.getMode(session.ResponseHeaders['Content-Type']);
				session.GetResponseBodyAsString(function(msg){
					$.showEditor(cmpsr_resbd,$.format(msg.Result.Return,{method: brush}),brush);
				});
			}else if(raw_guid === guid){
				session.GetResponse(function(msg){
					$.showEditor(cmpsr_resraw,msg.Result.Return,'text');
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
    var formatEditer=function(editor,mode){
		var body = editor.getValue();
		try{
			body = $.format(body,{method: mode});
			editor.setValue(body);
			editor.selection.moveCursorTo(0,0)
		}catch(e){
			//ignore
		}
    };
    var getParsed = function(){
    	var settings = localStorage['ComposerParsed']||'[]';
    	return JSON.parse(settings);
    };
    var setParsed = function(item){
    	var settings = getParsed();
    	settings.push(item);
    	localStorage['ComposerParsed']=JSON.stringify(settings);
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
    		Fiddler.InjectRaw(cmpsr_reqraw.getValue(),function(msg){
    			raw_guid = msg.Result;
    		});
    	}else{
    		var url = $url.val();
    		var method = $cmpsr_method.selectpicker('val');
    		var contenttype = $cmpsr_type.selectpicker('val');
    		var header = cmpsr_reqhd.getValue();
    		var body = cmpsr_reqbd.getValue();
    		Fiddler.Inject(url,method,contenttype,header,body,function(msg){
    			setParsed({
    				url:url,
    				method:method,
    				requesttype:contenttype,
    				requestheader:header,
    				requestbody:body
    			});
    			parsed_guid = msg.Result;
    		});
    	}
    };

    $('#exeInject').on('click',function (e) {
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
			process(getParsed());
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
		formatEditer(cmpsr_reqbd,mode);
	});
	[cmpsr_reqbd,cmpsr_resbd].forEach(function(itm){
		itm.on('paste',function(e){
			var mode = $cmpsr_type.find('option:selected').attr('mode');
			window.setTimeout(function(){formatEditer(cmpsr_reqbd,mode);},400);
		});
	});
});