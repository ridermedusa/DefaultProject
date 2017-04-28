$(function () {
    GetBodyinfo(1);
    $('#Serchbtn').unbind("click").click(function () {
        GetBodyinfo(1);
    });
    BindEdit();
    BindDel();
});


//获取页面列表信息
function BindPage(NowPageNum, AllPageCount) {
    $(".OptionPagerList").createPage({
        pageCount: AllPageCount,
        current: NowPageNum,
        IsShowSelect: true,
        IsShowPrevPage: true,
        IsShowNextPage: true,
        IsShowAllCount: false,
        AllCount: $('#ALLitemcount').val(),
        backFn: function (p) {
            //单击回调方法，p是当前页码
            //开始请求数据
            GetBodyinfo(p);
        }
    });
}

///获取当前数据信息
function GetBodyinfo(pageindex) {
    //尝试获取当前搜索条件
    var logcont = $('#keyword').val();
    var Jsondate = {};
    Jsondate["keyword"] = logcont;
    $.Majax("/Admin/Sys_Admin/Controller", { "Type": "GetPage", "pageindex": pageindex, "serchJson": JSON.stringify(Jsondate) }, function (data) {
        $('#ALLitemcount').val(data.ItemCount);
        $('#pagecount').val(data.PageCount);
        BindTable(data.GetModel);
        if (data.ItemCount == 0) {
            $(".OptionPagerList").hide();
        }
        else {
            $(".OptionPagerList").show();
            BindPage(pageindex, data.PageCount);
        }
    });
}

function BindTable(Model) {
    var pageContDom = "";
    var Mi = 1;
    for (var i = 0; i < Model.length; i++) {
        //pageContDom += "  <tr>" +
        //"<td><a href=\"javascript:void(0);\" name=\"roleedit\" clt=\"" + Model[i].ID + "\"><span class=\"glyphicon glyphicon-edit\"></span></a> " +
        //"<a href=\"javascript:void(0);\" name=\"roledel\" clt=\"" + Model[i].ID + "\" style=\"padding-left:5px;\"><span class=\"glyphicon glyphicon-trash\"></span></a></td>" +
        //"<td>" + Model[i].UName + "</td>" +
        //"<td>" + Model[i].TrueName + "</td>" +
        //"<td>" + Model[i].RoleName + "</td>" +
        //"<td>" + Model[i].Addtime.replace("T", " ") + "</td></tr>";

        pageContDom += " <tr>";
        pageContDom += "         <th>" + Model[i].UName + "</th>";
        pageContDom += "         <td>" + Model[i].TrueName + "</td>";
        pageContDom += "          <td>" + Model[i].RoleName + "</td>";
        pageContDom += "          <td>" + Model[i].Addtime.replace("T", " ") + "</td>";
        pageContDom += "         <td>";
        pageContDom += "               <span class=\"glyphicon glyphicon-edit\"><a href=\"javascript:void(0);\" name=\"roleedit\" style=\"color:black;margin-left:4px;\" clt=\"" + Model[i].ID + "\">修改</a></span>";
        pageContDom += "              <span class=\"glyphicon glyphicon-trash\"><a href=\"javascript:void(0);\" name=\"roledel\"  style=\"color:black;margin-left:4px;\" clt=\"" + Model[i].ID + "\" >删除</a></span>";
        pageContDom += "          </td>";
        pageContDom += "     </tr> "; 
        Mi++;
    }
    $('#GetBody').html(pageContDom);
}
//绑定删除事件
function BindDel() {
    $(document).on("click", "[name='roledel']", function () {
        var rid = $(this).attr("clt");
        layer.confirm("您是否要删除这条数据", function () {
            //开始删除
            $.Majax("/Admin/Sys_Admin/Controller", { "Type": "Delete", "ID": rid }, function (data) {
                if (data.Code == "0") {
                    layer.close();
                    layer.msg(data.Errmsg);
                    GetBodyinfo($('.OptionPagerList .current').html());
                }
            });
        });
    });
}
//绑定编辑和添加事件
function BindEdit() {
    $(document).on("click", "[name='roleedit']", function () {
        var roleedit = $(this).attr("clt");
        $.OpenParentEdit("/Admin/Sys_Admin/Edit/" + roleedit, 470, 900, "Edit");
    });
    $(document).on("click", "[name='roleadd']", function () {
        $.OpenParentEdit("/Admin/Sys_Admin/Edit", 470, 900, "Edit");
    });
}
