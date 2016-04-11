define(["calibur",'eventtarget'], function(Calibur,EventTarget) {
	var Session = function(session) {
		Calibur.extend(this, session);
		this.promise = Promise.resolve(session);
	}

	Session.prototype = {
		getIncParam: function(array) {
			return [this.Id];
		},
		onReturn:function(msg){
			if(msg){
				Calibur.extend(this, msg.Id?msg:msg.Result);
			}
		},
		ClassPath: "CEF.Lib.JSession.",
	};
	Calibur.ImplSchema("JSession", function(methods) {
		Calibur.extend(Session.prototype, methods)
	});

	return Session;
});