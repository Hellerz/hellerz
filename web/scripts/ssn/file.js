define(["calibur"], function(Calibur) {
	var File={};
	Calibur.ImplSchema("FileHelper", function(methods) {
		Calibur.extend(File, methods);
	});
	return File;
});