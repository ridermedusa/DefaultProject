/// <reference path="jquery.page.js" />
//鍒嗛〉鎻掍欢
/**
2014-08-05 ch
**/
(function($){
	var ms = {
		init:function(obj,args){
			return (function(){
				ms.fillHtml(obj,args);
				ms.bindEvent(obj,args);
			})();
		},
		//濉厖html
		fillHtml:function(obj,args){
		    return (function () {
		        obj.empty();
		        //涓婁竴椤�
		        if (args.IsShowPrevPage) {
		            if (args.current > 1) {
		                obj.append('<a href="javascript:;" class="prevPage">上一页</a>');
		            } else {
		                obj.remove('.prevPage');
		                obj.append('<span class="disabled">上一页</span>');
		            }
		        }
		        //涓棿椤电爜
		        if (args.current != 1 && args.current >= 4 && args.pageCount != 4) {
		            obj.append('<a href="javascript:;" class="tcdNumber">' + 1 + '</a>');
		        }
		        if (args.current - 2 > 2 && args.current <= args.pageCount && args.pageCount > 5) {
		            obj.append('<span>...</span>');
		        }
		        var start = args.current - 2, end = args.current + 2;
		        if ((start > 1 && args.current < 4) || args.current == 1) {
		            end++;
		        }
		        if (args.current > args.pageCount - 4 && args.current >= args.pageCount) {
		            start--;
		        }
		        for (; start <= end; start++) {
		            if (start <= args.pageCount && start >= 1) {

		                if (start != args.current) {
		                    obj.append('<a href="javascript:;" class="tcdNumber">' + start + '</a>');
		                } else {
		                    obj.append('<span class="current">' + start + '</span>');
		                }
		            }
		        }
		        if (args.current + 2 < args.pageCount - 1 && args.current >= 1 && args.pageCount > 5) {
		            obj.append('<span>...</span>');
		        }
		        if (args.current != args.pageCount && args.current < args.pageCount - 2 && args.pageCount != 4) {
		            obj.append('<a href="javascript:;" class="tcdNumber">' + args.pageCount + '</a>');
		        }
		        //涓嬩竴椤�

		        if (args.IsShowNextPage) {
		            if (args.current < args.pageCount) {
		                obj.append('<a href="javascript:;" class="nextPage">下一页</a>');
		            } else {
		                obj.remove('.nextPage');
		                obj.append('<span class="disabled">下一页</span>');
		            }
		        }
		      
		        if (args.IsShowSelect) {
		            var selecthtml = "";
		            selecthtml += "<select class=\"changeselect\"><option value=\"0\">请选择</option>";
		            for (var p = 1; p < parseInt(args.pageCount) + 1; p++) {
		                if (p == args.current) {
		                    selecthtml += '<option value=\"' + p + '\" selected=\"selected\">' + p + '</option>';
		                }
		                else {
		                    selecthtml += '<option value=\"' + p + '\">' + p + '</option>';
		                }
		            }
		            selecthtml += "</select>";
		            obj.append(selecthtml);
		        }
		        if (args.IsShowAllCount) {
		            obj.append('<span>共检索出' + args.AllCount + '条数据</span>');
		        }
		    })();
		},
		//缁戝畾浜嬩欢
		bindEvent:function(obj,args){
		    return (function () {
		        obj.unbind("click");
		        obj.unbind("change");
		        obj.on("change", ".changeselect", function () {
		            
		            var SelectVal = parseInt($(this).val());
		            if (SelectVal == 0)
		            {
		                SelectVal = 1;
		            }
		            ms.fillHtml(obj, {
		                "current": SelectVal, "pageCount": args.pageCount,
		                "IsShowSelect": args.IsShowSelect, "IsShowPrevPage": args.IsShowPrevPage, "IsShowNextPage": args.IsShowNextPage
                        , "IsShowAllCount": args.IsShowAllCount, "AllCount": args.AllCount
		            });
		            if (typeof (args.backFn) == "function") {
		                args.backFn(SelectVal);
		            }
		        });
				obj.on("click","a.tcdNumber",function(){
				    var current = parseInt($(this).text());
				    ms.fillHtml(obj, {
				        "current": current, "pageCount": args.pageCount,
				        "IsShowSelect": args.IsShowSelect, "IsShowPrevPage": args.IsShowPrevPage, "IsShowNextPage": args.IsShowNextPage
                          , "IsShowAllCount": args.IsShowAllCount, "AllCount": args.AllCount
				    });
					if(typeof(args.backFn)=="function"){
						args.backFn(current);
					}
				});
				//涓婁竴椤�
				obj.on("click","a.prevPage",function(){
					var current = parseInt(obj.children("span.current").text());
					ms.fillHtml(obj, {
					    "current": current - 1, "pageCount": args.pageCount,
					    "IsShowSelect": args.IsShowSelect, "IsShowPrevPage": args.IsShowPrevPage, "IsShowNextPage": args.IsShowNextPage
                          , "IsShowAllCount": args.IsShowAllCount, "AllCount": args.AllCount
					});
					if(typeof(args.backFn)=="function"){
						args.backFn(current-1);
					}
				});
				//涓嬩竴椤�
				obj.on("click","a.nextPage",function(){
					var current = parseInt(obj.children("span.current").text());
					ms.fillHtml(obj, {
					    "current": current + 1, "pageCount": args.pageCount,
					    "IsShowSelect": args.IsShowSelect, "IsShowPrevPage": args.IsShowPrevPage, "IsShowNextPage": args.IsShowNextPage
                          , "IsShowAllCount": args.IsShowAllCount, "AllCount": args.AllCount
					});
					if(typeof(args.backFn)=="function"){
						args.backFn(current+1);
					}
				});
			})();
		}
	}
	$.fn.createPage = function(options){
	    var args = $.extend({
	        pageCount: 10,
	        current: 1,
	        IsShowSelect: true,//是否显示下拉选择
	        IsShowPrevPage: true,//是否显示上一页
	        IsShowNextPage: true,//是否显示下一页
	        IsShowAllCount: false,//是否显示总数量
	        AllCount: "",//总行数
	        backFn: function () { }
	    }, options);
		ms.init(this,args);
	}
})(jQuery);

//浠ｇ爜鏁寸悊锛氭噿浜轰箣瀹� www.lanrenzhijia.com



