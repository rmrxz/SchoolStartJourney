var isActivitySearch = false;
$(function () {
    //筛选
    $(document).on('click', '.input-search-activity', function (e) {
        e.preventDefault();
        isActivitySearch = true;
        ActivityList();
    });

    //查看活动信息
    $(document).on('click', '.detailed-activity', function (e) {
        e.preventDefault();
        detailedActivity($(this).attr("data-acId"));
    });

    //查看活动信息
    $(document).on('click', '#detailed-activity', function (e) {
        e.preventDefault();
        detailedActivity($(this).attr("data-acId"));
    });

    //查看活动图片
    $(document).on('click', '#detailed-activity-images', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/ActivityDetailedImages/" + $(this).attr("data-acId"),
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
            }).fail(function () {
                toastr.error("后台连接失败");
        }).always(function () {
        });
    })

    //查看活动成员
    $(document).on('click', '#detailed-activity-member', function (e) {
        e.preventDefault();
        activityMember($(this).attr("data-acId"));
    })

    function activityMember(id) {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/ActivityDetailedMenber/" + id,
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    }

    //删除社团成员
    $(document).on('click', '.delete-activity-member', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/DeleteActivityMember/" + $(this).attr("data-member-id"),
            beforeSend: function () { }
        }).done(function (result) {
            activityMember($('#detailed-activity-member').attr('data-acId'));
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //点击取消
    $(document).on('click', '.cancel-activity', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/CancelActivity/" + $(this).attr("data-acId"),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                ActivityList();
                toastr.success(result.message)
            }
            else {
                toastr.warning(result.message)
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //点击退出
    $(document).on('click', '.out-activity', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/OutActivity/" + $(this).attr("data-acId"),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                ActivityList();
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message);
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    })
});
// 活动列表
function ActivityList() {
    var keywork = $("#search-activity").val();
    var listParaJson = nncqGetListParaJson();
    if ($('#nncqPageIndex').val() == undefined) {
        listParaJson = null;
    }
    if (isActivitySearch) {
        listParaJson = null;
    }
    var jsonData = { "listPageParaJson": listParaJson };
    $.ajax({
        cache: false,
        type: 'post',
        async: false,
        url: "../../ManageCenter/ActivityList/?keywork=" + keywork,
        data: jsonData,
        beforeSend: function () { }

    }).done(function (result) {
        $('#activityList').html(result);
    }).fail(function () {
        toastr.error("后台连接失败");
    }).always(function () {
    });
};

//活动明细
function detailedActivity(id) {
    $.ajax({
        cache: false,
        type: 'post',
        async: false,
        url: "../../ManageCenter/ActivityDetailed/" + id,
        beforeSend: function () { }
    }).done(function (result) {
        $('#managerCenter').html(result);
    }).fail(function () {
        toastr.error("后台连接失败");
    }).always(function () {
    });
};
