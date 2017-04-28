(function ($) { 
    $.fn.extend({           
        CreateTree: function (data) {
            //根据入参数据生成DOM
            var html = "";
            //总之先生成一个UL，当前第一级，应该一定有下一级
            if (data != null) {
                html += "<ul>"
                html += "  <li class=\"parent_li\">";
                //当前第一级使用全局图标
                if (data.photo == null) {
                    data.photo = "/Scripts/OrgChart-master/dist/img/default.png";
                }
                html += "   <span title=\"Collapse this branch\"><i class=\" glyphicon glyphicon-globe\"></i>" + data.title +"   "+ data.name + "(" +data.workcard + ")" + "<img class=\"avatar\" style=\"width:40px; height:40px;\" src=\"" + data.photo + "\" onerror=\"this.src='/Scripts/OrgChart-master/dist/img/default.png'\"></span><a href=\"javascript:void(0);\"  clt=\"" + data.id + "\"  name=\"roleedit\" style=\"margin-left:10px;\">编辑</a>";
                //开始判断是否有下级，如果有则增加li
                if (data.children.length > 0) {
                    html += GetKidhtml(data);
                }
                html += " </li>"; 
                html += "</ul> ";
            } 
            $(this).html(html);
            $(this).find('li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');
            $(this).find('li.parent_li > span').on('click', function (e) {
                var children = $(this).parent('li.parent_li').find(' > ul > li');   
                if (children.is(":visible")) {
                    children.hide('fast');
                    $(this).attr('title', 'Expand this branch').find(' > i').addClass('glyphicon-plus-sign').removeClass('glyphicon-minus-sign');
                } else {
                    children.show('fast');
                    $(this).attr('title', 'Collapse this branch').find(' > i').addClass('glyphicon-minus-sign').removeClass('glyphicon-plus-sign');
                }
                e.stopPropagation();
            });
        }
    });

    function  GetKidhtml(data)
    {
        var html = "";
        //检查是否存在下级，如果存在则有UL
        html += "<ul>" 
        var kidarr = data.children; 
        for (var item in kidarr) {
            if (kidarr[item].photo == null) {
                kidarr[item].photo = "/Scripts/OrgChart-master/dist/img/default.png";
            }
            if (kidarr[item].children.length > 0) {
                //如果当前存在下级，如果存在则使用带样式li
                html += "  <li class=\"parent_li\">";
                html += "   <span title=\"Collapse this branch\"><i class=\"glyphicon glyphicon-minus-sign\"></i>" + kidarr[item].title + "   " + kidarr[item].name + "(" + kidarr[item].workcard + ")" + "<img class=\"avatar\" style=\"width:40px; height:40px;\" src=\"" + kidarr[item].photo + "\" onerror=\"this.src='/Scripts/OrgChart-master/dist/img/default.png'\"></span><a href=\"javascript:void(0);\"  clt=\"" + kidarr[item].id + "\"  name=\"roleedit\" style=\"margin-left:10px;\">编辑</a> <a href=\"javascript:void(0);\"  clt=\"" + kidarr[item].id + "\"  name=\"roledel\" style=\"margin-left:10px;\">删除</a>";
            }
            else {
                html += "  <li>";
                html += "   <span title=\"\"><i class=\"glyphicon glyphicon-user\"></i>" + kidarr[item].title + "   " + kidarr[item].name + "(" + kidarr[item].workcard + ")" + "<img class=\"avatar\" style=\"width:40px; height:40px;\" src=\"" + kidarr[item].photo + "\" onerror=\"this.src='/Scripts/OrgChart-master/dist/img/default.png'\"></span><a href=\"javascript:void(0);\"  clt=\"" + kidarr[item].id + "\" name=\"roleedit\" style=\"margin-left:10px;\">编辑</a><a href=\"javascript:void(0);\"  clt=\"" + kidarr[item].id + "\"  name=\"roledel\" style=\"margin-left:10px;\">删除</a>";
             } 
            //开始判断是否有下级，如果有则增加li
            if (kidarr[item].children.length > 0) {
                html += GetKidhtml(kidarr[item]);
            }
            html += " </li>";
        }
        html += "</ul> ";
        return html;
    }
})(jQuery)