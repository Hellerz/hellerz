window.WebVersion = '201608162208';
window.ServiceVersion = '1.0.6068';
requirejs.config({
	urlArgs:"version=" + window.WebVersion,
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
		guid: 'lib/guid',

		//ztree
		ztreecore:'ztree/jquery.ztree.core',
		ztreeexcheck:'ztree/jquery.ztree.excheck',
		ztreeexedit:'ztree/jquery.ztree.exedit',

		//file directory
		file:'ssn/file',
		directory:'ssn/directory',
		system:'ssn/system',


		common:'plugin/common',
		autoresponser:'plugin/autoresponser',
		composer:'plugin/composer',
		setting:'plugin/setting',
		sessionpanel:'plugin/sessionpanel',
		inspector:'plugin/inspector',

		autoresponsersetting:'plugin/config/autoresponser',
		loading:'lib/loading',
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
		
		'loading': {
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
	  , Fiddler = require("fiddler")
	  , System = require("system")
	  , config = require("config");

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
		
		$('#load_screen').hide();
		var logo = $('#logo');
    	Calibur.SyncTimer(function(clear){
    		Fiddler.IsStarted&&Fiddler.IsStarted(function(isStart){
    			clear();
	    		logo.toggleClass('off',!isStart);
	    	});
    	});
        logo.on('click', function(e) {
        	Fiddler.IsStarted(function(isStart){
        		if(isStart){
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

   //      var re_exe = /\[(.+?)\]/;
   //      var re_caliburName = /(calibur|rcgui).*.exe/i;
   //      window.setInterval(function(){
   //      	Fiddler.GetPort&&Fiddler.GetPort(function(port) {
   //      		System.GetRunResult('netstat','-nbp tcp','',function(lines){
			// 		var searchPort = new RegExp('TCP\\s+127\\.0\\.0\\.1:'+port+'.+?ESTABLISHED','i');
			// 		var curExe = '';
			// 		for (var i = 0; i < lines.length; i++) {
			// 			if(re_exe.test(lines[i])){
			// 				curExe =  lines[i].replace(re_exe,'$1');
			// 				continue;
			// 			}
			// 			if(searchPort.test(lines[i])&&i>0){
			// 				if(!re_caliburName.test(curExe)){
			// 					$.statusbar('Port has occupied. ProcessName:'+ curExe +', Port:'+port,'danger');
			// 					break;
			// 				}
			// 			}
			// 		};		
   //      		});
			// });
   //  	},1000);
       
        //当网络代理端口被其他程序抢占时触发
		Calibur.webSocket.addMessageEvent('DetachedUnexpectedly',function(){
			$.statusbar('The system was changed.','warning');
			$.notifybar('The system was changed.Click to reenable Fiddler capture.','warning','thesystemwaschanged',function(e){
    			Fiddler.ReStart().GetPort(function(port) {
					$.statusbar("Proxy has started. Port:"+port,'info');
					logo.removeClass('off');
				});
			});
		});

    	Calibur.SyncTimer(function(clear){
    		Fiddler.IsStarted&&Fiddler.IsStarted(function(isStart){
    			clear();
	    		logo.toggleClass('off',!isStart);
	    	});
    	});
		Calibur.webSocket.onServerError=function(evt){
        	console.dir(JSON.parse(evt.data));
        };
		Calibur.webSocket.addEventListener('close',function(e){
			if(!e.target.FailedConnected){
				$.statusbar('WebSocket connection has closed.','warning');
				logo.addClass('off');
				window.open(' ','_self',' ');    
				window.close();
			}
		});

		var showSocketError = function(socket){
			if(socket.readyState === 3){
				$.notifybar('WebSocket connection to '+socket.url+' failed:If you have not Calibur.exe, <a href="'+config.ServerPakage+'">click here</a> to download.',
					'danger',
					'downloadclient',
					function(e){
						if($(e.target).is('a')){
							return true;
						}
						return false;
					});
				$.statusbar('WebSocket connection to '+socket.url+' failed: Error in connection establishment: net::ERR_CONNECTION_REFUSED','danger');
				socket.FailedConnected = true;
			}
		};

		if(Calibur.webSocket&&Calibur.webSocket.readyState === 3){
			showSocketError(Calibur.webSocket);
		}else{
			Calibur.webSocket.addEventListener('error',function(e){	
				showSocketError(e.target);
			});
		}
		
		
		$(window).on('unload',function(){
			Fiddler.removeRequest();
			Fiddler.removeResponse();
		});

        $('#navFun a:first').tab('show');
       
	});
});