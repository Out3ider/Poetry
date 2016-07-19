var moreTmpl = '<button class="btn btn-block btn-sm btn-default">点击加载更多</button>';
var noneTmpl = '<div class="weui_msg">' +
                    '<div class="weui_icon_area">' +
                        '<i class="weui_icon_safe weui_icon_safe_warn"></i>' +
                    '</div>' +
                    '<div class="weui_text_area"><p class="weui_msg_desc">没有找到相应的信息</p></div>' +
                '</div>';

jQuery.fn.extend({
    NoData: function (text) {
        $(this).html(noneTmpl);
        if (text) $(this).find(".weui_msg_desc").text(text);
    }
});



//jQuery.fn.extend({
//    LoadMore: function (set) {
//        set = jQuery.extend({
//            noData: "暂无数据",
//            pageSize: 40,
//            handleName: null,
//            tmpl: "#listTmpl",
//            events: [],
//            pager: null,
//            getPostKey: function () { return {}; }

//        }, set);


//        $(this).on("sail.nodata", function () {
//            $(this).NoData();
//        });

//        var result = {
//            pager: set.pager,
//            $list: $(this),
//            currentData: null,
//            currentIndex: 1,
//            pageCount: 1,
//            settings: set,
//            Query: function (pageIndex) {
//                result.pager.html("");
//                if (pageIndex) result.currentIndex = pageIndex;
//                pageIndex = result.currentIndex;

//                if (pageIndex === 1) result.$list.html("");
//                var settings = result.settings;

//                var postData = settings.getPostKey;
//                if (typeof (settings.getPostKey) == "function") postData = settings.getPostKey();

//                var url = settings.handleName;
//                if (typeof (url) == "function") url = url();

//                postData["pageSize"] = settings.pageSize;
//                postData["pageIndex"] = pageIndex;

//                $.ajax({
//                    url: url,
//                    data: postData,
//                    success: function (data) {
//                        if (data.Data == null || data.Data.Count === 0) {
//                            result.$list.trigger("sail.nodata", [result]);
//                            return;
//                        }
//                        var html = $.Loadtmpl(set.tmpl).render(data.Data.Data);
//                        result.$list.append(html);

//                        result.pageCount = data.Data.PageInfo.PageCount;

//                        result.pager.html(result.pageCount > result.currentIndex ? moreTmpl : null);

//                        result.$list.trigger("sail.queried", [result, data.Data]);
//                        var parent = result.$list.parents("div.tab-content");
//                        parent.scrollTop(parent.height() * set.pageSize);
//                    }
//                });
//                return result;
//            }
//        };
//        result.pager.on("click", "button.btn", function () {
//            result.Query(result.currentIndex + 1);
//        });
//        return result;
//    }
//});

(function ($, window, document, undefined) {
    var pluginName = "table2excel",

    defaults = {
        exclude: ".noExl",
        name: "Table2Excel"
    };

    // The actual plugin constructor
    function Plugin(element, options) {
        this.element = element;
        // jQuery has an extend method which merges the contents of two or
        // more objects, storing the result in the first object. The first object
        // is generally empty as we don't want to alter the default options for
        // future instances of the plugin
        // 
        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    Plugin.prototype = {
        init: function () {
            var e = this;

            e.template = {
                head: "<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets>",
                sheet: {
                    head: "<x:ExcelWorksheet><x:Name>",
                    tail: "</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet>"
                },
                mid: "</x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body>",
                table: {
                    head: "<table>",
                    tail: "</table>"
                },
                foot: "</body></html>"
            };

            e.tableRows = [];

            // get contents of table except for exclude
            $(e.element).each(function (i, o) {
                var tempRows = "";
                $(o).find("tr").not(e.settings.exclude).each(function (i, o) {
                    tempRows += "<tr>" + $(o).html() + "</tr>";
                });
                e.tableRows.push(tempRows);
            });


            e.tableToExcel(e.tableRows, e.settings.name);
        },

        tableToExcel: function (table, name) {
            var e = this, fullTemplate = "", i, link, a;

            e.uri = "data:application/vnd.ms-excel;base64,";
            e.base64 = function (s) {
                return window.btoa(unescape(encodeURIComponent(s)));
            };
            e.format = function (s, c) {
                return s.replace(/{(\w+)}/g, function (m, p) {
                    return c[p];
                });
            };
            e.ctx = {
                worksheet: name || "Worksheet",
                table: table
            };

            fullTemplate = e.template.head;

            if ($.isArray(table)) {
                for (i in table) {
                    //fullTemplate += e.template.sheet.head + "{worksheet" + i + "}" + e.template.sheet.tail;
                    fullTemplate += e.template.sheet.head + "Table" + i + "" + e.template.sheet.tail;
                }
            }

            fullTemplate += e.template.mid;

            if ($.isArray(table)) {
                for (i in table) {
                    fullTemplate += e.template.table.head + "{table" + i + "}" + e.template.table.tail;
                }
            }

            fullTemplate += e.template.foot;

            for (i in table) {
                e.ctx["table" + i] = table[i];
            }
            delete e.ctx.table;


            if (typeof msie !== "undefined" && msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
            {
                if (typeof Blob !== "undefined") {
                    //use blobs if we can
                    fullTemplate = [fullTemplate];
                    //convert to array
                    var blob1 = new Blob(fullTemplate, { type: "text/html" });
                    window.navigator.msSaveBlob(blob1, getFileName(e.settings));
                } else {
                    //otherwise use the iframe and save
                    //requires a blank iframe on page called txtArea1
                    txtArea1.document.open("text/html", "replace");
                    txtArea1.document.write(fullTemplate);
                    txtArea1.document.close();
                    txtArea1.focus();
                    sa = txtArea1.document.execCommand("SaveAs", true, getFileName(e.settings));
                }

            } else {
                link = e.uri + e.base64(e.format(fullTemplate, e.ctx));
                a = document.createElement("a");
                a.download = getFileName(e.settings);
                a.href = link;
                a.click();
            }

            return true;

        }
    };

    function getFileName(settings) {
        return (settings.filename ? settings.filename : "table2excel") + ".xls";
    }

    $.fn[pluginName] = function (options) {
        var e = this;
        e.each(function () {
            if (!$.data(e, "plugin_" + pluginName)) {
                $.data(e, "plugin_" + pluginName, new Plugin(this, options));
            }
        });

        // chain jQuery functions
        return e;
    };

})(jQuery, window, document);


$.fn.InitYear = function () {
    var $this = $(this);
    $this.empty();
    for (var i = -5; i <= 0; i++) {
        var year = new Date().getFullYear() + i;
        $("<option>").text(year).val(year).appendTo($this);
    }
    $this.val(new Date().getFullYear());
}