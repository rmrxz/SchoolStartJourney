$(function () {
    $(document).on('click', '.con-color', function (e) {
        infoFillIn();
        e = e || window.e;
        //阻止事件冒泡
        e.stopPropagation();
        // 方法阻止元素发生默认的行为
        e.preventDefault();
        $(this).append('<div class="operation-color"></div>');
        $('.operation-color').animate({ width: '108%' }, 0);
        //$('.operation-color').css("width","108%");
    });

    //点击评论区以外时初始下边框线
    $(document).click(function (e) {
        e.stopPropagation();
        infoFillIn();
    });

    //所有的输入框，除已有数据为初始下边框线
    function infoFillIn() {
        var con_color = $('.con-color');
        for (var i = 0; i < con_color.length; i++) {
            if (con_color.eq(i).children('.input-required').val() == "") {
                con_color.eq(i).children(".operation-color").remove();
            }
        }
    }
    ////单选框点击事件
    //$('.radio-input input[type=radio]').click(function () {
    //    $(this).attr("checked", true);
    //    radio($(this).val());
    //});

    ////所有的单选框去选中的之外都取消选择
    //function radio(value) {
    //    var input_checkeds = $('.radio-input input[type=radio]');
    //    for (var i = 0; i < input_checkeds.length; i++) {
    //        if (input_checkeds.eq(i).val() != value) {
    //            input_checkeds.eq(i).attr("checked", false);
    //        }
    //    }
    //}

    //保存用户信息
    $(document).on('click', '.save-userInfo', function () {
        var data = $('form[name=userInfoForm]').serializeArray();
        var mobileNumberCheck = /^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\d{8}$/;
        var emailCheck = /^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$/;
        var QQCheck = /^[1-9][0-9]{4,14}$/;
        for (var key in data) {
            //名称不能为空
            if (data[key].name == "Name") {
                if (data[key].value=="") {
                    var mobileNumber = $('input[name=Name]').parent('.operation-box').siblings('.input-cue');
                    mobileNumber.css('display', 'inline-block');
                    mobileNumber.text("名称不能为空")
                    return;
                };
            };

            //校验QQ
            if (data[key].name == "QQ") {
                if (!QQCheck.test(data[key].value) && data[key].value!="") {
                    var mobileNumber = $('input[name=QQ]').parent('.operation-box').siblings('.input-cue');
                    mobileNumber.css('display', 'inline-block');
                    mobileNumber.text("QQ输入不正确")
                    return;
                };
            };

            //校验邮箱
            if (data[key].name == "Email") {
                if (!emailCheck.test(data[key].value) && data[key].value != "") {
                    var mobileNumber = $('input[name=Email]').parent('.operation-box').siblings('.input-cue');
                    mobileNumber.css('display', 'inline-block');
                    mobileNumber.text("邮箱输入不正确")
                    return;
                };
            };

            //校验电话号码
            if (data[key].name == "MobileNumber") {
                if (!mobileNumberCheck.test(data[key].value) && data[key].value != "") {
                    var mobileNumber = $('input[name=MobileNumber]').parent('.operation-box').siblings('.input-cue');
                    mobileNumber.css('display', 'inline-block');
                    mobileNumber.text("手机号码输入不正确")
                    return;
                };
            };
        };
        var dataString = "";
        $.each(data, function (i, obj) {
            dataString += obj.name + ":";
            dataString += '"' + obj.value + '",';
        });
        var jsonData = { "userInfoData": "{" + dataString + "}" }
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/SaveInfo',
            data: jsonData,
            beforeSend: function () { }
        }).done(function (data) {
            if (data.result) {
                UserDetail();
                input_cue_hidden()
                toastr.success(data.message);
            }
            else {
                toastr.error(data.message);
            }
        }).fail(function () {
            toastr.error("后台连接失败")
        }).always(function () {

        });
    })

    $(document).on('click', 'input[value=man]', function (e) {
        e.preventDefault();
        $('input[name=Sex]').val("true");
    })
    $(document).on('click', 'input[value=female]', function (e) {
        e.preventDefault();
        $('input[name=Sex]').val("false");
    })
});
function input_cue_hidden() {
    var input_cue = $('.input-cue')
    input_cue.attr('style', '');
}