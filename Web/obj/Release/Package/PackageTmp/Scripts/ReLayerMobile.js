var T_click = "click";
if ('ontouchstart' in window) {
    T_click = 'touchstart';
}
$.extend({
    M_alert: function (content, btn) {
        layer.open({
            content: content
             , btn: btn
        });
    },
    M_Load: function (content, time) {
        if (!time) {
            time = 2;
        }
        if (!content) {
            content = "Loading...";
        }
        layer.open({
            content: content
            , skin: 'msg'
            , time: time //2秒后自动关闭
        });
    },
    M_confirm: function (content, btn, yes) {
        layer.open({
            content: content
          , btn: btn//['刷新', '不要']
          , yes: function (index) {
              yes(index);
              //location.reload();
              //layer.close(index);
          }
         
        });
    },
    M_bottomalert: function (content, btn, yes, no) {
        //底部对话框
        layer.open({
            content: content
          , btn: btn//['删除', '取消']
          , skin: 'footer'
          , yes: function (index) {
              yes(index);
              //layer.open({ content: '执行删除操作' })
          }, no: function (index) {
              yes(index);
          }
        });
    },
    M_bottomtip: function (content) {
        //底部提示
        layer.open({
            content: content
          , skin: 'footer'
        });
    },
    M_Loading: function (content) {
        if (!content)
        {
            content = "Loading...";
        }
        layer.open({
            type: 2,
            content: content,
            shadeClose: false
        });
    }
});