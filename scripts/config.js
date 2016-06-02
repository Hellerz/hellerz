define(function() {
    var config = {
    	Version : window.ServiceVersion,
    	websocketUrl : "ws://127.0.0.1:8181/",
    	ServerPakage :window.location.origin + window.location.pathname + 'server/Calibur/bin/Debug/Calibur.exe'
    };
    return config;
});