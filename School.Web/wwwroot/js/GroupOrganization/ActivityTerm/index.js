$(function () {
    var keywordValue;
    var pageIndex = 1;
    var activityId = ""
    // 活动列表
    function gotoList() {
        scrollTop();
        activityId = "";
        keywordValue = $('#inputKeyWord').val();
        var listParaJson = nncqGetListParaJson();
        var jsonData = { "listPageParaJson": listParaJson };
        $.ajax({
            cache: false,
            type: "POST",
            async: true,
            url: "../../ActivityTerm/List/?keywork=" + keywordValue,
            data: jsonData,
            beforeSend: function () {
              
            }
        }).done(function (data) {
            $("#activityTermWorkPlace").html(data);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            //console.error("调试错误:" + errorThrown);
        }).always(function () {
        });
    };
    $(document).on('click', '.activity-list', function (e) {
        e.preventDefault();
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
    })

    //查看图片
    $(document).on('click', '.upload-picture', function (e) {
        e.preventDefault();
        location.href = '../../ActivityTerm/UploadPicture/' + this.value;
    });

    //条件查询
    $(document).on('click', '.query-activityTerm', function () {
        gotoList();
    })

    //添加活动
    $(document).on('click', '.create-activityTerm', function (e) {
        e.preventDefault();
        openCreateOrEdit();
    })

    //编辑活动数据
    $(document).on('click', '.edit-activityTerm', function (e) {
        e.preventDefault();
        openCreateOrEdit(this.value);
    });

    //添加或编辑活动
    function openCreateOrEdit(id) {
        $('#createOrEditModal').modal({
            show: true,
            backdrop: 'static'
        });
        $('.modal-backdrop').show();
        //访问后台 CreateOrEdit 方法，获取新建或者编辑数据的呈现 imports page 页面内容
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ActivityTerm/CreateOrEdit/' + id,
            beforeSend: function () { }
        }).done(function (data) {
            document.getElementById('createOrEditArea').innerHTML = data;
            }).fail(function () {
                toastr.error("连接后台失败");
        }).always(function () {

        });
    };

    // 查看活动明细信息
    $(document).on('click', '.detail-activityTerm', function (e) {
        activityId = this.value;
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ActivityTerm/Detail/' + this.value,
            beforeSend: function () {
                //alert("开始访问");
            }
        }).done(function (data) {
            document.getElementById("activityTermWorkPlace").innerHTML = data;
        }).fail(function () {
            toastr.error("连接后台失败！");
        }).always(function () {
        });
    })

    //活动评论列表
    function an_commemt() {
        scrollTop();
        var listParaJson = twoNncqGetListParaJson();
        if ($('#acPageIndex').val() == undefined) {
            listParaJson = null;
        }
        var jsonData = { "listPageParaJson": listParaJson };
        $.ajax({
            cache: false,
            type: "POST",
            async: true,
            url: "../../ActivityTerm/ActivityComment/?id=" + activityId,
            data: jsonData,
            beforeSend: function () {

            }
        }).done(function (data) {
            $('#ac_comment').html(data);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            //console.error("调试错误:" + errorThrown);
        }).always(function () {
        });
    }

    //活动评论
    $(document).on('click', '.ac_CommentList', function (e) {
        e.preventDefault();
        an_commemt();
    })

    // 打开删除活动操作会话框
    $(document).on('click', '.delete-activityTerm', function (e) {
        e.preventDefault();
        var name = $(this).attr("a_name");
        document.getElementById("activityTremID").value = this.value;
        document.getElementById("deleteModalMessage").innerText = "你所选择删除的员工是：" + name + ",请确认是否继续执行。";
        $('#deleteConfirmModal').modal({
            show: true,
            backdrop: 'static'
        });
    });

    //确认删除活动
    $('.confirm-delete').click(function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ActivityTerm/Delete/' + $('#activityTremID').val(),
            beforeSend: function () {
            }
        }).done(function (data) {
            if (data.isOK == true) {
                $('#deleteConfirmModal').modal('hide')
                gotoList('');
            } else {
                document.getElementById("deleteModalErrMessage").innerText = data.message;
            }
        }).fail(function () {
            toastr.error("连接后台失败！");
        }).always(function () {
        });
    });

    //删除图片
    $(document).on('click', '.delete-activity-image', function (e) {
        e.preventDefault();
        var acId = $('#detailed-activity-images').attr("data-image-Id");
        var deleteImage = $(this).eq(0);
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ActivityTerm/DeleteActivityDetailedImages?id=" + $(this).attr("data-image-Id"),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOk) {
                deleteImage.parents('.pull-left').remove();
            }
            else {
                toastr.error(result.message)
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });


    //分页按钮
    $(document).on('click', '.page-index', function () {
        pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
    })
    //社团评论分页按钮
    $(document).on('click', '.page-comment-index', function () {
        comPageIndex = $(this).attr("pageGtoup");
        $('#acPageIndex').val(comPageIndex);
        an_commemt();
    })

    // 处理分页器响应
    function gotoPage(pageIndex) {
        pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
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
        gotoList();
    };


    function shutCreateOrEdit() {
        $('.modal-backdrop').hide();
        $('#Create_Edit').hide();
    };

    //禁用社团
    $(document).on('click', '.disable-anAssociation', function (e) {
        e.preventDefault();
        var $this = $(this)
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: '../../ActivityTerm/ProhibitActivityTrem?id=' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                $this.attr('disabled', true);
                $this.prevAll('.enabled-anAssociation').removeAttr('disabled');
                toastr.success(result.message);
            }
            else {
                toastr.error(result.message);
            }
        })
    })
    //解封社团
    $(document).on('click', '.enabled-anAssociation', function (e) {
        e.preventDefault();
        var $this = $(this)
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: '../../ActivityTerm/LiftedActivityTrem?id=' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                $this.attr('disabled', true);
                $this.nextAll('.disable-anAssociation').removeAttr('disabled');
                toastr.success(result.message);
            }
            else {
                toastr.error(result.message);
            }
        })
    });

    //添加进首页设置
    $(document).on('click', '.homeExhibition-present', function (e) {
        e.preventDefault();
        var $this = $(this)
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: '../../ActivityTerm/HomeExhibitionPresent?id=' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message);
            }
        })
    });

    function scrollTop() {
        $("html,body").animate({ scrollTop: 0 }, 0);
    }
    //  level分为 YM YMD H HM 四个有效值，分别表示年月 年月日 年月日时 年月日时分,less表示是否不可小于当前时间。年-月-日 时:分 时为24小时制
    //  为确保控件结构只出现一次，在有需要的时候进行一次调用。
    onLoadTimeChoiceDemo();
    borainTimeChoice({
        start: ".endDataTime",
        end: "",
        level: "H",
        less: false
    });
    borainTimeChoice({
        start: ".signDataTime",
        end: "",
        level: "H",
        less: false
    });
    borainTimeChoice({
        start: ".startDataTime",
        end: "",
        level: "H",
        less: false
    });
});

