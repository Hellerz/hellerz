window.WebVersion = '201609192318';
window.ServiceVersion = '1.0.6096';
requirejs.config({
	urlArgs: "version=" + window.WebVersion,
	paths: {
		config: 'config',
		common: 'common',

		jquery: 'lib/jquery-2.1.4',
		bootstrap: 'lib/bootstrap',
		bootstrapswitch: 'lib/bootstrap-switch',
		bootstrapselect: 'lib/bootstrap-select',
		bootstraptypeahead: 'lib/bootstrap-typeahead',
		format: 'lib/jquery.format',
		qrcode: 'lib/jquery.qrcode.min',
		//Grid
		grid: 'lib/grid',
		guid: 'lib/guid',

	},
	shim: {
		'bootstrap': {
			deps: ['jquery'],
		},
		'bootstrapselect': {
			deps: ['jquery', 'bootstrap'],
		},
		'bootstrapswitch': {
			deps: ['jquery', 'bootstrap'],
		},
		'bootstraptypeahead': {
			deps: ['jquery', 'bootstrap'],
		},

		'grid': {
			deps: ['jquery'],
		},

		//format
		'format': {
			deps: ['jquery'],
		},
		'qrcode': {
			deps: ['jquery'],
		},

	}
});
define(function(require, exports, module) {

	var $ = require("jquery"),
		config = require("config"),
		common = require("common");

	//load jQuery Plugin
	require("grid");
	require("bootstrap");
	require('bootstrapselect');
	require('format');
	$(document).ready(function() {
		$('#load_screen').hide();
		$.ajaxSetup({ dataType: "json",contentType:'application/json'});

		var $Search = $('#QueryMockInfoList');
		var $Add = $('#AddMockUI');
		var $Update = $('#UpdateMockInfo');
		var $Submit = $('#AddMockInfo');
		var $Duplicate = $('#DuplicateMockInfo');
		var $ReplaceCID = $('#ReplaceCID');

		//筛选项
		var $query_deviceid = $('#query_deviceid');
		var $query_name = $('#query_name');
		var $query_env = $('#query_env');
		var $query_operator = $('#query_operator');
		var $query_mid = $('#query_mid');
		var $query_appid = $('#query_appid');

		//mock信息
		var $info_desc = $('#info_desc');
		var $info_deviceid = $('#info_deviceid');
		var $info_operator = $('#info_operator');
		var $info_name = $('#info_name');
		var $info_env = $('#info_env');
		var $info_appid = $('#info_appid');
		var $info_match = $('#info_match');
		var $info_isregex = $('#info_isregex');
		var $info_mid = $('#info_mid');
		var $info_isvalid = $('#info_isvalid');
		var $info_responser = $('#responser');


		var $replace_cid = $('#replace_cid');


			
		var fws_pre = 'http://gateway.m.fws.qa.nt.ctripcorp.com/restapi/soa2/10124/';
		var uat_pre = 'http://gateway.m.uat.qa.nt.ctripcorp.com/restapi/soa2/10124/';

		//var fws_pre = 'http://localhost:3915/';
		//var uat_pre = 'http://bookingws.package.uat.qa.nt.ctripcorp.com/tour-h5-h5service/';
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
		var responserEditor = $.CreateEditor($info_responser);
		var isAddNew = true;//增加info时，选中增加的tr
		$Update.hide();
	    $Submit.hide();
	    $Duplicate.hide();
		responserEditor.getSession().setMode("ace/mode/javascript");
		responserEditor.on('paste',function(e){
			window.setTimeout(function(){formatEditer(responserEditor,"javascript");},400);
		});
		$('.can-select').on('selectstart', function(e) {
			e.stopPropagation()
		});
		$('.selectpicker').selectpicker({
			noneSelectedText:'GET',
			style:'btn-sm'
		});
		if(!localStorage['setting']){
			localStorage['setting'] = JSON.stringify({"author":"","appid":"860501","env":"FWS"});
		}
		var initInfo =function(){
			var setting = JSON.parse(localStorage['setting']);
			$query_env.selectpicker('val',setting.env)
			$info_operator.val(setting.author);
			$query_appid.val(setting.appid);

		    $info_desc.focus().val('');
		    $info_deviceid.val('');
		    $info_name.val('');
		    $info_match.val('');
		    $info_isregex.prop('checked',false);
		    $info_isvalid.prop('checked',true)
		    $info_mid.html('');
		    responserEditor.setValue('');
		    $Update.hide();
		    $Duplicate.hide();
		    $Submit.show();
		};
		var setting = JSON.parse(localStorage['setting']);

		$query_env.selectpicker('val',setting.env)
		$query_operator.val(setting.author);
		$query_appid.val(setting.appid);

		$info_env.selectpicker('val',setting.env);
		$info_operator.val(setting.author);
		$info_appid.val(setting.appid);


		$Search.on('click', function() {
			search();
		});

		$Add.on('click', function() {
			initInfo();
		});

		$Submit.on('click', function() {
			$Submit.hide();
			var url = 'GetMockInfo.json';
			if ($info_env.val() === 'FWS') {
				url = fws_pre + url;
			} else {
				url = uat_pre + url;
			}
			localStorage['setting'] = JSON.stringify({
				"author":$info_operator.val(),
				"appid":$info_appid.val(),
				"env":$info_env.val()
			});
			var request = {
				MockFilterInfo: {
					AppId: $info_appid.val(),
					AppName: $info_name.val(),
					DeviceId: $info_deviceid.val(),
					Author: $info_operator.val(),
					ValidStatus: $info_isvalid.prop('checked')?1:0,
					MatchRequest: $info_match.val(),
				    Description: $info_desc.val(),
				    Response: responserEditor.getValue(),
				    IsRegex: $info_isregex.prop('checked'),
				},
				Operation: "INSERT"
			}; 
			$.post(url,JSON.stringify(request) , function(respose) {
				$Submit.show();
				if(respose&&respose.ResponseStatus){
					if(respose.ResponseStatus.Ack==='Success'){
						var info =respose.Data.MockInfoList[0];
						isAddNew =true;
						$mockpanel.addRow(info, $mockpanel.rowsLength());
						$info_mid.html(info.Mid);
						$Update.show();
						$Duplicate.show();
		    			$Submit.hide();
						$.statusbar("Insert Success",'info');
					}else if(respose.ResponseStatus.Errors&&respose.ResponseStatus.Errors.length){
						$.statusbar(respose.ResponseStatus.Errors[0].Message,'danger');
					}else{
						$.statusbar('No Data Insert','danger');
					}
				}else{
					$.statusbar('No Data Insert','danger');
				}
			});
		});

		$Update.on('click', function() {
			var url = 'GetMockInfo.json';
			if ($info_env.val() === 'FWS') {
				url = fws_pre + url;
			} else {
				url = uat_pre + url;
			}
			localStorage['setting'] = JSON.stringify({
				"author":$info_operator.val(),
				"appid":$info_appid.val(),
				"env":$info_env.val()
			});
			var request = {
				MockFilterInfo: {
					Mid: $info_mid.html(),
					AppId: $info_appid.val(),
					AppName: $info_name.val(),
					DeviceId: $info_deviceid.val(),
					Author: $info_operator.val(),
					ValidStatus: $info_isvalid.prop('checked')?1:0,
					MatchRequest: $info_match.val(),
				    Description: $info_desc.val(),
				    Response: responserEditor.getValue(),
				    IsRegex: $info_isregex.prop('checked'),
				},
				Operation: "UPDATE"
			}; 
			$.post(url, JSON.stringify(request), function(respose) {
				if(respose&&respose.ResponseStatus){
					if(respose.ResponseStatus.Ack==='Success'){
						$mockpanel.updateRow(request.MockFilterInfo,$.data($mockpanel,'id_row')[request.MockFilterInfo.Mid]);
						$.statusbar("Update Success",'info');
					}else if(respose.ResponseStatus.Errors&&respose.ResponseStatus.Errors.length){
						$.statusbar(respose.ResponseStatus.Errors[0].Message,'danger');
					}else{
						$.statusbar('No Data Update','danger');
					}
				}else{
					$.statusbar('No Data Update','danger');
				}
			});
		});

		$Duplicate.on('click', function() {
			$Duplicate.hide();
			$Update.hide();
			$Submit.show();
			$info_mid.html('');
		});

		$ReplaceCID.on('click', function() {
			if(!$replace_cid.val()){
				$.notifybar('请输入要替换的设备号!','warning');
			}

			$.notifybar('请选择要替换的Mock!按住Ctrl可多选','warning');

		});


		var search =function(){
			if(!$query_operator.val()){
				$.notifybar('请在搜索栏创建人输入你的名字！','warning');
				return;
			}
			var url = 'GetMockInfo.json';
			if ($query_env.val() === 'FWS') {
				url = fws_pre + url;
			} else {
				url = uat_pre + url;
			}
			localStorage['setting'] = JSON.stringify({
				"author":$query_operator.val(),
				"appid":$query_appid.val(),
				"env":$query_env.val()
			});
			var request = {
				MockFilterInfo: {
					Mid: $query_mid.val(),
					AppId: $query_appid.val(),
					AppName: $query_name.val(),
					DeviceId: $query_deviceid.val(),
					Author: $query_operator.val(),
					ValidStatus: 0
				},
				Operation: "QUERY"
			}; 
			$.post(url, JSON.stringify(request), function(respose) {
				if(respose&&respose.ResponseStatus){
					if(respose.ResponseStatus.Ack==='Success'){
						$mockpanel.removeRow(function($tbody ){
							$tbody.find('tr').remove(); 
						});
						if(respose.Data.MockInfoList.length===0){
							initInfo();
							return;
						}
						for (var i = 0 ,len = respose.Data.MockInfoList.length; i < len ; i++) {
							$mockpanel.addRow(respose.Data.MockInfoList[i], $mockpanel.rowsLength());
						};
						$.statusbar("Success Fetch "+len+" Datas",'info');
					}else if(respose.ResponseStatus.Errors&&respose.ResponseStatus.Errors.length){
						$.statusbar(respose.ResponseStatus.Errors[0].Message,'danger');
					}else{
						$.statusbar('No Data','danger');
					}
				}else{
					$.statusbar('No Data','danger');
				}
			});
		};

		//整数前补齐0
		var prefixInteger = function(num, length) {
			return (Array(length).join('0') + num).slice(-length);
		}
		var cols = [{
			title: '#',
			name: 'Mid',
			width: 50,
			sortable: true,
			align: 'center',
			renderer: function(val) {
				return prefixInteger(val, 4);
			}
		}, {
			title: 'Env',
			name: 'AppEnv',
			width: 30,
			sortable: true,
			align: 'center'
		}, {
			title: 'Author',
			name: 'Author',
			width: 50,
			sortable: true,
			align: 'center'
		}, {
			title: 'Api',
			name: 'AppName',
			width: 160,
			sortable: true,
			align: 'left'
		}, {
			title: 'CID',
			name: 'DeviceId',
			width: 160,
			sortable: true,
			align: 'left'
		}];
		var $mockpanel = $('#mockTable').mmGrid({
			height: '100%',
			width: '100%',
			cols: cols,
			items: [],
			sortName: 'Id',
			nowrap: true,
			canDelete:false,
			sortStatus: 'asc',
			multiSelect: true,
			autoLoad: false,
			isAutoScroll: true,
			canSimpleUnselect: false
		});
		$.data($mockpanel,'id_row',[]);
		$mockpanel.on('loadSuccess', function(e, data) {
		}).on('rowInserted', function(e, item, index, $tr) {
			$.data($mockpanel,'id_row')[item.Mid] = $tr;
			if(isAddNew){
				$mockpanel.select($tr);
				isAddNew=false;
			}
		}).on('rowUpdated', function(e, oldItem, newItem, index, $tr) {

		}).on('selected', function(e, $trs) {
			var item = $trs.data('item');
			$info_mid.html(item.Mid);
			$info_appid.val(item.AppId);
			$info_name.val(item.AppName);
			$info_deviceid.val(item.DeviceId);
			$info_operator.val(item.Author);
			$info_isvalid.prop('checked',item.ValidStatus===1);
			$info_match.val(item.MatchRequest);
			$info_desc.val(item.Description);
			$.showEditor(responserEditor,item.Response,'javascript');
			$info_isregex.prop('checked',item.IsRegex);
			$Update.show();
			$Duplicate.show();
		    $Submit.hide();
		});
		$Search.trigger('click');
		
	});
});