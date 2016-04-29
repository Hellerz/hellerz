define(["calibur"], function(Calibur) {
	var System={};
	Calibur.ImplSchema("SystemHelper", function(methods) {
		Calibur.extend(System, methods);
	});
	return System;
});