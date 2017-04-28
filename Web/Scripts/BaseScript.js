
var loginload;
$(function () {
    $('#exitbtn').unbind("click").click(function () {
        $.get("/Ajax/Exit.ashx", function () { });
    });
    var w = screen.width;
    var h = $(window).height();
    var top = 70;
    var foot = 10;
    var cont = h - top - foot;
    $(".tops").css("height", top + "px");
    $(".conts").css("height", cont + "px");
    $(".foots").css("height", foot + "px");
    //alert(h);

    //左右收缩
    $("#switchmenu").toggle(
		function () {
		    $("#frmTitle").hide();
		    $("#switchdot").addClass("on");
		},
		function () {
		    $("#frmTitle").show("slow");
		    $("#switchdot").removeClass("on");
		}
	);

    $("#switchdot").mouseout(function () {
        $(this).css("background-color", "#1e1e1e");
    });
    $("#switchdot").mouseover(function () {
        $(this).css("background-color", "#333");
    });
    PostLogin();
});

//请求登陆方法
function PostLogin() {
    $('#LoginBtn').unbind("click").click(function () {
        var input_username = $('#username').val();
        var input_password = $('#password').val();
        if ($.trim(input_username) == "" || $.trim(input_password) == "") {
            layer.alert('用户名或密码不能为空');
        }
        else {
            //Post数据
            loginload = layer.load(1);
            $.post("/Ajax/Login.ashx", { "username": input_username, "password": input_password }, function (data) {
                layer.close(loginload);
                if (data.Code == "0") {
                    location.href = "index.aspx";
                }
                else {
                    layer.alert(data.Errmsg);
                }
            },"json");
        }
    });
}
//模板页方法
function iFrameHeight() {
    var ifm = document.getElementById("main");
    var subWeb = document.frames ? document.frames["main"].document : ifm.contentDocument;
    if (ifm != null && subWeb != null) {
        ifm.height = subWeb.body.scrollHeight;
    }
}

function changeUrl(url){
    $('#main').attr("src", url);
}

