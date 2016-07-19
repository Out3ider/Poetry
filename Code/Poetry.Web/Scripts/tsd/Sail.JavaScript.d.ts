/// <reference path="jquery.cookie.d.ts" />
/// <reference path="jquery.d.ts" />
/// <reference path="jsrender.d.ts" />
/// <reference path="underscore.d.ts" />

interface Template {
    link(...argv: any[]): any;
    render(...argv:any[]):any;
}
interface JQuery {
    Province(): JQuery;
    SetProvince(): JQuery;
    DateSwitch(): JQuery;
    datetimepicker(...args: any[]): JQuery;
    bootstrapSwitch(...args: any[]): JQuery;
    tooltip(): JQuery;
    modal(...args: any[]): JQuery;
    validationEngine(param: any): boolean;
    select2(...args: any[]): JQuery;
    ajaxAddOptions(...args: any[]): JQuery;
    /**
     * 声明点击事件(兼容iphone版本)
     * @param {type} target
     * @param {type} act
     */
    ClickOrTouch(target: string, act: Function): JQuery;
    /**
     * 表单初始化
     */
    initForm(): JQuery;
    /**
     * 重置表单
     */
    ResetForm(): JQuery;
    /**
     * 根据标题生成表头
     * @param {type} heads 标题数组
     */
    MakeHead(heads: Array<string>, widths?: Array<number>): JQuery;
    /**
     * 初始化编辑器
     */
    initEditor(): JQuery;
    /**
     *重置编辑器
     */
    resetEditor(): JQuery;
    /**
     * 设置对象值到编辑器
     * @param {Object} model
     */
    setToEditor(model: Object): JQuery;
    /**
     * 从编辑器取值到对象
     * @param {Object} model
     */
    editorToModel(model: Object): JQuery;
    /**
     * 定义日期插件
     */
    SetDate(): JQuery;
    /**
     * 定义时间插件
     */
    SetDateTime(): JQuery;
    /**
     * 给select2控件赋值
     * @param {string} value
     */
    ValSelect2(value: string): JQuery;
    /**
     * 获取html标签名称
     */
    tagName(): string;
    /**
     * 获取json对象
     */
    GetJson(): Object;
    /**
     * 关闭自定义的弹窗
     */
    closeModal(): JQuery;
    /**
     * 兼容bootstrap的自定义弹窗(或许在微信端用)
     * @param {string} o? 如果为'hide'则隐藏弹窗，否则显示弹窗
     */
    myModal(o?: string): JQuery;
    /**
     * 获得双向绑定工具
     * @param {type} o?
     */
    ItemBinder(o?: Object): Sail.Binder;
    /**
     * 表单验证
     */
    CheckValidation(): boolean;
    /**
     * 回车触发按钮
     * @param {any} btn
     */
    EnterToClick(btn: any): JQuery;
    /**
     * 给表格中空白td增加一个&nbsp;
     */
    TableChange(): JQuery;
    /**
     * 读取远程模板文件
     */
    loadRemoteTmpl(): JQuery;
    /**
     * 根据模板和数据输出html
     * @param {string} tmpl
     * @param {any} data
     */
    Render(tmpl: string, data: any): JQuery;
    /**
     * 根据模板和数据绑定html
     * @param {string} tmpl
     * @param {any} data
     */
    Link(tmpl: string, data: any): JQuery;
    /**
     * 表单对象的val转换为int 如果转换失败则为0
     */
    valToInt(): number;
    /**
     * 把表单对象的val转换为float 如果转换失败则为0
     */
    valToFloat(): number;
    /**
     * 设置元素显示或隐藏
     * @param {type} isshow
     */
    Display(isshow: boolean): JQuery;
    /**
     * 给table增加合计行 根据thead的第一行th数量生成对应的td数量
     * @param {type} cols 需合计的列
     * @param {type} title 合计行的名称，默认为:合计
     * @param {type} skipStep 每隔n行计算一次，默认为每行都计算
     */
    AddSum(cols: Array<any>, title?: string, skipStep?: number): JQuery;
}
declare function WdatePicker(param: any): any;
declare module Sail {
}

