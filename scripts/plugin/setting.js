define(function(require, exports, module) {
	var Fiddler = require("fiddler");
	var $ = require("jquery");
	var config = require("config");
	var System = require("system");

	require("bootstrap");
	require("bootstrapswitch");
	require('common');
	$('#status').on('click', function(e) {
		Fiddler.ResetCertificate(function() {
			$.statusbar("Root Certificate has been reset.",'info');
		});
	});
	$('#proxyoption').on('click', function(e) {
		Fiddler.OpenWinNet(function() {
		});
	});
	$('#calc').on('click', function(e) {
		Fiddler.RunExecutable('calc','',function() {
		});
	});
	$('#inetmgr').on('click', function(e) {
		Fiddler.RunExecutable('inetmgr','',function() {
		});
	});
	$('#certManager').on('click', function(e) {
		Fiddler.OpenCertManager();
	});


	$('#updateserverhref').attr('href',config.ServerPakage).on('click', function(e){
		if(System.UpdateApplication){
			System.UpdateApplication('Calibur.exe',config.ServerPakage,null);
			e.stopPropagation();
			e.preventDefault();
		}
	});


	var $sethttps = $('#sethttps')
	.bootstrapSwitch('size','small')
	.on('switchChange.bootstrapSwitch', function (e, value) {
		Fiddler.SetHttps(value).ReStart(function(){
			$.statusbar("HTTPS proxy has "+(value?'open':'closed'),'info');
		});
	});
	Fiddler.GetHttps&&Fiddler.GetHttps(function(msg){
		$sethttps.bootstrapSwitch('state',msg.Result);
	});

	var $portno = $('#portno')
	   ,$setport = $('#setport').on('click', function (e) {
	   	var port = parseInt($portno.val());
	   	if(port){
	   		Fiddler.SetPort(port).ReStart(function(){
				$.statusbar("Proxy port has been set to " + port,'info');
			});
	   	}else{
	   		$.statusbar("The format('"+$portno.val()+"') of Proxy port is invalid.",'danger');
	   	}
	});

	var timerInject = window.setInterval(function(){
		var version = config.Version;
		var newVersion = function(){
			var $popup = $.showPopup('Update',
				'A new version can be updated.',
				'<button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button><button type="button" class="btn btn-success updatenewversion">Update</button>');
			$popup.find('.updatenewversion').on('click',function(){
				System.UpdateApplication('Calibur.exe',config.ServerPakage,null);
			});
			localStorage.showUpdateVersion=version;
		};
		if(!$.isEmptyObject(System)&&!System.CompareVersion){
			newVersion();
		}
		System.CompareVersion&&System.CompareVersion(version,function(msg){
			window.clearInterval(timerInject);
			if(msg.Result < 0){
				if(localStorage.showUpdateVersion!==version){
					var $popup = $.showPopup('Update',
						'A new version can be updated.',
						'<button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button><button type="button" class="btn btn-success updatenewversion">Update</button>');
					$popup.find('.updatenewversion').on('click',function(){
						System.UpdateApplication('Calibur.exe',config.ServerPakage,null);
					});

					localStorage.showUpdateVersion=version;
				}
				$('#settingtab').html('.');		
				$('#updateserver').show();
			}
		});
	},100);


   var timer = window.setInterval(function(){
		Fiddler.GetPort&&Fiddler.GetPort(function(msg){
			window.clearInterval(timer);
    		$portno.val(msg.Result);
    	});
	},100);
});
