(function () {
    $.extend($.fn, {
        AutoComboBox: function (op) {
            op = $.extend({
                url: false,                         //获取数据的地址     
                type: "GET",                         //请求远程数据的方式 get/post
                data: true,                         //本地数据(Json格式)  只有当url参数设置为false时该参数才生效 否则优先从远程url获取数据
                cssClass: false,                    //下拉框使用的class样式
                firstValue: [true, "0", "请选择"],  //是否自动创建第一个值  这个值通常为 请选择 ,不限等
                nullDispaly: true,                  //当下一个下拉框的值为空时 是否隐藏 默认隐藏
                idPrefix: "Sail_combobox_",       //自动生成下拉框的 默认ID前缀
                defaultValue: false,                //如果此值不为false  将根据该值设置默认值 并且递归向上设置所有的下拉框默认值
                DefaultValues: "",
                wrap: false,
                changeEven: null
            }, op);
            var c = this;
            if (op.url) {    //从远程获取数据
                $.ajax({
                    type: op.type,
                    url: op.url,
                    async: false,
                    success: function (msg) {
                        op.data = eval(msg);
                    }
                });
            }

            if (op.data) {
                $(c).data("datas", op.data);
                c.CreateOption(c, 1, op, op.data);
                if (op.defaultValue) {
                    op.DefaultValues = "";
                    c.AutoDefaultValue(c, 1, op, op.data, op.data);
                    var ps = op.DefaultValues.split(',');
                    var sindex = 1;
                    for (var index = ps.length - 1; index >= 0; index--) {
                        $select = c.find("#" + op.idPrefix + sindex++);
                        $select.val(ps[index]);
                        $select.change();
                    }
                }
            }

        },
        /*设置默认值-start*/
        AutoDefaultValue: function (o, index, op, data, children, item_id) {
            var defaultValue = op.defaultValue;
            $.each(children, function (i, item) {
                if (item.id == defaultValue) {
                    //找到相同的ID了..   //index  代表当前的是第几个下拉框
                    op.DefaultValues += (op.DefaultValues ? "," : "") + op.defaultValue;
                    op.defaultValue = item_id;
                    children = data;
                    o.AutoDefaultValue(o, index + 1, op, data, children);
                    return false;
                }
                if (item.children && item.children.length > 0) {  //存在子类
                    o.AutoDefaultValue(o, index + 1, op, data, item.children, item.id);
                }
            });
        },

        /*设置默认值-end*/
        CreateOption: function (o, index, op, data) {
            var $select;
            if (o.find("#" + op.idPrefix + index).length > 0) {      //查询当前递归的下拉框是否存在 如果存在则删除
                $select = o.find("#" + op.idPrefix + index);
                var $ts = $("<select id='" + op.idPrefix + index + "' index='" + index + "'/>");
                $select.replaceWith($ts);
                $select = $ts;
            }
            else {
                $select = $('<select id="' + op.idPrefix + index + '" index="' + index + '"/>');   //创建新的下拉框
                o.append($select);
                if (op.wrap) {
                    $select.wrap(op.wrap);
                }

            }
            //附加用户自定义下拉框样式
            if (op.cssClass) {
                $select.addClass(op.cssClass);
            }


            var ComboBox = this;
            var tempOption = '';
            if (op.firstValue[0]) {
                tempOption += "<option value='" + op.firstValue[1] + "'>" + op.firstValue[2] + "</option>"
            }
            $.each(data, function (i, item) {
                tempOption += "<option value='" + item.id + "'>" + item.text + "</option>"
            });
            $select.empty().append(tempOption);

            $select.change(function () {    //绑定下拉框 onchange事件
                var val = $(this).val();
                nextIndex = parseInt($(this).attr("index")) + 1;
                var add = false;
                $.each(data, function (i, item) {
                    if (item.id == val) {
                        if (item.children && item.children.length > 0) {
                            ComboBox.CreateOption(o, nextIndex, op, item.children);   //change时 回调该方法
                            add = true;
                            return false;
                        }
                    }
                });
                if (!add) {                       //是否回调了方法
                    var $selects = new Array();   //如果回调则进行下面的处理
                    var j = 0;
                    while (true) {                //寻找当前下拉框的 所有子集下拉框  并放进array
                        nextIndex = nextIndex;
                        $select = o.find("#" + op.idPrefix + nextIndex);
                        if ($select.length > 0)
                            $selects[j] = $select;
                        else
                            break;
                        j++;
                        nextIndex++;
                    }
                    if (op.nullDispaly) {            //根据用户设置  如果为空时  隐藏下拉框 则直接 remove()
                        for (var i = 0; i < $selects.length; i++) {
                            if (op.wrap) {
                                $selects[i].parent().remove();
                            }
                            else
                                $selects[i].remove();

                        }

                    } else {                         //否则显示出 请选择等字样
                        for (var i = 0; i < $selects.length; i++) {
                            $select = $selects[i];
                            var t = "";
                            if (op.firstValue[0]) {
                                t += "<option value='" + op.firstValue[1] + "'>" + op.firstValue[2] + "</option>"
                            }
                            nextIndex = index + 1 + i;
                            var $ts = $("<select id='" + op.idPrefix + nextIndex + "' index='" + nextIndex + "'>" + t + "</select>");
                            $select.replaceWith($ts);
                            $select = $ts;
                            //附加用户自定义下拉框样式
                            if (op.cssClass) {
                                $select.addClass(op.cssClass);
                            }
                        }
                    }
                }

                if (op.changeEven) op.changeEven();
            });
            $select.change();
        },

        //返回第几级的值  默认返回最后一个下拉框的值  不过不满足条件弹出自定义错误消息
        ComboBoxGetValue: function (Atleast, defaultvalue) {
            var s = this.find("select");
            var result = s.eq(s.length - 1).val();

            if (result == defaultvalue && s.length > 1)
                return s.eq(s.length - 2).val();
            else
                return result;
        },

        ComboBoxSetValue: function (val) {
            var defaultValue = val;
            var c = this;
            var data = $(this).data("datas");
            var op = { defaultValue: val, DefaultValues: "", idPrefix: "Sail_combobox_" };
            this.AutoDefaultValue(this, 1, op, data, data);
            var ps = op.DefaultValues.split(',');
            var sindex = 1;
            for (var index = ps.length - 1; index >= 0; index--) {
                $select = c.find("#" + op.idPrefix + sindex++);
                $select.val(ps[index]);
                $select.change();
            }
        }
    });
})(jQuery);