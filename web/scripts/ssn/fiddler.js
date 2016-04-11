define(["calibur",'eventtarget','session'], function(Calibur,EventTarget,Session) {
	var _events = new EventTarget();
	var Fiddler = {};
	var _requestFun = function(res, evt) {
		var session = new Session(res.Result);
		_events.dispatchEvent("Request", {
			session: session
		});
		session.Resume();
	};
	var _responseFun = function(res, evt) {
		var session = new Session(res.Result);
		var args = {
			session: session
		};
		_events.dispatchEvent("Response", args);
		session.Resume(function(){
			args.callback&&args.callback();
		});
	};
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
	Calibur.ImplSchema("FiddlerHelper", function(methods) {
    	Calibur.extend(Fiddler, methods);
  	});
	return Fiddler;
});