interface SailConfig {
    Root: string;
    ApiRoot: string;
    PageContent: string;
}
interface JQueryBase64Static {
    encode(data: string, isUTF8?: boolean): string;
    decode(data: string, isUTF8?: boolean): string;
}
interface JQueryStatic {
    SailConfig: SailConfig;
    TemplatesRoot: string;
    view(obj: any): any;
    base64: JQueryBase64Static;
    growl(text: string, obj: any): JQuery;
    /**
     * 获取url参数
     * @param {type} name
     */
    Request(name: string): string;
    /**
     * 把url的参数序列化成对象
     */
    decodeParam(): Object;
    /**
     * 把对象编码成字符串
     * @param {Object} obj
     */
    encodeObject(obj: Object): string;
    /**
     * 把编码后的字符串还原成对象
     * @param {string} str
     */
    decodeObject(str: string): Object;
    /**
     * 双向绑定工具
     * @param container 待绑定的容器
     * @param {Object} objItem 要绑定的默认数据
     */
    ItemBinder(container: any, objItem: Object): Sail.Binder;
    /**
     * ajax delete方法
     * @param {type} url 地址
     * @param {type} data? 要提交的数据
     * @param {type} act? 成功后执行的方法
     */
    Delete(url: string, data?: any, act?: any): JQueryXHR;
    /**
     *  ajax put方法
     * @param {type} url 地址
     * @param {type} data? 要提交的数据
     * @param {type} act? 成功后执行的方法
     */
    Put(url: string, data?: any, act?: any): JQueryXHR;
    /**
     * 读取html文本
     * @param {string} tname
     */
    LoadHtml(tname: string): string;
    /**
     * 读取模板
     * @param {string} tname
     */
    Loadtmpl(tname: string): JsRender.Template;
    /**
     * 创建分页控件
     * @param {type} set
     */
    Pagination(set: Sail.PaginationSetting): Sail.Pagination;
    /**
     * 读取后台序列化并转换成json的对象
     * @param {type} obj
     */
    LoadParam(obj: JQuery): JSON;
    /**
     * 创建弹窗
     * @param {string} id
     * @param {Sail.ModalSetting} set
     */
    CreateModal(id: string, set: Sail.ModalSetting): Sail.Modal;
}
declare namespace Sail {
    interface AjaxResult {
        IsSuccess: Boolean;
        Data: any;
        Msg: string;
    }
    function BindSelect(obj: Object): void;
}

interface Object {
    [key: string]: any;
}
interface Date {
    /**
     * 获取日期属于周几
     */
    dayofWeek(): string;
    /**
     * 格式化日期字符串
     * @param {string} format 格式化字符串 同c#的格式
     */
    format(format: string): string;
    /**
     * 获取月头第一天日期
     */
    MonthFirst(): Date;
    /**
     * 获取约莫最后一天
     */
    MonthEnd(): Date;
    /**
     * 获取本周第一天(周一)
     */
    WeekFirst(): Date;
    /**
     * 获取本周最后一天(周日)
     */
    WeekEnd(): Date;
    /**
     * 增加n秒
     * @param sec
     */
    AddSeconds(seconds: number): Date;
    /**
     * 增加n分钟
     * @param {number} minutes
     */
    AddMinutes(minutes: number): Date;
    /**
     * 增加n个小时(可以为负数)
     * @param {number} hour
     */
    AddHours(hour: number): Date;
    /**
     * 增加n天
     * @param {number} days
     */
    AddDays(days: number): Date;
    /**
     * 增加n月
     * @param {number} month
     */
    AddMonths(month: number): Date;
    /**
     * 增加n年
     * @param {number} years
     */
    AddYears(years: number): Date;
}
declare namespace DateTime {
    /**
     * 把字符串解析成日期时间格式
     * @param {string} str
     * @returns {Date} 时间
     */
    function Parse(str: string): Date;
    /**
     * 判断指定年是否为闰年
     * @param {number} year
     * @returns
     */
    function IsLeapYear(year: number): Boolean;
    /**
     * 获取某年或某月的总天数
     * @param {number} year 年份
     * @param {number} month? 月份
     * @return {number} 天数
     */
    function DaysCount(year: number, month?: number): number;
    /**
     * 返回某年每个月的天数
     * @param {number} year
     */
    function MonthDaysList(year: number): Array<Number>;
}

