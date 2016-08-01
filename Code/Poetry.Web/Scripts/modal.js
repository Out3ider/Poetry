/// <reference path="tsd/jquery.cookie.d.ts" />
/// <reference path="tsd/jquery.d.ts" />
/// <reference path="tsd/jsrender.d.ts" />
/// <reference path="tsd/sail.javascript.d.ts" />
$.fn.ModalSelect = function (set) {
    var selecter = new Sail.ModalSelect($(this), set);
    $(this).data("selecter", selecter);
    return selecter;
};
var Sail;
(function (Sail) {
    var ModalSelect = (function () {
        function ModalSelect(target, set) {
            var _this = this;
            this.title = set.title;
            this.api = new Sail.ApiHelper(set.api);
            this.target = $(target);
            var tool = this;
            tool.setting = $.extend({
                defaultId: "00000000-0000-0000-0000-000000000000",
                resultTarget: ".select-result[data-target=" + this.target.data("target") + "-result]",
                resultTmpl: "#" + this.target.data("target") + "ResultTmpl",
                modalTmpl: "#SearcModalTmpl",
                isAllowClear: true,
                onSelect: function () { },
                onClear: function () { },
            }, set);
            this.$Text = tool.target.find('input[type=text]');
            this.$Value = tool.target.find('input[type=hidden]');
            this.$btnClear = tool.target.find("a.clear-input");
            this.modal = new Sail.Modal(tool.setting.id, {
                title: tool.setting.title,
                tmplName: tool.setting.modalTmpl,
                cssClass: "modal-lg",
                okTitle: "",
                cancelTitle: "关闭",
                init: function (modal) {
                    tool.setting.pageSet = $.extend({
                        handleName: function () { return tool.api.GetApi("Query"); },
                        bodyContainer: modal.find("table tbody"),
                        headContainer: modal.find("table thead"),
                        footContainer: modal.find(".table-pages"),
                        reQueryHandle: modal.find(".btnQuery"),
                        getPostKey: function () { return modal.find(".table-toolbar").GetJson(); }
                    }, tool.setting.pageSet);
                    tool.page = new Sail.Pagination(tool.setting.pageSet);
                    modal.find(".btnQuery").click(function () {
                        tool.page.Query(1);
                    });
                    modal.find(".btnReset").click(function () {
                        modal.find(".table-toolbar").ResetForm();
                        tool.page.Query(1);
                    });
                    if (tool.setting.href)
                        modal.find("a.btnAdd").on("click", function () {
                            if (typeof (tool.setting.href) == "function")
                                window.open(tool.setting.href());
                            else
                                window.open(tool.setting.href);
                        });
                    else
                        modal.find("a.btnAdd").hide();
                    $(tool.setting.pageSet.bodyContainer).on("click", ".btnSelect", function () {
                        var data = $.view(this).data;
                        tool.$Value.val(data[tool.cfg.id]).trigger("change");
                        tool.$Text.val(data[tool.cfg.name]);
                        tool.setting.onSelect(data, this, tool);
                        tool.setData(data);
                        tool.modal.Hide();
                    });
                }
            });
            this.cfg = $.extend({ id: "Id", name: "Name" }, this.target.data());
            this.target.on("click", "button,a.button", function () {
                tool.query();
            });
            this.$btnClear.click(function () {
                _this.$Value.val(_this.setting.defaultId).trigger("change");
                _this.$Text.val("");
                tool.setting.onClear(tool.target);
                tool.setData(null);
            });
            this.$Value.change(function () {
                if ($(this).val() && tool.setting.isAllowClear)
                    tool.$btnClear.show();
                else
                    tool.$btnClear.hide();
            });
        }
        /**
         * 查询并弹窗
         */
        ModalSelect.prototype.query = function () {
            this.modal.modal.find(".btnReset").click();
            this.modal.Show();
        };
        ModalSelect.prototype.setData = function (data) {
            $(this.setting.resultTarget).empty();
            if (data) {
                this.$Value.val(data[this.cfg.id]);
                this.$Text.val(data[this.cfg.name]);
                console.log(data);
                if (data[this.cfg.name])
                    $(this.setting.resultTarget).Render(this.setting.resultTmpl, data);
            }
        };
        return ModalSelect;
    }());
    Sail.ModalSelect = ModalSelect;
})(Sail || (Sail = {}));
//# sourceMappingURL=modal.js.map