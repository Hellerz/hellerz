define(["calibur",'eventtarget','session'], function(Calibur,EventTarget,Session) {
	var _events = new EventTarget();
	var Fiddler = {};
	var _requestFun = function(res, evt) {
		var session = new Session(res.Result);
		_events.dispatchEvent("Request", {
			session: session
		});
		if(!session.manual){
			session.Resume();
		}
	};
	var _responseFun = function(res, evt) {
		var session = new Session(res.Result);
		var args = {
			session: session
		};
		_events.dispatchEvent("Response", args);
		if(!session.manual){
			session.Resume(function(){
				args.callback&&args.callback();
			});
		}
	};

	// var _completeFun = function(res, evt) {
	// 	var session = new Session(res.Result);
	// 	var args = { session: session };
	// 	_events.dispatchEvent("Complete", args);
	// 	args.callback&&args.callback();
	// };

	Fiddler.addRequest = function() {
		if (!_events.hasEventListener("Request")) {
			Calibur.webSocket.addEvent("BeforeRequest", _requestFun);
		}
		var arg = [].slice.call(arguments);
		arg.unshift("Request");
		_events.addEventListener.apply(_events,arg);
	};
	Fiddler.addResponse = function() {
		if (!_events.hasEventListener("Response")) {
			Calibur.webSocket.addEvent("BeforeResponse", _responseFun);
		}
		var arg = [].slice.call(arguments);
		arg.unshift("Response");
		_events.addEventListener.apply(_events,arg);
	};
	// Fiddler.addComplete = function() {
	// 	if (!_events.hasEventListener("Complete")) {
	// 		Calibur.webSocket.addEvent("AfterSessionComplete", _completeFun);
	// 	}
	// 	var arg = [].slice.call(arguments);
	// 	arg.unshift("Complete");
	// 	_events.addEventListener.apply(_events,arg);
	// };

	Fiddler.removeRequest = function(fn) {
		_events.removeEventListener("Request", fn);
		if (!_events.hasEventListener("Request")) {
			Calibur.webSocket.removeEvent("BeforeRequest");
		}
	};
	Fiddler.removeResponse = function(fn) {
		_events.removeEventListener("Response", fn);
		if (!_events.hasEventListener("Response")) {
			Calibur.webSocket.removeEvent("BeforeResponse");
		}
	};
	// Fiddler.removeComplete = function(fn) {
	// 	_events.removeEventListener("Complete", fn);
	// 	if (!_events.hasEventListener("Complete")) {
	// 		Calibur.webSocket.removeEvent("AfterSessionComplete");
	// 	}
	// };
	Calibur.ImplSchema("FiddlerHelper", function(methods) {
    	Calibur.extend(Fiddler, methods);
  	});
	return Fiddler;
});