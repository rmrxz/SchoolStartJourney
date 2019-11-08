$(function () {
    //切换登录和注册界面
    $('#new-register').click(function (e) {
        $('input[name="UserName"]').val("");
        $('input[name="Email"]').val("");
        $('input[name="Password"]').val("");
        $('input[name="ConfirmPassword"]').val("");
        e.preventDefault();
        $('#signin_interface').addClass("nterface-none");
        $('#register_interface').removeClass("nterface-none");
        $("#bookmark").html("校园启行-注册界面"); 
    });

    $('#return_in').click(function (e) {
        e.preventDefault();
        $('#signin_interface').removeClass("nterface-none");
        $('#register_interface').addClass("nterface-none");
        $("#bookmark").html("校园启行-登录界面");
    });

    // 处理登录系统操作
    $('.logon_user').click(function (e) {
        e.preventDefault();//此处阻止提交表单  
        var userName = $('#userName').val();
        var password = $('#password').val();
        $('#logonModalErrMessage').text = "用户名或者密码不能为空。";
        if (userName === "" || password === "") {
            $('#logonModalErrMessage').text("用户名或者密码不能为空。");
        } else {
            $('#logonModalErrMessage').text("正在登录系统，请稍候......");
            // 创建登录数据模型
            var logonDataModel = "{" +
                "UserName:'" + userName + "'," +
                "Password:'" + password + "'" +
                "}";
            // 转换为 Json 模型
            var logonJsonModel = { 'jsonLogonInformation': logonDataModel };
            // 执行提交
            $.ajax({
                cache: false,
                type: 'POST',
                async: false,
                url: "../../Account/Login",
                data: logonJsonModel,
                dataType: 'json',
                beforeSend: function () {
                }
            })
                .done(function (logonStatus) {
                    if (logonStatus.result === true) {
                        location.href = "../../" + logonStatus.message;
                    } else {
                        document.getElementById("logonModalErrMessage").innerText = logonStatus.message;
                    }
                }).fail(function () {
                    toastr.error("这个连接后台失败！");
                }).always(function () {
                });
        }
    });

    //注册
    $('.register_user').click(function (e) {
        e.preventDefault();//此处阻止提交表单  
        var userName = $('#UserName').val();
        var userNameCheck =/^[0-9a-zA-Z]*$/
        var email = $('#Email').val();
        var emailCheck = /^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$/;
        var password = $('#rPassword').val();
        var confirmPassword = $('#confirmPassword').val();
        var passwordCheck = /^(?![0-9]+$)(?![a-z]+$)(?![A-Z]+$)(?![,@\.#%'\+\*\-:;^_`]+$)[,@\.#%'\+\*\-:;^_`0-9A-Za-z]{6,16}$/;
        if (userName === "") {
            $('#registerModalErrMessage').text("用户名不能为空");
            return;
        }
        else if (!userNameCheck.test(userName))
        {
            $('#registerModalErrMessage').text("用户登录名只能输入英文或数字")
        }
        else if (email == "")
        {
            $('#registerModalErrMessage').text("邮箱不能为空");
            return;
        }
        else if (!emailCheck.test(email)) {
            $('#registerModalErrMessage').text("邮箱格式出错");
            return;
        }
        else if (password === "") {
            $('#registerModalErrMessage').text("密码不能为空");
            return;
        }
        else if (!passwordCheck.test(password)) {
            $('#registerModalErrMessage').text("密码输入格式不对，请输6~20位数字,英文,英文符号至少两种组合字符的密码");
            return;
        }
        else if (confirmPassword === "") {
            $('#registerModalErrMessage').text("请再次输入密码");
            return;
          
        }
        else if (password !== confirmPassword) {
            $('#registerModalErrMessage').text("密码和重复密码不一致，请重新输入。");
            return;
        }
        else {
            $('#registerModalErrMessage').text("正在注册新账号，请稍候......");
            // 创建登录资料数据模型
            var registerDataModel = "{" +
                "UserName:'" + userName + "'," +
                "Email:'" + email + "'," +
                "Password:'" + password + "'," +
                "ConfirmPassword:'" + confirmPassword + "'" +
                "}";
            // 转换为 Json 模型
            var registerJsonModel = { 'jsonRegisterInformation': registerDataModel };
            $.ajax({
                cache: false,
                type: 'POST',
                async: false,
                url: "../../Account/Register",
                data: registerJsonModel,
                beforeSend: function () { }
            }).done(function (registerStatus) {
                console.log(registerStatus.validateMessageItems[0]);
                if (registerStatus.isOK === true) {                   
                    $("#return_in").trigger("click");
                    $('#logonModalErrMessage').text("注册成功！！！");
                    $('#registerModalErrMessage').text(registerStatus.validateMessageItems[0].message);
                } else {
                    $('#registerModalErrMessage').text(registerStatus.validateMessageItems[0].message);
                }
            }).fail(function () {
                toastr.error("这个连接后台失败！");
            }).always(function () { });
        }
    });
});