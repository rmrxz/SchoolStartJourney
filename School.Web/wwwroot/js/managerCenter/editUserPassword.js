function password() {
    $(document).on('click', '.save-user-password', function (e) {
        e.preventDefault();
        $('.user-password-error').text("");
        $('.user-new-password-erroe').text("");
        $('.identify-password-error').text("");
        var passwordCheck = /^(?![0-9]+$)(?![a-z]+$)(?![A-Z]+$)(?![,@\.#%'\+\*\-:;^_`]+$)[,@\.#%'\+\*\-:;^_`0-9A-Za-z]{6,16}$/;
        var password = $('input[name=password]').val();
        var newPassword = $('input[name=newPassword]').val();
        var identifyPassword = $('input[name=identifyPassword]').val();
        if (password == "") {
            $('.user-password-error').text("密码不能为空");
            return;
        }
        //if (!passwordCheck.test(password)) {
        //    $('.user-password-error').text("密码输入格式不对，密码要包含字符、数字和字母，长度为6-12位字符");
        //    return;
        //}
        if (newPassword == "") {
            $('.user-new-password-erroe').text("新密码不能为空");
            return;
        }
        if (!passwordCheck.test(newPassword)) {
            $('.user-new-password-erroe').text("密码输入格式不对，请输6~16位数字,英文,英文符号至少两种组合字符的密码");
            return;
        }

        if (identifyPassword == "") {
            $('.identify-password-error').text("确认密码不能为空");
            return;
        }
        if (!passwordCheck.test(identifyPassword)) {
            $('.identify-password-error').text("密码输入格式不对，请输6~16位数字,英文,英文符号至少两种组合字符的密码");
            return;
        }
        if (newPassword != identifyPassword) {
            $('.identify-password-error').text("新密码和确认密码不一致");
            return;
        }
        $.ajax({
            cache: false,
            type: 'type',
            async: false,
            url: '../../ManageCenter/SaveUserPassword?password=' + password + "&newPassword=" + newPassword,
            beforeSend: function () {
            }
        }).done(function (result) {
            if (result.isOK) {
                window.location.href = "../../Account/LoginRegister";
                toastr.success(result.message);
            }
            else {
                toastr.error(result.message)
            }

            }).fail(function () {
                toastr.error("连接后台失败");
        })
    })
};