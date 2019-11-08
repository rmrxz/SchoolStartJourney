$(function () {
    var keywordValue;
    var pageIndex = 1;
    // 社团列表
    function gotoList() {

        anAssociationId = "";
        keywordValue = $('#inputKeyWord').val();
        var listParaJson = nncqGetListParaJson();
        var jsonData = { "listPageParaJson": listParaJson };
        $.ajax({
            cache: false,
            type: "POST",
            async: true,
            url: "../../AnAssociation/List/?keywork=" + keywordValue,
            data: jsonData,
            beforeSend: function () {

            }
        }).done(function (data) {
            $('#anAssociationWorkPlace').html(data);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            //console.error("调试错误:" + errorThrown);
        }).always(function () {
        });
    };
    var anAssociationId = ""
    //社团活动列表
    function an_ActivityList() {
        var listParaJson = twoNncqGetListParaJson();
        if ($('#acPageIndex').val() == undefined) {
            listParaJson = null;
        }
        var jsonData = { "listPageParaJson": listParaJson };
        $.ajax({
            cache: false,
            type: "POST",
            async: true,
            url: "../../AnAssociation/ActivityList/?id=" + anAssociationId,
            data: jsonData,
            beforeSend: function () {

            }
        }).done(function (data) {
            $('#an_activityList').html(data);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            //console.error("调试错误:" + errorThrown);
        }).always(function () {
        });
    }
    $(document).on('click', ".activityList", function (e) {
        e.preventDefault();
        an_ActivityList(anAssociationId);
    })

    //条件查询
    $(document).on('click', '.query-anAssociation', function (e) {
        e.preventDefault();
        pageIndex = 1;
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
    });

    //返回列表
    $(document).on('click', '.anAssociation-list', function (e) {
        e.preventDefault();
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
    });

    //添加社团
    $(document).on('click', '.create-anAssociation', function (e) {
        e.preventDefault();
        openCreateOrEdit();
    })

    //编辑社团数据
    $(document).on('click', '.edit-anAssociation', function (e) {
        e.preventDefault();
        openCreateOrEdit(this.value);
    });

    //添加或编辑社团

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
            url: '../../AnAssociation/CreateOrEdit/' + id,
            beforeSend: function () { }
        }).done(function (data) {
            document.getElementById('createOrEditArea').innerHTML = data;
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    };

    // 查看社团明细信息
    $(document).on('click', '.detail-anAssociation', function (e) {
        anAssociationId = this.value;
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../AnAssociation/Detail/' + this.value,
            beforeSend: function () {
                //alert("开始访问");
            }
        }).done(function (data) {
            document.getElementById("anAssociationWorkPlace").innerHTML = data;
        }).fail(function () {
            toastr.error("连接后台失败！");
        }).always(function () {
        });
    })

    $(document).on('click', '.upload-picture', function (e) {
        e.preventDefault();
        location.href = '../../AnAssociation/UploadPicture/' + this.value;
    });

    // 打开删除社团操作会话框
    $(document).on('click', '.delete-anAssociation', function (e) {
        e.preventDefault();
        var name = $(this).attr("a_name");
        document.getElementById("anAssociationID").value = this.value;
        document.getElementById("deleteModalMessage").innerText = "你所选择删除的员工是：" + name + ",请确认是否继续执行。";
        $('#deleteConfirmModal').modal({
            show: true,
            backdrop: 'static'
        });
    });

    //确认删除社团
    $('.confirm-delete').click(function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../AnAssociation/Delete/' + $('#anAssociationID').val(),
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

    //分页按钮
    $(document).on('click', '.page-index', function () {
        pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
    });

    //社团活动分页按钮
    $(document).on('click', '.page-activity-index', function () {
        pageIndex = $(this).attr("pageGtoup");
        $('#acPageIndex').val(pageIndex);
        an_ActivityList();
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
            url: '../../AnAssociation/ProhibitAnAssociation?id=' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                $this.attr('disabled', true);
                $this.prevAll('.enabled-anAssociation').removeAttr('disabled');
                toastr.success(result.meesage);
            }
            else {
                toastr.error(result.meesage);
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
            url: '../../AnAssociation/LiftedAnAssociation?id=' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                $this.attr('disabled', true);
                $this.nextAll('.disable-anAssociation').removeAttr('disabled');
                toastr.success(result.meesage);
            }
            else {
                toastr.error(result.meesage);
            }
        })
    });


    function scrollTop() {
        $("html,body").animate({ scrollTop: 0 }, 0);
    }

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