var totalPage = 1;
var lst = null;
function selectClass(ctl) {
    let lop = $(ctl).val();
    if (lop != "0" && lop != undefined && lop != null) {
        getCourse(lop, 1);
        $("#tblResult").show(500);
    }
    else {
        $("#tblResult").hide(500);
    }
}
function getCourse(grp, p) {
    $.ajax({
        type: "POST",
        url: "/Home/get_course",
        data: { 'Group': grp, 'Page': p, 'Size': 5 },
        async: false,
        success: function (res) {
            if (res.success) {
                let data = res.data;
                if (data.data != null && data.data != undefined) {
                    let stt = (p - 1) * 5 + 1;
                    let data1 = [];
                    for (var i = 0; i < data.data.length; i++) {
                        let item = data.data[i];
                        item.stt = stt;
                        data1.push(item);
                        stt++;
                    }
                    lst = data1;
                    $("#tblResult tbody").html("");
                    $("#courseTemplate").tmpl(data1).appendTo("#tblResult tbody");
                }
                totalPage = data.totalPage;
                $("#curPage").text(p);
            }
            else
                alert(res.message);
        },
        failure: function (res) { },
        error: function (res) { }
    });
}

function goPrev() {
    var curPage = parseInt($("#curPage").text());
    if (curPage == 1)
        alert("Dang o trang dau tien!!");
    else {
        var p = curPage - 1;
        var grp = $("#selClass").val();
        getCourse(grp, p);
    }
}

function goNext() {
    var curPage = parseInt($("#curPage").text());
    if (curPage == totalPage)
        alert("Dang o trang cuoi!!");
    else {
        var p = curPage + 1;
        var grp = $("#selClass").val();
        getCourse(grp, p);
    }
}
function OpenModal(id) {
    $("#btnSave").show();
    $("#btnInsert").hide();
    if (lst != null && id != null && id > 0) {
        var item = $.grep(lst, function (obj) {
            return obj.id == id;
        })[0];
        $("#txtID").val(item.id);
        $("#txtCourse").val(item.courseName);
        $("#txtGroup").val(item.group);
        $("#txtCredit").val(item.credit);
        $("#txtCode").val(item.subCode);
        $("#txtMajor").val(item.major);
        $("#txtNote").val(item.note);
    }
}

function save() {
    var item = {
        id: $("#txtID").val(),
        courseName: $("#txtCourse").val(),
        group: $("#txtGroup").val(),
        credit: $("#txtCredit").val(),
        subCode: $("#txtCode").val(),
        major: $("#txtMajor").val(),
        note: $("#txtNote").val(),
    };
    $.ajax({
        type: "POST",
        url: "/Home/update_course",
        data: { 'course': item },
        async: false,
        success: function (res) {
            if (res.success) {
                alert("Update success!!")
                let c = res.data;
                var i;
                for (i = 0; i < lst.length; i++) {
                    if (lst[i].id == c.id) {
                        c.stt = lst[i].stt;
                        break;
                    }
                }
                lst[i] = c;
                $("#tblResult tbody").html("");
                $("#courseTemplate").tmpl(lst).appendTo("#tblResult tbody");
            } else
                alert(res.message);
        },
        failure: function (res) { },
        error: function (res) { }
    });
}
function addNew(){
    $("#btnSave").hide();
    $("#btnInsert").show();

    $("#txtID").val("");
    $("#txtName").val("");
    $("#txtGroup").val($("#selClass").val());
    $("#txtCredit").val("");
    $("#txtCode").val("");
    $("#txtMajor").val("");
    $("#txtNote").val("");
}
function Insert() {
    var item = {
        id: 0,
        courseName: $("#txtCourse").val(),
        group: $("#txtGroup").val(),
        credit: $("#txtCredit").val(),
        subCode: $("#txtCode").val(),
        major: $("#txtMajor").val(),
        note: $("#txtNote").val(),
    };
    $.ajax({
        type: "POST",
        url: "/Home/insert_course",
        data: { 'course': item },
        async: false,
        success: function (res) {
            if (res.success) {
                alert("Insert success!!")
                let c = res.data;
                $("#txtID").val(c.id);
                var grp = $("#selClass").val();
                getCourse(item.group, 1);
            } else
                alert(res.message);
            
        },
        failure: function (res) { },
        error: function (res) { }
    });
}
function Delete(id)
{
    if (confirm("Bạn có chắc xóa course này?") == false)
        return;
    if (id != undefined && id != null && id > 0) {
        var item = $.grep(lst, function (obj) {
            return obj.id == id;
        })[0];
    }
    $.ajax({
        type: "POST",
        url: "/Home/delete_course",
        data: { 'course': item },
        async: false,
        success: function (res) {
            if (res.success) {
                alert("Delete success!!");
                var page = parseInt($("#curPage").text());
            } else
                alert(res.message);
            getCourse(item.group, page);
        },
        failure: function (res) { },
        error: function (res) { }
    });
}