function twoNncqGetListParaJson() {
    // 提取缺省的页面规格参数
    var acPageTypeID = $("#acTypeID").val();
    var acPagePageIndex = $("#acPageIndex").val();
    var acPagePageSize = $("#acPageSize").val();
    var acPagePageAmount = $("#acPageAmount").val();
    var acPageObjectAmount = $("#acObjectAmount").val();
    var acPageKeyword = $("#acKeyword").val();
    var acPageSortProperty = $("#acSortProperty").val();
    var acPageSortDesc = $("#acSortDesc").val();
    var acPageSelectedObjectID = $("#acSelectedObjectID").val();
    var acPageIsSearch = $("#acIsSearch").val();
    // 创建前端 json 数据对象
    var listParaJson = "{" +
        "ObjectTypeID:\"" + acPageTypeID + "\", " +
        "PageIndex:\"" + acPagePageIndex + "\", " +
        "PageSize:\"" + acPagePageSize + "\", " +
        "PageAmount:\"" + acPagePageAmount + "\", " +
        "ObjectAmount:\"" + acPageObjectAmount + "\", " +
        "Keyword:\"" + acPageKeyword + "\", " +
        "SortProperty:\"" + acPageSortProperty + "\", " +
        "SortDesc:\"" + acPageSortDesc + "\", " +
        "IsSearch:\"" + acPageIsSearch + "\", " +
        "SelectedObjectID:\"" + acPageSelectedObjectID + "\"" +
        "}";

    return listParaJson;
}