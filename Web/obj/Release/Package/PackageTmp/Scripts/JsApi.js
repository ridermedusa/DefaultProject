var GetUrl = "http://localhost:1002/api";
//请求DEMO
//var JsonParam = {};
//JsonParam.code = "PPSh-43";
//$.Api("LC_Api.Text", JsonParam, function (result) {
//    alert(result);
//});
$(function () {
    jQuery.Api = function (Method, Param, success, error) {
        var Jsondata = {};
        Jsondata.method = Method;
        Jsondata.param = Param;
        $.ajax({
            url: GetUrl,//jsoncode=1
            dataType: 'jsonp',
            data: "jsoncode=" + JSON.stringify(Jsondata),
            jsonp: 'callback',
            async: false,
            success: function (result) {
                success(JSON.stringify(result));
            },
            error: function (e) {
                error(JSON.stringify(result));
            }
            //timeout: 3000
        });
    };
});