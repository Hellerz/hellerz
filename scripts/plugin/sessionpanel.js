define(function(require, exports, module) {

	var Fiddler = require("fiddler");
	var $ = require("jquery");
	require("grid");
	require('common');
	var prefixInteger = function (num, length) { 
		return (Array(length).join('0') + num.toString(16).toUpperCase()).slice(-length); 
	}
	var cols=[{title:'#',name:'Id',width:55,sortable:true,align:'center',renderer:function(val){return prefixInteger(val,5);}},{title:'Code',name:'ResponseCode',width:50,sortable:true,align:'center'},{title:'Prtcl',name:'UriScheme',width:45,sortable:true,align:'center'},{title:'Host',name:'Host',width:120,sortable:true,align:'right'},{title:'URL',name:'PathAndQuery',width:450,sortable:true,align:'left'}];
	var $ssnpanel = $('#mmg').mmGrid({
		height: '100%',
		width: '100%',
		cols: cols,
		items: [],
		sortName: 'Id',
		nowrap: true,
		sortStatus: 'asc',
		multiSelect: true,
		autoLoad: false,
		isAutoScroll:true,
		canSimpleUnselect:false
	});
	var sessionfilter = {
		regex:[{
			re:/\/(.+?)?(json|javascript)/,
			ext:' js json ',
			type:'js'
		},{
			re:/\/(.+?)?xml/,
			ext:' xml xhtml ',
			type:'xml'
		},{
			re:/\/(.+?)?css/,
			ext:' css ',
			type:'css'
		},{
			re:/image\//,
			ext:' png gif jpg jpeg bmp ico tiff ',
			type:'img'
		},{
			re:/audio\//,
			ext:' mp3 ',
			type:'media'
		},{
			ext:' ttf otf svg eot woff woff2  ',
			type:'font'
		},{
			re:/\/(.+?)?html/,
			ext:' html htm aspx ',
			type:'doc'
		}],
		re_urlExt:/\/([^\/?]+?\.([^\/?\.]+?))(\?.+)?$/,
		getUrlExtLower:function(url){
			if(url){
				var m = url.match(this.re_urlExt);
				if(m&&m.length){
					return m[2].toLowerCase();
				}
			}
			return null;
		},
		getMatchType : function(typeStr,ext){
			if(typeStr){
				for (var i = 0; i < this.regex.length; i++) {
					var re_re = this.regex[i].re;
					if(re_re&&re_re.test(typeStr)){
						return 'filter-'+this.regex[i].type;
					}
				};
			}
			if(ext){
				for (var i = 0; i < this.regex.length; i++) {
					var re_ext =this.regex[i].ext;
					if(ext&&re_ext&&re_ext.indexOf(' '+ext+' ')>-1){
						return 'filter-'+this.regex[i].type;
					}
				};
			}
			return 'filter-other';
		},
		getFilterType : function(session){
			var reqHeaders =session.RequestHeaders;
			var resHeaders =session.ResponseHeaders;
			var ext = this.getUrlExtLower(session.RequestPath);
			if(reqHeaders['X-Requested-With'] === 'XMLHttpRequest'){
				return 'filter-xhr';
			}
			var typeStr =resHeaders['Content-Type']||reqHeaders['Content-Type']||reqHeaders['accept'];
			return this.getMatchType(typeStr,ext);
		},
		re_klass:/\bwrapper-.+?\b/g,
		init : function($filtergroup,$wrapper){
			$filtergroup.on('click','[data-filter]',function(e){
				var $filter =$(e.target);
				var $filters = $filtergroup.children('[data-filter]');
				var $all = $filters.eq(0);
				var klass =$filter.data('filter');
				var wrapklass = 'wrapper-'+klass;
				var isactive =$wrapper.hasClass(wrapklass);
				var $tmpWrapper = $('<div>').addClass($wrapper.attr('class'));
				if(wrapklass==='wrapper-all'||!e.ctrlKey){
					$filters.each(function(i,itm){
						$tmpWrapper.removeClass('wrapper-'+ $(itm).removeClass('label-info').data('filter'));
					});
					$filter.addClass('label-info');
					$tmpWrapper.addClass(wrapklass);
				}
				else if(e.ctrlKey){
					$tmpWrapper.removeClass('all');
					$all.removeClass('label-info');
					var klasses = $tmpWrapper.attr('class');
					var m =klasses.match(this.re_klass);
					if(m&&m.length===1)
					if(!( m&&m.length===1 && m[0] === wrapklass)){
						$tmpWrapper.toggleClass(wrapklass);
						$filter.toggleClass('label-info');
					}
				}
				$wrapper.attr('class',$tmpWrapper.attr('class'));
			});
		},
	};
	$.data($ssnpanel,'id_row',{});
	$ssnpanel.on('loadSuccess', function(e, data) {
		sessionfilter.init($('#mmg-filter'),$ssnpanel.$bodyWrapper);
		$('#mmg-filter [data-filter]:first').trigger('click');
	}).on('rowInserted', function(e, item, index, $tr) {
		$tr.addClass('filter-all '+ sessionfilter.getFilterType(item));
	}).on('rowUpdated', function(e, oldItem, newItem, index, $tr) {
		var klass = $tr.attr('class');
		klass = klass.replace(/\bfilter-.+?\b/g,'');
		$tr.attr('class',klass + ' filter-all '+ sessionfilter.getFilterType(newItem));
	});
	var $pausessn = $('#pausessn');
	
	var timer = window.setInterval(function(){
		Fiddler.IsPauseSession&&Fiddler.IsPauseSession(function(msg){
			window.clearInterval(timer);
			$pausessn.toggleClass('off',!msg.Result);
			$pausessn.trigger('switch',msg.Result);
		});
	},100);
	
    $pausessn.on('click', function(e) {
    	Fiddler.IsPauseSession(function(msg){
    		if(msg.Result){
    			Fiddler.SetPauseSession(false,function() {
					$.statusbar("AutoResponser has closed.",'info');
					$pausessn.addClass('off');
					$pausessn.trigger('switch',false);
				});
    		}else{
    			Fiddler.SetPauseSession(true,function() {
					$.statusbar("AutoResponser has open.");
					$pausessn.removeClass('off','info');
					$pausessn.trigger('switch',true);
				});
    		}
    	});
	});
	var clearssn = $('#clearssn').on('click',function(e){
		$ssnpanel.removeRow(function($tbody ){
			var klasses = $ssnpanel.$bodyWrapper.attr('class').match(/\bwrapper-(.+?)\b/g);
			if(klasses&&klasses.length){
				$.each(klasses,function(i,itm){
					$tbody.find('.'+ itm.replace('wrapper','filter')).remove(); 
				});
				Fiddler.ClearAllSession();
			}
		});
	});
	exports.SessionPanel = $ssnpanel;
	exports.pausessn=$pausessn;
});
