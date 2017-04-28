$(function () {
    PageEvenCreatDOM();
    $('[name="Roletype"]').click(function () {
        PageloadEven();
    });
    
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    GetModel();
    $('#EditBtn').unbind("click").click(function () {
        var DEPNAME = $('#RightName').val();
        if ($.trim(DEPNAME) == "") {
            layer.alert("请输入网址名称");
            return false;
        }
        var Jsondata = {};
        Jsondata.ID = $('#ID').val();               
        Jsondata.RightName = $('#RightName').val();
        Jsondata.Ename = $('#Ename').val();
        Jsondata.RightUrl = $('#RightUrl').val();
        Jsondata.RightParent = $('#RightParent').val();
        var IsButton = "";
        $('[name="IsButton"]').each(function () {
            if ($(this).is(":checked"))
            {
                IsButton = $(this).val();
            }
        });
        Jsondata.IsButton = IsButton;
        var btnrole = GetRoleSubinfo();
        //var Data = GetFormSerialize("Subinfo");
        //开始提交数据
        $.Majax("/Admin/Menu/controller", { "type": "save", "data": JSON.stringify(Jsondata), "roledata": JSON.stringify(btnrole) }, function (data) {
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

function GetModel() {
    var Rid = $('#ID').val();
    if (Rid != "0") {
        //尝试获取数据信息
        $.Majax("/Admin/Menu/controller", { "type": "GetModel", "ID": Rid }, function (data) {
            if (data.Code == "0") {
                var model = data.GetModel;
                InnerData(model);
                GetInList(data.GetInList);
                PageloadEven();
            }
            else {
                layer.alert("获取信息失败");
            }
        });
    }
}

//页面加载时事件
function PageloadEven()
{
    var IsRoletype = 0;
    //确认对应权限系统
    $('[name="Roletype"]').each(function () {
        if ($(this).is(":checked")) {
            IsRoletype = $(this).val();
        }
    });
    if (IsRoletype == "1" || IsRoletype == 1) {
        //禁用
        $('#addButtonName').val("");
        $('#addButtonName').prop("disabled", true);
    }
    else {
        $('#addButtonName').prop("disabled", false);
    }
}

//点击页面添加事件 创建页面DOM
function PageEvenCreatDOM()
{
    $(document).on("click", "[name='Delbtn']", function () {
        $(this).parent().parent().remove();
    });
    $(document).on("click","#addbtn", function () {
        //开始收集写入资料
        var Jsondata = {};
        var roletype = 0;
        $('[name="Roletype"]').each(function () {
            if ($(this).is(":checked")) {
                roletype = $(this).val();
            }
        });
        Jsondata.Roletype = roletype;
        Jsondata.Operation = $('#addOperation').val();
        Jsondata.ButtonName = $('#addButtonName').val();
        //开始在页面生成对应DOM
        var html = "";
        html += "   <tr clt=\"0\"> ";
        html += " <td  style=\" padding-left:8px;padding-top:5px;\">";
        html += " 权限类型：";
        html += "<span name=\"Roletype\" clt=" + Jsondata.Roletype + ">" + GetRole(Jsondata.Roletype) + "</span>";
        html += "   </td>";
        html += " <td  style=\" padding-left:8px;padding-top:5px;\">";
        html += " 操作名： <input type=\"text\"  name=\"Operation\" value=\"" + Jsondata.Operation + "\"/>";
        html += " </td> ";
        html += "<td  style=\" padding-left:8px;padding-top:5px;\"> ";
        html += "控制钮name: <input type=\"text\"  name=\"ButtonName\" value=\"" + Jsondata.ButtonName + "\"/>";
        html += "    </td>          ";
        html += "    <td  style=\"padding-left:8px;padding-top:5px;\">        ";
        html += "     <input type=\"button\" name=\"Delbtn\" value=\"删除\" />    ";
        html += "   </td>  ";
        html += "  </tr>";
        $('#BtnRoleBody').append(html);
        $('#addOperation').val("");
        $('#addButtonName').val("");
    });
}

function GetInList(GetInList)
{
    if (GetInList != null)
    {
        for (var i in GetInList)
        {
            var html = "";
            html += "   <tr clt=\"" + GetInList[i].ID + "\"> ";
            html += " <td  style=\" padding-left:8px;padding-top:5px;\">";
            html += " 权限类型：";
            html += "<span name=\"Roletype\" clt=" + GetInList[i].Roletype + ">" + GetRole(GetInList[i].Roletype) + "</span>";
            html += "   </td>";
            html += " <td  style=\" padding-left:8px;padding-top:5px;\">";
            html += " 操作名： <input type=\"text\"  name=\"Operation\" value=\"" + GetInList[i].Operation + "\"/>";
            html += " </td> ";
            html += "<td  style=\" padding-left:8px;padding-top:5px;\"> ";
            html += "控制钮name: <input type=\"text\"  name=\"ButtonName\" value=\"" + GetInList[i].ButtonName + "\"/>";
            html += "    </td>          ";
            html += "    <td  style=\"padding-left:8px;padding-top:5px;\">        ";
            html += "     <input type=\"button\" name=\"Delbtn\" value=\"删除\" />    ";
            html += "   </td>  ";
            html += "  </tr>";
            $('#BtnRoleBody').append(html);
        }
    }
}
function GetRole(Roletype) {
    if (Roletype == 0) {
        return "按钮";
    }
    else {
        return "事件";
    }
}

//拼接一个针对该项的submit数据
function GetRoleSubinfo() {
    var JsonList = [];
    //循环每一行TR 获取TR下全部信息
    $('#BtnRoleBody').find("tr").each(function () {
        var thisjson = {};
        thisjson.ID = $(this).attr("clt");
        thisjson.Roletype = $(this).find("[name='Roletype']").attr("clt");
        thisjson.Operation = $(this).find("[name='Operation']").val();
        thisjson.ButtonName = $(this).find("[name='ButtonName']").val();
        thisjson.PageID = $('#ID').val();
        JsonList.push(thisjson);
    });
    return JsonList;
}