
var MobileModal = Sail.Model;

function Refresh($data, tool,modal,isInside) {
    var data = $data.data();
    var $select =isInside?modal.modal.find(data.target): tool.$Editor.find(data.target);
    var val = $select.val();
    $select.off("ajax.Done").on("ajax.Done", function () {
        $select.val(val);
    }).ajaxAddOptions();
}

function setEmergency(div, data) {
    div.find("#CName").val(data.Emergency.Name);
    div.find("#CPhone").val(data.Emergency.Phone);
    div.find("#CRelation").val(data.Emergency.Relation);
    div.find("#CBirthDay").val(data.Emergency.BirthDay);
    div.find("#CBirthMonth").val(data.Emergency.BirthMonth);
    div.find("#CBirthYear").val(data.Emergency.BirthYear);
    div.find("#CCalendarType").val(data.Emergency.CalendarType);
}

function getEmergency(div) {
    return {
        Name: div.find("#CName").val(),
        Phone: div.find("#CPhone").val(),
        Relation: div.find("#CRelation").val(),
        BirthDay: div.find("#CBirthDay").val(),
        BirthMonth: div.find("#CBirthMonth").val(),
        BirthYear: div.find("#CBirthYear").val(),
        CalendarType: div.find("#CCalendarType").val()
    }
}

function TypeChang() {
    $(".VisitTimes").on("click", "li", function () {
        var value = $(this).data("value");
        var text = $(this).data("text");
        $("#visitText").text(text);
        $("#visitType").val(value);
    })
}


function TypeChang(className, idText, idType) {
    $(className).on("click", "li", function () {
        var value = $(this).data("value");
        var text = $(this).data("text");
        $(idText).text(text);
        $(idType).val(value);
    })
}

function TimeChange() {
    $(".timeType").on("click", "li", function () {
        var value = $(this).data("value");
        var text = $(this).data("text");
        $("#timeText").text(text);
        $("#selectTime").val(value);
    })
}

function IsShowAdd(id,tool) {
    if (id == "add") {
        $("#btnAdd").click();
    }
    tool.content.on("after.Save", function () {
        if (id == "add") {
            window.close();
        }
    }).on("after.Cancel", function () {
        if (id == "add") {
            window.close();
        }
    });
}

$(function() {
    $(".myScroller").each(function () {
        var height = $(this).data("height");
        if (!height) height = "600px";
        $(this).slimScroll({ height: height });
    });
})