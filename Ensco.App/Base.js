var RequiredMessage = 'Please fill out the required fields.'
var InitFoldDone
var IsCookie = true
var WimsUrl = 'https://wims.enscoplc.com/wi/'
var Lang
var XMLTranslation
var UserId = "";
var UserName = ""
$(document).ready(function () {
   //InitStorage() 
    var s, src, w, h
    if ($('#__EVENTARGUMENT').length == 0) {
        s = '<input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" /> '
        s += '<input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="" /> '
        $(s).appendTo($('form'))
    }
    SetupTitle()
    CheckIE()
    //GetUser()

    // Get Logged in user information
    var arr = GetUserProfile()
    //arr='011311:Frank Wang:1'
    var items = arr.split(":");
    if (items.length > 2) {
        UserId = items[0];
        UserName = items[1];
    }
    InitAttachment()
    ConfigSave()
    Lang = GetLanguage();//localStorage.Lang
    if (Lang ==null || Lang =='')
        Lang = 'EN'
    Lang=Lang.toUpperCase()
	//Listener() 
});
function Listener() {
    if (GetPageName() == 'popuplistener')
        return
    var xml = GetArray(['usp_JobListener', UserId , Lang ])
    var text = Trim($(xml).text())
    if (text != '' )
        showModal('../job/PopupListener.htm', xml, 900, 800)
    setTimeout(Listener, 10000)
}
function InitStorage(reset) {
    return 
    var dt = localStorage['dt']
    var dt0 = new Date();

    if (!reset && dt != null && dt0 > (new Date(dt))  ) {
        localStorage.removeItem('dt')
        localStorage.removeItem('userId')
    } else {
        dt0.setSeconds(dt0.getSeconds() + 60*20);
        localStorage['dt'] = dt0
        
    }
}
function CheckIE() {
	return 
    if (ie_ver() != 11 && !(navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPod/i))) {
        $('body').hide()
        alert('IE11 is required to run IRMA application. Please have IE11 installed from Ensco IT Portal Manager.')
    }
}
function ie_ver() {
    var iev = 0;
    var ieold = (/MSIE (\d+\.\d+);/.test(navigator.userAgent));
    var trident = !!navigator.userAgent.match(/Trident\/7.0/);
    var rv = navigator.userAgent.indexOf("rv:11.0");

    if (ieold) iev = new Number(RegExp.$1);
    if (navigator.appVersion.indexOf("MSIE 10") != -1) iev = 10;
    if (trident && rv != -1) iev = 11;

    return iev;
}
function GetUser() {
    debugger;
    var url = window.location.toString().toLowerCase()
    if (url.indexOf('popuplogin') > -1)
        return 

    var userId //= localStorage['userId'] 
    var userName// = localStorage['userName'] 
    if (IsCookie) {
        userId = getCookie('userId')
       // alert(userId )
        userName = getCookie('userName')
    }
    if (userId == null  || userId == '') { //alert('is null ')
        var arr = GetUserProfile()
        if (arr.split(':')[0] =='011311'){
            SaveLoginCookie(arr )
            userId = localStorage['userId']
            userName = localStorage['userName']
            if (IsCookie) {
                userId = getCookie('userId')
                userName = getCookie('userName')
            }
        } else {
            SaveLoginCookie(arr)
            userId = localStorage['userId']
            userName = localStorage['userName']
            //on_Login(true)
            //return
        }
    }
    var script = document.createElement("script");

    var code = ' var UserId="' + userId + '", UserName="' + MyReplace( userName, "'", "''") + '"'
    script.setAttribute("type", "text/javascript");
    script.appendChild(document.createTextNode(code));
    document.body.appendChild(script);

    if (window.dialogArguments != null || (  url.indexOf('popupuser') == -1 && url.indexOf('popup') != -1 ) || url.indexOf('empdirectory') != -1 || url.indexOf('.aspx') != -1)
        return
    
    var html = IncludeFile('../common/header.htm?id=1')
    if (GetPagePath().split('/').length==1) 
        html = IncludeFile('common/header.htm?id=1')
    $(html).prependTo($('body'))
    $('#HeaderUserName').text(userName)

    if (UserId == '011311' || localStorage['Impersonate'] == 1)
        $('#011311').show()
    else 
        $('#011311').hide()
}
function on_Logout() {
    return;

    //if (IsCookie) {
    //    setCookie('userId', '')
    //    setCookie('dt', '')

    //} else {
    //    localStorage.removeItem('dt')   
    //    localStorage.removeItem('userId')
    //}
    //var html = '<br><br>Logged out successfully '
    //$('body').html(html)
    //on_Login( false )
   
}

