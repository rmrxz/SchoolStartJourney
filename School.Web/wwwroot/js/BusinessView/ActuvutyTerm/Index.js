$(function () {
    var keywork = "";
    function List(probably) {
        scrollTop();
        keywork = $('.form-control').val();
        if (keywork == undefined) {
            keywork = "";
        }
        var listParaJson = nncqGetListParaJson();
        if ($('#nncqPageIndex').val() == undefined) {
            listParaJson = null;
        }
        var jsonData = { "listPageParaJson": listParaJson };
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ActivityTermView/List/?keywork=" + keywork + "&&probably=" + probably,
            data: jsonData,
            beforeSend: function () { }
        }).done(function (result) {
            $('#activityItem').html(result);
            $('.form-control').val(keywork)
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    };
    List("");

    //分页按钮
    $(document).on('click', '.page-index', function (e) {
        e.preventDefault();
        var pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        if ($(this).parent('li').attr("class") == "disabled") {
            return;
        }
        List();
    })

    function scrollTop() {
        $("html,body").animate({ scrollTop: 0 }, 0);
    }

    // 处理分页器响应
    function gotoPage(pageIndex) {
        var pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        List();
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
        List();
    };

    //点击查看活动明细
    $(document).on('click', '.activity-detailied', function (e) {
        e.preventDefault();
        window.location.href = "../../ActivityTermView/Detailied/" + $(this).attr("data_Id");
    });

    //点击添加活动
    $(document).on('click', '.addActivity', function (e) {
        e.preventDefault();
        var id = $(this).attr("data_Id")
        $.ajax({
            cache: false,
            type: "post",
            async: false,
            url: "../../ActivityTermView/AddActivity/" + id,
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message);
            }
        }).fail(function () {
        }).always(function () {
        });
    });

    //点击取消活动
    $(document).on('click', '.cancelActivity', function (e) {
        e.preventDefault();
        var id = $(this).attr("data_Id")
        $.ajax({
            cache: false,
            type: "post",
            async: false,
            url: "../../ActivityTermView/CancelActivity/" + $('input[name=detailied_Id]').val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message);
            }
        }).fail(function () {
        }).always(function () {
        });
    });

    $(document).on('click', '.choice li', function (e) {
        e.preventDefault();
        var index = $(this).index();
        List(index.toString());
    });

    //筛选
    $(document).on('click', '.input-search', function (e) {
        e.preventDefault();
        List("");
    })
    //查看社团成员或图片
    function otherInfo(Action, id) {
        $.ajax({
            cache: false,
            type: 'poxt',
            async: false,
            url: "../../ActivityTermView/" + Action + "/" + id,
            beforeSend() { }
        }).done(function (result) {
            $('#otherInfo').html(result);
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    }
    ///查看社团成员
    $(document).on('click', '#ac-user-detailed', function (e) {
        e.preventDefault();
        otherInfo("UserList", $(this).attr("data_Id"));
        $('.auxiliaryClickComment').css('display', 'none');
    });
    ///所有评论
    $(document).on('click', '#ac-user-comments', function (e) {
        e.preventDefault();
        getComments($(this).attr("data_Id"));
        $('.auxiliaryClickComment').css('display', 'inline-block');
    });

    //点击评论
    $(document).on('click', '.auxiliaryClickComment', function (e) {
        e = e || window.e;
        //阻止事件冒泡
        e.stopPropagation();
        // 方法阻止元素发生默认的行为
        e.preventDefault();
        $('.com-special-sign').css('display', "block");
        addComment($('.per-comment'));
    });
    //点击评论区以外时隐藏评论输入框
    $(document).click(function (e) {
        perComment();
        $('.per-comment').css('display', "none")
    });

    //添加评论
    $(document).on('click', '.publish-comment', function (e) {
        e.preventDefault();
        var com = $(this).parents('.publish-dis').siblings('.comment-text');
        var commentText = com.text();
        var comment = commentText.split(":")[1];
        var activityID = $(this).parents('.com-special-sign').siblings('input[name="Id"]').val();
        var parentGrade = com.attr('data_acid');
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ActivityTermView/AddComment/?id=" + activityID + "&&parentGrade=" + parentGrade + "&&comment=" + comment,
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                getComments(activityID);
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message)
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //删除评论
    $(document).on('click', '.delete-reply', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ActivityTermView/DeleteComment/?id=" + $(this).attr('data_acid'),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                getComments($(this).attr('data_acid'));
                toastr.success(result.message)
            }
            else {
                toastr.warning(result.message);
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });
    function getComments(id) {
        var listParaJson = nncqGetListParaJson();
        if ($('#nncqPageIndex').val() == undefined) {
            listParaJson = null;
        }
        var jsonData = { "listPageParaJson": listParaJson };
        $.ajax({
            cache: false,
            type: 'poxt',
            async: false,
            url: "../../ActivityTermView/GetComments?id=" + $('#ac-user-comments').attr("data_Id"),
            data: jsonData,
            beforeSend() { }
        }).done(function (result) {
            $('#otherInfo').html(result);
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    }


    //评论分页按钮
    $(document).on('click', '.page-index-comment', function (e) {
        e.preventDefault();
        var pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        if ($(this).parent('li').attr("class") == "disabled") {
            return;
        }
        getComments();
    })
    ////评论分页
    //function CommentList(probably) {
    //    scrollTop();
    //    keywork = $('.form-control').val();
    //    if (keywork == undefined) {
    //        keywork = "";
    //    }
    //    var listParaJson = nncqGetListParaJson();
    //    if ($('#nncqPageIndex').val() == undefined) {
    //        listParaJson = null;
    //    }
    //    var jsonData = { "listPageParaJson": listParaJson };
    //    $.ajax({
    //        cache: false,
    //        type: 'post',
    //        async: false,
    //        url: "../../ActivityTermView/List/"
    //        data: jsonData,
    //        beforeSend: function () { }
    //    }).done(function (result) {
    //        $('#activityItem').html(result);
    //        $('.form-control').val(keywork)
    //    }).fail(function () {
    //        alert("后台连接失败");
    //    }).always(function () {
    //    });
    //};

    ///查看社团图片
    $(document).on('click', '#ac-imges-detailed', function (e) {
        e.preventDefault();
        otherInfo("ImageList", $(this).attr("data_Id"));
        $('.auxiliaryClickComment').css('display', 'none');
    });
    otherInfo("ImageList", $('#ac-imges-detailed').attr("data_Id"));

    //取消a标签的默认事件
    $(document).on('click', '#pagingt-process a', function (e) {
        e.preventDefault();
    });

    //查看用户信息
    $(document).on('click', '.seeUserDetailed', function (e) {
        e.preventDefault()
        window.location.href = "../../PersonalCenter/Index?id=" + $(this).attr("data_Id");
    })
});