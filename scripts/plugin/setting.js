define(function(require, exports, module) {
	var Fiddler = require("fiddler");
	var $ = require("jquery");
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
	Fiddler.GetPort&&Fiddler.GetPort(function(msg){
		$portno.val(msg.Result);
	});


	
});
