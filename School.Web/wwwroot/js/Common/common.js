$(function () {
    //======================
    //个人设置显示和隐藏
    //======================
    var divHeight;
    $('.per-drop-down').mouseenter(function () {
        var li_height = $(this).find(".info-set-up li");
        divHeight = li_height.length * li_height.height();
        $(this).find(".info-set-up").height(divHeight);
        $(this).find(".info-set-up").css({ "border": "1px solid #eee" });
    });
    $('.per-drop-down').mouseleave(function () {
        $(this).find('.info-set-up').height(0);
        $(this).find(".info-set-up").css({ "border": "none" });
        $(".info-set-up").mouseenter(function () {
            $(this).find(".info-set-up").height(divHeight);
            $(this).find(".info-set-up").css({ "border": "1px solid #eee" });
        });

        $(".info-set-up").mouseleave(function () {
            $(this).find('.info-set-up').height(0);
            $(this).find(".info-set-up").css({ "border": "none" });
        });
    });

    $(document).on('click', '.add_explicit_person', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../Account/AddUser/" + $(this).attr("data_Id"),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message)
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        })
    })
})