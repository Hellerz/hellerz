define(["calibur"], function(Calibur) {
	var Directory={};
	Calibur.ImplSchema("DirectoryHelper", function(methods) {
		Calibur.extend(Directory, methods);
	});
	return Directory;
});