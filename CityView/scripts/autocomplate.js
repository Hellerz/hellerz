define([],function() {
// 百度地图API功能
	var map = require("map");

	// 定义一个控件类,即function
	function AutoComplateControl() {
		this.defaultAnchor = BMAP_ANCHOR_TOP_RIGHT;
		this.defaultOffset = new BMap.Size(10, 40);
	}

	// 通过JavaScript的prototype属性继承于BMap.Control
	AutoComplateControl.prototype = new BMap.Control();

	// 自定义控件必须实现自己的initialize方法,并且将控件的DOM元素返回
	// 在本方法中创建个div元素作为控件的容器,并将其添加到地图容器中
	AutoComplateControl.prototype.initialize = function(map) {
		var self = this;
		var div = document.createElement("div");

		var searchDiv = document.createElement("div");
		searchDiv.id = "searchResultPanel";
		searchDiv.style = "border:1px solid #C0C0C0;width:300px;height:auto; display:none;";


		var input = document.createElement('input');
		input.type = "text";
		input.style.width = "300px";
		input.size = 20;
		input.id = "suggestId";
		
		div.appendChild(searchDiv);
		div.appendChild(input);
			
		map.getContainer().appendChild(div);

		var ac = new BMap.Autocomplete({
			"input" : input,
			"location" : map
		});

		ac.addEventListener("onhighlight", function(e) {  //鼠标放在下拉列表上的事件
			var str = "";
			var _value = e.fromitem.value;
			var value = "";
			if (e.fromitem.index > -1) {
				value = _value.province +  _value.city +  _value.district +  _value.street +  _value.business;
			}    
			str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;
			
			value = "";
			if (e.toitem.index > -1) {
				_value = e.toitem.value;
				value = _value.province +  _value.city +  _value.district +  _value.street +  _value.business;
			}    
			str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
			searchDiv.innerHTML = str;
		});

		ac.addEventListener("onconfirm", function(e) {    //鼠标点击下拉列表后的事件
			var _value = e.item.value;
			var	myValue = _value.province +  _value.city +  _value.district +  _value.street +  _value.business;
			searchDiv.innerHTML ="onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;
			
			var local = new BMap.LocalSearch(map, { //智能搜索
			  	onSearchComplete: self.onSearchComplete
			});
			local.search(myValue);
		});

		return div;
	}

	AutoComplateControl.prototype.onSearchComplete = function(results){
	};

	return AutoComplateControl;
	
	
});