define(function(require, exports, module) {

	var EditSession = require("ace/edit_session").EditSession;
	var UndoManager = require("ace/undomanager").UndoManager;
	var Editor = require("ace/editor").Editor;
	var Renderer = require("ace/virtual_renderer").VirtualRenderer;
	var Fiddler = require("fiddler");
	var $	    = require("jquery");

	var runComposer = function (request) {
    	Fiddler.Inject(request);
    }
    $('#exeInject').on('click',function (e) {
    	runComposer(requestRaw.getValue());
    });
    var requestRaw = new Editor(new Renderer($('#requestRaw').get(0)));
	requestRaw.getSession().setUndoManager(new UndoManager());
	requestRaw.getSession().setMode("ace/mode/text");
	requestRaw.commands.addCommand({
	    name: 'Run',
	    bindKey: {win: 'Ctrl-R',  mac: 'Command-R'},
	    exec: function(editor) {
	        runComposer(editor.getValue());
	    },
	    readOnly: false
	});
	var responseRaw = new Editor(new Renderer($('#responseRaw').get(0)));
	responseRaw.getSession().setUndoManager(new UndoManager());
	responseRaw.getSession().setMode("ace/mode/text");
});