/// <reference path="../../../../../../Program Files (x86)/Microsoft Visual Studio 14.0/Common7/IDE/CommonExtensions/Microsoft/TypeScript/lib.d.ts" />
interface JQuery {
    /**
    * 获取控件类型
    * @param control
    * @returns
    */
    GetControlType(): Sail.ControlType;
    SetValue(value: any): JQuery;
    GetValue(type: string): JQuery;
}
declare module Sail {
    enum ControlType {
        Val = 0,
        Checked = 1,
        Src = 2,
        Html = 3,
    }
    class Binder {
        private item;
        CurrentItem: Object;
        /**
         * 构造函数
         * @param {any} container 容器
         * @param {Object} objItem 默认对象 可为空
         */
        constructor(container: any, objItem?: Object);
        /**
         * 获取对象的所有属性
         * @param {any} obj
         * @returns
         */
        GetProperties(obj: Object): any[];
        /**
         * 对象赋值到html
         * @param {Object} model
         */
        private ToHtml(model);
        SetObject(obj: Object): void;
    }
}

declare namespace Sail {
    interface ModalSetting {
        title: string;
        modalHandle?: string;
        tmplName?: string;
        addEvent?: any;
        okEvent?: any;
        init?: any;
        hideEvent?: any;
        cssClass?: string;
        okTitle?: string;
        cancelTitle?: string;
    }
    var ModalTemplates: {
        dialog: string;
        header: string;
        footer: string;
        cancleButton: string;
        okButton: string;
    };
    /**
     * 弹窗插件
     */
    class Modal {
        modal: JQuery;
        /**
         * 显示弹窗
         */
        Show(): void;
        /**
         * 隐藏弹窗
         */
        Hide(): void;
        /**
         * 修改弹窗标题
         * @param {string} title
         */
        Title(title: string): void;
        private head;
        /**
         * 初始化
         * @param {string} id
         * @param {any} settings
         */
        constructor(id: string, settings: ModalSetting);
    }
}

/**
 * 提示浮动层
 * @param {type} text
 */
declare module MsgBox {
    /**
     * 显示错误提示 (红色)
     * @param {type} text
     * @returns
     */
    function Error(text: string): JQuery;
    /**
     * 显示信息提示 (蓝色)
     * @param {type} text
     * @returns
     */
    function Info(text: string): JQuery;
    /**
     * 显示成功提示 (绿色)
     * @param {type} text
     * @returns
     */
    function Success(text: string): JQuery;
    /**
     * 根据返回结果决定显示成功或错误提示
     * @param {type} result 数据结果
     * @param {type} successText? 成功后显示的文字，如果不填默认为result.Msg
     * @param {type} act? 成功后执行的方法，会将result.Data传入作为变量
     */
    function Show(result: Sail.AjaxResult, successText?: any, act?: Function): void;
    /**
     * 操作成功才提示
     * @param {Sail.AjaxResult} result
     * @param {Function} act?
     */
    function Action(result: Sail.AjaxResult, act?: Function): void;
}
declare var ShowMessage: typeof MsgBox.Show;
declare var ShowInfo: typeof MsgBox.Info;
declare var ShowSuccess: typeof MsgBox.Success;
declare var ShowError: typeof MsgBox.Error;

