$(function () {
    $(document).on('click', '.con-color', function (e) {
        radio();
        e = e || window.e;
        //阻止事件冒泡
        e.stopPropagation();
        // 方法阻止元素发生默认的行为
        e.preventDefault();
        $(this).append('<div class="operation-color"></div>');
        $('.operation-color').animate({ width: '108%' }, 0);
        //$('.operation-color').css("width","108%");
    });

    //点击评论区以外时隐藏评论输入框
    $(document).click(function (e) {
        e.stopPropagation();
        radio();
    });

    //所有的单选框去选中的之外都取消选择
    function radio() {
        var con_color = $('.con-color');
        for (var i = 0; i < con_color.length; i++) {
            if (con_color.eq(i).children('.input-required').val() == "") {
                con_color.eq(i).children(".operation-color").remove();
            }
        }
    }
    $(document).on('click', '.AnAssociationNameSelect', function () {
        var anName = $(".AnAssociationNameSelect").find("option:selected").attr("value");
        var name = $(this).next();
        name.val(anName);
    })

    //新增或更新活动保存事件'../../ManageCenter/SaveActivity',
    $(document).on('click', '.save-activity', function () {
        var data = $('form[name=activityForm]').serializeArray();
        //校验截止时间
        //var signDateCheck =/^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$/
        var dataString = "";
        $.each(data, function (i, obj) {
            dataString += obj.name + ":";
            dataString += '"' + obj.value + '",';
        });
        var jsonData = { "activityData": "{" + dataString + "}" }
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/SaveActivity',
            data: jsonData,
            beforeSend: function () { }
        }).done(function (data) {
            if (data.result) {
                detailedActivity(data.id);
                toastr.success(data.message)
            }
            else {
                toastr.warning(data.message);
            }
            }).fail(function () {
                toastr.error("后台连接失败")
        }).always(function () {

        });
    });
    //  level分为 YM YMD H HM 四个有效值，分别表示年月 年月日 年月日时 年月日时分,less表示是否不可小于当前时间。年-月-日 时:分 时为24小时制
    //  为确保控件结构只出现一次，在有需要的时候进行一次调用。
    onLoadTimeChoiceDemo();
});