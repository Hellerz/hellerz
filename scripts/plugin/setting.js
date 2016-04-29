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

	$('#updateserverhref').attr('href', config.ServerPakage);
	var $sethttps = $('#sethttps').bootstrapSwitch('size','small')
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

	// var timerInject = window.setInterval(function(){
	// 	Fiddler.Inject&&Fiddler.Inject(config.ServerPakage,'HEAD','','','',function(msg){
	// 		window.clearInterval(timerInject);
 //    		$portno.val(msg.Result);
	// 	});
	// },5000);

	var timerInject = window.setInterval(function(){
		System.CompareVersion&&System.CompareVersion(config.Version,function(msg){
			window.clearInterval(timerInject);
			if(msg.Result < 0){
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
