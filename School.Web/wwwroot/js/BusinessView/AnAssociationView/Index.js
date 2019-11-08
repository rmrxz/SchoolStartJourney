$(function () {

    var keywork = "";
    function List(probably) {
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
            url: "../../AnAssociationView/List/?keywork=" + keywork + "&&probably=" + probably,
            data: jsonData,
            beforeSend: function () { }
        }).done(function (result) {
            $('#anAssociationList').html(result);
            $('.form-control').val(keywork);
        }).fail(function () {
            toastr.error("后台连接失败!");
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
    });

    //筛选
    $(document).on('click', '.input-search', function () {
        List("");
    })

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

    //点击查看社团明细
    $(document).on('click', '.an-detailied', function (e) {
        e.preventDefault();
        window.location.href = "../../AnAssociationView/Detailed/" + $(this).attr("data_Id");
    });

    //查看社团成员或图片
    function otherInfo(Action, id) {
        $.ajax({
            cache: false,
            type: 'poxt',
            async: false,
            url: "../../AnAssociationView/" + Action + "/" + id,
            beforeSend() { }
        }).done(function (result) {
            $('#otherInfo').html(result);
        }).fail(function () {
            toastr.error("后台连接失败!");
        }).always(function () {
        });
    }
    ///查看社团成员
    $(document).on('click', '#ac-user-detailed', function (e) {
        e.preventDefault();
        otherInfo("UserList", $(this).attr("data_Id"));
    });

    ///查看社团图片
    $(document).on('click', '#ac-imges-detailed', function (e) {
        e.preventDefault();
        otherInfo("ImageList", $(this).attr("data_Id"));
    });
    otherInfo("ImageList", $('#ac-imges-detailed').attr("data_Id"));

    //点击添加社团
    $(document).on('click', '.addAnAssociation', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: "post",
            async: false,
            url: "../../AnAssociationView/AddAnAssociation/" + $(this).attr("data_Id"),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message);
            }
        }).fail(function () {
            toastr.error("后台连接失败!");
        }).always(function () {
        });
    });

    //点击取消社团
    $(document).on('click', '.cancelAnAssociation', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: "post",
            async: false,
            url: "../../AnAssociationView/CancelAnAssociation/" + $(this).attr("data_Id"),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message);
            }
        }).fail(function () {
            toastr.error("后台连接失败!");
        }).always(function () {
        });
    });

    $(document).on('click', '.choice li', function (e) {
        e.preventDefault();
        var index = $(this).index();
        List(index.toString());
    })

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