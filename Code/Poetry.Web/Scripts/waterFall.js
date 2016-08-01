/// <reference path="tsd/jquery.d.ts" />
/// <reference path="tsd/jsrender.d.ts" />
/// <reference path="tsd/sail.javascript.d.ts" />
jQuery.fn.extend({
    scrollEnd: function (item, act) {
        var $this = $(this);
        $this.on("scroll", function () {
            var scrollTop = $this.scrollTop();
            var scrollHeight = $(item).height();
            var windowHeight = $this.height();
            if (scrollTop + windowHeight >= scrollHeight - 50) {
                act();
            }
        });
    }
});
var WaterFall = (function () {
    function WaterFall(set) {
        var _this = this;
        this.isHasNextPage = false;
        this.set = $.extend({
            left: ".boxleft",
            right: ".boxright",
            tmplName: "#listTmpl",
            pageSize: 10,
            getPostKey: function () { return {}; }
        }, set);
        this.currentData = null;
        this.currentIndex = 1;
        this.pageCount = 0;
        this.isLoding = false;
        this.$left = $(this.set.left);
        this.$right = $(this.set.right);
        this.tmpl = $.templates(this.set.tmplName);
        $("body > .zqui-content").scrollEnd(".box", function () { _this.Next(); });
    }
    WaterFall.prototype.render = function (data) {
        var _this = this;
        if (this.set.left == this.set.right) {
            $(this.tmpl.render(data)).appendTo(this.$left);
        }
        else {
            $.each(data, function (i, o) {
                var target = _this.$left.height() <= _this.$right.height() ? _this.$left : _this.$right;
                var $news = $(_this.tmpl.render(o));
                $news.appendTo(target);
            });
        }
    };
    WaterFall.prototype.Query = function (pageIndex) {
        var _this = this;
        if (pageIndex)
            this.currentIndex = pageIndex;
        pageIndex = this.currentIndex;
        if (pageIndex === 1) {
            this.$left.empty();
            this.$right.empty();
            $(".content-over").remove();
        }
        var postData = this.set.getPostKey;
        if (typeof (this.set.getPostKey) == "function")
            postData = postData();
        var url = this.set.api;
        if (typeof (url) == "function")
            url = url();
        postData.pageSize = this.set.pageSize;
        postData.pageIndex = pageIndex;
        var $spinner = $(".spinner").show();
        $.ajax({
            type: "get", url: url, data: postData, success: function (data) {
                _this.pageCount = data.Data.PageInfo.PageCount;
                _this.isHasNextPage = _this.pageCount > _this.currentIndex;
                _this.render(data.Data.Data);
                if (_this.pageCount == 0) {
                    if ($("div.content-over").length == 0)
                        $('<div class="content-over" ></div>').insertAfter(_this.$right);
                }
                $spinner.hide();
                _this.isLoding = false;
            }
        });
    };
    WaterFall.prototype.Next = function () {
        if (!this.isLoding) {
            this.isLoding = true;
            if (this.isHasNextPage)
                this.Query(this.currentIndex + 1);
            else {
                if ($("div.content-over").length == 0)
                    $('<div class="content-over" ></div>').insertAfter(this.$right);
            }
        }
    };
    WaterFall.prototype.on = function (target, act) {
        var func = function () {
            var data = $(this).data();
            act(data, this);
            return false;
        };
        this.$left.on("click", target, func);
        this.$right.on("click", target, func);
    };
    return WaterFall;
}());
var DatetimePicker = (function () {
    function DatetimePicker(item, target) {
        var _this = this;
        this.$target = $(target);
        this.$item = $(item);
        this.$year = this.$item.find(".year input");
        this.$month = this.$item.find(".month input");
        this.$day = this.$item.find(".day input");
        var _me = this;
        this.$item.on("click touchend", ".btnClose", function () { _this.$item.hide(); })
            .on("click touchend", ".btnToday", function () {
            _this.$item.hide();
            _this.$target.val(new Date().format("yyyy-MM-dd"));
        })
            .on("click touchend", ".btnOk", function () {
            _this.$item.hide();
            _this.$target.val(_this.getDate().format("yyyy-MM-dd"));
        })
            .on("click touchend", ".year a.plus", function () {
            _this.yearChange(1);
        })
            .on("click touchend", ".year a.minus", function () {
            _this.yearChange(-1);
        })
            .on("click touchend", ".month a.plus", function () {
            _this.monthChange(1);
        })
            .on("click touchend", ".month a.minus", function () {
            _this.monthChange(-1);
        })
            .on("click touchend", ".day a.plus", function () {
            _this.dayChange(1);
        })
            .on("click touchend", ".day a.minus", function () {
            _this.dayChange(-1);
        });
        this.$target.on("click touchend", function () {
            var dateStr = _this.$target.val();
            var date = !dateStr ? new Date() : DateTime.Parse(dateStr);
            _this.rendenDate(date);
            _this.$item.show();
        });
    }
    /**
     *
     * @returns
     */
    DatetimePicker.prototype.maxDate = function () {
        var maxDate = DateTime.DaysCount(this.$year.valToInt(), this.$month.valToInt());
        return maxDate;
    };
    /**
     * 生成日期
     * @returns
     */
    DatetimePicker.prototype.getDate = function () { return new Date(this.$year.valToInt(), this.$month.valToInt() - 1, this.$day.valToInt()); };
    /**
     *
     * @param {type} num
     */
    DatetimePicker.prototype.dayChange = function (num) {
        var day = this.$day.valToInt();
        day += num;
        var maxDate = this.maxDate();
        if (day > maxDate)
            day = 1;
        if (day < 1)
            day = maxDate;
        this.$day.val(day.PadLeft(2, '0'));
        this.rendenDate(this.getDate());
    };
    /**
     *
     * @param {type} num
     */
    DatetimePicker.prototype.yearChange = function (num) {
        var year = this.$year.valToInt();
        year += num;
        console.log(year);
        this.$year.val(year.PadLeft(4, '0'));
        var maxDate = this.maxDate();
        var days = this.$day.valToInt();
        if (days > maxDate)
            this.$day.val(maxDate);
        this.rendenDate(this.getDate());
    };
    /**
     *
     * @param {type} num
     */
    DatetimePicker.prototype.monthChange = function (num) {
        var month = this.$month.valToInt();
        month += num;
        if (month > 12)
            month = 1;
        if (month < 1)
            month = 12;
        this.$month.val(month.PadLeft(2, '0'));
        var maxDate = this.maxDate();
        var days = this.$day.valToInt();
        if (days > maxDate)
            this.$day.val(maxDate);
        this.rendenDate(this.getDate());
    };
    /**
     *
     * @param {type} date
     */
    DatetimePicker.prototype.rendenDate = function (date) {
        this.$year.val(date.format("yyyy"));
        this.$month.val(date.format("MM"));
        this.$day.val(date.format("dd"));
        this.$item.find(".zqui-date-time").text(date.format("yyyy年MM月dd日"));
    };
    return DatetimePicker;
}());
//# sourceMappingURL=waterFall.js.map