declare module Sail {
    class ApiHelper {
        private Controller;
        constructor(controller: string);
        GetApi(actionName: string): string;
    }
    class RazorPage {
        private ApiName;
        private IdName;
        private Id;
        private Title;
        $List: JQuery;
        btnAdd: JQuery;
        Page: Sail.Pagination;
        modal: Sail.Modal;
        $form: JQuery;
        content: JQuery;
        $Editor: JQuery;
        $Viewer: JQuery;
        constructor(title: string, api: string, idName: string);
        SetTitle(title: string): this;
        /**
         * 生成顶部返回按钮
         */
        private MarkBtnReturn();
        /**
         * 取消事件
         * 会触发after.Cancel事件，如需自定义请使用this.content.on("after.Cancel",function(){});
         * @returns
         */
        private Cancel();
        /**
         * 修改表单页面的标题
         * @param {type} title
         */
        SetFormTitle(title: string): void;
        GetApi(name: string): string;
        /**
         * 返回对象的id值(根据idName)
         * @param {type} obj
         * @returns
         */
        GetDataId(obj: Object): any;
        /**
         * 获取列表api的url
         * @returns
         */
        ListApi(): string;
        /**
         * 新增事件
         * 会触发after.Add事件，如需自定义请使用this.content.on("after.Add",function(){});
         * @returns
         */
        Add(): this;
        /**
         * 保存事件
         * 保存前会触发PreSave函数，如果定义了则会在PreSave函数之中对数据进行修改
         * 保存成功会触发after.Save事件，如需自定义请使用this.content.on("after.Save",function(){});
         * @param {type} btn
         * @returns
         */
        Save(btn: JQuery): PageTools;
        /**
         * 创建分页控件
         * @param {type} param
         */
        CreatePage(param: Sail.PaginationSetting): Pagination;
        RegisterAct(body: JQuery, page: any): void;
        /**
         * 编辑事件
         * 最后会触发after.Edit事件，如需自定义请使用this.content.on("after.Edit",function(data,page,obj){});
         * @param {type} data 要编辑的数据对象
         * @param {type} page 分页控件
         * @param {type} obj 触发事件的jquery对象
         */
        EditAct(data: Object, page: Pagination, obj: JQuery): void;
        /**
         * 删除事件
         * 最后会触发after.Delete事件，如需自定义请使用this.content.on("after.Delete",function(data,page,obj){});
         * @param {type} data 要删除的对象
         * @param {type} page 分页控件
         * @param {type} obj 触发事件的jquery对象
         */
        DeleteAct(data: Object, page: Pagination, obj: JQuery): void;
        /**
         * 按钮操作事件
         * @param {Object} data
         * @param {Pagination} page
         * @param {JQuery} obj
         */
        BtnAct(data: Object, page: Pagination, obj: JQuery): void;
        /**
         * 查看事件
         * 最后会触发after.View事件，如需自定义请使用this.content.on("after.View",function(data,page,obj){});
         * 暂时不提供对弹窗页面的ViewAct支持
         * @param {type} data
         * @param {type} page
         * @param {type} obj
         */
        ViewAct(data: Object, page: Pagination, obj: JQuery): void;
        /**
         * 保存前对数据进行处理
         * @param {type} form
         * @param {type} modal
         */
        PreSave(form: JQuery, model: Object): Object;
    }
    /**
     * 页面控件
     * @param {type} title
     * @param {type} api
     * @param {type} idName
     * @param {type} pageTmpl
     * @returns
     */
    class PageTools extends RazorPage {
        constructor(title: string, api: string, idName: string, pageTmpl: string);
    }
}
declare var PageTools: typeof Sail.PageTools;

declare namespace Sail {
    interface Event {
        handle: string;
        act: Function;
    }
    interface PaginationSetting {
        bodyContainer?: any;
        getPostKey?: Function;
        pageSize?: number;
        handleName: any;
        headContainer?: any;
        footContainer?: any;
        ajaxType?: string;
        bodyTmpl?: any;
        footTmpl?: any;
        queryed?: any;
        titles?: Array<string>;
        titleWidth?: Array<number>;
        events?: Array<Event>;
        requeryHandle?: string;
    }
    class Pagination {
        constructor(set: PaginationSetting);
        currentIndex: number;
        Container: JQuery;
        settings: PaginationSetting;
        Query(pageIndex?: number): this;
    }
}

declare namespace PinYin {
    /**
     * 获取汉字首字母 （英文和数字原样返回）
     * @param {string} str
     * @returns
     */
    function ZhToPinyin(str: string): string;
}

interface Number {
    /**
     * 从左补齐字符串宽度
     * @param {number} length 目标字符串宽度
     * @param {string} padStr 用来补齐的字符，默认为空格
     */
    PadLeft(length: number, padStr: string): string;
    /**
     * 从右补齐字符串宽度
     * @param {number} length 目标字符串宽度
     * @param {string} padStr 用来补齐的字符，默认为空格
     */
    PadRight(length: number, padStr: string): string;
}
declare namespace NumberHelper {
}
interface String {
    /**
     * 从左补齐字符串宽度
     * @param {number} length 目标字符串宽度
     * @param {string} padStr 用来补齐的字符，默认为空格
     */
    PadLeft(length: number, padStr: string): string;
    /**
     * 从右补齐字符串宽度
     * @param {number} length 目标字符串宽度
     * @param {string} padStr 用来补齐的字符，默认为空格
     */
    PadRight(length: number, padStr: string): string;
    /**
     * 格式化字符串，用法同c#
     * @param {type} ...args
     */
    format(...args: any[]): string;
    /**
     * 字符串转整数
     */
    ToInt(): Number;
    /**
     * 字符串转浮点
     */
    ToFloat(): Number;
    /**
     * 移除字符串中的html标签
     */
    RemoveHtmlTag(): string;
    /**
     * 根据字符数截断文字(汉字算2个字符),如果原字符串超长自动追加……
     * @param {type} len
     */
    SubStringByByte(len: number): string;
}
declare namespace StringHelper {
}

declare module Sail {
}
