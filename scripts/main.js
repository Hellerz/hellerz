requirejs.config({
	paths: {
		config: 'config',

		eventtarget: 'ssn/eventtarget',
		websoket: 'ssn/websoket',
		calibur: 'ssn/calibur',
		session: 'ssn/session',
		fiddler: 'ssn/fiddler',
		utils: 'ssn/utils',
		storage:'ssn/storage',

		jquery: 'lib/jquery-2.1.4',
		bootstrap: 'lib/bootstrap',
		bootstrapswitch: 'lib/bootstrap-switch',
		bootstrapselect: 'lib/bootstrap-select',
		bootstraptypeahead: 'lib/bootstrap-typeahead',
		format:'lib/jquery.format',
		qrcode:'lib/jquery.qrcode.min',
		//Grid
		grid: 'lib/grid',

		//ztree
		ztreecore:'ztree/jquery.ztree.core',
		ztreeexcheck:'ztree/jquery.ztree.excheck',
		ztreeexedit:'ztree/jquery.ztree.exedit',

		//file directory
		file:'ssn/file',
		directory:'ssn/directory',

		common:'plugin/common',
		autoresponser:'plugin/autoresponser',
		composer:'plugin/composer',
		setting:'plugin/setting',
		sessionpanel:'plugin/sessionpanel',
		inspector:'plugin/inspector',
	},
	shim: {
		'bootstrap': {
			deps: ['jquery'],
		},
		'bootstrapselect': {
			deps: ['jquery','bootstrap'],
		},
		'bootstrapswitch': {
			deps: ['jquery','bootstrap'],
		},
		'bootstraptypeahead': {
			deps: ['jquery','bootstrap'],
		},
		
		'grid': {
			deps: ['jquery'],
		},

		//ztree
		'ztreecore':{
			deps: ['jquery'],
		},
		'ztreeexcheck':{
			deps: ['jquery','ztreecore'],
		},
		'ztreeexedit':{
			deps: ['jquery','ztreecore'],
		},

		//format
		'format':{
			deps: ['jquery'],
		},
		'qrcode':{
			deps: ['jquery'],
		},
		
	}
});
define(function(require, exports, module) {

	var $ = require("jquery")
	  , Calibur = require("calibur")
	  , Fiddler = require("fiddler");

	//load jQuery Plugin
	require("bootstrap");
	require('qrcode');

	$(document).ready(function() {
		var $ssnpanel = require('sessionpanel').SessionPanel;
		require('common');
		//Calibur Plugin
		require('inspector');
		require('setting');
		require('composer');
		require('autoresponser');
		require('setting');
		
		// File.OpenDialog("SB",'D:\\Demos1','文本文件|*.*|C#文件|*.cs|所有文件|*.*',2,function(msg){
		// 	console.dir(msg);

		// });
		// Directory.OpenDialog("SB1",'Desktop','D:\\Demos1',function(msg1){
		// 	console.dir(msg1);
		// });

		var logo = $('#logo');
        Fiddler.IsStarted&&Fiddler.IsStarted(function(msg){
    		logo.toggleClass('off',!msg.Result);
    	});
        logo.on('click', function(e) {
        	Fiddler.IsStarted(function(msg){
        		if(msg.Result){
        			Fiddler.Stop(function() {
						$.statusbar("Proxy has stopped.",'warning');
						logo.addClass('off');
					});
        		}else{
        			Fiddler.ReStart().GetPort(function(port) {
						$.statusbar("Proxy has started. Port:"+port.Result,'info');
						logo.removeClass('off');
					});
        		}
        	});
		});
		
		Calibur.webSocket.onServerError=function(evt){
        	console.dir(JSON.parse(evt.data));
        };
		Calibur.webSocket.addEventListener('close',function(e){
			if(!e.target.FailedConnected){
				$.statusbar('WebSocket connection has closed.','warning');
				logo.addClass('off');
			}
		});
		Calibur.webSocket.addEventListener('error',function(e){	
			if(e.target.readyState === 3){
				$.statusbar('WebSocket connection to '+e.target.url+' failed: Error in connection establishment: net::ERR_CONNECTION_REFUSED','danger');
				e.target.FailedConnected = true;
			}
		});
		
		$(window).on('unload',function(){
			Fiddler.removeRequest();
			Fiddler.removeResponse();
		});

        $('#navFun a:first').tab('show');
       
	});
});