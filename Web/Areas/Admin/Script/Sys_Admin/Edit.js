$(function () {
    Addbtn(); 
});
function Addbtn() {
    $(document).on("click", "#EditBtn", function () { 
        var RoleID = "";
        var clt = "";
        $('[name="RoleIDinfo"]').each(function () {
            if ($(this).is(":checked")) {
             
                RoleID += $(this).val() + ",";
                clt += $(this).attr("clt") + ",";
            }
        });
        if ($.trim(RoleID) != "")
        {
            RoleID = RoleID.substring(0, RoleID.length - 1);
            clt = clt.substring(0, clt.length - 1);
        }
        $('#RoleID').val(RoleID);
        $('#RoleName').val(clt);
        var data = GetFormSerialize("Subinfo");
        console.log(data);
 
        $.Majax("/Admin/Sys_Admin/Controller", { "type": "save", "data": JSON.stringify(data) }, function (data) {
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
