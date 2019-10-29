function DataBind(t, xml, i) {
    t.find('tr').remove()
    var table = 'Table'
    if (i > 0)
        table = 'Table' + i
    $($.parseXML(xml)).find(table).each(function (index) {
        var row = $(this)
        var tr0 = $('<tr/>')
        var tr = $('<tr>')
        row.children().each(function ( ) {
            var column = $(this)
            var name = MyReplace(column.prop('tagName'), '_x0020_', ' ')
            name = MyReplace(name, '_x002F_', ' / ')
            name = MyReplace(name, '_x0023_', ' # ')
            name = MyReplace(name, '_x0025_', ' % ')
            name = MyReplace(name, '_x002A_', '*')
            
            AddTd(tr0, name, 'th')
            AddTd(tr, column.text(), 'td')
            if (index == 0)
                tr0.appendTo(t).addClass('label').children().addClass('label')
            tr.appendTo(t).attr('active', 1)
            if ( tr.index() %2==0)
                tr.addClass('WhiteSmoke')
        })
    })
    t.find('tr').eq(0).find('th').css('border', 'solid').css('border-width', 1)
    InitSorting(t)
    t.find('.label').closest('tr').children().each(function () {
        var src = $(this)
        TranslateLabel(src, null )
    })
}
function AddTd(tr, html, tag, colSpan) { 
    if (colSpan == null)
        colSpan = 1
    if (tag == null)
        tag = 'td'
    tr.append($('<' + tag + '>'))
    var td = tr.children().last()
    td.html(html).attr('colSpan', colSpan)
    return td
}
function SetTabIndex() {
    $('input,select,a, img ').attr("tabindex", -1)
}
function InitSorting(t) {
    var color = 'black'
    var wingding = ' <span id="225" style="margin:0; font-family: Wingdings; color: ' + color + '">&#225;</span>'
    t.find('tr').eq(0).children().each(function () {
        var td = $(this)
        td.append($(wingding).css('margin-left', 5)).append($(MyReplace(wingding, '225', '226')))
    })
    t.off('click')
    t.on('click', function (e) {
        var td = $(e.target)
        var id = td.attr('id')
        if (id == 225 || id == 226)
            td = td.parent()
        if (!td.find('#225').length || td[0].tagName != 'TH')
            return
        td.siblings().find('span').show().css('color', color)
        var span = td.find('span:visible[id=225], span:visible[id=226]')
        var direction = 1

        if (span.length == 2) {
            td.find('#225').hide()
            direction = -1
        } else {
            if (span.attr('id') == '225')
                direction = -1
            else
                direction = 1
            span.hide().siblings().show()
        }
        td.find(':visible').css('color', 'orange')
        Sort(t, direction, td)

    })
}
function InitSorting2(t) {
    var wingding = ' <span id="225" style="margin: 0; font-family: Wingdings; color: white">&#225;</span>'
    t.find('tr').eq(0).children().each(function () {
        var td = $(this)
        td.append($(wingding)).append($(MyReplace(wingding, '225', '226')))
    })
    t.off('click')
    t.on('click', function (e) {
        var td = $(e.target)
        var id = td.attr('id')
        if (id == 225 || id == 226)
            td = td.parent()
        if (td[0].tagName != 'TH' || td.parent().index() != 1)
            return
        td.siblings().find('span').show().css('color', 'white')
        var span = td.find('span:visible[id=225], span:visible[id=226]')
        var direction = 1

        if (span.length == 2) {
            td.find('#225').hide()
            direction = -1
        } else {
            if (span.attr('id') == '225')
                direction = -1
            else
                direction = 1
            span.hide().siblings().show()
        }
        td.find(':visible').css('color', 'red')
        Sort(t, direction, td)

    })
}
function Sort(t, direction, td0) {
    var index = td0.index()
    var rows = t.find('tr[active]') // td0.parent().nextUntil('[nosort]')
    var rows1 = rows.last().nextAll()

    var type = 'int'
    GetTd(rows, index).each(function () {
        var td = $(this)
        if (isNaN(td.text() ))
            type='string'
    })
    rows.sort(function (a, b) {
        var A = $(a).children('td').eq(index).text().toUpperCase();
        var B = $(b).children('td').eq(index).text().toUpperCase();
        var clone = td0.parent().find('th').eq(index).clone()
        clone.find('span').remove()
        var title = clone.text()
        if (title.indexOf('Date') != -1) {
            A = GetDate(A)
            B = GetDate(B)
        }
        if (type=='int' ) {
            A = parseInt(A)
            B = parseInt(B)
        }
        var src = $('<input type=text  />')

        var value = 0
        if (A < B)
            value = -1;
        if (A > B)
            value = 1;
        return value * direction
    });
    $.each(rows, function (index, row) {
        t.children('tbody').append(row);
    });
    rows1.appendTo(t)
    return 
    var tr = t.find('tr:not([active])') //t.find('tr[nosort]')
    tr.appendTo(t)
}
function GetDate(s) {
    if (s == '')
        return new Date('1/1/00')
    try {
        var arr = s.split('-')

        var d = new Date (2000 + parseInt(arr[2]), MonthNameToNum(arr[1] ) - 1, parseInt( arr[0]) ) 
        if (d == 'Invalid Date')
            return s
        else
            return d
    } catch (e) {
        alert(e.message)
        return s
    }
}
function InitClear(t) {
    t.find('tr:gt(0)').not(':last').find('input, select, span, table').not(':disabled').each(function () {
        var src = $(this)
        if ( !src.is(':visible'))
            return 
        if (src.prop('tagName') == 'SELECT') {
            src.val('')
            return
        }
        src.val('')
        src.text('')
        src.prop('checked', false)
    })
    t.next().hide()
}
function on_Keyup(src) {
    src = $(src)
    if (/\D/g.test(src.val())) {
        // Filter non-digits from input value.
        src.val(src.val().replace(/\D/g, ''))
    }
}
function AddHeader(t, name, c) {
    if (c == null)
        c='header'
    var colSpan = 30 // t.find('tr').first().children().length 
    var td = t.find('th.'+c )
    if (td.find('span:contains(-)').length)
        return
    if (!td.length) {
        var tr = $('<tr><th class='+c+'></tr>').prependTo(t)
        td = tr.children()
        td.html(name).attr('colSpan', colSpan)
    }
    var span = $('<span />').text('-').addClass('minus')
    td.html('<span>' + Trim(td.html()) + '</span>')
    span.prependTo(td)
    return span.closest('th')
}
function InitFold(fn) {
    $('.header, .darkblue').each(function (index) {
        var td = $(this)
        if (td.find('span:contains(-)').length)
            return
        if (td.parents('table[id=H]').length)
            return 
        var span = $('<span />').text('-').addClass('minus')
        td.html('<span>' + Trim(td.text()) + '</span>')
        span.prependTo(td)
    })
    $(document).on('click', 'span', function (index) {
        var span = $(this)
        var text = GetVal(span)
        var th = span.parent()
        var c = th.attr('class')
        if (text != '-' && text != '+') //&& !th.hasClass('header'))  
            return
        var t = span.closest('table')
        var trs = span.closest('tr').nextAll() //  t.find('tr').not(':first')

        var t1 = t.next()
        if (c == 'header') {
            while (t1.length && !t1.find('.' + c).length && t1.prop('tagName') == 'TABLE' )
                t1 = t1.next()
        } else if (c == 'darkblue') {
            while (t1.length && !t1.find('.' + c).length && !t1.find('.header').length && t1.prop('tagName') =='TABLE' )
                t1 = t1.next()
        } else {
                t1 =t
        }
        if ( !t1.is(t))
             trs = trs.add(t.nextUntil(t1))

        if (text == '-') {
            span.text('+')
            trs.hide()
        }
        else {
            span.text('-')
            trs.show()
        }
        if (fn !=null)
            fn(text)
        if ( typeof Page !='undefined' && Page != null && Page.toLowerCase() == 'ei')
            $('#t0').add($('#t1')).hide()
        if (t.attr('id')=='tPacket')
            LoadJobPacket(t, true)
        if (t.attr('id') == 'tProgress0' &&  typeof Page != 'undefined' && Page=='job' )
            InitProgress(true)
    })
    InitFoldDone=true
}
function AddTr(t) {
    return $('<tr/>').appendTo(t)
}
function IncludeFile(file) {
    var xhttp = new XMLHttpRequest();
    var html
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            html = this.responseText;
        }
    }
    xhttp.open("GET", file, false);
    xhttp.send();
    return html
}
function BindSelect(xml, select, index, id, name) {
    select.children().remove()
    if (index == null)
        index = 0
    if (id == null)
        id = 'id'
    if (name == null)
        name = 'Name'
    var table = 'Table' + index
    if (index == 0)
        table = 'Table'
    $($.parseXML(xml)).find(table).find(id).each(function () {
        var element = $(this)
        var value = element.text()
        var text = value
        //if (id != name)
        //   text = element.next().text()
        text=element.parent().find(name).text() 
        $('<option/>').appendTo(select).val(value).text(text)
    })
    AddOptionAll(select)
}
function AddSelect(t, index, arr) {
    var src = t.find('select').eq(index)
    if (t.prop('tagName') == 'SELECT')
        src=t 
    src.find('option').remove()
    for (var i in arr)
        $('<option/>').appendTo(src).val(arr[i]).text(arr[i])
    AddOptionAll(src)
}
function AddSelectYesNo(t, index) {
    AddSelect(t, index, ['Yes', 'No'])
    t.find('select').eq(index).find('option').eq(1).val(1).next().val(0)
}
function AddOptionAll(src) {
    $('<option/>').prependTo(src)
}
function MonthNameToNum(monthname) {
    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    for (var i in months) {
        monthname = monthname.replace(new RegExp(String.fromCharCode(8206), 'g'), '');

        if (months[i].toLowerCase().indexOf(monthname.toLowerCase() ) != -1) {
            var m = i
            m++
            //if (m < 10)
            //    m = '0' + m
            return m
        }
    }
}
function GetValTd(td) {
    var src = td.children()
    if (src.length)
        return GetVal(src)
    else
        return td.text()
}
function GetVal(src) {
    if ( src == null || !src.length)
        return null
    var tagName = src[0].tagName
    if (src.attr('type') && (src.attr('type').toLowerCase() == 'checkbox' || src.attr('type').toLowerCase() == 'radio') )
        value = src.is(':checked') ? '1' : '0'
    else if (['SELECT', 'INPUT'].indexOf(tagName) != -1)
        value = src.val()
    else
        value = src.text()
    var en = src.attr('en')
    if (en != null)
		value = en 
	value = MyReplace(value, '"', "''")
    value = MyReplace(value, "'", "''")
    value = Trim(value)
    return value
}
function LogElement(element) {
    var arr = []
    for (var i in element[0].attributes) {
        var name = element[0].attributes[i].name
        console.log(i + name)
        if (!isNaN(i))
            arr.push("Tbl.Col.value('@" + name + "', 'varchar(max)') as " + name + '\n')
    }

    console.log(arr.join(','))
}
function GetSearchWhere(t, arr) {
    var where = ' and 1=1  '
    t.find('tr:gt(0)').not(':last').find('td:odd').each(function () {
        var td = $(this)
        var src = td.children()
        var id = src.attr('id')
        if (arr.indexOf(id) != -1)
            return
        var value = GetVal(src)

        if (value != '' && value != '0')
            where += " and t." + id + "='" + value + "'"
    })
    return MyReplace(where, "'", "''")
}
function on_SearchBase(sp, debug) {
    SaveScroll()
    var t = tSearch
    var root = $('<r />')
    var element = $.createElement('common')
    t.find('tr').not(':first').not(':last').find('td:even').each(function (index) {
        var td = $(this)
        var id = Trim(td.text())
        id = MyReplace(id, ' ', '')
        if (id == 'Date') {
            var from = GetVal(td.next().find(':text').eq(0))
            var to = GetVal(td.next().find(':text').eq(1))
            if (from != '' || debug)
                element.addAttr('FromDate', from)
            if (to != '' || debug)
                element.addAttr('ToDate', to)
            return
        }
        var value = GetVal(td.next().children())
        if (value != '' || debug)
            element.addAttr(id, value)
    })
    LogElement(element)
    root.append(element)
    var xml = root[0].outerHTML
    xml = GetArray([sp, xml])
    t = tSearchResult
    t.hide()
    if ($(xml).text() == '')
        return
    DataBind(t, xml)
    t.show()
    AddHeader(t, 'Search Results')
}
function on_Fold(src) {
    var span = $(src)
    if (GetVal(span) == '-') {
        span.text('+')
        span.closest('table').find('tr').not(':first').hide()
    }
    else {
        span.text('-')
        var t = span.closest('table')
        t.find('tr').show()
    }
}
function SetVal(src, value) {
    if (!src.length)
        return
    var tagName = src[0].tagName
    if (src.attr('id') == 'Id')
        src.attr('id', value)
    if (src.attr('type') && (src.attr('type').toLowerCase() == 'checkbox' || src.attr('type').toLowerCase() == 'radio' ))
        src.prop('checked', ( IsTrue( value ) || value == 1 ? true : false))
    else if (['SELECT', 'INPUT'].indexOf(tagName) != -1)
        src.val(value)
    else
        src.html(value)
}
function SetEnglishVal(src, value) {
    if (!src.length)
        return
    SetVal(src, value )
    src.attr('en', null)
    TranslateLabel(src)
}
function RemoveWingDing(t) {
    t.find('th').find('[id=225], [id=226]').remove()
}
function WingDing(code, color  ) {
    return ' <span class=center style="font-family:wingdings;color:'+color+' ">&#'+code+';</span>'
}
function on_Help() {

}
function on_Print() {
   window.print()
}
function CleanSpecialCharacter(xml) {
    return MyReplace(xml, "'", "''")
}
function CleanUp(t) {
    $(document).on('click', '.ui-datepicker-current', function () {
        $('.ui-datepicker-close').trigger('click')
    })
    $(':text, textarea, select', t).each(function () {
        var src = $(this)
        var td = src.parent()
        if (td.prop('tagName') == 'TD') {
            td.css('padding', 0)
            src.addClass('textPadding')
        }
        if (src.prop('tagName') == 'TEXTAREA') {
            AutoGrowHeight(src)
        }
    })
    var save = $('#Save')
    if (save.length) {
        var td = save.parent().parent().children().first()
        var html = '<a href="javascript:void(0)" onclick="on_Goback(this);">Go Back</a>'
        td.html(html)
    }
    $('#011311').children().prop('disabled', false)
    var jobId = $('#JobId')
    if (jobId.length && !jobId.find('a').length && typeof Page != 'undefined' && Page != 'swa') {
        var html = '<a href=job.htm?id=' + jobId.text() + '>' + jobId.text() + '</a>'
        jobId.html(html)
    }
    $('#tFooter').nextAll().andSelf().find('a').prop('disabled', false)
    if (Lang != 'EN') {
        if (GetId('Lang').length)
            GetId('Lang').val(Lang)
        InitTranslation() 
    }
    if (window.dialogArguments != null) {
        $('body').css('margin', 20)
    }
    if ( getParameterByName('crawl') == 1)
        InitTranslation(1 )
}
function on_Lang(src) {
    if (src != null) {
        src = $(src)
        Lang = GetVal(src)
        localStorage.Lang = Lang
    }
    Refresh()
}
function InitTranslation(crawl ) {
    var root = $('<r />')
    $('.label, .labelRight, .labelCenter, .Hazard, .minus, :button, .lang,  a').not('.nolang').each(function () {
        var src = $(this)
        //if (!NeedTranslation(src)   )// && !NeedTranslation(src.parent()))
        //    return
        if (src.attr('class') == 'minus')
            src = src.next()
        if (src.prop('tagName') == 'TR') {
            src.children().each(function () {
                var td = $(this)
                TranslateLabel(td, root, crawl)

            })
        } else
            TranslateLabel(src, root, crawl)
    })
    if (crawl == 1) {
        GetArray(['utl_SaveLabel', GetCleanXml(root)])
        console.log('Done: ' + GetPagePath())
        window.parent.Process()
    }
}
function GetPagePath() {
    var url = window.location.toString().toLowerCase()
    url = url.split('?')[0]
    url = url.replace(GetSiteUrl()+'/', '')
    return url 
}
function GetPageName () {
    var arr = GetPagePath().replace('.htm', '') .split('/')
    if (arr.length == 2)
        return arr[1]
    else 
        return arr
}
function TranslateLabel(src, root, crawl) {
    if (crawl == 1) {
        var el = $.createElement('en')
        el.addAttr('Page', GetPagePath())
        el.addAttr('Name', GetEnglishName(src) )
        root.append(el)
        return 
    }
    if (Lang =='EN' || !src.length)
        return
    if (src.parent().attr('id') == '011311')
        return 
    if (src.attr('en') != null)
        return 
    var en = GetEnglishName(src)
    if (!isNaN(en) || en=='EN')
        return 
    var pt = GetTranslation(en)
    if (en == '')
        pt = ''
    var tag = src.prop('tagName')
    if (tag == 'INPUT')
        src.val(pt).attr('en', en)
    else {
        var html = src.html()
        html = html.replace(en, pt)
        src.html(html).attr('en', en)
    }
}
function NeedTranslation(src) {
    if (['TD', 'TH'].indexOf(src.prop('tagName')) == -1)
        return true
    var c = src.attr('class')
    if (c == null)
        c=''
    c=c.toLowerCase()
    if (src.css('background-color') == 'rgb(211, 211, 211)' || ['hazard'].indexOf( c ) !=-1 )
        return true
    else 
        return false 
}
function GetEnglishName(src) {
    var s = GetVal(src)
    if (src.find('span').length) {
        src = src.clone()
            src.find('span').remove()
        s=GetVal(src)
    }
    //if (src.prop('tagName') == 'INPUT')
    //    s = src.val()
    return s
}
function GetTranslation(en) {
    //return 'P' + en
    if (XMLTranslation == null) {
        XMLTranslation = GetArray(['usp_getTranslationLabel', GetPagePath(), Lang  ])
        XMLTranslation = '<a>' + $(XMLTranslation).text() + '</a>'
    }
    var src = $(XMLTranslation).find('[Name="' + en + '"]').first()
    if (src.length)
        return src.attr('pt')
    return en
}
function on_JobLink(src) {
    src = $(src)
    var xml = GetArray(['usp_JobGetLink'])
    var r = showModal('../common/popup.htm', xml, 800, 800)
    if (r == null)
        return
    var span = src.next()
    span.text(r[0])
    span.next().text(r[1])
}
function NumberOnly(arr) {
    if (arr.jquery) {
        arr.each(function () {
            var src = $(this)
            src.on('keyup change', function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
              //  this.value = this.value.replace(/[^0-9]/g, '');
            });
        })
    } else {
        for (var i in arr) {
            var src = arr[i]
            if (!src.jquery)
                src = $('#' + arr[i])
            NumberOnly(src)
        }
    }
}
function InitTime(arr) {
    if (!$.isArray(arr)) {
        arr.each(function () {
            var src = $(this)
            InitCommonTime(src)
        })
        return
    }
    for (var i in arr) {
        InitCommonTime($('#' + arr[i]))
    }
}
function InitCommonTime(src, dateOnly) {
    var w = '150'
    var obj = {
        timeFormat: "H:i"
    }
    src.timepicker(obj).width(w)
}
function InitCommonDate(src, dateOnly, fn) {
    var url = "../images/calendar.gif"
    var w = '150'
    var obj = {
        dateFormat: 'd-M-y',
        showOn: "button",
        buttonImage: url,        
        buttonImageOnly: true,
        onClose: fn,
        closeOnNowClick: true, 

        beforeShow: function (input) {
            setTimeout(function () {
                var buttonPane = $(input).datepicker("widget").find(".ui-datepicker-buttonpane");
                buttonPane.find('button').eq(0).css('font-weight', 'bold').css('color', 'black')
            }, 1);
        }
    }
    //src.prop('disabled', true )
    src.on('keypress', function (e) {
        e.stopPropagation();
        return false
    })
    if ( dateOnly)
        src.width(w).datepicker(obj)
    else
        src.width(w).datetimepicker(obj)

}
function InitDate(arr, fn) {
    if (!$.isArray(arr)) {
        arr.each(function () {
            var src = $(this)
            InitCommonDate(src, true, fn)
        })
        return
    }
    for (var i in arr) {
        InitDate($('#' + arr[i]), fn)
    }
}
function InitDateTime(arr, fn) {
    if (!$.isArray(arr)) {
        arr.each(function () {
            var src =$(this)
            InitCommonDate(src, false, fn )
        })
        return
    }
    for (var i in arr) {
        InitDateTime ( $('#' + arr[i]) , fn ) 
    }
}
function DestroyDateTime(arr, fn) {
    if (!$.isArray(arr)) {
        arr.each(function () {
            var src = $(this)
            src.datepicker("destroy");
        })
        return
    }
    for (var i in arr) {
        DestroyDateTime($('#' + arr[i]), fn)
    }
}
function InsertCommon() {
    if (Id <1)
        return
    var html = IncludeFile('../common/common.htm')
    $(html).appendTo($('body'))
    $('#Save').closest('table').find('td').css('border', 'none')
    InitFooter()
}
function InsertOapSearch() {
    var html = IncludeFile('../oap/search.htm')
    $(html).appendTo($('body'))    
    InitOapSearch()
}
function InsertCommonEquipment(t, fn ) {
    var html = IncludeFile('../common/Equipment.htm')
    $(html).insertAfter(t)
    t.next().addClass('marginBottom')
    InitEquipment(fn)
    return
    FormatEquipment()
}
function FormatEquipment() {
    var t = $('#tEquipment')
    if (!t.find('tr').length)
        return
    //$('<tr><td colspan=22 ><span class=minus>-</span> Equipment</tr>').prependTo(t)
   //t.find('tr').eq(0).children().css('background-color', 'rgb(61, 183, 228)').css('color', 'white')
    t.find('tr').first().find('td').addClass('noborder')
    t.find('tr').eq(1).children().addClass('darklabel')
    t.prev().find('tr').find('td').css('border-bottom', 'none')
}
function IsEmpty(src) {
    var id = src.attr('id')
    if (!src.length )
        return
    //if (id == 'MiddleName')
    //    return false
    var value
    var tagName = src[0].tagName
    if (tagName == 'INPUT') {
        var type = src.attr('type')
        if (type == 'radio') {
            var name = src.attr('name')
            var radios = $('[name=' + name + ']:checked')
            if (radios.length)
                return false 
            else {
                Highlight(src.parent(), 'yellow')
                return true  
            }
        }
        value = src.val()
    }
    else if (tagName == 'SELECT')
        value = src.prop('selectedIndex') < 1 ? '' : 'dummy'
    else
        value = src.text()
    value = Trim(value)
    if (value == '') {
        if (['SPAN', 'TABLE'].indexOf(tagName) != -1)
            src = src.parent()
        Highlight(src, 'yellow')
        return true
    }
    Highlight(src, 'white')
    return false
}
function Highlight(src, color) {
    src.css('background-color', color)
}
function AddGridHeader(t, s, subheader) {
    var type = 'header'
    if (subheader)
        type = subheader
    var html = '<tr><th class=' + type + ' style="text-align: left;" colspan="20"><span class=minus>-</span><span>' + s + '</span></tr>'
    var th = $(html).prependTo(t).find('th')
    TranslateLabel(th.find('span').eq(1), null)
    return th 
}
function AddGridSubHeader(t, s) {
    AddGridHeader(t, s, 'darkblue')
}
function GetTd(t, i) {
    var trs = t
    if (trs.length == 1 && trs.prop('tagName')=='TABLE')
        trs = trs.children().children()
   return   trs.children('td').filter(function () {
        return $(this).index() == i 
   })
}
function GetDelete() {
    return '<img src="../Images/delete.png" width="16" onclick="on_Delete(this)" />'
}
function on_Reset() {
    GetArray(['usp_JobReset', Page, Id])
    Refresh()
}
function InitPaperView(t0) {
    var xml = GetArray(['usp_JobGetPaper', Page, Id])
    var element = $($.parseXML(xml)).find('Electronic')
    var text

    if (element.length == 1)
        text = element.text()
    else {
        var jobId = $('#JobId').text()
        text = $($.parseXML(xml)).find('JobId').filter(function () {
            return  Trim($(this).text()) == Trim(jobId ) 
        }).prev().text()
    }
    if (Id > 0 && !IsTrue( text ) ) {
        IsPaperView = true
        var html = '<table class=marginBottom id=tAttachment><tr><th class=darkblue><span class=minus>-</span><span>Attachments</span><tr><td ><a onclick="$(this).next().trigger(\'click\')" href="javascript:void(0)">Add Attachment</a>  <input id="Document" style="display: none;" type="file"></tr></table>'
        var t = $(html).insertAfter(t0)
        t.nextAll().hide()
        var xml = GetArray(['usp_JobGetAttachment', Page, Id])
        $(xml).find('guid').each(function () {
            var guid = $(this)
            InsertAttachment($(':file').closest('td'), guid.text(), guid.next().text())
        })
        $('#tEquipment').show()
    }
}
function SaveXmlAttachment() {
    if (!IsPaperView)
        return
    var root = $('<r />')
    root.addAttr('id', Id)
    root.addAttr('Page', Page)
    $('#tAttachment').find('a:gt(0)').each(function () {
        var a = $(this)
        var Document = $.createElement('Document')
        var guid = a.attr('href').replace('upload/', '')
        Document.addAttr('guid', guid)
        Document.addAttr('Name', a.text())
        root.append(Document)
    })
    var xml = root[0].outerHTML
    GetArray(['usp_JobSaveAttachment', xml])
}
function on_HistoryLog() {
     showModal('../common/HistoryLog.htm?page='+Page+'&id='+Id, '', screen.width-100, screen.height-100)
}
function on_Outlook() {
    var xml = GetArray(['usp_AdminGetCustomOption'])
    var rig = $(xml).find('Table1').find('Name:contains(RigName)').next().text()
    var text = Trim( $('#Crumb').text()) 
    var arr = text.split('>')

    var subject = rig + (arr.length > 1 ? arr[arr.length - 1] : '') + ': ' + Id
    if (Page == 'Capa')
        subject= rig + ' '+GetId('Source').text() 
    var body = encodeURIComponent(window.location) + '\n'
    var s = ' <a href="mailto:?to=&body=' + body + '&subject=' + subject + '">Link text goes here</a>'
    $(s)[0].click()
}
function InitCapaGrid(page) {
    var t = tCapa
    var xml = GetArray(['usp_CapaGrid', page])
    DataBind(t, xml)
    AddGridHeader(t, 'Open CAPA')
}
function AddCommonPaging(fn, t1, sp, total, index, Callback, type, exportToExcel) {
    var fn_name = fn.toString();
    fn_name = fn_name.substr('function '.length);
    fn_name = fn_name.substr(0, fn_name.indexOf('('));

    var s = ''
    var excel = '<img style="margin-left:10" width=20  align="middle" src=../images/excel.png  />'
    if (fn_name == 'InitGrid' && !exportToExcel)// && window.Page !== undefined && Page != 'dashboard')
        excel = ''
    for (var i = 1; i <= total; i++) {
        if (i == index)
            s += '<span style="padding-left:7px" >' + i + '</span>'
        else
            s += GetPagingLink(i)
        if (i % 30 ==0)
            s +=' <br>'
    }
    if (t1.find('tr').length == 0) 
        s = ''
    else if (total == 1)
        s = excel  
    else
        s += GetPagingLink('View All') + excel 
    var tr = $('<tr nosort=1 ><td colspan=22></tr>').appendTo(t1)
    tr.children().html(s)
    if (s == '')
        tr.remove()
    tr.find('a').each(function () {
        var a = $(this)
        a.on('click', function () {
            var index1 =GetVal(a)
            if (index1 == 'View All')
                index1 = -1
            if (isNaN(index1))
                return
            fn(a, sp, index1, Callback, type)
        })
    })
    tr.find('img').on('click', function () {
        fn( $(this), sp, -2, Callback, type)
    })
}

