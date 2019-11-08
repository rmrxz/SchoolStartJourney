var isActivitySearch = false;
$(function () {
    //筛选
    $(document).on('click', '.input-search-meeageNotification', function (e) {
        e.preventDefault();
        isActivitySearch = true;
        meeageNotificationList();
    });

    //删除单条通知
    $(document).on('click', '.out-notification', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/DeleteMessageNotification?id=" + $(this).attr('data-acId'),
            beforeSend: function () { }
        }).done(function (result) {
            meeageNotificationList();
            }).fail(function () {
                toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //删除所有通知
    $(document).on('click', '.delete-all-notification', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/DeleteMessageNotificationAll",
            beforeSend: function () { }
        }).done(function (result) {
            meeageNotificationList();
            }).fail(function () {
                toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //查看通知明细
    $(document).on('click', '.detailed-notification', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/GetMessageNotificationDetailed?id=" + $(this).attr('data-acId'),
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    })

    //全部标记为已读
    $(document).on('click', '.see-all-notification', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/ReadMessageNotificationAll",
            beforeSend: function () { }
        }).done(function (result) {
            meeageNotificationList();
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    })

    //标记单条通知为已读
    $(document).on('click', '.sign-read-notification', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/ReadMessageNotification?id=" + $(this).attr('data-acId'),
            beforeSend: function () { }
        }).done(function (result) {
            meeageNotificationList();
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });
    //分页按钮
    $(document).on('click', '.page-index', function (e) {
        e.preventDefault();
        var pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        if ($(this).parent('li').attr("class") == "disabled") {
            return;
        }
        meeageNotificationList();
    });

    // 处理分页器响应
    function gotoPage(pageIndex) {
        var pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        meeageNotificationList();
    };

    // 处理排序响应
    function gotoSort(sortPropertyName, sortID) {
        var sortStatus = $('#nncqSortDesc').val();  // 获取当前的排序形式
        if (sortStatus == 'Default') {
            document.getElementById(sortID).innerHTML = '<span aria-hidden="true" class="glyphicon glyphicon-chevron-down" style="color:white"></span>';
            $('#nncqSortDesc').val('')

        } else {
            document.getElementById(sortID).innerHTML = '<span aria-hidden="true" class="glyphicon glyphicon-chevron-up" style="color:white"></span>';
            $('#nncqSortDesc').val('Default')
        }
        $('#nncqSortProperty').val(sortPropertyName)
        meeageNotificationList();
    };
});
// 通知列表
function meeageNotificationList() {
    messageNotification();
    var keywork = $("#search-meeageNotification").val();
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
        url: "../../ManageCenter/GetMessageNotificationList/?keywork=" + keywork,
        data: jsonData,
        beforeSend: function () { }

    }).done(function (result) {
        $('#notificationList').html(result);
    }).fail(function () {
        toastr.error("后台连接失败");
    }).always(function () {
    });
};

function messageNotification() {
    $.ajax({
        cache: false,
        type: 'type',
        async: false,
        url: '../../ManageCenter/MessageNotificationNumber',
        beforeSend: function () { }
    }).done(function (result) {
        $('#messageNotificationNumber').text(result);
    }).fail(function () {
        toastr.error("连接后台失败");
    }).always(function () {

    });
}