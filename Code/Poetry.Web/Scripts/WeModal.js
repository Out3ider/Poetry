/// <reference path="tsd/jquery.cookie.d.ts" />
/// <reference path="tsd/jquery.d.ts" />
/// <reference path="tsd/jsrender.d.ts" />
/// <reference path="tsd/sail.javascript.d.ts" />
var WeChat;
(function (WeChat) {
    var modalTmpl = {
        dialog: '<div class="zqui-modal">' +
            '<div class="zqui-modal-mask"></div>' +
            '<div class="zqui-modal-dialog distance-two zqui-modal-action">' +
            '<ul class="zqui-table-view buttons"></ul>' +
            ' <ul class="zqui-table-view">' +
            '<li class="zqui-table-view-cell"><a class="btnClose"><b>取消</b></a></li>' +
            '</ul>' +
            '</div>' +
            '</div>',
        Button: '<li class="zqui-table-view-cell"><a class="btnAction"></a></li>'
    };
    var SheetAction = (function () {
        function SheetAction(btns) {
            var _this = this;
            this.$modal = $(modalTmpl.dialog);
            btns.forEach(function (x) {
                _this.addButton(x);
            });
            this.$modal.on("click", ".btnClose", function () { _this.$modal.hide(); });
            this.$modal.hide();
        }
        SheetAction.prototype.addButton = function (btn) {
            var $buttons = this.$modal.find(".buttons");
            var $btn = $(modalTmpl.Button).appendTo($buttons);
            $btn.find("a").text(btn.title).addClass(btn.cssClass);
            if (btn.act) {
                $btn.click(btn.act);
            }
        };
        return SheetAction;
    }());
    WeChat.SheetAction = SheetAction;
})(WeChat || (WeChat = {}));
$(function () {
    $.views.converters({
        Pre: function (str) {
            if (!str)
                return $.SailConfig.Root + "content/images/default.jpg";
            if (str.indexOf("/uploads/") <= 0)
                return str;
            var index = str.lastIndexOf("/");
            if (index < 0)
                return str;
            return str.substr(0, index) + "/Pre" + str.substr(index);
        },
        MidPre: function (str) {
            if (!str || str.indexOf("/uploads/") <= 0)
                return str;
            var index = str.lastIndexOf("/");
            if (index < 0)
                return str;
            return str.substr(0, index) + "/MediumPre" + str.substr(index);
        }
    });
    jQuery.each(jQuery('textarea[data-autoresize]'), function () {
        var offset = this.offsetHeight - this.clientHeight;
        var resizeTextarea = function (el) {
            jQuery(el).css('height', 'auto').css('height', el.scrollHeight + offset);
        };
        jQuery(this).on('keyup input', function () { resizeTextarea(this); }).removeAttr('data-autoresize');
    });
});
function ActOrLogin(isLogined, act) {
    var $this = $("#divLogin");
    if (!isLogined)
        $this.show();
    else if (act && typeof act == "function")
        act();
    var loginApi = new Sail.ApiHelper("wxuser");
    $this.off("click", "#btnLogin").on("click", "#btnLogin", function () {
        $.post(loginApi.GetApi("login"), { "": JSON.stringify($this.GetJson()) }, function (result) {
            ShowMessage(result, "登录成功", function () {
                $this.hide();
                if (act && typeof act == "function")
                    act();
            });
        });
    }).off("click", ".btnClose").on("click", ".btnClose", function () { $("#divLogin").hide(); });
}
//# sourceMappingURL=WeModal.js.map