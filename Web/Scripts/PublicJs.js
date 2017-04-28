//AJAX请求 加入layer载入
jQuery.Majax = function (Url, Data, success) {
    var loginload = layer.load();
    $.ajax({
        url: Url,//jsoncode=1
        type: "post",
        dataType: 'json',
        data: Data,
        success: function (result) {
            layer.close(loginload);
            success(result);
        }
        //timeout: 3000
    });
};
jQuery.MajaxBE = function (Url, Data, success, error) {
    var loginload = layer.load();
    $.ajax({
        url: Url,
        type: "post",
        dataType: 'json',
        data: Data,
        success: function (result) {
            layer.close(loginload);
            success(result);
        },
        error: function (e) {
            layer.close(loginload);
            error(e);
        }
        //timeout: 3000
    });
};
//打开弹出框
jQuery.OpenEdit = function (url, witdh, height, title) {
    layer.open({
        type: 2,
        area: [height + 'px', witdh + 'px'],
        fix: false, //不固定
        maxmin: true,
        content: url,
        title: title
    });
}
//在父级弹出框
jQuery.OpenParentEdit = function (url, witdh, height, title) {
    parent.layer.open({
        type: 2,
        area: [height + 'px', witdh + 'px'],
        fix: false, //不固定
        maxmin: true,
        content: url,
        title: title
    });
}

// 循环表单，并生成JSON
function GetFormSerialize(FormName) {
    var json = {}
    $('#' + FormName).find("input").each(function () {
        var Intype = $(this).attr("type");
        var Issubmit = $(this).attr("IsNosubmit");
        if (Issubmit != "1") {
            switch (Intype) {
                case "checkbox":
                    var checkboxindate = "";
                    $('[name="' + $(this).attr("name") + '"]').each(function () {
                        if ($(this).is(":checked")) {
                            checkboxindate += $(this).val() + ",";
                        }
                    });
                    if ($.trim(checkboxindate) != "") {
                        checkboxindate = checkboxindate.substring(0, checkboxindate.length - 1);
                    }
                    json[$(this).attr("name")] = checkboxindate;
                    break;
                case "radio":
                    if ($(this).is(":checked")) {
                        json[$(this).attr("name")] = $(this).val();
                    }
                    break;
                default:
                    json[$(this).attr("name")] = $(this).val();
                    break;
            }
        }
    });
    $('#' + FormName).find("select").each(function () {
        json[$(this).attr("id")] = $(this).val();
    });
    $('#' + FormName).find("textarea").each(function () {
        json[$(this).attr("id")] = $(this).val();
    });
    return json;
}

//将返回的JSON值 返回到对应控件中去
function InnerData(jsondata) {
    for (var key in jsondata) {
        var data = jsondata[key];
        //根据KEY 尝试获取页面控件
        var pageinput = $("[name='" + key + "']");
        if (pageinput.size() != 0) {
            //获取对应控件类型
            var type = pageinput.eq(0).attr("type");
            switch (type) {
                case "checkbox":
                    if (data != "") {
                        //开始赋值
                        pageinput.each(function () {
                            var Tval = $(this).val();
                            var eachdata = data.split(',');
                            for (var i in eachdata) {
                                if (eachdata[i] == Tval) {
                                    $(this).prop("checked", true);
                                    $(this).parent().attr("class", "icheckbox_minimal checked");
                                }
                            }
                        });
                    }
                    break;
                case "radio":
                    pageinput.each(function () {
                        var Tval = $(this).val();
                        //console.log(Tval + "" + data);
                        if (Tval == data) {
                            $(this).prop("checked", true);
                            $(this).parent().attr("class", "iradio_minimal checked");
                        }
                    });
                    break;
                case "password": //如果是密码类型，则不进行任何赋值操作
                    pageinput.val("");
                    break;
                default:
                    pageinput.val(data);
                    break;
            }
        }
    }
}

function ReAuditStatus(status) {
    switch (status) {
        case 0:
            return "待审核";
        case 1:
            return "审核完成";
    }
}

