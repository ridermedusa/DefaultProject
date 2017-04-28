$(function () {
    LoginFunction();
});
//绑定页面方法
function LoginFunction()
{
    $(document).on("click", "#LoginBtn", function () {
        var uid = $('#uid').val();
        var password = $('#password ').val();
        if ($.trim(uid) == "" || $.trim(password) == "") {
            layer.alert("用户名或密码不能为空");
        }
        else {
            try { 
                $.Majax("/Admin/Login/LoginSubmit", { "uid": uid, "password": password }, function (data) {

                    if (data.Code == "0") {
                        //直接跳转
                        location.href = "/admin";
                    }
                    else {
                        layer.alert(data.Errmsg);
                    }
                });
            } 
            catch (e)
            {
                $('#uid').val(e.Message);
            }
        }
    })
}