function GetUserProfile() {
    var data = { userId: '' }
    data = JSON.stringify(data)
    var r
    $.ajax({
        type: "POST",
        url: "/IRMA/Jobs/GetUserInfo",
        contentType: "application/json; charset=utf-8",
        data: data,
        dataType: "json",
        success: function (data) {
            r = data.Info;
        },
        failure: function (response) {
            console.error(response.d);
        }
        , async: false
    });
    return r
}
function GetLanguage() {
    var data = { userId: '' }
    data = JSON.stringify(data)
    var r
    $.ajax({
        type: "POST",
        url: "/IRMA/Jobs/GetLanguage",
        contentType: "application/json; charset=utf-8",
        data: data,
        dataType: "json",
        success: function (data) {
            r = data.Info;
        },
        failure: function (response) {
            console.error(response.d);
        }
        , async: false
    });
    return r
}
function SaveLoginCookie(r) {
    var arr = r.split(':')
    userId = arr[0]
    userName = arr[1]
    if (IsCookie) {
        setCookie('userId', userId)
        setCookie('userName', userName)
    }else {
        localStorage['userId'] = userId
        localStorage['userName'] = userName
    }
}
function VerifyLogin(index) {
    return true;

    //var r = Authenticate() 
    //if (Trim(r) == '') {
    //    return false
    //}
    //return true
}
function Authenticate(firstLogin) {
    return;
    //return Popup('../common/popupLogin.htm?l=' + firstLogin, 700, 500)
}
function on_Login(firstLogin) {
    //if (firstLogin)
    //    $('body').html('')
    //var r = Authenticate(firstLogin )

    //if (r == null || r == '') {
    //    if (firstLogin) {
    //        var html = '<br><br>Log in failed. <a href="javascript:void(0)" onclick="Refresh()"  > Please try it again</a>'
    //        $('body').html(html)
    //    }
    //}else {
    //    SaveLoginCookie(r)
    //    InitStorage( true )
    //    Refresh()
    //}
}

