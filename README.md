# Calibur
## 简介
  Calibur是基于FiddlerCore进行的二次开发，目的在于改善用户体验，友好的数据格式，并支持查询替换以及语法高亮，同时支持js脚本定制化篡改请求和响应，支持Fiddler的部分常用功能。由于交互界面用的是h5，更容易扩展、维护和更新。  

## 环境，安装/使用
环境要求：
  1. .Net Framework 4.0及以上
  2. Chrome浏览器
  3. Windows OS  
  
安装/使用 
下载[Calibur](http://hellerz.github.io/hellerz/server/Calibur/bin/Release/Calibur.exe)服务端并打开，如果有杀毒软件弹出框，需要允许应用程序运行和访问网络。通常Calibur会默认使用Chrome浏览器打开UI界面（首次运行需要加载ACE编辑器js资源，比较耗时）。  
服务端开启后，在任务栏通知区域出现应用logo，右键可打开UI界面和关闭应用，关闭应用的同时会关闭UI界面。

## 基本结构
Calibur应用包括客户端和服务端，客户端通常用于UI的交互和展示，服务端用于跟操作系统通信和监听`http(s)`数据。客户端跟服务端之间通过`websocket`进行通信；UI界面分为菜单栏、会话区域、功能区域、和底部的状态栏。
### 菜单栏  
界面的左上角有个小logo，表示**监听**状态，彩色旋转表示开启**监听**，灰色表示关闭，通过点击可切换状态；  
右边的菜单栏可以切换下方的功能区。**包括**
  1. `Inspector`：展示`http(s)`请求响应数据，包括`header`和`body`
  2. `AutoResponder`：分为`Normal`和`Advance`两种模式，`Normal`模式仅对匹配的请求重新代理到新的资源，包括本地和网络上的。
  3.  `Composer`：分为`Normal`和`Raw`，`Normal`类似于`Postman`，只要输入请求地址，请求方式，报文格式，请求头(选填)，请求体，就能发送`http(s)`请求。`Raw`直接发送一个未加工标准的`http(s)`协议报文，通常能更好地模拟请求。
  4. `Settings`：一些配置信息和一些额外的功能。  

### 会话区域
这里的会话是指一个终端用户与交互系统进行通讯的过程，通常一次http(s)请求响应就是一个会话。会话区域包括会话菜单栏和会话列表。
当服务端监听到这些会话数据就会展示在会话区域的表格里，通常会展示**会话号**，**耗时**，**请求方法**，**请求地址**，**协议类型**，**响应码**这些信息，由于一个网页需要加载的资源比较多，可以通过菜单栏的分类按钮筛选。
如果之前的会话已经没用了，可以全部清除。会话菜单栏最左边的实心圆表示**篡改**状态，蓝色表示开启，否则表示关闭。

#### 快捷键  
当焦点在会话区域上时，可以通过下列快捷键进行操作
* ↑选择上一个会话
* ↓选择下一个会话
* delete从列表删除一个会话
* home 选择列表的第一个会话
* end 选择列表最后一个会话
* pageup 会话列表移动到上一页
* pagedown 会话列表移动到下一页
* ctrl+a 选择所有会话
* ctrl+leftclick 依次选择会话

### 功能区
功能区包含四个部分，`Inspector`(查看会话)、`AutoResponder`(自动代理会话)、`Composer`(模拟会话)、`Settings`(设置)
### Inspector  
此功能可查看当前会话的所有数据，**包括**  
* `Statistics` 会话在`Socket`各个阶段的时间、`http(s)`协议报文结构化数据(除了`body`)，其他数据  
* `Headers` 请求响应的报文头信息，PS：由于使用Json格式展示，Key重复的信息只会展示最后一个
* `TextView` 暂无
* `HexView` 暂无
* `FormsView` Json的格式展示请求地址查询参数，即 `?version=201610251741 --> {"version": "201610251741"}`
* `Raw` 原始的http(s)协议报文
* `Format` 格式化的请求响应Body，支持的格式有`javascript`,`json`,`css`,`xml`,`html`,`text`
* `QrCode` 请求地址的二维码

### AutoResponder
此功能可**改篡**`http(s)`请求和响应报文，开启此功能必须开启**篡改**状态；  
此功能有**普通模式**和**进阶模式**两种模式  
#### 普通模式  
普通模式可添加匹配规则，包括描述、匹配的请求、代替原本响应的资源地址，
 
### 状态栏
一些状态更改的提示，用颜色表示不同的状态等级。
 
## AutoResponder 高级用法

## 库引用  
* ace编辑器  
* jQuery及其插件qrcode、format
* bootstrap及其插件select、switch、typeahead
* ztree
* mmGrid
* guid
* requirejs

