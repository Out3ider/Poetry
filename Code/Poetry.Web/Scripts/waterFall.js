/// <reference path="tsd/jquery.d.ts" />
/// <reference path="tsd/jsrender.d.ts" />
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
        this.$left = $(this.set.left);
        this.$right = $(this.set.right);
        this.tmpl = $.templates(this.set.tmplName);
        $("body > .zqui-content").scrollEnd(".box", function () { _this.Next(); });
        this.Query(1);
    }
    WaterFall.prototype.render = function (data) {
        var _this = this;
        $.each(data, function (i, o) {
            var target = _this.$left.height() >= _this.$right.height() ? _this.$right : _this.$left;
            var $news = $(_this.tmpl.render(o));
            $news.appendTo(target);
        });
    };
    WaterFall.prototype.Query = function (pageIndex) {
        var _this = this;
        if (pageIndex)
            this.currentIndex = pageIndex;
        pageIndex = this.currentIndex;
        if (pageIndex === 1) {
            this.$left.empty();
            this.$right.empty();
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
                $spinner.hide();
            }
        });
    };
    WaterFall.prototype.Next = function () {
        if (this.isHasNextPage)
            this.Query(this.currentIndex + 1);
        else {
            if ($("div.content-over").length == 0)
                $('<div class="content-over" ></div>').insertAfter(this.$right);
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
//# sourceMappingURL=waterFall.js.map