define(["config",'websocket'], function(config,WebSocketEx) {
  var type = ({}).toString;
  var arr = [];
  var slice = arr.slice;


  var Calibur = function() {};
  Calibur.IsType = function(o, typeName) {
    return type.call(o) === '[object ' + typeName + ']';
  };
  //转utf-8
  Calibur.UTF16to8 = function (str) {  
    var out, i, len, c;  
    out = "";  
    len = str.length;  
    for(i = 0; i < len; i++) {  
    c = str.charCodeAt(i);  
    if ((c >= 0x0001) && (c <= 0x007F)) {  
        out += str.charAt(i);  
    } else if (c > 0x07FF) {  
        out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));  
        out += String.fromCharCode(0x80 | ((c >>  6) & 0x3F));  
        out += String.fromCharCode(0x80 | ((c >>  0) & 0x3F));  
    } else {  
        out += String.fromCharCode(0xC0 | ((c >>  6) & 0x1F));  
        out += String.fromCharCode(0x80 | ((c >>  0) & 0x3F));  
    }  
    }  
    return out;  
  };
  //当对象存在再执行，延时执行
  Calibur.SyncTimer = function(action,timer){
    var timer = window.setInterval(function(){
      action(function(){
        window.clearInterval(timer);
      });
    },timer||100);
  };
  Calibur.extend = function() {
    var options, name, src, copy, copyIsArray, clone,
      target = arguments[0] || {},
      i = 1,
      length = arguments.length,
      deep = false;

    // Handle a deep copy situation
    if (typeof target === "boolean") {
      deep = target;

      // skip the boolean and the target
      target = arguments[i] || {};
      i++;
    }

    if (typeof target !== "object" && !type.call(target) !== '[object Function]') {
      target = {};
    }

    if (i === length) {
      target = this;
      i--;
    }

    for (; i < length; i++) {
      // Only deal with non-null/undefined values
      if ((options = arguments[i]) != null) {
        // Extend the base object
        for (name in options) {
          src = target[name];
          copy = options[name];

          // Prevent never-ending loop
          if (target === copy) {
            continue;
          }

          if (copy !== undefined) {
            target[name] = copy;
          }
        }
      }
    }

    return target;
  };
  Calibur.Fiddler = {}

  var schemaCache = {};
  //获取服务端方法Schema集合
  var getJSchema = function(schema,isFlush) {
    if (schemaCache[schema]&&!isFlush) return schemaCache[schema];
    schemaCache[schema] = new Promise(function(resolve, reject) {
      Calibur.webSocket.shuttle({
        Name: "JSchema",
        Body: {
          Assembly: "CEF.Lib",
          Class: schema
        }
      }, function(res, socket) {
        resolve(res);
      });
    });
    return schemaCache[schema];
  };
  //同步调用服务端方法,并执行回调
  var invokeMethod = function(invokeParam) {
    return new Promise(function(resolve, reject) {
      Calibur.webSocket.shuttle({
        Name: "Instance",
        Body: {
          Operate: "InvokeMethod",
          MemberPath: invokeParam.path,
          InstanceParameters: invokeParam.inc,
          MemberParameters: invokeParam.met
        }
      }, function(res, socket) {
        res=res||{};
        var lastReturn = invokeParam.fn && invokeParam.fn(res.Result,res, socket);
        res.LastReturn=lastReturn;
        resolve(res);
      });
    });
  };
  //预处理arguments，转换成对象形式
  //{
  //  inc:[fn,[a,b]],
  //  met:[fn,[a,b]],
  //  fn:fn
  //}
  var preArguments = function() {
    var rest, finals = {};
    if (arguments.length === 0) return finals;
    rest = slice.call(arguments);
    if (Calibur.IsType(rest[0], 'Object')) { //option 形式
      finals = rest[0];
    } else {
      finals.fn = popLastCallback(rest);
      finals.met = popLastCallback(rest); //met == fn
      finals.inc = popLastCallback(rest); //inc == fn
      if (!finals.met) { //met != fn / inc != fn
        finals.met = rest;
      } else if (!finals.inc) { //met == fn / inc != fn
        finals.inc = rest;
      }
    }
    return finals;
  };
  var popLastCallback = function(rest) {
    var fnCallback = rest.pop();
    if (!Calibur.IsType(fnCallback, 'Function')) {
      rest.push(fnCallback);
      fnCallback = undefined;
    }
    return fnCallback;
  };

  var promiseCache = {};
  Calibur.RestartSchema = function(){
    promiseCache={};
  };
  //注册服务端方法集合
  Calibur.ImplSchema = function(schemaName, callback,isFlush) {
    Promise.resolve(getJSchema(schemaName,isFlush)).then(function(schema) {
      var method = {},
        i, len;
      if (schema && schema.MemberList) {
        var list = schema.MemberList;
        for (i = 0, len = schema.MemberList.length; i < len; i++) {
          (function(memberInfo) {
            var name = memberInfo.Member;
            var path = schema.Assembly + "." + schema.Class + "." + memberInfo.Member;
            method[name] = function() {
              var hasCtor, hasParam, finals;
              var _this = this;
              hasCtor = schema.HasArgCtor && !memberInfo.IsStatic;
              hasParam = memberInfo.Types.length > 0;

              finals = preArguments.apply(null, arguments);
              if (this.getIncParam && !finals.inc) {
                finals.inc = this.getIncParam.apply(this, arguments);
              }
              finals.inc = hasCtor ? finals.inc : undefined;
              finals.met = hasParam ? finals.met : undefined;
              finals.path = path;
              if(promiseCache[schemaName]){
                this.promise = promiseCache[schemaName];
              }else{
                this.promise = Promise.resolve();
                promiseCache[schemaName]=this.promise;
              }
              this.promise = this.promise.then(function(msg) {
                var lastRet = msg&&msg.LastReturn;
                _this.onReturn&&_this.onReturn.call(msg,lastRet,msg);
                if(Calibur.IsType(finals.inc,"Function")){
                  finals.inc = finals.inc.call(msg,lastRet,msg);
                }
                if(Calibur.IsType(finals.met,"Function")){
                  finals.met = finals.met.call(msg,lastRet,msg);
                }
                if(Calibur.IsType(finals.stopmet,"Function")){
                  finals.stopmet = finals.stopmet.call(msg,lastRet,msg);
                }
                if(!finals.stopmet){
                  return invokeMethod(finals);
                }else{
                  return new Promise(function(resolve, reject) {
                     resolve();
                  });
                }
              });
              return this;
            };
          }(list[i]));
        }
      }
      callback && callback(method);
    });
  };
  Calibur.Status = "init";//init:初始化，started：已启动，restart:表示处于重启状态
  Calibur.StartSocket = function(){
    Calibur.webSocket = WebSocketEx(config.websocketUrl);
  }
  //初始化WebSocket对象
  Calibur.StartSocket();
  Calibur.Status = "started";
  return Calibur;
})