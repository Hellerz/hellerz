define(function(require, exports, module) {
	var Fiddler = require("fiddler");
	var $ = require("jquery");
	var $ssnpanel = require('sessionpanel').SessionPanel;
	var pausessn = require('sessionpanel').pausessn;
	var File = require('file');
	var Directory = require('directory');

	require('bootstraptypeahead');
	require('common');
	require("ztreecore");
	require("ztreeexcheck");
	require("ztreeexedit");

	var $autoResponder = $('#autoResponder');
	var $addRule = $('#addRule');
	var $importRule = $('#importRule');
	var $exportRule = $('#exportRule');

	var $saveRule = $('#saveRule');
	var $deleteRule = $('#deleteRule');

	var $savecrt = $('#savecrt');
	var $cancelcrt = $('#cancelcrt');

	var $autoRequest = $('#auto-request');
	var $autoRespond = $('#auto-respond');

	var $autonor = $('#auto-nor');
	var $autowrap = $autonor.find('.auto-grid-wrap');
	var $automap =  $autonor.find('.auto-url-map');
	var $autocrt =  $autonor.find('.auto-file-crt');

	var $crteditor =  $('#crteditor');
	var $autoIndicate = $('.auto-indicate');
	var crteditor = $.CreateEditor($crteditor);
	var ispause = false;
	pausessn.on('switch',function(e,status){
		ispause=status;
		$autoResponder.toggleClass('unpausessn',!status);
	});
	$autoIndicate.on('click',function(){
		pausessn.trigger('click');
	});
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

	var autoReqRe = /((regex|exact):|)(.+)/i;
	var autoResRe = /(\*|\w:\\|http(s?):\/\/|).+/i;
	var aynreq = function(reqPattern){
		var match = reqPattern.match(autoReqRe);
		if(match&&match.length===4){
			return {
				"option":(match[2]||'exact').toLowerCase(),
				"text":match[3],
			} 
		}
		return null;
	};
	var aynres = function(resPattern){
		var match = resPattern.match(autoResRe);
		if(match&&match.length===3){
			return {
				"option":match[1].toLowerCase(),
				"text":match[0]
			} 
		}
		return null;
	};
	var isMatchRequest = function(url,reqmatch){
		if(reqmatch&&url){
			if(reqmatch.option==='regex'){
				return new RegExp(reqmatch.text,'i').test(url);
			}else{
				return url.indexOf(reqmatch.text) >- 1;
			}
		}
		return false;
	};

	var autoRedirect = function(context){
		var setting =$autopanel.rows()
		  , session = context.session
		  , len = setting.length
		  , index = 0
		  , cursetting
		  , match
		  , reqmatch
		  , resmatch
		if(setting.length>0){
			for (; index < len; index++) {
				cursetting=setting[index];
				if(cursetting.checked&&cursetting.request&&cursetting.response){
					reqmatch = aynreq(cursetting.request);
					if(isMatchRequest(session.FullUrl,reqmatch)){
						resmatch = aynres(cursetting.response);
						if(reqmatch.option === 'regex'){
							resmatch.text = session.FullUrl.replace(new RegExp(reqmatch.text,'i'),resmatch.text);
						}
						if(resmatch.text.indexOf('http')>-1){//web
							session.SetfullUrl(resmatch.text);
						}else if(resmatch.option.indexOf(':\\')>-1){//file
							session.SetBypassGateway(true)
							.UtilCreateResponseAndBypassServer()
							.LoadResponseFromFile(resmatch.text)
							.UtilChunkResponse(1)
							.SetResponseHeaders(JSON.stringify({
							    "Access-Control-Allow-Headers": "accept, content-type, cookieorigin",
							    "Access-Control-Allow-Origin":session.RequestHeaders.Origin || "*",
							    "Access-Control-Allow-Credentials": "true",
							    "Access-Control-Allow-Methods":"POST",
							}));
						}else if(resmatch.option === '*'){//function

						}
					}
				}
			};
		}
	};
	Fiddler.addRequest(function(e, args) {
		if(ispause){
			autoRedirect(args);
			autoAction('requestScript',args);
		}
	});
	Fiddler.addResponse(function(e, args) {
		if(ispause){
			autoAction('responseScript',args);
		}
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
	var saveEditor=function(editor){
		File.SaveDialog("Save File",'','所有文件|*.*',0,function(msg){
			if(msg.Result&&msg.Result.length){
				File.CreateFile(msg.Result[0],editor.getValue(),'UTF8',function(){
					editor.setValue('');
					var path = msg.Result[0];
					$autoRespond.val(path);
					$.statusbar('file '+path+' saved successfully.','success');
					collapscrt();
					$saveRule.trigger('click');
				});
			}
		});
	};
	
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
	        saveEditor(editor);
	    },
	    readOnly: false
	});
	$savecrt.on('click',function(e){
		saveEditor(crteditor);
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
	$autopanel.on('cellAfterSelected',function(){
		var  hasSelect = $autopanel.selectedRows().length>0;
		 $automap.toggleClass('disabled',!hasSelect);
		 if(!hasSelect){
		 	$autoRequest.val('');
			$autoRespond.val('');
		 }
	}).on('selected', function(e, $trs) {
		var item = $trs.data('item');
		$autoRequest.val(item.request);
		$autoRespond.val(item.response);
		$('#auto-cur-uid').val(item.uid);
		$trs.eq(0).find('input').get(0).focus()
	}).on('rowInserted', function(e, item, index, $tr) {
		if(item.checked){
			$autopanel.select($tr)
		};
		autoNormalSetting.push(item);
		setAutoNormalSetting();
		$automap.toggleClass('disabled',$autopanel.selectedRows().length==0);
	}).on('rowUpdated', function(e, oldItem, newItem, index, $tr) {
		autoNormalSetting = $autopanel.rows();
		setAutoNormalSetting();
	}).on('rowsRemoved', function(e) {
		autoNormalSetting = $autopanel.rows();
		setAutoNormalSetting();
		$autoRequest.val('');
		$autoRespond.val('');
	}).load();
	$addRule.on('click',function(e){
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
	$saveRule.on('click',function(e){
		var uid =$('#auto-cur-uid').val();
		var item =findNormalSetting(uid);
		item.request=$autoRequest.val();
		item.response=$autoRespond.val();
		var $curTr = $autopanel.find('[uid="'+uid+'"]').parents('tr');
		$autopanel.updateRow(item,$curTr);
	});
	$deleteRule.on('click',function(e){
		var uid = $('#auto-cur-uid').val();
		var $curTr = $autopanel.find('[uid="'+uid+'"]').parents('tr');
		$autopanel.removeRow([$curTr.index()]);
	});
	var extendcrt = function(height){
		var mapHeight = $automap.outerHeight();
		// $autocrt.css({
		// 	height:height,
		// 	marginTop:-height
		// });
		$autowrap.animate({bottom: mapHeight+height}, 'fast',function(){
			$autopanel.resize();
		} );
		$automap.animate({marginTop: -(mapHeight+height) }, 'fast' );
		crteditor.focus();
	};
	var collapscrt = function(){
		var mapHeight = $automap.outerHeight();
		// $autocrt.css({
		// 	height:500,
		// 	marginTop:-500
		// });
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
		var $a =$(e.target);
		var oprate = $a.attr('oprate');
		if(oprate === 'create'){
			extendcrt($autocrt.outerHeight());
		}else if(oprate === 'find'){
			File.OpenDialog("Select a response file",'','所有文件|*.*',0,function(msg){
				if(msg.Result&&msg.Result.length){
					$autoRespond.val(msg.Result[0]);
				}
			});
		}else{
			$autoRespond.val($a.html());
		}
	});

	var pathSplitRegex = /(.+\\)(.*)/;
	var pathEndRegex = /(.+?)(\\+)$/;
	var correctPath =function(path){
		if(!path)return path;
		if(!pathEndRegex.test(path)){
			return path+'\\';
		}else{
			return path.replace(/(.+?)(\\+)$/,'$1\\');
		}
	};
	$autoRespond.typeahead({
		items : 30,
		source: function(query, process) {
			var corPath = correctPath(query);
			Directory.Exists(corPath,function(msg){
				if(msg.Result){
					Directory.GetFileFolders(corPath,function(member){
						process(member.Result||[]);
					});
				}else{
					var m =query.match(pathSplitRegex);
					if(m&&m.length===3&&m[2]){
						var path =m[1];
						var search =m[2]+'*';
						Directory.Exists(path,function(exists){
							if(exists.Result){
								Directory.GetFileFoldersSearch(path,search,function(member){
									process(member.Result||[]);
								});
							}
						});
					}
				}
			});
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
