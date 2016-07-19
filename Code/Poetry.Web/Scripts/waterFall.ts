/// <reference path="tsd/jquery.d.ts" />
/// <reference path="tsd/jsrender.d.ts" />


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

        this.$left = $(this.set.left);
        this.$right = $(this.set.right);
        this.tmpl = $.templates(this.set.tmplName);
        $("body > .zqui-content").scrollEnd(".box", () => { this.Next(); });
        this.Query(1);
    }





    private render(data: any) {
        $.each(data, (i, o) => {
            var target = this.$left.height() >= this.$right.height() ? this.$right : this.$left;
            var $news = $(this.tmpl.render(o));
            $news.appendTo(target);
        });
    }


    private Query(pageIndex: number) {
        if (pageIndex) this.currentIndex = pageIndex;
        pageIndex = this.currentIndex;
        if (pageIndex === 1) {
            this.$left.empty();
            this.$right.empty();
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
                $spinner.hide();
            }
        });
    }

    public Next() {
        if (this.isHasNextPage)
            this.Query(this.currentIndex + 1);
        else {
            if ($("div.content-over").length == 0)
                $('<div class="content-over" ></div>').insertAfter(this.$right);
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