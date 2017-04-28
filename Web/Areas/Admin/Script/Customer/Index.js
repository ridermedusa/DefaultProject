$(function () {
    BindDel();
    BindEdit();
    showinfo();
});
 
//绑定删除事件
function BindDel() {
    $(document).on("click", "[name='roledel']", function () {
        var rid = $(this).attr("clt");
        parent.layer.confirm("您是否要删除这条数据", function () {
            //开始删除
            $.Majax("/Admin/Customer/DelRow", { "RowID": rid }, function (data) {
                if (data.Code == "0") {
                    parent.layer.close();
                    parent.layer.msg(data.Errmsg, function () { location.href = location.href;});

                }
            });
        });
    });
}
//绑定编辑和添加事件
function BindEdit() {

    $(document).on("click", "[name='roleedit']", function () {
        var roleedit = $(this).attr("clt");
        $.OpenParentEdit("/Admin/Customer/Edit/" + roleedit, 570, 850, "Edit");
    });
    $(document).on("click", "[name='roleadd']", function () {
        $.OpenParentEdit("/Admin/Customer/Edit", 570, 850, "Edit");
    });
}

function showinfo() {
    $('[name="showitem"]').hover(function () {
        var clt = $(this).attr("clt");
        $('#showitemA_' + clt).hide();
        $('#showitem_' + clt).slideDown();

    }, function () {
  
    });
    $('[name="showitemB"]').hover(function () {

    }, function () { 
        var clt = $(this).attr("clt");
        $('#showitem_' + clt).hide();
        $('#showitemA_' + clt).slideDown();
    });
}