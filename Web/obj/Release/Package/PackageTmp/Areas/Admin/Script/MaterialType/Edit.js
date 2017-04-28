$(function () {
    SaveBtn();
});
function SaveBtn() {
    $(document).on("click", "#EditBtn", function () {
        var TypeName = $('#TypeName').val();
        var Sort = $('#Sort').val();
        $.Majax("/admin/MaterialType/Save", { ID: $('#ID').val(), TypeName: $('#TypeName').val(), Sort: $('#Sort').val() }, function (data) {
            if (data.Code == "0") {
                parent.layer.alert(data.Errmsg, function () {
                    parent.main.location.href = parent.main.location.href;
                    parent.layer.closeAll();
                });
            }
            else {
                parent.layer.alert(data.Errmsg);
            }
        });
    });
}