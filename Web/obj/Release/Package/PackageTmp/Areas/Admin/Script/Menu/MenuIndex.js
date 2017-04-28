/// <reference path="../../Views/File/MenuIndex.aspx" />
$(function () {
    GetPage(1, $('#pagecount').val());
    GetBodyinfo($('.OptionPagerList .current').html());
    $('#Serchbtn').unbind("click").click(function () {
        GetBodyinfo(1);
    });
    BindEdit();
    BindDel();
});


//获取页面列表信息
function GetPage(NowPageNum, AllPageCount) {
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
    loginload = layer.load();

    $.post("/Admin/Menu/GetTableinfo", { "pageindex": pageindex, "serchJson": JSON.stringify(Jsondate) }, function (data) {
        layer.close(loginload);
        var pageContDom = "";
        var Mi = 1;
        for (var i = 0; i < data.length; i++) {
            pageContDom += " <tr>";
            pageContDom += "         <th>" + data[i].RightName + "</th>";
            pageContDom += "         <td>" + data[i].AddTime.replace('T', ' ') + "</td>"; 
            pageContDom += "         <td>";
            pageContDom += "               <span class=\"glyphicon glyphicon-edit\"><a href=\"javascript:void(0);\" name=\"roleedit\" style=\"color:black;margin-left:4px;\" clt=\"" + data[i].ID + "\">修改</a></span>";
            pageContDom += "              <span class=\"glyphicon glyphicon-trash\"><a href=\"javascript:void(0);\" name=\"roledel\"  style=\"color:black;margin-left:4px;\" clt=\"" + data[i].ID + "\" >删除</a></span>";
            pageContDom += "          </td>";
            pageContDom += "     </tr> ";



            Mi++;
        }
        $('#GetBody').html(pageContDom);
    }, "json");
}

//绑定删除事件
function BindDel() {
    $(document).on("click", "[name='roledel']", function () {
        var rid = $(this).attr("clt");
        layer.confirm("您是否要删除这条数据", function () {
            //开始删除
            $.Majax("/Admin/Menu/DelRow", { "RowID": rid }, function (data) {
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
        $.OpenParentEdit("/Admin/Menu/MenuIndex/" + roleedit, 670, 900, "Edit");
    });
    $(document).on("click", "[name='roleadd']", function () {
        $.OpenParentEdit("/Admin/Menu/MenuIndex", 670, 900, "Edit");
    });
}
