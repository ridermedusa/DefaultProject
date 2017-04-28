var s = ["Province", "City", "Area"];//三个select的name
var opt0 = ["省份","地级市","市、县级市"];//初始值
function _init_area(){  //初始化函数
	for(i=0;i<s.length-1;i++){
	  document.getElementById(s[i]).onchange=new Function("change("+(i+1)+")");
	}
	change(0);
}
$(function () {
    _init_area();
    $('#Province').val($('#ProvinceHide').val());
    change(1);
    $('#City').val($('#CityHide').val());
    change(2);
    $('#Area').val($('#AreaHide').val());
    BindAdd();
    BindEven();
});

function BindAdd()
{
    $(document).on("click", "#EditBtn", function () {
        //验证身份证
        if (!$("#ID_Card").Rule(["1", "7"]).success)
        {
            $("#ID_Card").blur();
            window.location.hash = "#ID_Card";
            return false;
        }
        //验证姓名非空
        if (!$("#Name").Rule(["1"]).success) {
            $("#Name").blur();
            window.location.hash = "#Name";
            return false;
        }
        //验证手机号非空
        if (!$("#Phone").Rule(["3"]).success) {
            $("#Phone").blur();
            window.location.hash = "#Phone";
            return false;
        }
        //验证地址非空
        if (!$("#Province").Rule(["8"], "省份").success) {
            $("#Province").blur();
            window.location.hash = "#Province";
            return false;
        }
        //验证地址非空
        if (!$("#City").Rule(["8"], "地级市").success) {
            $("#City").blur();
            window.location.hash = "#City";
            return false;
        }
        if (!$("#Area").Rule(["8"], "市、县级市").success) {
            $("#Area").blur();
            window.location.hash = "#Area";
            return false;
        }
        //验证地址非空
        if (!$("#Address").Rule(["1"]).success) {
            $("#Address").blur();
            window.location.hash = "#Address";
            return false;
        } 
        //绑定提交事件
        var data = GetFormSerialize("Subinfo");
        $.Majax("/admin/Customer/Save", { "data": JSON.stringify(data) }, function (data) {
            if (data.Code == "0") {
                var index = parent.layer.getFrameIndex(window.name); //获取窗口索引 
                layer.alert(data.Errmsg, function () {
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

function BindEven() {
    $("#ID_Card").bind('input propertychange', function () {
        var value = $(this).val();
        value = value.replace(/x/g, "X");
        $(this).val(value);
    });
    $("#Sale").autocomplete("/admin/Customer/getInv", {
        max: 10,// 显示数量  
        width: 320,
        matchContains: true,
        autoFill: false,
        multiple: false,
        formatItem: function (data, i, n, v) {
            return '<span style="padding-right:10px;"> ' + data.Name + '</span> <span>[' + data.Title + '][' + data.WorkNo + ']</span>';
        },
        formatResult: function (data, v) {
            return v;
        },
        parse: function (data) {//解释返回的数据，把其存在数组里  
            if (data.length>0) {
                var array = eval("(" + data + ")");
                var parsed = [];
                if (array == null) {
                    return parsed;
                }
                for (var i = 0; i < array.length; i++) {
                    parsed[i] = {
                        data: array[i],
                        value: array[i].ID,
                        result: array[i].Name
                    };
                }
                return parsed;
            }
        }
    }).result(function (event, row, formatted) {
        $('#SaleID').val(row.ID);
    });
}
