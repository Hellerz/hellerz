define(function(require, exports, module) {
	var Fiddler = require("fiddler");
	var $ = require("jquery");
	var $ssnpanel = require('sessionpanel').SessionPanel;
	var File = require('file');
	require('common');
	require("ztreecore");
	require("ztreeexcheck");
	require("ztreeexedit");

	
	var autoAction=function(scriptName,context){
		var setting =zTree.getNodes();
		var resper =  setting[0];
		if(resper.checked&&resper.children){
			for (var parentIndex = 0; parentIndex < resper.children.length; parentIndex++) {
				var parent = resper.children[parentIndex];
				if(parent.checked&&parent.children){
					for (var leafIndex = 0; leafIndex < parent.children.length; leafIndex++) {
						var leaf = parent.children[leafIndex];
						if(leaf.checked){
							var sctipt = leaf[scriptName];
							(function(context){
								eval(sctipt);
							}(context));
						}
					};
				}
			};
		}
	};

	var autoReqRe = /((regex|direct):|)(.+)/i;
	var autoResRe = /(\*|\w:\\|http(s?):\/\/|).+/i;
	var isMatchRequest = function(url,reqPattern){
		var match = reqPattern.match(autoReqRe);
		if(match&&match.length===4){
			return {
				"option":match[2]||'direct',
				"text":match[3],
				"match":""
			} 
		}
		return null;
	};
	var aynres = function(resPattern){
		var match = resPattern.match(autoResRe);
		if(match&&match.length===3){
			return {
				"option":match[1],
				"text":match[0]
			} 
		}
		return null;
	};
	var autoRedirect = function(context){
		var setting =$autopanel.rows()
		  , session = context.session
		  , index = 0
		  , len = setting.length
		  , match
		  , reqmatch
		  , resmatch
		if(setting.length>0){
			for (; index < len; index++) {
				if(setting[i].checked){
					//if(isMatchRequest(session.FullUrl,setting[i].request);
					//resmatch = aynres(setting[i].request);
					
				}
			};
		}

	};
	Fiddler.addRequest(function(e, args) {
		//autoRedirect(args);
		autoAction('requestScript',args);
	});
	Fiddler.addResponse(function(e, args) {
		autoAction('responseScript',args);
	});
	

	//NORMAL
	var setAutoNormalSetting = function(){
		localStorage['autoNormalSetting'] =JSON.stringify(autoNormalSetting);
	};
	var findNormalSetting = function(uid){
		var len=autoNormalSetting.length,i=0;
		for(;i<len;i++){
			if(uid === autoNormalSetting[i].uid){
				return autoNormalSetting[i];
			}
		}

	};
	var addAutoUrl = $('#addAutoUrl');
	var saveAutoUrl = $('#saveAutoUrl');
	var deleteAutoUrl = $('#deleteAutoUrl');

	var $savecrt = $('#savecrt');
	var $cancelcrt = $('#cancelcrt');

	var autoRequest = $('#auto-request');
	var autoRespond = $('#auto-respond');

	var $autonor = $('#auto-nor');
	var $autowrap = $autonor.find('.auto-grid-wrap');
	var $automap =  $autonor.find('.auto-url-map');
	var $autocrt =  $autonor.find('.auto-file-crt');

	var $crteditor =  $('#crteditor');
	var crteditor = $.CreateEditor($crteditor);
	crteditor.commands.addCommand({
	    name: 'Cancel',
	    bindKey: {win: 'ESC',  mac: 'ESC'},
	    exec: function(editor) {
	        collapscrt();
	    },
	    readOnly: false
	});
	crteditor.commands.addCommand({
	    name: 'Save',
	    bindKey: {win: 'Ctrl-S',  mac: 'Command-S'},
	    exec: function(editor) {
	        File.SaveDialog("Save File",'C:\\GIT\\github\\FairyKey','文本文件|*.*|C#文件|*.cs|所有文件|*.*',2,function(msg){
				if(msg.Result&&msg.Result.length){
					File.CreateFile(msg.Result[0],editor.getValue(),'UTF8',function(){
						editor.setValue('');
						$.statusbar('file '+msg.Result[0]+' saved successfully.','success');
					});
				}
			});
	    },
	    readOnly: false
	});

	var autoNormalSetting=JSON.parse(localStorage['autoNormalSetting']||'[]');
	var cols=[
			{
				title:'#',name:'checked',width:24,align:'center',
				renderer:function(val,item){
					var checkbox = $('<input type="checkbox">')
					.addClass('auto-nor-uid')
					.attr('uid',item.uid)
					.attr('checked',val?'checked':undefined);
					return checkbox[0].outerHTML;
				}
			},
			{title:'If request matches',name:'request',width:400,align:'left'},
			{title:'then respond with...',name:'response',width:400,align:'left'},
			//{title:'Latency',name:'latency',width:75,align:'left'}
		];
	var $autopanel = $('#auto-grid').mmGrid({
		height: '100%',
		width: '100%',
		cols: cols,
		items: autoNormalSetting,
		nowrap: true,
		sortStatus: 'asc',
		multiSelect: true,
		autoLoad: false,
		showBackboard:false,
		isAutoScroll:true,
	});

	$('#navFun a[href="#autoResponder"]').on('shown.bs.tab',function(){
		$autopanel.resize();
	});
	$autopanel.$body.on('click','.auto-nor-uid',function(e){
		var $checkbox = $(e.target);
		var uid = $checkbox.attr('uid');
		var curItem = findNormalSetting(uid);
		curItem&&(curItem.checked = $checkbox.prop('checked'));
		setAutoNormalSetting();
        e.stopPropagation();
	});
	$autopanel.on('selected', function(e, $trs) {
		var item = $trs.data('item');
		autoRequest.val(item.request);
		autoRespond.val(item.response);
		$('#auto-cur-uid').val(item.uid);
		$trs.eq(0).find('input').get(0).focus()
	}).on('rowInserted', function(e, item, index, $tr) {
		if(item.checked){
			$autopanel.select($tr)
		};
		autoNormalSetting.push(item);
		setAutoNormalSetting();
	}).on('rowUpdated', function(e, oldItem, newItem, index, $tr) {
		autoNormalSetting = $autopanel.rows();
		setAutoNormalSetting();
	}).on('rowsRemoved', function(e) {
		autoNormalSetting = $autopanel.rows();
		setAutoNormalSetting();
		autoRequest.val('');
		autoRespond.val('');
	}).load();
	addAutoUrl.on('click',function(e){
		var item = {};
		var selectItem = $ssnpanel.selectedRows()[0];
		var uid =Math.random().toString().replace(/0\.0*/,'');
		if(selectItem&&selectItem.FullUrl){
			item.request = selectItem.FullUrl;
		}else{
			item.request = 'StringToMatch['+uid+']';
		}
		item.checked = true;
		item.uid = uid;
		$autopanel.addRow(item);
	});
	saveAutoUrl.on('click',function(e){
		var uid =$('#auto-cur-uid').val();
		var item =findNormalSetting(uid);
		item.request=autoRequest.val();
		item.response=autoRespond.val();
		var $curTr = $autopanel.find('[uid="'+uid+'"]').parents('tr');
		$autopanel.updateRow(item,$curTr);
	});
	deleteAutoUrl.on('click',function(e){
		var uid = $('#auto-cur-uid').val();
		var $curTr = $autopanel.find('[uid="'+uid+'"]').parents('tr');
		$autopanel.removeRow([$curTr.index()]);
	});
	var extendcrt = function(height){
		var mapHeight = $automap.outerHeight();
		$autocrt.css({
			height:500,
			marginTop:-500
		});
		$autowrap.animate({bottom: mapHeight+height}, 'fast',function(){
			$autopanel.resize();
		} );
		$automap.animate({marginTop: -(mapHeight+height) }, 'fast' );
		crteditor.focus();
	};
	var collapscrt = function(){
		var mapHeight = $automap.outerHeight();
		$autocrt.css({
			height:500,
			marginTop:-500
		});
		$autowrap.animate({bottom: mapHeight}, 'fast',function(){
			$autopanel.resize();
		} );
		$automap.animate({marginTop: -mapHeight }, 'fast' );
		crteditor.blur();
	};
	$cancelcrt.on('click',function(e){
		collapscrt();
	});
	$('.auto-respond-list').on('click','a',function(e){
		var oprate = $(e.target).attr('oprate');
		if(oprate === 'create'){
			extendcrt(500);
		}
	});
	

	//ADVANCED
	var newCount = 1;
	var editorTreeNode;
	var setting = {
        view: {
            addHoverDom: function (treeId, treeNode) {
            	localStorage['AutoResponserSettings'] = JSON.stringify(zTree.getNodes());
            	if(treeNode.level === 0){
            		$("#"+treeNode.tId+"_edit").hide();
            		$("#"+treeNode.tId+"_remove").hide();
            	};
            	if(treeNode.level > 1){
            		return ;
            	}
            	var addBtnId = "addBtn_"+treeNode.tId;
	            var sObj = $("#" + treeNode.tId + "_span");
	            if (treeNode.editNameFlag || $('#'+addBtnId).length>0) return;
	            var addStr = "<span class='button add' id='" + addBtnId + "' title='add node' onfocus='this.blur();'></span>";
	            sObj.after(addStr);
	            var btn = $('#'+addBtnId);
	            btn && btn.on("click", function(){
	                zTree.addNodes(treeNode, {id:(100 + newCount), pId:treeNode.id, name:"new node" + (newCount++)});
	                return false;
	            });
	        },
            removeHoverDom: function (treeId, treeNode) {
	            $("#addBtn_"+treeNode.tId).unbind().remove();
	        },
	        dblClickExpand: function (treeId, treeNode) {
	            return treeNode.level > 0;
	        },
            selectedMulti: false,
            nameIsHTML:true,
        },
        check: {
            enable: true,
            nocheckInherit :true
        },
        edit: {
            enable: true,
            drag:{
            	isCopy : false,
				isMove : true,
				prev : false,
				inner : false,
				next : true
            }
        },
        callback: {
			beforeDrag: function (treeId, treeNodes) {
				for (var i=0,l=treeNodes.length; i<l; i++) {
					if (treeNodes[i].drag === false) {
						return false;
					}
				}
				return true;
			},
			beforeDrop: function (treeId, treeNodes, targetNode, moveType) {
				var targetLevel = +targetNode.level;
				for (var i=0,l=treeNodes.length; i<l; i++) {
					if (+treeNodes[i].level !== targetLevel) {
						return false;
					}
				}
				return true;
			},
			onClick:function(event, treeId, treeNode){
				if(!treeNode.isParent){
					editorTreeNode = treeNode;
					requester.setValue(treeNode.requestScript||''); 
					responser.setValue(treeNode.responseScript||''); 
				}
			},
		}
    };
    
    var zNodes = localStorage['AutoResponserSettings']||'[{"id": 1,"name": "AutoResponser","drag": false,"open": true,"level": 0, "isParent": true,"zAsync": true,"isFirstNode": true,"isLastNode": true,"isAjaxing": false,"isHover": true,"editNameFlag": false,"checked": true,"checkedOld": true,"nocheck": false,"chkDisabled": false,"halfCheck": false,"check_Child_State": 1,"check_Focus": false}]';

    var zTree = $.fn.zTree.init($("#autoResponserTree"), setting,JSON.parse(zNodes));


    var requester = $.CreateEditor($('#requester'));
	requester.getSession().setMode("ace/mode/javascript");
	requester.commands.addCommand({
	    name: 'Save',
	    bindKey: {win: 'Ctrl-S',  mac: 'Command-S'},
	    exec: function(editor) {
	        editorTreeNode.requestScript = editor.getValue();
	    },
	    readOnly: false
	});
	var responser = $.CreateEditor($('#responser'));
	responser.getSession().setMode("ace/mode/javascript");
	responser.commands.addCommand({
	    name: 'Save',
	    bindKey: {win: 'Ctrl-S',  mac: 'Command-S'},
	    exec: function(editor) {
	        editorTreeNode.responseScript = editor.getValue();
	    },
	    readOnly: false 
	});
	$('#autoresponder-nav a:first').tab('show');
});
