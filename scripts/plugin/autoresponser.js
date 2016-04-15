define(function(require, exports, module) {
	var Fiddler = require("fiddler");
	var $ = require("jquery");
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
	Fiddler.addRequest(function(e, args) {
		autoAction('requestScript',args);
	});
	Fiddler.addResponse(function(e, args) {
		autoAction('responseScript',args);
	});
	

	//NORMAL
	var autoNormalSetting=localStorage['autoNormalSetting']||'[]';
	var cols=[
			{title:'If request matches',name:'request',width:400,align:'left'},
			{title:'then respond with...',name:'response',width:400,align:'left'},
			//{title:'Latency',name:'latency',width:75,align:'left'}
		];
	var $autopanel = $('#auto-grid').mmGrid({
		height: '100%',
		width: '100%',
		cols: cols,
		items: JSON.parse(autoNormalSetting),
		nowrap: true,
		sortStatus: 'asc',
		multiSelect: true,
		checkCol:true,
		autoLoad: false,
		showBackboard:false,
	});

	$autopanel.load();


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
