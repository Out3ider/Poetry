

function PageLoad(title, pageTmpl) {

    this.Title = title;
    this.page = undefined;
    this.modal = undefined;
    var tool = this;

    //加载主模板页面
    if (!pageTmpl) pageTmpl = "WeGenera";

    this.content = $($.LoadHtml(pageTmpl)).appendTo("body");
    this.content.find(".title").eq(0).html(title);

    this.$form = $('#content');
    if (this.$form.length > 0) {
        this.$form.html($("#modalTmpl").html());
    }

    this.$form.initForm();
};


(function ($) {

    $.fn.ClickOrTouch = function (target, act) {
        var clickOrTouch = (('ontouchend' in window)) ? 'touchend' : 'click';
        $(this).on(clickOrTouch, target, act);
        return $(this);
    }

    jQuery.fn.makeCalendar = function (settings) {
        var now = new Date();
        var config = {
            Year: now.getFullYear(),
            Month: now.getMonth() + 1,
            onRender: null
        };
        var $this = $(this);
        if (settings) jQuery.extend(config, settings);
        var $continer = $('<div class="calendar"></div>');

        $('<ul class="date"><li><a class="btnLeft"><i class="icon iconfont icon-jiantouzuo greenFont"></i></a><p>月</p>' +
                    '<a class="btnRight"><i class="icon iconfont icon-jiantouyou greenFont"></i></a></li></ul>').appendTo($continer);
        $('<table border="0" cellspacing="0" cellpadding="0">' +
                '<thead><tr><th>日</th><th>一</th><th>二</th><th>三</th><th>四</th><th>五</th><th>六</th></tr></thead>' +
                '<tbody></tbody></table> ').appendTo($continer);

        render();
        $continer.on("click", ".btnLeft", function () {
            config.Month--;
            if (config.Month < 0) {
                config.Month = 12;
                config.Year--;
            }
            render();
        }).on("click", ".btnRight", function () {
            config.Month++;
            if (config.Month > 12) {
                config.Month = 1;
                config.Year++;
            }
            render();
        })

        $this.html($continer);

        return this;

        function render() {
            $this.data("year", config.Year);
            $this.data("month", config.Month);

            $continer.find(".date p").html($.templates("{{>Year}}年{{>Month}}月").render(config));
            var weeks = getWeeks(config.Year, config.Month);
            var days = new Date(config.Year, config.Month - 1, 1).MonthDaysCount();
            var starts = new Date(config.Year, config.Month - 1, 1).getDay();
            $continer.find("table tbody").html("");
            for (var i = 0; i < weeks; i++) {
                var $tr = $("<tr></tr>").appendTo($continer.find("table tbody"));
                for (var j = 0; j < 7; j++) {
                    $("<td><p></p></td>").appendTo($tr);
                }
            }
            for (var i = 1; i <= days ; i++) {
                $continer.find("tbody td").eq(starts + i - 1).html("<p>" + i + "</p>").data("day", i).addClass("day day" + i);
            }
            if (config.onRender) {
                config.onRender(config.Year, config.Month);
            }
        };

        function getWeeks(y, m) {
            var fd = new Date(y, m - 1, 1)
            var fday = fd.getDay();
            var fwd = 7 - fday;
            var weeks = 1;
            fd.setDate(fwd);
            while (fd <= new Date(y, m, 1)) {
                fd.setDate(fd.getDate() + 7);
                if (fd.getMonth() != m || (fd.getMonth() == m && fd.getDate() < 7)) weeks++;
            }
            return weeks;
        }
    };
})(jQuery);



(function (doc, win) {
    var docEl = doc.documentElement,
      resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize',
      recalc = function () {
          var clientWidth = docEl.clientWidth;
          if (!clientWidth) return;
          docEl.style.fontSize = 20 * (clientWidth / 320) + 'px';
      };

    if (!doc.addEventListener) return;
    win.addEventListener(resizeEvt, recalc, false);
    doc.addEventListener('DOMContentLoaded', recalc, false);
})(document, window);


var moreTmpl = '<div class="last showMore"><div class="more">点击显示更多</div><div class="clearfix"></div></div>';
var noneTmpl = '<div class="last"><div class="data"></div><div class="noMore">没有更多了</div><div class="clearfix"></div></div>';

function Nodata() {
    $("#pager").Render("NoOrder");
}

$.fn.modal = $.fn.myModal;

jQuery.fn.extend({
    LoadMore: function (set) {
        set = jQuery.extend({
            noData: "暂无数据",
            queryed: null, //查询完成后的回调
            pageSize: 3,//每页行数   
            Type: 1,
            handleName: null,//handle  
            events: [],//事件集合
            getPostKey: function () { return {}; }//获取提交数据的方法 
        }, set);

        var result = {
            pager: $("#pager"),
            $list: $(this),
            currentData: null,
            currentIndex: 1,
            settings: set,
            Query: function (pageIndex) {
                result.pager.html("");
                if (pageIndex) result.currentIndex = pageIndex;
                pageIndex = result.currentIndex;

                if (pageIndex == 1) result.$list.html("");

                var settings = result.settings;
                var postData = settings.getPostKey();
                postData["pageSize"] = settings.pageSize;
                postData["pageIndex"] = pageIndex;
                var url = settings.handleName;
                if (typeof url == "function") url = url();
                $.ajax({
                    url: url,
                    data: postData,
                    success: function (data) {

                        if (data.Data == null) {
                            Nodata();
                            return;
                        }
                        if (data.Data.Count == 0) {
                            Nodata();
                            return;
                        }
                        result.$list.append($.Loadtmpl("#listTmpl").render(data.Data.Data));

                        var pageCount = data.Data.PageInfo.PageCount;
                        result.pager.html(pageCount > result.currentIndex ? moreTmpl : noneTmpl);
                    }
                });
                return result;
            }
        };
        result.pager.on("click", ".showMore", function () {
            result.Query(result.currentIndex + 1);
        });
        return result;
    },

    BindList: function (listTmpl, data, msgtext, noTmpl) {  //绑定列表并在没数据的时候显示暂无数据
        $(this).parent().find(".noResult").remove();
        if (!noTmpl) noTmpl = "NoResult";
        $(this).Link(listTmpl, data);

        if (!data || data.length == 0) {

            var noresult = $("<div class='noResult'></div>");
            console.log(noTmpl);
            noresult.Render(noTmpl);

            if (msgtext) noresult.find(".massage").text(msgtext);
            noresult.insertAfter(this);
        }
    },
});


function scanCode(act) {

    wx.error(function (res) {
        alert("微信初始化错误，请重新打开页面")
    });
    wx.scanQRCode({
        needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
        scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
        success: function (res) {

            var cardCode = "";
            if (res.resultStr.indexOf(",") > 0) {
                cardCode = res.resultStr.split(",")[1];
            } else {
                cardCode = res.resultStr;
            }
            act(cardCode);
        }
    });
}


