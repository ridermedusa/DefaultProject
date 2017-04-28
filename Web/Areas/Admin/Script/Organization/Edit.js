$(function () { 
    Upfileload();
    SaveSubmit();
});

function Upfileload() {
    $('#CreateJPropByIndex').CreateJcrop({ more: "ByIndex" });
    $('#fileuploadByIndex').fileupload({
        dataType: 'json',
        done: function (e, data) {
            ///重新初始化一次
            $('#CreateJPropByIndex').CreateJcrop({ "more": "ByIndex" });
            var wid = 60;
            var hei = 60;
            //开始处理 
            $('#CreateJPropByIndex').BindJcropEven(data.result.FileUrl, wid / hei,
                {
                    maxSize: [],
                    minSize: [],
                    bgColor: "lightgreen",
                    bgOpacity: 0.5,
                    more: "ByIndex"
                }
                , function () {
                    //选定后，以及单击
                }, function () {
                    //请求裁剪
                    $.post('/Scripts/Jcrop/MyJcrop.ashx', {
                        "url": $('#CreateJPropByIndex').find("#targetByIndex").attr("src"),
                        "x": $('#x1ByIndex').val(),
                        "y": $('#y1ByIndex').val(),
                        "w": $('#wByIndex').val(),
                        "h": $('#hByIndex').val(),
                        "PicName": "Art_ByIndex_" + $('#ID').val()
                    }, function (data) {
                        if (data.Code = "0") {
                            $('#AfterPicByIndex').html("<img src=\"" + data.url + "?t=" + Math.random() + "\" id=\"afterpicByIndex\"/>");
                            $('#Photo').val(data.url);
                        }
                    }, "json");
                });
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress_divByIndex').css("width", progress + "%");
            $('#progress_divcenterByIndex').html(progress + "% Complete");
        }
    });
}


function SaveSubmit()
{
    $(document).on("click", "#EditBtn", function () {
        var jsonD = {};
        jsonD.ID = $('#ID').val();
        jsonD.Name = $('#Name').val();
        jsonD.Dep = $('#Dep').val();
        jsonD.Position = $('#Position').val();
        jsonD.Sort = $('#Sort').val();
        jsonD.Photo = $('#Photo').val();
        jsonD.ParentID = $('#ParentID').val();
        jsonD.WorkNo = $('#WorkNo').val();
        //开始提交数据
        $.Majax("/Admin/Organization/Save", { "data": JSON.stringify(jsonD) }, function (data) {
            if (data.Code == "0") {
                var index = parent.layer.getFrameIndex(window.name); //获取窗口索引 
                layer.alert(data.Errmsg, function () {
                    parent.main.ReloadOrgS();
                    parent.layer.closeAll();
                });
            }
            else {
                parent.layer.alert(data.Errmsg);
            }
        });
    });
}