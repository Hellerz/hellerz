requirejs.config({
	paths: {
		cities: "cities"
	},
	shim: {

	}
});

define(function(require, exports, module) {
	var cities = require("cities");

	// 百度地图API功能
	var map = new BMap.Map("allmap"); // 创建Map实例
	map.centerAndZoom(new BMap.Point(104.21, 34), 6); // 初始化地图,设置中心点坐标和地图级别
	//map.addControl(new BMap.MapTypeControl());   //添加地图类型控件
	//map.setCurrentCity("中国");          // 设置地图显示的城市 此项是必须设置的
	map.enableScrollWheelZoom(true); //开启鼠标滚轮缩放
	var cityMapping = [];
	for (var i = 0; i < cities.pySort.length; i++) {
		var curinfo = cities.pySort[i];
		cityMapping[curinfo.id] = curinfo;
	}

	var createLabel = function(point, text, color) {
		var opts = {
			position: point, // 指定文本标注所在的地理位置
			offset: new BMap.Size(0, 0) //设置文本偏移量
		};
		var label = new BMap.Label(text, opts); // 创建文本标注对象
		label.setStyle({
			color: color,
			fontSize: "12px",
			height: "20px",
			lineHeight: "20px",
			fontFamily: "微软雅黑"
		});
		return label;
	}

	var findNear = function(cityInfo, status){
	    var nearSaleCity ={};
	    var distance = Infinity;
	    var flag = true;
	    
	    for (var i = 0; i < cities.pySort.length; i++) {
	        var saleCity = cities.pySort[i];
	        switch(status){
		        case "1":
		        	flag = !saleCity.parentCity;
		        	break;
		        case "2":
		        	flag = !!saleCity.parentCity;
		        	break;
		        default:
		        	flag = true;
		        	break;
	        }
	        if(flag&&saleCity.location&&cityInfo.location.lat>0&&cityInfo.location.long>0)
	        {
	            var newDistance = Math.pow(Math.pow(saleCity.location.lat-cityInfo.location.lat,2)+Math.pow(saleCity.location.long-cityInfo.location.long,2),0.5);
	            if(newDistance<distance)
	            {
	                nearSaleCity=saleCity;
	                distance = newDistance;
	            }
	        }
	    }
	    return nearSaleCity;
	};

	var createMarker = function(point, color) {
		return new BMap.Marker(point, {
			// 指定Marker的icon属性为Symbol
			icon: new BMap.Symbol(BMap_Symbol_SHAPE_POINT, {
				scale: 1, //图标缩放大小
				fillColor: color, //填充颜色
				fillOpacity: 1 //填充透明度
			})
		});
	}

	var renderCityPoint = function(map,city,color){
		var cityPoint = new BMap.Point(city.location.long, city.location.lat);
		map.addOverlay(createMarker(cityPoint, color));
		map.addOverlay(createLabel(cityPoint, city.name, color));
	}
	var renderSubCity = function(map,city,parentCity,isSub){
		var cityPoint = new BMap.Point(city.location.long, city.location.lat);
		var parentCityPoint = new BMap.Point(parentCity.location.long, parentCity.location.lat);
		var polyline = new BMap.Polyline([cityPoint, parentCityPoint], {
			strokeColor: isSub?"black":"#f70254",
			strokeWeight: 2,
			strokeOpacity: 1
		});
		map.addOverlay(polyline);
		renderCityPoint(map,city, isSub?'black':"#099fde");
		var center = new BMap.Point((city.location.long + parentCity.location.long) / 2, (city.location.lat + parentCity.location.lat) / 2);
		var distance = (city.name + "-" + parentCity.name + " " + (map.getDistance(cityPoint, parentCityPoint) / 1000).toFixed(2)) + ' KM。';
		map.addOverlay(createLabel(center, distance, isSub?'#1e801e':"green"));

	}

	

	for (var i = 0; i < cities.pySort.length; i++) {
		var curinfo = cities.pySort[i];
		if (curinfo.location) {
			if (curinfo.parentCity) {
				var parentCityInfo = cityMapping[curinfo.parentCity];
				renderSubCity(map,curinfo,parentCityInfo);
			} else {
				renderCityPoint(map,curinfo,'red');
			}
		} else {

		}
	}



	// 定义一个控件类,即function
	function CityNearControl() {
		// 默认停靠位置和偏移量
		this.defaultAnchor = BMAP_ANCHOR_TOP_RIGHT;
		this.defaultOffset = new BMap.Size(10, 10);
	}

	// 通过JavaScript的prototype属性继承于BMap.Control
	CityNearControl.prototype = new BMap.Control();

	// 自定义控件必须实现自己的initialize方法,并且将控件的DOM元素返回
	// 在本方法中创建个div元素作为控件的容器,并将其添加到地图容器中
	CityNearControl.prototype.initialize = function(map) {
		// 创建一个DOM元素
		var div = document.createElement("div");

		var slction = document.createElement('select');
		var op1 = document.createElement('option');
		op1.value = "0";
		op1.text = "全部";
		var op2  = document.createElement('option');
		op2.value = "1";
		op2.text = "真实售卖站";
		var op3  = document.createElement('option');
		op3.value = "2";
		op3.text = "虚拟售卖站";
		slction.appendChild(op1);
		slction.appendChild(op2);
		slction.appendChild(op3);

		var llat = document.createElement('label');
		llat.innerHTML = 'lat';
		var ilat = document.createElement('input');
		ilat.type = 'text';
		ilat.value = '29.8';
		var llong = document.createElement('label');
		llong.innerHTML = 'long';
		var ilong = document.createElement('input');
		ilong.type = 'text';
		ilong.value = '121.5';

		var button = document.createElement('input');
		button.type = 'button';
		button.value = 'ENTER';

		div.appendChild(slction);
		div.appendChild(llat);
		div.appendChild(ilat);
		div.appendChild(llong);
		div.appendChild(ilong);
		div.appendChild(button);
		// 设置样式
		div.style.cursor = "pointer";
		div.style.backgroundColor = "white";
		button.onclick = function(e) {
			var curinfo = {
                name: "定位",
                countryId: 1,
                ename: "dingwei",
                location: {
                    long: parseFloat(ilong.value),
                    lat: parseFloat(ilat.value)
                }
            };
			renderSubCity(map,curinfo,findNear(curinfo,slction.selectedOptions[0].value),true);
			map.centerAndZoom(new BMap.Point(curinfo.location.long, curinfo.location.lat), 10);
		}
			// 添加DOM元素到地图中
		map.getContainer().appendChild(div);

		// 将DOM元素返回
		return div;
	}

	// 创建控件
	var cityNearControl = new CityNearControl();
	// 添加到地图当中
	map.addControl(cityNearControl);
});