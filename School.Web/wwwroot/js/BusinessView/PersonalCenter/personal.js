$(function () {
    //活动赛选条件确认
    $('.selectionConditions li').click(function () {
        $('.conditionName').text($(this).children('a').text())
    });

    $(document).on('click', '.seeUserDetailed', function (e) {
        e.preventDefault()
        window.location.href = "../../PersonalCenter/Index?id=" + $(this).attr("data_Id");
    })

    //历史活动列表
    // 活动列表
    function ActivityList(pageNumber) {
        var add_more = $('#add-more');
        var number = parseInt($('.activitys-number h2').text())
        if (pageNumber == "undefined") {
            pageNumber = 0;
        }
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../PersonalCenter/List/?id=" + $('input[name=per_user_Id]').val() + "&&pageNumber=" + pageNumber,
            beforeSend: function () {
                add_more.text("正在加载....");
                add_more.removeAttr('Id');
            }
        }).done(function (result) {
            if (!result) {
                if (pageNumber == 0) {
                    add_more.text("暂无活动记录");
                }
                else {
                    add_more.text("已加载全部");
                }
                add_more.removeAttr('Id');
                return;
            }
            $('#activity-record').append(result);
            console.log(number);
            if (number <= (10 * (pageNumber + 1))) {
                add_more.text("已加载全部");
                add_more.removeAttr('Id');
                return;
            }
            add_more.attr("Id", "add-more");
            add_more.text("加载更多");
            }).fail(function () {
        }).always(function () {
        });
    };
    ActivityList(0);

    var pageNumber = 0;
    //加载更多
    $(document).on('click', '#add-more', function () {
        pageNumber = pageNumber + 1;
        ActivityList(pageNumber);
    })

    //删除活动记录
    $(document).on('click', '.delete-activity-record', function () {
        var activityList = $(this).parents('li');
        console.log(activityList);
        $.ajax({
            cache: false,
            type: "post",
            async: false,
            url: "../../PersonalCenter/DeleteActivityRecord/?id=" + $(this).attr('data_Id'),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                activityList.remove();
                toastr.success(result.message);
            }
            else {
                toastr.error(result.message);
            }
        }).fail(function () { });
    });

    //添加评论
    $(document).on('click', '.publish-comment', function (e) {
        e.preventDefault();
        var com = $(this).parents('.publish-dis').siblings('.comment-text');
        //分割字符串
        var comment = com.text().split(":")[1];
        var activityComments = $(this).parents('.activityComments');
        var activityID = activityComments.siblings('input[name="Id"]').val();
        var parentGradeBusiness = com.attr('data_acid');
        var acceptUserId = com.attr('data_acceptUserId');
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../PersonalCenter/AddComment/?id=" + activityID + "&&parentGradeBusiness=" + parentGradeBusiness + "&&acceptUserId=" + acceptUserId + "&&comment=" + comment,
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                publishComment(activityID, activityComments);
            }
            else {
                toastr.error(result.message);
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    function publishComment(Id, activityComments) {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../PersonalCenter/GetActivityComments/?id=" + Id,
            beforeSend: function () {
            }
        }).done(function (result) {
            activityComments.html(result);
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    }

    //删除评论
    $(document).on('click', '.delete-reply', function (e) {
        e.preventDefault();
        var delete_comment = $(this).parents('li');
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../PersonalCenter/DeleteComment/?id=" + $(this).attr('data_acid'),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                delete_comment.eq(0).remove();
            }
            else {
                toastr.error("删除失败")
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });
});
//======================
//兴趣相同固定
//======================
function InterestFixed() {
    var r = $('#p_fixed').offset().top;
    $(document).scroll(function () {
        var width = $('#p_fixed').width();
        var R = $('#p_fixed').offset().top;
        var W = $(window).scrollTop();
        if (R - r < 0) {
            $('#p_fixed').removeAttr("style");
        }
        if (R - W < 0) {
            $('#p_fixed').css({ "position": "fixed", "top": 0, "width": width })
        }
    });
}