//返回对应文件审核状态
function ReFileStatus(status) {
    switch (status) {
        case 0:
            return "待审核";
        case 1:
            return "审核中";
        case 3:
            return "审核完成";
        case 4:
            return "<span style=\"color:red;\" name=\"BackBtn\">退回</span>";
        case 5:
            return "培训中";
        case 6:
            return "已生效";
    }
}
//返回对应文件审核状态
function ReFileStatusE(status) {
    switch (status) {
        case 0:
            return "Pending audit";
        case 1:
            return "Audit";
        case 2:
            return "Waiting for approval";
        case 3:
            return "Approval completed";
        case 4:
            return "<span style=\"color:red;\" name=\"BackBtn\">return</span>";
        case 5:
            return "Deleted";
        case 6:
            return "Approval";
    }
}
//返回对应文件审核状态
function ReFileStatusByMyList(status, OnlyStr) {
    switch (status) {
        case 0:
            return "待审核";
        case 1:
            return "审核中";
        case 2:
            return "等待审批";
        case 3:
            return "审批完成";
        case 4:
            return "<span style=\"color:red; cursor: pointer;\" name=\"BackBtn\" clt=\"" + OnlyStr + "\">退回</span>";
        case 5:
            return "已删除";
        case 6:
            return "审批中";
    }
}
//返回对应文件审核状态
function ReFileStatusByMyListE(status, OnlyStr) {
    switch (status) {
        case 0:
            return "Pending audit";
        case 1:
            return "Audit";
        case 2:
            return "Waiting for approval";
        case 3:
            return "Approval completed";
        case 4:
            return "<span style=\"color:red; cursor: pointer;\" name=\"BackBtn\" clt=\"" + OnlyStr + "\">return</span>";
        case 5:
            return "Deleted";
        case 6:
            return "Approval";
    }
}
//返回对应文件退回状态
function ReFileIsback(Isback) {
    switch (Isback) {
        case 0:
            return "<span style=\"color:green;\">×</span>";
        case 1:
            return "<span style=\"color:red;\">√</span>";
    }
}
//返回对应文件退回状态
function ReFileE_learningType(E_learningType) {
    switch (E_learningType) {
        case 0:
            return "培训";
        case 1:
            return "考试";
    }
}
var item = ["roleadd", "roleedit", "roledel", "roleser", "roleimport", "roleexport"];
//返回当前用户权限类表
function GetRole(pageid, RoleStr, fun) {
    if (parseInt(pageid) > 0) {
        $.Majax("/Admin/Ajax/GetButtonRole", { "pageid": pageid }, function (data) {
            if (data == null) {
                ClearPage();
            }
            else {
                //开始执行对比 
                $(item).each(function (index, val) {
                    var Flag = false; //默认按钮应该被消除
                    for (var i in data) {
                        if (data[i].ButtonName == val) {
                            //表示当前数据内存在对应信息,则返回一个ture
                            Flag = true;
                            break;
                        }
                    }
                    if (!Flag) {
                        //在页面上删除该按钮
                        $('[name="' + val + '"]').remove();
                    }
                    else {
                        $('[name="' + val + '"]').show();
                    }
                });
                //如果当前页面存在额外按钮需要进行控制时候，
                if (RoleStr) {
                    //开始执行额外按钮比对 
                    $(RoleStr).each(function (index, val) {
                        var Flag = false; //默认按钮应该被消除
                        for (var i in data) {
                            if (data[i].ButtonName == val) {
                                //表示当前数据内存在对应信息,则返回一个ture
                                Flag = true;
                                break;
                            }
                        }
                        if (!Flag) {
                            //在页面上删除该按钮
                            $('[name="' + val + '"]').remove();
                        }
                        else {
                            $('[name="' + val + '"]').show();
                        }
                    });
                }
            }
        });
    }
    else {
        ClearPage();
    }
}
function ClearPage() {
    //将页面所有基础按钮放空
    $(item).each(function (index, val) {
        $('[name="' + val + '"]').remove();
    });
}


///设置LocalStorage
function SetLocalStorage(Key, data) {
    localStorage.setItem(Key, data);
}
//获取指定储存项目，如果不存在则创建指定数据
function GetLocalStorage(Key, data) {
    if (!localStorage.getItem(Key)) {
        if (data) {
            localStorage.setItem(Key, data);
        }
        else {
            localStorage.setItem(Key, "[]");
        }
    }
    return localStorage.getItem(Key);
}
//删除指定   localStorage
function DelLocalStorage(Key) {
    localStorage.removeItem(Key);
}

//传入数据模型，然后储存到localStorage内
function SaveGood(SID, Numtxt) {
    var data = {};
    data.Count = Numtxt.val();
    data.Gid = Numtxt.attr("Gid");
    data.SiD = Numtxt.attr("SiD");
    data.Price = Numtxt.attr("Price");
    //根据店铺ID获取对应店铺的数据信息
    var JsonData = GetLocalStorage("Shop_" + SID);
    if (JsonData == "[]") {
        //当前表示本地未存在任何数据，直接开数组入数据
        var GoodList = [];
        GoodList.push(data);
        SetLocalStorage("Shop_" + SID, JSON.stringify(GoodList));
    }
    else {
        //表示本地存在数据，检测当前添加数据是否符合条件
        try {
            var locatlData = eval("(" + JsonData + ")");
            var isadd = true;//用于标识是否可以直接添加的变量
            for (var i in locatlData) {
                if (locatlData[i].Gid == data.Gid) {
                    if (data.Count > 0) {
                        //当前已有选中商品，于是直接替换
                        locatlData[i] = data;
                        isadd = false;//表示已经处理过了
                    }
                    else if (data.Count == 0) {
                        //表示当前都去掉了
                        locatlData.splice(i, 1);
                        isadd = false;//表示已经处理过了
                    }
                }
            }
            if (isadd) {
                if (parseInt(data.Count) > 0) {
                    //如果可以直接添加
                    locatlData.push(data);
                }
            }
            SetLocalStorage("Shop_" + SID, JSON.stringify(locatlData));
        }
        catch (e) {

        }
    }
}
//返回数据列表
function GetGood(SID) {
    return GetLocalStorage("Shop_" + SID);
}
//当页面存在符合条件的值得时候，自动进行赋值 ,入参方式为条件
function SetData(Numselect, SID) {
    //获取当前数据信息
    var NowData = eval("(" + GetGood(SID) + ")");
    if (NowData != null) {
        for (var i in NowData) {
            //当前入参控件内一定存在几个值，Count,Gid,SiD,Price   Count为value
            $(Numselect).each(function () {
                if (NowData[i].Gid == $(this).attr("Gid")) {
                    $(this).parent().parent().find(".minus_btn").show();
                    $(this).val(NowData[i].Count);
                }
            });
        }
    }
}


