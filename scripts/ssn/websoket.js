define(function(require, exports, module) {
  var Guid = require('guid');
  var type = ({}).toString;
  webSocket = function(url) {
    return new webSocket.fn.init(url);
  };
  webSocket.fn = webSocket.prototype = {
    __url : null,
    __webSocket : null,
    __shuttleCache: {},
    __readyStateCallbackCache: [],
    __eventListenerCache:[],
    __sendMessage: function(message) {
      if (this.__webSocket.readyState === 1) {
        this.__webSocket.send(message);
      } else {
        this.__readyStateCallbackCache.push(message);
      }
    },
    __invokeReadyStatusCallback : function() {
      while (this.__readyStateCallbackCache[0]) {
        this.__webSocket.send(this.__readyStateCallbackCache.pop());
      }
      this.__webSocket.removeEventListener("open", this.__invokeReadyStatusCallback)
    },
    __message : function(evt) {
      try {
        var message = JSON.parse(evt.data);
        if (message && message.Status && message.Status.ACK === 1) {
          this.onServerError && this.onServerError(evt);
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
    },
    init: function(url) {
      __url = url;
      this.__webSocket = new window.WebSocket(__url);
      this.addEventListener('open', this.__invokeReadyStatusCallback.bind(this));
      this.addEventListener('message', this.__message.bind(this));
    },
    reConnection:function(callback){
      var self = this;
      var timer = window.setTimeout(function(){
        var oldSocket = self.__webSocket;
        self.__webSocket = new window.WebSocket(__url);
        //重新绑定事件
        var i,len;
        for (i = 0, len = self.__eventListenerCache.length; i < len; i++) {
          var cur = self.__eventListenerCache[i];
          if(cur){
            oldSocket.removeEventListener(cur.eventName, cur.callback);
            self.__webSocket.addEventListener(cur.eventName, cur.callback);
          }
        }
        oldSocket = null;
        callback && self.__webSocket.addEventListener('open', callback.bind(self.__webSocket));
      },1500);
    },
    getReadyState:function(){
      if(this.__webSocket){
        return this.__webSocket.readyState;
      }
      return 0;
    },
    getUrl:function(){
      return __url;
    },
    getFailedConnected:function(isConnected){
      if(this.__webSocket){
        return this.__webSocket.FailedConnected;
      }
      return false;
    },
    setFailedConnected:function(isConnected){
      if(this.__webSocket){
        this.__webSocket.FailedConnected = isConnected;
      }
    },
    addMessageEvent: function(id, callback) {
      this.__shuttleCache[id] = callback;
    },
    addEventListener:function(eventName, callback, capturing){
      this.__eventListenerCache.push({
        eventName:eventName,
        callback:callback
      });
      this.__webSocket.addEventListener(eventName, callback, capturing);
    },
    removeEventListener:function(eventName, callback){
      var i,len;
      if(!eventName){//clear all
        for (i = 0, len = this.__eventListenerCache.length; i < len; i++) {
          var cur = this.__eventListenerCache[i];
          if(cur){
            this.__webSocket.removeEventListener(cur.eventName, cur.callback);
          }
        }
        this.__eventListenerCache.length = 0;
      }
      else if(eventName&&!callback){ //clear eventName
        for (i = 0, len = this.__eventListenerCache.length; i < len; i++) {
          var cur = this.__eventListenerCache[i];
          if(cur.eventName === eventName){
            delete this.__eventListenerCache[i];
            this.__webSocket.removeEventListener(cur.eventName, cur.callback);
          }
        }
      }
      else{//remove special Listener
        for (i = 0, len = this.__eventListenerCache.length; i < len; i++) {
          var cur = this.__eventListenerCache[i];
          if(cur.eventName === eventName&&cur.callback === callback){
            delete this.__eventListenerCache[i];
            this.__webSocket.removeEventListener(eventName, callback);
            break;
          }
        }
      }
    },
    addEvent: function(eventName, callback) {
      if (!(this.__shuttleCache[eventName] && this.__shuttleCache[eventName].length)) {
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
    },
    removeEvent: function(eventName, callback) {
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
    },
    shuttle: function(message, callback) {
      var id = Guid.raw();
      this.__shuttleCache[id] = callback;
      message = JSON.stringify({
        ID: id,
        Type: 1,
        Msg: message
      });
      this.__sendMessage(message);
    }
  };
  webSocket.fn.init.prototype = webSocket.fn;

  return webSocket;
});