$.createElement = function (name, value) {
    return $('<' + name + ' name=' + value + ' />');
};
$.createElement = function (name) {
    return $($.parseXML('<' + name + '/>').documentElement)
    return $('<' + name + '/>');
};
$.fn.addAttr = function (name, value) {
    $(this).attr(name, value)
};
$.fn.removeAttr = function (name) {
    $(this).removeAttr(name)
};
function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}
function escapeXml(unsafe) {
    return unsafe.replace(/[<>&'"]/g, function (c) {
        switch (c) {
            case '<': return '&lt;';
            case '>': return '&gt;';
            case '&': return '&amp;';
            case '\'': return '&apos;';
            case '"': return '&quot;';
        }
    });
}
function htmlEscape(str) {
    return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
}
function htmlUnescape(str) {
    return String(str)
        .replace('&amp;', /&/g)
        .replace('&quot;', /"/g)
        .replace('&#39;', /'/g)
        .replace(/&lt;/g, '<')
        .replace('&gt;', />/g);

    return String(str)
            .replace('&amp;', /&/g)
            .replace('&quot;', /"/g)
            .replace('&#39;', /'/g)
            .replace('&lt;', /</g)
            .replace('&gt;', />/g);
}
function __doPostBack(eventTarget, eventArgument) {
    if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
        theForm.__EVENTTARGET.value = eventTarget;
        theForm.__EVENTARGUMENT.value = eventArgument;
        theForm.submit();
    }
}
function setCookie(cname, cvalue) {
    document.cookie = cname + "=" + cvalue + ";path=/ " //+ expires;
}
function getCookie(c_name) {
    var c_value = " " + document.cookie;
    var c_start = c_value.indexOf(" " + c_name + "=");
    if (c_start == -1) {
        c_value = null;
    }
    else {
        c_start = c_value.indexOf("=", c_start) + 1;
        var c_end = c_value.indexOf(";", c_start);
        if (c_end == -1) {
            c_end = c_value.length;
        }
        c_value = unescape(c_value.substring(c_start, c_end));
    }
    return c_value;
}
function MyReplace(str, s1, s2) {
    if (str == null) return ''
    var re = new RegExp(s1, 'g');
    return str.replace(re, s2)//.replace(s2, s1);
}
function GetSiteUrl() {
    var url = window.location.toString().toLowerCase()
    var index = 4
    if (url.indexOf('enscoplc.com') != -1)
        index = 3
    return url.split('/').slice(0, index).join('/').replace('common', '')
}
function GetArray(arr, fn) {
    if (arr[2] == -2) {
        PostForm(arr)
        return null
    }
    var xml
    if (fn == null) {
        CallAjaxArray(arr, 'RunArray', function (data, status, xhr) {
            xml = data.xml;
        }, function (e) {
            console.error(e.responseText);
        }
        , false
        )
        return xml
    } else {
        CallAjaxArray(arr, 'RunArray', fn, function (e) {
            //alert( e.responseText);
        }
      )
    }
}
function GetArrayRaw(arr) {
    var xml
    CallAjaxArray(arr, 'RunArrayRaw', function (data, status, xhr) {
        xml = data.xml;
    }, function (e) {
        console.error(e.responseText);
    }
    , false
    )
    return xml
}
function GetSearch(arr) {
    var xml
    if (arr[1] == -2) {
        PostForm(arr)
        return null 
    }
    CallAjaxArray(arr, 'RunSearch', function (data, status, xhr) {
        xml = data.xml
    }, function (e) {
        console.error(e.responseText);
    }
    , false
    )
    return xml
}
function PostForm(arr) {
    var f = $('form')
    var div = $('<div/>').appendTo(f)
    var page = GetPageInfo()[1]
    for (var i in arr) {
        $('<input type=text name=n'+i+' />').val( arr[i] ).appendTo(div)
    }
    SetTabIndex() 
    f.attr('method', 'post').attr('action', '../common/excel.aspx?page='+page ).submit()
    div.remove()
}
var CallAjaxArray = function (arr, method, fn1, fn2, mode) {
    var async = true
    if (mode != null)
        async = mode
    $.ajax({
        type: "POST",
        async: async,
        url: "/IRMA/Jobs/" + method,
        data: "{\"array\":" + JSON.stringify(arr) + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: fn1,
        error: fn2
    });
}
var CallAjax = function (name, value, method, fn1, fn2, mode) {
    value = escapeXml(value)
    var parameters = "{'" + name + "':'" + value + "'}"
    var async = true
    if (mode != null)
        async = mode
    $.ajax({
        type: 'POST',
        async: async,
        url: "/IRMA/Jobs/" + method,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: fn1,
        error: fn2
    });
}
function getParameterByName(name) {
    name = name.toLowerCase()
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search.toLowerCase());
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
function SetupTitle() {
    var arr = [] //72, 160, 160, 160, 160, 160, 160, 160, 160, 76, 79]
    for (var i = 0; i < 400; i++)
        arr.push(160)
    var s = String.fromCharCode.apply(String, arr)
    if (typeof window.dialogArguments == 'undefined')
        s = "IRMA" + s
    $(document).prop('title', s)
    $('head').append('<link rel="shortcut icon" href="images/wims_Vwz_icon.ico?test=1" type="image/x-icon">')
}
String.prototype.format = function (args) {
    var str = this;
    return str.replace(String.prototype.format.regex, function (item) {
        var intVal = parseInt(item.substring(1, item.length - 1));
        var replace;
        if (intVal >= 0) {
            replace = args[intVal];
        } else if (intVal === -1) {
            replace = "{";
        } else if (intVal === -2) {
            replace = "}";
        } else {
            replace = "";
        }
        return replace;
    });
};
String.prototype.format.regex = new RegExp("{-?[0-9]+}", "g");
function IsRole() {
    for (var i = 0; i < Roles.length; i++) {
        for (var j = 0; j < arguments.length; j++) {
            if (Roles[i][0] == arguments[j])
                return true
        }
    }
    return false
}
function Refresh() {
    var url = window.location.toString()
    url = MyReplace(url, '#', '')
    window.location = url
}
function Popup(url, w, h) {
    var features = 'dialogWidth:' + w + 'px;dialogHeight:' + h + 'px;center:on';
    var r = showModalDialog(url, null, features);
    return r
}
function showModal(url, data, w, h) {
    var features = 'resizable:Yes; dialogWidth:' + w + 'px;dialogHeight:' + h + 'px;center:on';
    return showModalDialog(url, data, features);
}
function RestoreScroll() {
    var scroll = getCookie('scroll')
    if (scroll != null)
        $(window).scrollTop(scroll);
}
function SaveScroll() {
    setCookie('scroll', $(window).scrollTop());
}
function Encode(s) {
    return MyReplace(s, '<', '!1s23!')
}
function Decode(s) {
    return MyReplace(s, '!1s23!', '<')
}
function SingleQuote(s) {
    return MyReplace(s, "'", "''")
}
Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};
function InitDropDownList() {
    var imgs = $('img[src*=downArrow]')
    imgs.add(imgs.parent().prev().find('input')).on('click', function () {
        var src = $(this)
        var input = src.parent().prev().find('input')
        if (src.prop('tagName') == 'INPUT')
            input = src
        var f = src.closest('table').find('iframe')
        var doc = f.contents()[0];
        if ($(doc.body).html() == '')
            $(doc.body).html(f.attr('data'))

        var offset = input.offset();
        var w = 250, h = 400
        var left = offset.left
        if (left + w > screen.width - 10)
            left = screen.width - w - 22
        f.css({
            position: "fixed",
            'background-color': 'white',
            margin: 1,
            padding: 1,
            width: w,
            height: h,
            top: offset.top - $(window).scrollTop(),
            left: left
        }).show()
        $(document).mousemove(function (e) {
            //var x = e.clientX, y = e.clientY, element = document.elementFromPoint(x, y);
            $('iframe:visible').each(function () {
                var f = $(this)
                if (f.prev()[0].tagName != 'IMG')
                    return
                f.hide()

                var t = f.closest('table')
                var doc = f.contents()[0]
                var count = $(doc.body).find(':checked').length
                var s = ''
                if (count > 0)
                    s = count + ' selected'
                t.find('tr').children().eq(0).children().eq(0).val(s)
            })
        })
    })
}
function AddSearchAttr(root, id) {
    var src = $('#' + id)
    var value
    if (!src.length) {
        var value = $('[name=' + id + ']:checked').val()
        root.addAttr(id, value)
        return
    }
    var tagName = src[0].tagName

    if (tagName == 'IFRAME') {
        var doc = src.contents()[0];
        $(doc.body).find(':checked').each(function () {
            root.append($.createElement(id)).children().last().addAttr('id', $(this).attr('id'))
        })
        return
    }
    else if (['SELECT', 'INPUT'].indexOf(tagName) != -1) {
        value = src.val()
        if (tagName == 'SELECT' && src.prop('selectedIndex') == 0)
            return
    }
    else
        value = src.text()
    value = $.trim(value)
    root.addAttr(id, value)
}
function Trim(s) {
    return $.trim(s)
}
function CleanXml(xml) {
    if (xml == null)
        return null
    else
        return xml.replace('<?XML:NAMESPACE PREFIX = "PUBLIC" NS = "URN:COMPONENT" />', '')
}
function DeleteCookie(name) {
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
function AdjustXml(xml) {
    return MyReplace(xml, 'Table>', 'Table0>')
}
function EscapeBackslash(s) {
    return s.replace(/\\/g, '&#92;');
}
function InitAttachment() {
    $('body').on('change', 'input[type=file]', function () {
        var file = this.files[0];
        if (file == null)
            return
        var name = file.name;
        var size = file.size;
        var type = file.type;
        if (size > 1000000) {
            alert('The max file size is 1M')
            return
        }
        var src = $(this)
        ext = src.val().split('.').pop();
        photo = guid() + '.' + ext
        src.attr('name', photo)
        var xhr = new XMLHttpRequest();
        xhr.file = file;
        var isSuccess
        xhr.onreadystatechange = function (e) {
            if (4 == this.readyState && this.status == 200) {
                isSuccess = true
            }
        };
        var url = '../common/upload.aspx'
        xhr.open('post', url, false);
        var formData = new FormData();
        formData.append(photo, file);
        xhr.send(formData)

        if (isSuccess) {
            var a = src.next()
            if (a.prop('tagName') == 'A')
                a.attr('href', 'upload/' + photo).text(name)
            else
                InsertAttachment(src.parent(), photo, name)
        }
        else
            alert('The file failed to upload. Please try it again.');
        $('input:file').val('')
    })
}
function InsertAttachment(td, guid, name) {
    if (guid == null)
        return 
    guid=guid.replace('../', '')
    var div = '<table ><tr><td width=1><img src=../images/delete.png width=16 onclick=\'$(this).closest("table").remove()\' /><td><a target=_blank href=../upload/' + guid + ' >' + name + '</a></table>'
    td.append($(div)).find('td').css('border', 'none')
}
function ConfigSave() {
    document.onkeydown = function () {
        var k = event.keyCode
        if (k == 27)
            window.close()
        if (!event.ctrlKey)
            return
        if (k == 83 || k == 81 || k == 87) {
            event.cancelBubble = true;
            event.returnValue = false;
            event.keyCode = false;
            if (k == 83 && GetId('Save').length && GetId('Save').is(':visible') ) {
                on_Save()
            }
            if (k == 81)
                on_PDF()
            if (k == 87)
                window.close()
            return false;
        }
    }
    //ConfigKeyPress()
}
//function ConfigKeyPress() {
//    $(':text, :password').keypress(function (e) {
//        if (e.which != 13) return
//        e.preventDefault()
//        var src = $(this)
//        var path = '../images/search.png'
//        var img = src.closest('table').find('[src="../images/search.png"]').last()
//        if (!img.length)
//            img = src.closest('table').find('[src="' + path + '"]').last()
//        if (img.length)
//            img.trigger('click')
//        else
//            on_Save()
//    })
//}