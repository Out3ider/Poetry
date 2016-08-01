/// <reference path="tsd/jquery.d.ts" />
/// <reference path="tsd/jsrender.d.ts" />
/// <reference path="tsd/sail.javascript.d.ts" />



interface IWaterFallSet {
    left: string,
    right: string,
    tmplName: string,
    api: Function,
    pageSize: number,
    getPostKey: Function,

}

interface JQuery {
    scrollEnd(...args: any[]): JQuery;
}

jQuery.fn.extend({
    scrollEnd: function (item, act) {
        var $this = $(this);
        $this.on("scroll", () => {
            var scrollTop = $this.scrollTop();
            var scrollHeight = $(item).height();
            var windowHeight = $this.height();
            if (scrollTop + windowHeight >= scrollHeight - 50) {
                act();
            }
        });
    }
});

class WaterFall {
    private $left: JQuery;
    private $right: JQuery;
    private tmpl: JsRender.Template;
    private set: IWaterFallSet;

    private currentData: any;
    private currentIndex: number;
    private pageCount: number;
    private isHasNextPage: boolean;
    private isLoding: boolean;
    public constructor(set: IWaterFallSet) {
        this.isHasNextPage = false;
        this.set = $.extend({
            left: ".boxleft",
            right: ".boxright",
            tmplName: "#listTmpl",
            pageSize: 10,
            getPostKey() { return {} }
        }, set);
        this.currentData = null;
        this.currentIndex = 1;
        this.pageCount = 0;
        this.isLoding = false;
        this.$left = $(this.set.left);
        this.$right = $(this.set.right);
        this.tmpl = $.templates(this.set.tmplName);
        $("body > .zqui-content").scrollEnd(".box", () => { this.Next(); });

    }





    private render(data: any) {
        if (this.set.left == this.set.right) {
            $(this.tmpl.render(data)).appendTo(this.$left);
        }
        else {
            $.each(data, (i, o) => {
                var target = this.$left.height() <= this.$right.height() ? this.$left : this.$right;
                var $news = $(this.tmpl.render(o));
                $news.appendTo(target);
            });
        }

    }


    private Query(pageIndex: number) {
        if (pageIndex) this.currentIndex = pageIndex;
        pageIndex = this.currentIndex;
        if (pageIndex === 1) {
            this.$left.empty();
            this.$right.empty();
            $(".content-over").remove();
        }

        var postData: any = this.set.getPostKey;
        if (typeof (this.set.getPostKey) == "function") postData = postData();
        var url: any = this.set.api;
        if (typeof (url) == "function") url = url();

        postData.pageSize = this.set.pageSize;
        postData.pageIndex = pageIndex;
        var $spinner = $(".spinner").show();
        $.ajax({
            type: "get", url: url, data: postData, success: (data: any) => {
                this.pageCount = data.Data.PageInfo.PageCount;
                this.isHasNextPage = this.pageCount > this.currentIndex;
                this.render(data.Data.Data);
                if (this.pageCount == 0) {
                    if ($("div.content-over").length == 0)
                        $('<div class="content-over" ></div>').insertAfter(this.$right);
                }
                $spinner.hide();
                this.isLoding = false;
            }
        });
    }

    public Next() {
        if (!this.isLoding) {
            this.isLoding = true;
            if (this.isHasNextPage)
                this.Query(this.currentIndex + 1);
            else {
                if ($("div.content-over").length == 0)
                    $('<div class="content-over" ></div>').insertAfter(this.$right);
            }
        }
    }

    public on(target: string, act: Function) {
        var func = function () {
            var data = $(this).data();
            act(data, this);
            return false;
        }

        this.$left.on("click", target, func);
        this.$right.on("click", target, func);
    }

}

class DatetimePicker {
    public $target: JQuery;
    public $item: JQuery;

    private $year: JQuery;
    private $month: JQuery;
    private $day: JQuery;

    public constructor(item: string, target: string) {
        this.$target = $(target);
        this.$item = $(item);
        this.$year = this.$item.find(".year input");
        this.$month = this.$item.find(".month input");
        this.$day = this.$item.find(".day input");

        var _me = this;
        this.$item.on("click touchend", ".btnClose", () => { this.$item.hide(); })
            .on("click touchend", ".btnToday", () => {
                this.$item.hide();
                this.$target.val(new Date().format("yyyy-MM-dd"));
            })
            .on("click touchend", ".btnOk", () => {
                this.$item.hide();
                this.$target.val(this.getDate().format("yyyy-MM-dd"));
            })
            .on("click touchend", ".year a.plus", () => {
                this.yearChange(1);
            })
            .on("click touchend", ".year a.minus", () => {
                this.yearChange(-1);
            })
            .on("click touchend", ".month a.plus", () => {
                this.monthChange(1);
            })
            .on("click touchend", ".month a.minus", () => {
                this.monthChange(-1);
            })
            .on("click touchend", ".day a.plus", () => {
                this.dayChange(1);
            })
            .on("click touchend", ".day a.minus", () => {
                this.dayChange(-1);
            });

        this.$target.on("click touchend", () => {
            var dateStr = this.$target.val();
            var date = !dateStr ? new Date() : DateTime.Parse(dateStr);
            this.rendenDate(date);
            this.$item.show();
        });
    }

    /**
     * 
     * @returns
     */
    maxDate() {
        var maxDate = DateTime.DaysCount(this.$year.valToInt(), this.$month.valToInt());
        return maxDate;
    }

    /**
     * 生成日期
     * @returns
     */
    getDate(): Date { return new Date(this.$year.valToInt(), this.$month.valToInt() - 1, this.$day.valToInt()); }

    /**
     * 
     * @param {type} num
     */
    dayChange(num) {
        var day = this.$day.valToInt();
        day += num;
        var maxDate = this.maxDate();
        if (day > maxDate) day = 1;
        if (day < 1) day = maxDate;
        this.$day.val(day.PadLeft(2, '0'));
        this.rendenDate(this.getDate());
    }

    /**
     * 
     * @param {type} num
     */
    yearChange(num) {

        var year = this.$year.valToInt();
        year += num;
        console.log(year);
        this.$year.val(year.PadLeft(4, '0'));

        var maxDate = this.maxDate();
        var days = this.$day.valToInt();
        if (days > maxDate) this.$day.val(maxDate);

        this.rendenDate(this.getDate());
    }

    /**
     * 
     * @param {type} num
     */
    monthChange(num) {

        var month = this.$month.valToInt();
        month += num;
        if (month > 12) month = 1;
        if (month < 1) month = 12;
        this.$month.val(month.PadLeft(2, '0'));
        var maxDate = this.maxDate();
        var days = this.$day.valToInt();
        if (days > maxDate) this.$day.val(maxDate);


        this.rendenDate(this.getDate());
    }

    /**
     * 
     * @param {type} date
     */
    rendenDate(date: Date) {
        this.$year.val(date.format("yyyy"));
        this.$month.val(date.format("MM"));
        this.$day.val(date.format("dd"));
        this.$item.find(".zqui-date-time").text(date.format("yyyy年MM月dd日"));
    }
}