function login(){
    let uid = $("#txtUsername").val();
    let pwd = $("#txtPassword").val();
    console.log("Username: ", uid);
    console.log("Password: ", pwd);
    let s = check(uid, pwd);
    if (s == "") {
        let dataLogin = { "username": uid, "password": pwd };
        $.ajax({
            type: "POST",
            url: "/login/doLogin",
            data: { "login": dataLogin },
            async: false,
            success: function (res) {
                if (res.success == true) {
                    let usr = res.users;
                    alert("Xin chao: " + usr.fullname);
                    document.location.href = "/Home";
                }
                else {
                        alert(res.message);
                }
                
            },
            failure: function (res){ 
            },
            error: function (res){ 
                
            },
        });
    }
    else {
        alert(s);
    }
}
function check(email, pass) {
    var s = "";
    if (email == ""||email == undefined || email == null)
    s += "Chưa nhập email";
    if (pass == ""||pass == undefined || pass == null)
    s += "\nChưa nhập pass";
    return s;
}
// Example starter JavaScript for disabling form submissions if there are invalid fields
(() => {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    const forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})()