function CheckCollection(type, data) {

    switch (type) {
        case "phone":
            return /^1[34578]\d{9}$/.test(data);
        case "tel":
            return /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/.test(data);
        case "peopleid":
            return /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{4}$/.test(data);

    }
}

function RegularClass(type, data, eqval) {
    var json = {};
 
    switch (type) {
        case "1":
            if ($.trim(data) == "") {
                json.flag = false;
            }
            else {
                json.flag = true;
            }
            json.message = "此项不可空";
            break;
        case "2":
            json.flag = /^\d+$/.test(data);
            json.message = "只能输入非负整数";
            break;
        case "3":
            json.flag = /^1[34578]\d{9}$/.test(data);
            json.message = "手机号格式错误";
            break;
        case "4":
            json.flag = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(data)
            json.message = "邮件格式错误，例:niuxiaoshan@eime.com";
            break;
        case "5":
            json.flag = /[^\d.]/g.test(data);
            json.message = "只能输入小数";
            break;
        case "6":
            json.flag = /\D/g.test(data);
            json.message = "只能输数字!";
            break;
        case "7":
            json.flag = /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$/.test(data);
            json.message = "身份证格式不正确!";
            break;
        case "8": 
            if ($.trim(data) == eqval) {
                json.flag = false;
            }
            else {
                json.flag = true;
            }
            json.message = "此项不可空";
            break;
        default: 
            json.flag = false;
            json.message = "";
            break;
    }
    return json;
}

var Mindex = 0;
$.fn.extend({
    Rule: function (arrtype, eqval) {
        var val = this.val();
        var result = {};
        result.success = true;
        result.message = "";
        $.each(arrtype, function (index, value) { 
            var Return = JSON.stringify(RegularClass(value, val, eqval));
 
            var json = eval('(' + Return + ')');
            if (!json.flag) {
                result.success = false;
                result.message += json.message + ",";
            }
        });
        if (result.success) {
            layer.close($(this).attr("index"));
            $(this).css("border", "");
        }
        else { 
            Mindex = layer.tips('' + result.message, $(this), {
                time: 1000000,
                tips: [2, '#FF0000'],
                tipsMore: true
            }); 
            layer.close($(this).attr("index"));
            $(this).attr("index", Mindex);
            $(this).css("border", "1px red solid");
        }
        return result;
    }
});


var index = 0;



function RegularNull(LabelIDList, type) {

    var Arr = LabelIDList.split(',');
    for (var i = 0; i < Arr.length; i++) {
        $(Arr[i]).attr("RegularType", type);
        $(Arr[i]).blur(function () {
            var val = $(this).val();
            var Return = JSON.stringify(RegularClass(type, val));
            console.log(Return);
            var json = eval('(' + Return + ')');
            if (!json.flag) {
                index = layer.tips('' + json.message, $(this), {
                    time: 1000000,
                    tips: [2, '#FF0000'],
                    tipsMore: true
                });
                $(this).css("border", "1px red solid");
                layer.close($(this).attr("index"));
                $(this).attr("index", index);
                $(this).attr("allow", "False");


            } else {
                $(this).removeAttr("allow");
                layer.close($(this).attr("index"));
                $(this).css("border", "");
            }


        })

        if ($(Arr[i]).val() == '') {
            $(Arr[i]).attr("allow", "False");
        }

    }
}


function Regular() {
    var Return = true;
    var Count = true;
    var i = 0;
    $("input,textarea").each(function () {
        var allow = $(this).attr("allow");
        if (allow == 'False') {
            var RegularType = $(this).attr("regulartype");

            var val = $(this).val();
            var Return = JSON.stringify(RegularClass(RegularType, val));
            var json = eval('(' + Return + ')');
            if (!json.flag) {
                if (Count) {
                    scrollOffset($(this).offset());
                    Count = false;
                }
                index = layer.tips('' + json.message, $(this), {
                    time: 1000000,
                    tips: [2, '#FF0000'],
                    tipsMore: true
                });
                $(this).css("border", "1px red solid");
                layer.close($(this).attr("index"));
                $(this).attr("index", index);
                $(this).attr("allow", "False");
                i++;

            } else {
                $(this).removeAttr("allow");
                layer.close($(this).attr("index"));
                $(this).css("border", "");
            }


        }

    })



    if (i > 0) {
        Return = false;
    }
    return Return;
}



function scrollOffset(scroll_offset) {
    $("body,html").animate({
        scrollTop: scroll_offset.top - 70
    }, 0);
}



