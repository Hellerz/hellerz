define(["config",'websoket'], function(config,WebSocketEx) {
  var type = ({}).toString;
  var arr = [];
  var slice = arr.slice;


  var Calibur = function() {};
  Calibur.IsType = function(o, typeName) {
    return type.call(o) === '[object ' + typeName + ']';
  };
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

    // Return the modified object
    return target;
  };
  Calibur.Fiddler = {}

  var schemaCache = {};
  var getJSchema = function(schema) {
    if (schemaCache[schema]) return schemaCache[schema];
    schemaCache[schema] = new Promise(function(resolve, reject) {
      Calibur.webSocket.shuttle({
        Name: "JSchema",
        Body: {
          Assembly: "CEF.Lib",
          Class: schema
        }
      }, function(res, socket) {
        resolve(res);
        //reject(Error(req.statusText));
      });
    });
    return schemaCache[schema];
  };

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
        var lastReturn = invokeParam.fn && invokeParam.fn(res, socket);
        res.LastReturn=lastReturn;
        resolve(res, socket);
      });
    });
  };

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



  Calibur.ImplSchema = function(schemaName, callback) {
    Promise.resolve(getJSchema(schemaName)).then(function(schema) {
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
              this.promise = this.promise || Promise.resolve();
              this.promise = this.promise.then(function(msg) {
                _this.onReturn&&_this.onReturn(msg);
                if(Calibur.IsType(finals.inc,"Function")){
                  finals.inc = finals.inc(msg);
                }
                if(Calibur.IsType(finals.met,"Function")){
                  finals.met = finals.met(msg);
                }
                if(Calibur.IsType(finals.stopmet,"Function")){
                  finals.stopmet = finals.stopmet(msg);
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
  
  Calibur.webSocket = new WebSocketEx(config.websocketUrl);

  return Calibur;
})