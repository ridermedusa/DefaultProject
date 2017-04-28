$(function () {
    BindEdit();
    BindDel();
    ReloadOrgS();
    reload();
});
function reload() {
    $(document).on("click", "#reload", function () {
        
        ReloadOrgS();
    });
}
//绑定编辑和添加事件
function BindEdit() {
    $(document).on("click", "[name='roleedit']", function () {
        var roleedit = $(this).attr("clt");
        $.OpenParentEdit("/Admin/Organization/Edit/" + roleedit, 470, 900, "Edit");
    });
    $(document).on("click", "[name='roleadd']", function () {
        $.OpenParentEdit("/Admin/Organization/Edit", 470, 900, "Edit");
    });
}
//绑定删除事件
function BindDel() {
    $(document).on("click", "[name='roledel']", function () {
        var rid = $(this).attr("clt");
        parent.layer.confirm("删除该数据会使下级所有数据不可见，您是否要删除这条数据", function () {
            //开始删除
            $.Majax("/Admin/Organization/DelRow", { "RowID": rid }, function (data) {
                if (data.Code == "0") { 
                    parent.layer.alert(data.Errmsg, function () {
                        CreateOrg();
                        parent.layer.closeAll();
                    });
                }
            });
        });
    });
}


function ReloadOrgS()
{
 
    $.get('/admin/Organization/GetOrganization', function (MyData) { 
        $('#Treeinfo').CreateTree(MyData);
    }, "json");
}



function CreateOrg()
{
    $('#chart-container').html("");
    $.get('/admin/Organization/GetOrganization', function (MyData) { 
        $('#chart-container').orgchart({
            'data': MyData,
            'depth': 10,
            'nodeContent': 'title',
            'nodeID': 'id',
            'createNode': function ($node, data) {
                $('.con_right0').css("width", "100%");
                var secondMenuIcon = $('<i>', {
                    'class': 'fa fa-info-circle second-menu-icon',
                    click: function () {
                        $(this).siblings('.second-menu').toggle();
                        $(this).siblings('.diy-menu').toggle();
                    }
                });
                if (data.photo == null) {
                    data.photo = "/Scripts/OrgChart-master/dist/img/default.png";
                }
                var secondMenu = '<div class="second-menu"><img class="avatar" src=\"' + data.photo + '\" onerror="this.src=\'/Scripts/OrgChart-master/dist/img/default.png\'"></div>';
                var threeMenu = '<div class="diy-menu"><a href=\"javascript:void(0);\" name=\"roleedit\"  clt=\"' + data.id + '\" style=\"margin-left:3px;margin-right:3px;color:10px;\"><span class="glyphicon glyphicon-edit">编辑</span></a>';
                if (data.id != 1) {
                    threeMenu += '<a href=\"javascript:void(0);\" name=\"roledel\"  clt=\"' + data.id + '\"  style=\"margin-left:3px;margin-right:3px;color:10px;\"><span class="glyphicon glyphicon-trash">删除</span></a>';
                }
                threeMenu += '</div>'; 
                $node.append(secondMenuIcon).append(secondMenu).append(threeMenu);
            }
        });
    }, "json");
}
