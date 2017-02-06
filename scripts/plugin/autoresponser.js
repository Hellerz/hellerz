define(function(require, exports, module) {
	var Fiddler = require("fiddler");
	var $ = require("jquery");
	var Session = require("session");
	var Storage = require("storage");
	var $ssnpanel = require('sessionpanel').SessionPanel;
	var pausessn = require('sessionpanel').pausessn;
	var File = require('file');
	var Directory = require('directory');
	var ARSettings = require('autoresponsersetting');

	require('bootstraptypeahead');
	require('common');
	require("ztreecore");
	require("ztreeexcheck");
	require("ztreeexedit");
	require('beautify');
	var $autoResponder = $('#autoResponder');
	var $addRule = $('#addRule');
	var $importRule = $('#importRule');
	var $exportRule = $('#exportRule');
	var ConfigKeyHelper = $.storageHelper;

	var $editRule = $('#editRule');
	var $saveRule = $('#saveRule');
	var $deleteRule = $('#deleteRule');

	var $saveascrt = $('#saveascrt');
	var $savecrt = $('#savecrt');

	var $closecrt = $('#closecrt');

	var $autoDescription = $('#auto-description');
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
	//AutoResponder下的Advanced下，修改请求和响应的
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

	var  autoReqRe= /(\w+):((\/(.+?)\/([igm]*)\s)|(.+?\s(?=\w+:))|(.+\s))|(.+)/ig;
	var autoResRe = /(\*|\w:\\|http(s?):\/\/|).+/i;
	var aynreq = function(reqPattern){
		var match,matchRules=[];
		while(match,match = autoReqRe.exec(reqPattern+' ')){
			if(match&&match.length===9){
				if(match[1]){
					match[1] = match[1].toLowerCase();
					if(match[1] === 'https'||match[1] === 'http'){
						match[1]= 'url' ;
						match[2] = match[1]+match[2];
					} 
				}
				var rule = {
					"option":match[8]?'url':match[1],
					"regex":match[4],
					"flags":match[5],
					"text":match[8]||match[7]||match[6],
				};
				rule.option = rule.option&&rule.option.trim();
				rule.regex = rule.regex&&rule.regex.trim();
				rule.flags = rule.flags&&rule.flags.trim();
				rule.text = rule.text&&rule.text.trim();
				matchRules.push(rule);
			}
		}
		return matchRules;
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
	var reqRule = {
		url : function(session,rule){
			return session.FullUrl;
		},
		method : function(session,rule){
			return session.Method;
		},
		body : function(session,rule){
			return session.RequesetBody;
		},
		head : function(session,rule){
			return JSON.stringify(session.RequestHeaders);
		},
	};
	
	var isMatchRequest = function(session,rules){
		var valid = rules.length;
		for (var i = rules.length - 1; i >= 0; i--) {
			var rule = rules[i];
			var ioperate = reqRule[rule.option](session);
			if(rule&&ioperate){
				if(rule.regex&&new RegExp(rule.regex,rule.flags).test(ioperate)){
				}
				else if(ioperate.indexOf(rule.text) >- 1){
				}
				else{
					return false;
				}
			}
		};
		return valid;
	};
	//AutoResponder下的Normal下，代理响应到本地或重定向到其他链接
	var autoRedirect = function(context){
		var setting =$autopanel.rows()
		  , session = context.session
		  , len = setting.length
		  , index = 0
		  , cursetting
		  , match
		  , reqmatches
		  , resmatch
		if(setting.length>0){
			for (; index < len; index++) {
				cursetting=setting[index];
				if(cursetting&&cursetting.checked&&cursetting.request&&cursetting.response){
					reqmatches = aynreq(cursetting.request);
					if(isMatchRequest(session,reqmatches)){
						resmatch = aynres(cursetting.response);

						if(reqmatches){
							for (var matchIndex = reqmatches.length - 1; matchIndex >= 0; matchIndex--) {
								var ireqmatch = reqmatches[matchIndex];
								if(ireqmatch&&ireqmatch.option==='url'&&ireqmatch.regex){
									resmatch.text = session.FullUrl.replace(new RegExp(ireqmatch.regex,reqmatches.flags),resmatch.text);
								}
							};
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
		//localStorage['autoNormalSetting'] =JSON.stringify($autopanel.rows());
		ConfigKeyHelper.setStorageValueByKey('autoNormalSetting',$autopanel.rows(),function(){
			//回调函数
			$.statusbar("autoNormalSetting设置成功!!");
		})
	};

	var findNormalSetting = function(uid){
		var iautoNormalSetting = $autopanel.rows();
		var len=iautoNormalSetting.length,i=0;
		for(;i<len;i++){
			if(uid === iautoNormalSetting[i].uid){
				return iautoNormalSetting[i];

			}
		}

	};
	var silentSaveEditor = function(editor){
		var item = $automap.data('selectedItem');
		if(item&&item.response){
			File.Exists(item.response,function(isExist){
				if(isExist){
					File.CreateFile(item.response,editor.getValue(),'UTF8',function(){
						$autoRespond.val(item.response);
						$.statusbar('file '+item.response+' saved successfully.','success');
						$editRule.show();
						$saveRule.trigger('click');
					});
				}else{
					saveEditor(editor);
				}
			});
		}else{
			saveEditor(editor);
		}

	};
	var saveEditor=function(editor){
		var fileName = '';
		var item = $automap.data('selectedItem');
		if(item&&item.data){
			fileName = item.data.SuggestedFilename;
		}
		File.SaveDialog("Save File",'',fileName,'所有文件|*.*',0,function(files){
			if(files&&files.length){
				var path = files[0];
				File.CreateFile(path,editor.getValue(),'UTF8',function(){
					$autoRespond.val(path);
					$.statusbar('file '+path+' saved successfully.','success');
					$editRule.show();
					$saveRule.trigger('click');
				});
			}
		});
	};
	crteditor.commands.addCommand({
	    name: 'Close',
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
	        silentSaveEditor(editor);
	    },
	    readOnly: false
	});
	$savecrt.on('click',function(e){
		silentSaveEditor(crteditor,true);
	});
	$saveascrt.on('click',function(e){
		saveEditor(crteditor);
	});
	
	var autoNormalSetting;
	ConfigKeyHelper.getStorageValueByKey("autoNormalSetting",function(data){
			autoNormalSetting = data;
	});
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
			{title:'Description',name:'description',width:100,align:'left'},
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
		var ss =$autopanel.rows();
        e.stopPropagation();
	});
	$autopanel.on('cellAfterSelected',function(){
		var  hasSelect = $autopanel.selectedRows().length>0;
		 $automap.toggleClass('disabled',!hasSelect);
		 collapscrt();
		 if(!hasSelect){
		 	$autoDescription.val('');
		 	$autoRequest.val('');
			$autoRespond.val('');
			$automap.data('selectedItem',null);
		 }
	}).on('selected', function(e, $trs) {
		var item = $trs.data('item');
		$automap.data('selectedItem',item);
		$autoDescription.val(item.description);
		$autoRequest.val(item.request);
		$autoRespond.val(item.response);
		$('#auto-cur-uid').val(item.uid);
		$trs.eq(0).find('input').get(0).focus();
		if(item&&item.response){
			File.Exists(item.response,function(isExist){
				$editRule.toggle(isExist);
			});
		}else{
			$editRule.hide();
		}
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
		$autoDescription.val('');
		$autoRequest.val('');
		$autoRespond.val('');
	}).load();
	$addRule.on('click',function(e){
		var item = {};
		var selectItem = $ssnpanel.selectedRows()[0];
		var uid =Math.random().toString().replace(/0\.0*/,'');
		if(selectItem&&selectItem.FullUrl){
			item.request = selectItem.FullUrl;
			item.brush = $.getMode(selectItem.ResponseHeaders['Content-Type']);
			item.data = selectItem;
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
		item.description=$autoDescription.val();
		item.request=$autoRequest.val();
		item.response=$autoRespond.val();
		var $curTr = $autopanel.find('[uid="'+uid+'"]').parents('tr');
		$autopanel.updateRow(item,$curTr);
	});
	$editRule.on('click',function(e){
		var item = $automap.data('selectedItem');
		if(item&&item.response){
			File.Exists(item.response,function(isExist){
				if(isExist){
					extendcrt($autocrt.outerHeight());
					File.ReadAllText(item.response,function(fileContent){
						var fmt = $.vkbeautify[item.brush](fileContent);
						$.showEditor(crteditor,fmt,item.brush);
					});
				}
			});
		}
	});
	$deleteRule.on('click',function(e){
		var uid = $('#auto-cur-uid').val();
		var $curTr = $autopanel.find('[uid="'+uid+'"]').parents('tr');
		$autopanel.removeRow([$curTr.index()]);
	});
	var extendcrt = function(height){
		var mapHeight = $automap.outerHeight();
		$autowrap.animate({bottom: mapHeight+height}, 'fast',function(){
			$autopanel.resize();
		} );
		$automap.animate({marginTop: -(mapHeight+height) }, 'fast' );
		crteditor.focus();
	};
	var collapscrt = function(){
		var mapHeight = $automap.outerHeight();
		$autowrap.animate({bottom: mapHeight}, 'fast',function(){
			$autopanel.resize();
		} );
		$automap.animate({marginTop: -mapHeight }, 'fast' );
		crteditor.blur();
	};
	$closecrt.on('click',function(e){
		collapscrt();
	});
	$('.auto-respond-list').on('click','a',function(e){
		var $a =$(e.target);
		var oprate = $a.attr('oprate');
		if(oprate === 'create'){
			extendcrt($autocrt.outerHeight());
			var item = $automap.data('selectedItem');
			if(item!=null&&item.data!=null){
				var session = item.data;
				if(session instanceof Session)
				{
					session.GetResponseBodyAsString(function(fileContent){
						var fmt = $.vkbeautify[item.brush](fileContent.Return);
						$.showEditor(crteditor,fmt,item.brush);
					});
				}
			}
		}else if(oprate === 'find'){
			File.OpenDialog("Select a response file",'','所有文件|*.*',0,function(files){
				if(files&&files.length){
					$autoRespond.val(files[0]);
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
			Directory.Exists(corPath,function(isExists){
				if(isExists){
					Directory.GetFileFolders(corPath,function(folders){
						process(folders||[]);
					});
				}else{
					var m =query.match(pathSplitRegex);
					if(m&&m.length===3&&m[2]){
						var path =m[1];
						var search =m[2]+'*';
						Directory.Exists(path,function(exists){
							if(exists){
								Directory.GetFileFoldersSearch(path,search,function(folders){
									process(folders||[]);
								});
							}
						});
					}
				}
			});
		}
    });

	//ADVANCED
	var newCount = 1,
		editorTreeNode,
		zTree,
		setting = {
        view: {
            addHoverDom: function (treeId, treeNode) {
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
			onCheck(e, treeId, treeNode) {
				setAutoResponserSettings();
			}
		}
    };
    
    
    //换成本地文件实现 author:zkj time:2017 1 19
	//localStorage['AutoResponserSettings'] = JSON.stringify(zNodes);
	ConfigKeyHelper.getStorageValueByKey("AutoResponserSettings",function(zNodes){
		if(!(zNodes&&zNodes[0]&&zNodes[0].children)){
	    	zNodes = ARSettings.config;
			ConfigKeyHelper.setStorageValueByKey("AutoResponserSettings",zNodes,function(){
		    	$.statusbar("AutoResponserSettings设置成功");
		    });
	   	 }
    	 zTree = $.fn.zTree.init($("#autoResponserTree"), setting, zNodes);

	});
	
    var setAutoResponserSettings = function(){
    	//localStorage['AutoResponserSettings'] = JSON.stringify(zTree.getNodes());
    	ConfigKeyHelper.setStorageValueByKey("AutoResponserSettings",zTree.getNodes(),function(){
    		$.statusbar("AutoResponserSettings设置成功");
    	})
    }

    var requester = $.CreateEditor($('#requester'));
	requester.getSession().setMode("ace/mode/javascript");
	requester.commands.addCommand({
	    name: 'Save',
	    bindKey: {win: 'Ctrl-S',  mac: 'Command-S'},
	    exec: function(editor) {
	        editorTreeNode.requestScript = editor.getValue();
	        setAutoResponserSettings();
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
	        setAutoResponserSettings();
	    },
	    readOnly: false 
	});
	$('#autoresponder-nav a:first').tab('show');
});
