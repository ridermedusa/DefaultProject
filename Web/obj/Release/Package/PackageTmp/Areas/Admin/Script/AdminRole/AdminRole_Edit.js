$(function () {
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    GetBody();
    $('#EditBtn').unbind("click").click(function () { 
        var Jsondata = {};
        //开始提交页面权限
        var ROLEVALUE = "";
        $('[name="menuinfo"]').each(function () {
            if ($(this).is(":checked"))
            {
                ROLEVALUE += $(this).val() + ",";
            }
        });
        if ($.trim(ROLEVALUE) != "")
        {                     
            ROLEVALUE = ROLEVALUE.substring(0, ROLEVALUE.length - 1);
        }
        //开始提交按钮权限
        var ButtonRole = "";
        $('[name="IsButton"]').each(function () {
            if ($(this).is(":checked")) {
                ButtonRole += $(this).val() + ",";
            }
        });
        if ($.trim(ButtonRole) != "") {
            ButtonRole = ButtonRole.substring(0, ButtonRole.length - 1);
        }
        Jsondata.ID = $('#ID').val();
        //Jsondata.Ename = $('#Ename').val();
        Jsondata.ROLENAME = $('#ROLENAME').val();
        Jsondata.ROLEVALUE = ROLEVALUE;
        Jsondata.ButtonRole = ButtonRole;
        Jsondata.NOTE = $('#NOTE').val();
        //开始提交数据
        $.Majax("/Admin/AdminRole/controller", { "type": "save", "data": JSON.stringify(Jsondata) }, function (data) {
            if (data.Code == "0") {
                parent.main.layer.msg(data.Errmsg);
                parent.main.GetBodyinfo(parent.main.$('.OptionPagerList .current').html());
                parent.layer.close(index);
            }
            else {
                parent.layer.alert(data.Errmsg);
            }
        });
    });
});


//获取html数据
function GetBody() {
    $.post("/Admin/AdminRole/controller", { "type": "GetHtml" }, function (data) {
        $('#GetLiHtml').html(data);
        EvenCheck();
        GetRoleinfo();
    });
}

//控制触发事件
function EvenCheck() {
    $('[name="menuinfo"]').unbind("click").click(function () {
        if ($(this).is(":checked")) {
            $('[id^="menuinfo_'+$(this).val()+'"]').each(function () {
                $(this).prop("checked", true);
            });
        }
        else {
            $('[id^="menuinfo_' + $(this).val() + '"]').each(function () {
                $(this).prop("checked", false);
            });
        }
    });
}
function GetRoleinfo() {
    var Rid = $('#ID').val();
    if (Rid != "0") {
        //尝试获取数据信息
        $.Majax("/Admin/AdminRole/controller", { "type": "GetModel", "ID": Rid }, function (data) {
            if (data.Code == "0") {
                var model = data.GetModel;
                $('#ROLENAME').val(model.ROLENAME);
                $('#Ename').val(model.Ename);
                $('#NOTE').val(model.NOTE);
                //复制对应数据
                var Roleinfo = model.ROLEVALUE;
                if ($.trim(Roleinfo) != "") {
                    //如果当前存在值，则直接开始拆分
                    $('[name="menuinfo"]').each(function () {
                        var spl = Roleinfo.split(',');
                        var Checkval = $(this);
                        for(var i in spl)
                        {
                            if (Checkval.val() == spl[i]) {
                                Checkval.prop("checked", true);
                                break;
                            }
                            else {
                                continue;
                            }
                        }
                    });
                }
                var ButtonRole = model.ButtonRole;
                if ($.trim(ButtonRole) != "") {
                    //如果当前存在值，则直接开始拆分
                    $('[name="IsButton"]').each(function () {
                        var spl = ButtonRole.split(',');
                        var Checkval = $(this);
                        for (var i in spl) {
                            //console.log(Checkval.val() + ":::" + spl[i]);
                            if (Checkval.val() == spl[i]) {
                                Checkval.prop("checked", true);
                                break;
                            }
                            else {
                                continue;
                            }
                        }
                    });
                }
            }
            else {
                layer.alert("获取信息失败");
            }
        });
    }
}