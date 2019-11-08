// 打开登录会话框
function openLogonModal(){
    $('#logonModal').modal({
        show: true,
        backdrop: 'static'
    });
}

// 处理登录系统操作
function logon() {
    var userName = document.getElementById('UserName').value;
    var password = document.getElementById('Password').value;
    if (userName === "" || password === "") {
        document.getElementById('logonModalErrMessage').innerText = "用户名或者密码不能为空。";
    } else {
        document.getElementById('logonModalErrMessage').innerText = "正在登录系统，请稍候......";
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
            url: "../../Account/Logon",
            data: logonJsonModel,
            dataType: 'json',
            beforeSend: function () {
            }
        })
            .done(function (logonStatus) {
                if (logonStatus.result === true) {
                    location.href ="../../User/Index"
            } else {
                document.getElementById("logonModalErrMessage").innerText = logonStatus.message;
            }
        }).fail(function () {
            alert("这个连接后台失败！");
        }).always(function () {
        });
    }
}

// 打开用户资料注册会话框
function openRegisterModal() {
    $('#registerModal').modal({
        show: true,
        backdrop: 'static'
    });
}
// 处理注册用户操作
function register() {
    // 提取所需要的注册数据
    var regUserName        = document.getElementById('RegisterUserName').value;
    //var regFullName        = document.getElementById('RegisterFullName').value;
    var regEmail           = document.getElementById('RegisterEmail').value;
    var regMobile          = document.getElementById('RegisterMobile').value;
    var regPassword        = document.getElementById('RegisterPassword').value;
    var regPasswordConfirm = document.getElementById('RegisterPasswordConfirm').value;
    // 处理校验 TBD
    if (regPassword !== regPasswordConfirm) {
        document.getElementById('registerModalErrMessage').innerText = "密码和重复密码不一致，请重新输入。"; 
    } else {
        
        // 创建登录资料数据模型
        var registerDataModel = "{" +
            "UserName:'" + regUserName + "'," +
            //"Name:'" + regFullName + "'," +
            "EMail:'" + regEmail + "'," +
            "MobileNumber:'" + regMobile + "'," +
            "Password:'" + regPassword + "'" +
            "}"; 
        // 转换为 Json 模型
        var registerJsonModel = { 'jsonRegisterInformation': registerDataModel };
        // 提交数据
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: "../Account/Register",
            data: registerJsonModel,
            dataType: 'json',
            beforeSend: function () {
                
            }
        }).done(function (registerStatus) {            
            if (registerStatus.result === true) {
                location.href = "../UserManager/Login"
            } else {
                document.getElementById("registerModalErrMessage").innerText = registerStatus.message;
            }
        }).fail(function () {
            alert("这个连接后台失败！");
        }).always(function () {
        });
    }
}

// 获取用户登录名称
function getUserName() {
    $.ajax({
        cache: false,
        type: 'post',
        async: false,
        url: '../../Account/GetUserName',
        beforeSend: function () {
            // alert("开始访问");
        }
    }).done(function (data) {
        document.getElementById("userLogonInfo").innerHTML = '<a class="btn btn-default" href="../../Account/EditProfile">'
            + '<span class="glyphicon glyphicon-asterisk"></span>'
            + '  欢迎你：' + data.userName
            + '</a>';
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });

}

function ListApplicationUserCreateOrEditForm()
{

}