function GetPagingLink(text) {
    return '<a style="padding-left:7px" href="javascript:void(0)" >' + text + '</a>'
}
function on_CommonSearch(src, sp, index, Callback, type) {
    src = $(src)
    var text = src.text()
    var t, t1
    if (src[0].tagName == 'A' || index == -2 ) {
        t1 = src.closest('table')
        t = t1.prev()
    } else {
        t = src.closest('table')
        t1 = t.nextAll ('table').first()
    }

    var arr = [sp, index]
    if (index == -2 && sp=='usp_PobGetAll')
        arr.push(0)
    if (type != null)
        arr.push(type)

    t.find('tr:gt(0)').not(':last').find('td:odd').each(function () {
        var td = $(this)
        var location = td.find('#Location')
        if (location.length) {
            var value = location.text()
            arr.push(value)
        } else {
            if (td.find(':text, select').length) {
                td.find(':text, select').each(function () {
                    var src = $(this)
                    var value = GetVal(src)
                    arr.push(value)
                })
            } else {
                arr.push(GetVal( td.children())) 
            }
        }
    })
    if (sp.toLowerCase() == 'usp_PobSearch'.toLowerCase()) 
            arr.push(0 )
    if (sp.toLowerCase() == 'usp_ComSearchUser'.toLowerCase()) {
        if (getParameterByName('NonEnsco') == 1)
            arr.push(-1)
        else if (getParameterByName('all') == '')
            arr.push(0)
        else
            arr.push(1)
    } else {
        arr.push(Lang )
    }
   // alert(arr )
    var xml = GetSearch(arr)
    if (xml == null)
        return
    t1.show()
    DataBind(t1, xml)

    var total = $(xml).find('Table1').text()
    //if (total != 0)
    AddCommonPaging(on_CommonSearch, t1, sp, total, index, Callback, type)
    Callback(t1)
}
function GetPobUser(type, employeeId, all) {
    if (type == null  )
        type = ''
    var url
    if (type =='NonEnsco')
        url = '../common/popupUser.htm?a=11&NonEnsco=1' 
    else
        url = '../common/popupUser.htm?a=11&role=' + type
    if (employeeId != null)
        url += '&employeeId=' + employeeId
    if (all ==1)
        url += '&all=1'
    var r = showModal(url, type, 1300, 900)
    if (r == null)
        return null 
    r = JSON.parse(r)
    if ( r.length == 0)
        r = null

    return r
}
function on_PDF(src) {
    SetTabIndex()
    window.location = '../common/pdf.aspx?page=' + Page + '&id=' + PageId+'&lang='+Lang
}
function on_Location(src) {
    src = $(src)
    var r = Popup('../common/PopupLocation.htm?a=1', 900, 900)

    if (r == null)
        return
    AddLocation(src, r[0], r[1])
}
function AddLocation(src, id, name) {
    var span = src.next()
    span.text(name).next().text(id)  // .parent().attr('LocationId', id)
    span.find('td').css('border', 'none')
}
function GetPageInfo() {
    var id = getParameterByName('id')
    var url = window.location.toString().toLowerCase()
    var arr = url.split('/')
    var page = arr.slice(-1)
    root = arr.slice(-2, -1).toString()

    page = page.toString().split('?').slice(0, 1).toString().split('.').slice(0, 1).toString()
    if (root == 'checklists') {
        root = 'SWA'
        id = page
        page = 'checklists'
    }
    return [id, page, root]
}
function on_Goback(src) {
    var page = GetPageInfo()[1]
    if (window.dialogArguments != null) {
        window.close()
        return 
    }
    if (page  == null )
        history.go(-1) 
    switch (page.toLowerCase() ) {
        case 'pob':
            window.location='landingPob.htm'
            break
        case 'job':
            window.location = 'home.htm'
            break
        default:
            history.go(-1) 
    }
}
function ToggleDisable(src, disable) {
    src.prop('disabled',  disable)
}
function GetADUserName(userId) {
    var name
    var data = { name: userId, includeGroup: false }
    $.ajax({
        type: "POST",
        async: false,
        url: "../common/EmpDirectory.aspx/FindEmployee",
        data: JSON.stringify( data ),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {            
            var tr = $(data.d).find('tr').eq(1)
            name =tr.find('td').eq(2).text()
        },
        failure: function (response) {
            alert(response.d);
        }
    });
    return name 
}
function GetADUser( ) {
    var r = Popup('../common/empDirectory.aspx', screen.width - 200, 500)
    if (r == null)
        return null

    r = JSON.parse(r) 
    if (r.length == 0)
        r = null
    return r
}
function IsTrue(s) {
    s=Trim(s).toLowerCase()
    if ( s == 'true' || s=='1')
        return true
    else 
        return false 
}
function LoadJobPacket(t0, expand) {
    var t = $('<table id= tPacket  class=marginBottom />')
    if (t0.attr('Done') == 1 || Id<1)
        return
    if ( !expand){
        var tr = $('<tr/>').prependTo(t)
        AddTd(tr, ' <span class="minus">+</span> <span>Job Packet</span> ', 'th', 11).addClass('darkblue')
        t.insertAfter(t0)
        if (true || Page == 'job') {
            t.find('.minus').trigger('click')
           // t.find('.minus').trigger('click')
        }
        return
    }
    t = t0
    t0.attr('Done', 1 )
    GetArray(['usp_JobLoadPacket', Id, 0, Page, Lang], function (data) {
        var xml = data.xml
        DataBind(t, xml)
       // t.find('tr').hide()
        var tr = $('<tr/>').prependTo(t)
        AddTd(tr, ' <span class="minus">-</span> <span>Job Packet</span> ', 'th', 11).addClass('darkblue')
        TdByName(t, 'Status1', true ).hide()
        t.find('tr:gt(1)').find('td:nth-child(1)').each(function () {
            var td = $(this)
            td.text(td.parent().index() - 1)
            //if (GetVal(TdByName(td.parent(), 'Status')) == 'Closed' && td.parent().html().toUpperCase().indexOf('WI-') == -1)
            if (GetVal(TdByName(td.parent(), 'Status1')) != '')
                return
            var img = $(GetDelete()).appendTo(td)
            img.attr('onclick', null)
            img.on('click', function () {
                var a = $(this).parent().next().find('a')
                var id, page
                if (a.length && GetVal(a) != '') {
                    id = a.text()
                    page = a.attr('href')
                    page = page.split('.')[0]
                } else {
                    page = 'verbal'
                    if (Page == 'job')
                        id = Id
                    else
                        id = GetId('JobId').text()
                }
                //var wiNo = a.parent().next().text()
                //if (wiNo.indexOf('WI-') != -1) {
                //    var xml = GetArray(['usp_JobCheckAssociated', wiNo])
                //    var arr = []
                //    GetXmlTable(xml, 1).find('wiNo').each(function () {
                //        var w = $(this).text()
                //        var td = t.find('td:contains("' + w + '")')
                //        if (td.length) {
                //            arr.push(td.prev().find('a').text())
                //        }
                //    })
                //    if (arr.length) {
                //        if (confirm(arr + ' are associated work instructions. Do you want to delete them all together?')) {
                //            arr.push(id)
                //            for (var i in arr) {
                //                var xml1 = GetArray(['usp_JobDeletePacket', page, arr[i], UserId])
                //                if (GetXmlTable(xml1).text() == 1)
                //                    alert('Cannot delete the saved form.')
                //            }
                //            Refresh()
                //        }
                //        return
                //    }
                //}
                if (!confirm('Are you sure to delete this from Job Packet?'))
                    return
                var xml = GetArray(['usp_JobDeletePacket', page, id, UserId])
                //if ($(xml).text() == 1) {
                //    //alert('Cannot be deleted because it has been authorized. ')
                //    alert('Cannot delete the saved form.')
                //    return
                //}
                a.closest('tr').remove()
            })
        })
        //var xml = GetArray(['usp_AdminGetAdminUser', UserId])
        //if ($(xml).text() == '')
        //    GetTd(t, 0).find('img').hide()
        t.find('tr:gt(1)').find('td:nth-child(2)').each(function () {
            var td = $(this)
            var id = td.text()
            var name = td.next().text()
            var url = name + '.htm?id=' + id
            td.html('<a/>').find('a').html(id).attr('href', url)
            
            if (GetVal(td.next()).indexOf('Wims') !=-1 ) {
                var td1 = td.next().next()
                var wiNo = td1.text()
                td1.html('<a/>').find('a').html(wiNo).attr('href', 'javascript:void(0)').attr('id', id).on('click', function () {
                    var a = $(this)
                    var tr = a.closest('tr')

                    var wiNo = a.text().split(' ')[0]
                    var lang = wiNo.substring(wiNo.length - 1, wiNo.length).toUpperCase()
                    if (['A', 'P'].indexOf(lang) == -1)
                        lang = 'E'
                    var wiType = wiNo.split('-')[0]
                    var xml = GetArray(['usp_JobGetWiIdFromWiNo', wiNo])
                    var id = Trim(GetXmlTable(xml).text())
                    var url = WimsUrl + 'create.aspx?id=' + id + '&WiType=' + wiType
                    url += '&action=preview&lang=' + lang
                    window.location = url
                    return false
                })
            }
            td.next().remove()
        })
        t.find('th:nth-child(2)').remove()
        GetTd(t, 4).each(function () {
            var td = $(this)
            var text = td.text()
            var html = '<input type=radio name=Packet' + td.parent().index() + '  onclick=on_TogglePacket(this,event) />'
            td.html(html).next().html(html)
            if (IsTrue(text))
                td.children().prop('checked', true)
            else
                td.next().children().prop('checked', true)
        })

        var html = '<a href=# onclick="on_AddPacket(this); return false">+ Add </a>'
        if (Trim(GetXmlTable(xml, 1).text()) != 'Closed')
            $('<tr><td colspan=11 >' + html + '</tr>').appendTo(t).attr('nosort', 1)
        else
            t.find('input').prop('disabled', true )
        TdByName(t, 'PermitId', true).remove()

        //TdByName(t, 'Status').each(function () {
        //    var td = $(this)
        //    if (GetVal(td) == '')
        //        td.text('Open')
        //    else
        //        td.parent().find('img').first().hide()
        //})
        TdByName(t, 'ID').find('a').on('click', function () {
            var a = $(this)
            if ( a.attr('href').indexOf('Wims' )  !=-1) 
                return 
            if (TdByName(a.closest('tr'), 'Electronic').find(':checked').length)
                return true 
            var url =a.attr('href')
            var page=url.split('.htm')[0]
            var id =url.split('id=')[1]

            window.location = '../common/pdf.aspx?page=' + page + '&id=' + id
            return false
        })
    })

}
function on_TogglePacket(src, event) {
    var tr = $(src).closest('tr')
    if (confirm('Are you sure you want to change the type of this Permit/Certificate?')) {
        var tr = $(src).closest('tr')
        var url = tr.find('a').attr('href')

        var arr = url.split('/')
        var page = arr[arr.length - 1]
        var id = page.split('=')[1]
        var page = page.split('.')[0]
        if (id == '')
            return
        if (page == 'javascript:void(0)') {
            page = 'WI'
            id = tr.find('a').attr('id')
        }

        if (page == 'Permit') {
            var hasOpenEI = false;
            // Prevents user from changing permit to paper if there is any open energy isolation                
            $.each(tr.closest('table').find('td:contains(Energy Isolation)'), function (index, elem) { 
                if ($(elem).next().text() == "Open") {
                    alert("There is an open energy isolation, please close it out first!");
                    hasOpenEI = true;
                    return false;
                }
            });

            if (hasOpenEI)
                return;
        }

        GetArray(['usp_JobPacketToggle', page, id])// tr.find(':radio').eq(0).prop('checked')])
        Refresh()
    }
    else { // Check the previous radio button again
        $radios = tr.find("input[type='radio']");
        $.each($radios, function (index, element) {
            if (!$(element).is($(src))) {
                $(element).prop('checked', true);
                return false;
            }
        });
    }
    
}
function ExpandPacket(t) {
    if ( !InitFoldDone) {
        setTimeout(function () {
            ExpandPacket(t );
        }, 100)
        return
    }
    
   // alert(t.html());return 
    t.find('.minus').trigger('click')
}
function on_CloneEI(src) {
    src=$(src )
    var r = showModal('../job/popupEI.htm', src.closest('table')[0].outerHTML, 800, 800)
    if (r == null)
        return
    Refresh()
}
function on_AddPacket() {
    var r = showModal('../job/popupJobPacket.htm', '', screen.width - 10, 800)
    if (r == null)
        return
    var jobId =Trim( $('#JobId').text()) 
    var t=$('#tPacket')
    if (jobId == '') {
        var arr = $(r).find('general, ei, wi').map(function () {
            var element = $(this)
            return element.attr('Name') 
        }).get()
        $('<tr><td >' + arr.join('<br>') + '</tr>').insertBefore(t.find('tr').last()).attr('xml', r )
        return 
    }
    GetArray(['usp_JobAddPacket', jobId,  r])
    Refresh()
}
function NoRow(t) {
    return !t.find('tr').length 
}
function HideColumn(t, index) {
    GetTd(t, index ).hide()
    t.find('tr').eq(0).children().eq(index ).hide()
}
function GetId(id, src) {
    return $('#'+id , src )
}
function GetXmlTable(xml, index) {
    if (index == null || index ==0 ) {
        xml = MyReplace(xml, 'Table>', 'Table0>')
        index =0
    }        
    return $(xml).find('Table'+ index )
}
String.prototype.capitalize = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
}
function CommonValidate(arr, src ,  notShow) {
    var ok = true
    for (var i in arr)
        if (IsEmpty(GetId(arr[i])))
            ok = false
    if (src != null )
        src.each(function () {
            if (IsEmpty($(this) ))
                ok = false
        })
    if (!ok && ! notShow)
        alert('Please fill out the required fields.')
    return ok
}
function LockPage() {
    if ( typeof Id != 'undefined' && Id == 0)
        return
    $('#Save').hide()
    GetId('Suspend').hide()
    LockAddPacket()

    if (typeof tPlanning == 'undefined' )
        return
    var src = tPlanning.nextAll().andSelf()
    src.find(':text, :radio, :checkbox, select, textarea').prop('disabled', true)
    src.find(' img').hide()
}
function LockEquipment() {
    var t = GetId('tEquipment')
    if (t.length) {
        t.find('img').hide()
    } else
        setTimeout(LockEquipment, 1000)
}
function LockAddPacket() {
    var a = GetId('tPacket').find('a').last()
    if (a.length) {
        a.closest('table').find('img').hide()
      //  a.hide()
    }else
        setTimeout(LockAddPacket, 1000)
}
jQuery.fn.reverse = [].reverse;
function GetCleanXml(root) {
    var xml =  root[0].outerHTML
    return xml.replace('<?XML:NAMESPACE PREFIX = "PUBLIC" NS = "URN:COMPONENT" />', '')//.replace("'", "''")
}
function TdByName(trs, name, includeHeader) {
    var th = trs.closest('table').find('th').filter(function (index) {
        return GetVal($(this)).replace('áâ', '').replace(' / ', '/') == name
    })
    var index = th.index()
    var src = GetTd(trs, index).filter(function () {
        return $(this).attr('colspan')==1 
    })
    if (includeHeader)
        src = src.add(th)
    return src
}
function TdFilter(src, name) {
    return  src.find('td').filter(function () {
        return GetVal($(this))== name 
    })
}
function TdContains(src, i, name) {
    return GetTd(src, i).filter(function () {
        return GetVal($(this)).indexOf(name) != -1
    })
}
function Strip(t0) {
    //return '<br/>' + CleanSpecialCharacter(t0[0].outerHTML)
    var t = t0
    if (!t.length)
        return ''
    t.removeClass()
    t.find('.minus').remove()
    RemoveWingDing(t)
   // t.find(':checkbox').remove()
    var src = t.find('*').filter(function () {
        return $(this).css("display") == "none"
    })
    src.remove()
    t.find("tr:not(:has(*))").remove()
    t.find('img').remove()
    t.find(':text').each(function () {
        var src = $(this)
        src.parent().html(src.val())
    })
    t.find('a').each(function () {
        var src1 = $(this)
        var url = src1.attr('href')
        url = GetSiteUrl() + '/Job/' + url
        src1.attr('href', url)
        var text = GetVal(src1)
        if (['Authorize', 'Reject'].indexOf(text) != -1) {
           // src1.removeAttr('href')
            //src1.remove() // .parent().html(text )
        }
    })
    t.find(':radio, :checkbox').each(function () {
        var src = $(this)
        var html = ''
        if (src.prop('checked'))
            html = WingDing(252, '')
        if (src.siblings().length)
            src.remove()
        else 
            src.parent().html(html).width(50)
    })
    t.find('select').each(function () {
        var src = $(this)        
        src.parent().html(src.find(':selected').text())
    })
    
    return '<br/>' + CleanSpecialCharacter(t[0].outerHTML)
}
function LockField(id) {
    var src = GetId(id)
    var value =GetVal(src )
    if (value == '')
        return
    var text =value 
    var page = 'Permit'
    if (id == 'JobId')
        page = 'Job'
    if (id == 'Wi') {
        page = 'Wims'
        text = src.find(':selected').text()
    }
    var html = '<a id=' + id + ' href=' + page + '.htm?id=' + value + ' title="'+text+'">' + value + '</a>'
    src.parent().html(html )
}
function AutoGrowHeight(ta) {
    ta.on('input', function () {
        var scroll_height = Math.max( ta.get(0).scrollHeight, 100) 
        ta.css('height', scroll_height + 'px');
    }).trigger('input')
}