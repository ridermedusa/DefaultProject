(function ($) {
    //创建实例
    $.fn.extend({
        CreateJcrop: function (Option) {
            $('#CreateJProp').html("");
            //创建基础DOM
            var $this = this;
            //开始动态加载JS 
            var width = "";
            var height = "";
            var more = "";
            if (Option) {
                width = Option.width;
                height = Option.height;
                if (Option.more) { more = Option.more; }
            }
            //在指定DOM内创建,包括一个空的图片容器，一个提交容器
            var html = "<img src=\"\" id=\"target" + more + "\"  style=\"width:" + width + "px;height:" + height + "px;\"  />";
            html += "<form id=\"coords" + more + "\" class=\"coords\" onsubmit=\"return false;\">";
            html += "<input type=\"hidden\"  id=\"x1" + more + "\" name=\"x1" + more + "\" />";
            html += "<input type=\"hidden\" id=\"y1" + more + "\" name=\"y1" + more + "\" />";
            html += "<input type=\"hidden\"   id=\"x2" + more + "\" name=\"x2" + more + "\" />";
            html += "<input type=\"hidden\"  id=\"y2" + more + "\" name=\"y2" + more + "\" />";
            html += "<input type=\"hidden\"  id=\"w" + more + "\" name=\"w" + more + "\" />";
            html += "<input type=\"hidden\"   id=\"h" + more + "\" name=\"h" + more + "\" />";
            html += "</form>";
            $this.html(html);

        }
        , BindJcropEven: function (src, Recommend, option, SelectEven, DbclickEven) {
            var maxSize = [];
            var minSize = [];
            bgColor = "black";
            bgOpacity = 0.6;
            var more = "";
            if (option) {
                if (option.maxSize) { maxSize = option.maxSize; }
                if (option.minSize) { minSize = option.minSize; }
                if (option.bgColor) { bgColor = option.bgColor; }
                if (option.bgOpacity) { bgOpacity = option.bgOpacity; }
                if (option.more) { more = option.more; }
            }
            $('#target' + more).attr("src", src);
            var jcrop_api;
            $('#target' + more).Jcrop({
                allowSelect: true, //允许新选框
                allowMove: true, //是否允许选框移动
                allowResize: true,//是否允许选框缩放
                trackDocument: true,//拖动选框时，允许超出图像以外的地方时继续拖动。
                bgColor: bgColor,//背景颜色。颜色关键字、HEX、RGB 均可。
                bgOpacity: bgOpacity,//背景透明度
                bgFade: true,//背景色过度效果
                aspectRatio: Recommend,//选框宽高比
                keySupport: true,//支持键盘控制。按键列表：上下左右（移动选框）、Esc（取消选框） 
                maxSize: maxSize, //最大尺寸
                minSize: minSize, //最小尺寸
                onChange: function (c) {
                    $('#x1' + more).val(parseInt(c.x));
                    $('#y1' + more).val(parseInt(c.y));
                    $('#x2' + more).val(parseInt(c.x2));
                    $('#y2' + more).val(parseInt(c.y2));
                    $('#w' + more).val(parseInt(c.w));
                    $('#h' + more).val(parseInt(c.h));
                },
                onSelect: function (c) {
                    $('#x1' + more).val(parseInt(c.x));
                    $('#y1' + more).val(parseInt(c.y));
                    $('#x2' + more).val(parseInt(c.x2));
                    $('#y2' + more).val(parseInt(c.y2));
                    $('#w' + more).val(parseInt(c.w));
                    $('#h' + more).val(parseInt(c.h));
                },
                onRelease: function () {
                    $('#coords' + more + ' input').val('');
                },
                onDblClick: DbclickEven //双击事件
            }, function () {
                jcrop_api = this;
            });
        }
    });
})(jQuery);

