// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function logout()
{
    if (confirm("Are you sure?") == true)
    {
        $.ajax({
            type: "POST",
            url: "/login/logout",
            async: false,
            success: function (res) {
                if (res.success == true) {
                 
                    document.location.href = "/";
                }
                else {
                    alert(res.message);
                }

            },
            failure: function (res) {
            },
            error: function (res) {

            },
        });
    }
}
function changePass() {
    username = $("#txtUsername").val();
    Oldpass = $("#txtOldPass").val();
    pass1 = $("#txtPass1").val();
    pass2 = $("#txtPass2").val();
    if (Oldpass == "" || Oldpass == undefined || Oldpass == null || Oldpass < 3) {
        alert("Vui lòng nhập mật khẩu cũ!!");
        return;
        S
    }
    if (pass1 == "" || pass1 == undefined || pass1 == null || pass1 < 3) {
        alert("Vui lòng nhập mật khẩu mới!!");
        return;
        S
    }
    if (pass2 == "" || pass2 == undefined || pass2 == null || pass2 < 3) {
        alert("Vui lòng nhập mật khẩu mới!!");
        return;
        S
    }
    if (pass1!=pass2) {
        alert("Mật khẩu mới chưa trùng khớp!!");
        return;
        S
    }else if (Oldpass == pass1) {
        alert("Mật khẩu mới trùng mật khẩu cũ!!");
        return;
    }
    if (confirm("Bạn có chắc thay đổi pass?") == true) {
        $.ajax({
            type: "POST",
            url: "/login/change_pass",
            data: { 'username': username, 'oldPass': Oldpass, 'newPass': pass1 },
            async: false,
            success: function (res) {
                if (res.success == true) {
                   
                    document.location.href = "/";
                }
                else {
                    alert(res.message);
                }

            },
            failure: function (res) {
            },
            error: function (res) {

            },
        });
    }
}
function edit() {

}