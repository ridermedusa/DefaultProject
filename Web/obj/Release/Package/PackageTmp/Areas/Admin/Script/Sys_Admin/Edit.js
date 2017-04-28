$(function () {
    Addbtn(); 
});
function Addbtn() {
    $(document).on("click", "#EditBtn", function () {
        $('#RoleName').val($('#RoleID').find("option:selected").html());
        var data = GetFormSerialize("Subinfo");
        console.log(data);
 
        $.Majax("/Admin/Sys_Admin/Controller", { "type": "save", "data": JSON.stringify(data),  }, function (data) {
            if (data.Code == "0") {
                var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                parent.main.layer.msg(data.Errmsg);
                if (parent.main.$('.OptionPagerList').html() == "") {
                    parent.main.GetBodyinfo(1);
                }
                else {
                    parent.main.GetBodyinfo(parent.main.$('.OptionPagerList .current').html());
                }
                parent.layer.close(index);
            }
            else {
                parent.layer.alert(data.Errmsg);
            }
        });
    });
}
