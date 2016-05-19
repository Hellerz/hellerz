define(function(require, exports, module) {
  var Guid = require('guid');
  var type = ({}).toString;
  var invoke = function(constructor, argument) {
    var type = ({}).toString,
      arg,
      len,
      i,
      instance;
    if (type.call(constructor) !== '[object Function]') {
      throw new Error("the first arg is not a Function.");
    }
    if (type.call(argument) !== '[object Arguments]') {
      throw new Error("the second arg is not a Arguments.");
    }
    for (arg = [], len = argument.length, i = 0; i < len; i++) {
      arg.push('argument[' + i + ']');
    };
    eval('instance = new ' + constructor.name + '(' + arg.join(',') + ');');
    return instance;
  };
  WebSocket.prototype.__shuttleCache = {};
  WebSocket.prototype.__readyStateCallbackCache = [];
  WebSocket.prototype.__sendMessage = function(message) {
    if (this.readyState === 1) {
      this.send(message);
    } else {
      this.__readyStateCallbackCache.push(message);
    }
  }
  WebSocket.prototype.addMessageEvent = function(id,callback) {
    this.__shuttleCache[id] = callback;
  };
  WebSocket.prototype.addEvent = function(eventName, callback) {
    if (!this.__shuttleCache[eventName]) {
      this.__shuttleCache[eventName] = [];
      this.__sendMessage(JSON.stringify({
        ID: Guid.raw(),
        Type: 2,
        Msg: {
          Name: "SessionHandler",
          Body: {
            EventType: 1,
            EventName: eventName
          }
        }
      }));
    }
    this.__shuttleCache[eventName].push(callback);
  }
  WebSocket.prototype.removeEvent = function(eventName, callback) {
    var events = this.__shuttleCache[eventName],
      len, i;
    if (!callback) { //removeAll
      events.length = 0;
    }
    if (events) {
      for (i = 0, len = events.length; i < len; i++) {
        if (callback === events[i]) {
          events.splice(i, 1);
          break;
        }
      }
    }
    if (!events || events.length === 0) {
      this.__sendMessage(JSON.stringify({
        ID: Guid.raw(),
        Type: 2,
        Msg: {
          Name: "SessionHandler",
          Body: {
            EventType: 2,
            EventName: eventName
          }
        }
      }));
    }
  }
  WebSocket.prototype.shuttle = function(message, callback) {
    var id = Guid.raw();
    this.__shuttleCache[id] = callback;
    message = JSON.stringify({
      ID: id,
      Type: 1,
      Msg: message
    });
    this.__sendMessage(message);
  };
 
  return function WebSocketEx() {
    var webSocket = invoke(window.WebSocket || window.MozWebSocket, arguments);
    var invokeReadyStatusCallback = function() {
      while (webSocket.__readyStateCallbackCache[0]) {
        webSocket.send(webSocket.__readyStateCallbackCache.pop());
      }
      webSocket.removeEventListener("open", invokeReadyStatusCallback)
    };
    webSocket.addEventListener("open", invokeReadyStatusCallback);
    webSocket.onmessage = function(evt) {
      try {
        var message = JSON.parse(evt.data);
        if(message&&message.Status&&message.Status.ACK ===1){
          this.onServerError&&this.onServerError(evt);
        }
        var callbacks = this.__shuttleCache[message.ID];
        if (message.Type !== 2) {
          delete this.__shuttleCache[message.ID];
        }
        if (type.call(callbacks) === '[object Function]') {
          callbacks(message.Body, evt);
        } else if (type.call(callbacks) === '[object Array]') {
          for (var i = 0; i < callbacks.length; i++) {
            callbacks[i](message.Body, evt);
          }
        }
      } catch (ex) {
        console.dir(ex);
        this.onsimplex && this.onsimplex(evt);
      }
    };
    return webSocket;
  };
});