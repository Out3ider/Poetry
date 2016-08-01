/// <reference path="tsd/jquery.cookie.d.ts" />
/// <reference path="tsd/jquery.d.ts" />
/// <reference path="tsd/jsrender.d.ts" />
/// <reference path="tsd/sail.javascript.d.ts" />

interface JQuery {
    ModalSelect(set: Sail.IModalSelectSetting): Sail.Modal;
}

$.fn.ModalSelect = function (set: Sail.IModalSelectSetting) {
    var selecter = new Sail.ModalSelect($(this), set);
    $(this).data("selecter", selecter);
    return selecter;
};

module Sail {
    export interface IModalSelectSetting {
        defaultId: string,
        id: string;
        resultTarget: string;
        resultTmpl: string;
        title: string;
        modalTmpl: string;
        api: string;
        href: any;
        pageSet: PaginationSetting;
        onSelect: Function;
        onClear: Function;
        isAllowClear: boolean;
    }

    export interface ITargetCfg {
        id: string;
        name: string;
        idtarget: string;
        nametarget: string;
    }

    export class ModalSelect {
        public modal: Modal;
        public page: Pagination;
        private api: ApiHelper;
        private title: string;
        private target: JQuery;
        private $Text: JQuery;
        private $Value: JQuery;
        private $btnClear: JQuery;

        private cfg: ITargetCfg;
        private setting: IModalSelectSetting;
        constructor(target: any, set: IModalSelectSetting) {

            this.title = set.title;
            this.api = new ApiHelper(set.api);
            this.target = $(target);
            var tool = this;
            tool.setting = $.extend({
                defaultId: "00000000-0000-0000-0000-000000000000",
                resultTarget: ".select-result[data-target=" + this.target.data("target") + "-result]",
                resultTmpl: "#" + this.target.data("target") + "ResultTmpl",
                modalTmpl: "#SearcModalTmpl",
                isAllowClear: true,
                onSelect() { },
                onClear() { },
            }, set);

            this.$Text = tool.target.find('input[type=text]');
            this.$Value = tool.target.find('input[type=hidden]');
            this.$btnClear = tool.target.find("a.clear-input");


            this.modal = new Modal(tool.setting.id, {
                title: tool.setting.title,
                tmplName: tool.setting.modalTmpl,
                cssClass: "modal-lg",
                okTitle: "",
                cancelTitle: "关闭",
                init(modal: JQuery) {
                    tool.setting.pageSet = $.extend({
                        handleName() { return tool.api.GetApi("Query") },
                        bodyContainer: modal.find("table tbody"),
                        headContainer: modal.find("table thead"),
                        footContainer: modal.find(".table-pages"),
                        reQueryHandle: modal.find(".btnQuery"),
                        getPostKey() { return modal.find(".table-toolbar").GetJson() }
                    }, tool.setting.pageSet);

                    tool.page = new Pagination(tool.setting.pageSet);


                    modal.find(".btnQuery").click(() => {
                        tool.page.Query(1);
                    });

                    modal.find(".btnReset").click(() => {
                        modal.find(".table-toolbar").ResetForm();
                        tool.page.Query(1);
                    });

                    if (tool.setting.href)
                        modal.find("a.btnAdd").on("click", () => {
                            if (typeof (tool.setting.href) == "function")
                                window.open(tool.setting.href());
                            else
                                window.open(tool.setting.href);
                        });
                    else modal.find("a.btnAdd").hide();

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

            this.target.on("click", "button,a.button", () => {
                tool.query();
            });

            this.$btnClear.click(() => {
                this.$Value.val(this.setting.defaultId).trigger("change");
                this.$Text.val("");
                tool.setting.onClear(tool.target);
                tool.setData(null);

            });

            this.$Value.change(function () {
                if ($(this).val() && tool.setting.isAllowClear) tool.$btnClear.show();
                else tool.$btnClear.hide();
            });
        }

        /**
         * 查询并弹窗
         */
        public query() {
            this.modal.modal.find(".btnReset").click();
            this.modal.Show();
        }

        public setData(data) {
            $(this.setting.resultTarget).empty();

            if (data) {
                this.$Value.val(data[this.cfg.id]);
                this.$Text.val(data[this.cfg.name]);
                console.log(data);
                if (data[this.cfg.name])
                    $(this.setting.resultTarget).Render(this.setting.resultTmpl, data);
            }
        }
    }


}

