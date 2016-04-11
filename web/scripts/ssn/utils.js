define(["calibur"], function(Calibur) {
	var Utils={};
	Calibur.ImplSchema("UtilsHelper", function(methods) {
		Calibur.extend(Utils, methods);
	});
	return Utils;
});