define(["calibur"], function(Calibur) {
	var Storage={};
	Calibur.ImplSchema("StorageHelper", function(methods) {
		Calibur.extend(Storage, methods);
	});
	return Storage;
});