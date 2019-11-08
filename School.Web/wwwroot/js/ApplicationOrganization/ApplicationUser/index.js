$(function () {
    var pageIndex = 1;
    var keywordValue;
    var listParaJson;
    var keywork = "";
    // 人员列表
    function gotoList() {
        keywork = $('#inputKeyWord').val();
        listParaJson = nncqGetListParaJson();
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
            type: "POST",
            async: true,
            url: "../../ApplicationUser/List/?keywork=" + keywordValue + "&&pageIndex=" + pageIndex,
            data: jsonData,
            beforeSend: function () {
            }
        }).done(function (data) {
            console.log(data);
            $("#userWorkPlace").html(data);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            //console.error("调试错误:" + errorThrown);
        }).always(function () {
        });
    };
    gotoList();

    //条件查询
    $(document).on('click', '.query-user', function (e) {
        e.preventDefault();
        pageIndex = 1;
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
    })

    //添加人员
    $(document).on('click', '.create-user', function (e) {
        e.preventDefault();
        openCreateOrEdit();
    })

    //编辑人员数据
    $(document).on('click', '.edit-user', function (e) {
        e.preventDefault();
        openCreateOrEdit(this.value);
    });
    //添加或编辑人员
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
            url: '../../ApplicationUser/CreateOrEdit/' + id,
            beforeSend: function () { }
        }).done(function (data) {
            document.getElementById('createOrEditArea').innerHTML = data;
            }).fail(function () {
                toastr.error("连接后台失败");
        }).always(function () {

        });
    };
    $(document).on('click', '.applicationList', function () {
        window.location.href = "../../ApplicationUser/Index";
    })

    //返回列表
    $(document).on('click', '.application-list', function (e) {
        e.preventDefault();
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
    });
    // 新建或编辑人员信息
    function gotoCreateOrEdit(id) {
        // TBD
    };

    // 查看人员明细信息
    $(document).on('click', '.detail-user', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ApplicationUser/Detail/' + this.value,
            beforeSend: function () {
                //alert("开始访问");
            }
        }).done(function (data) {
            document.getElementById("userWorkPlace").innerHTML = data;
        }).fail(function () {
            toastr.error("连接后台失败！");
        }).always(function () {
        });
    })

    // 打开删除人员操作会话框
    $(document).on('click', '.delete-user', function (e) {
        e.preventDefault();
        var name = $(this).attr("p_name");
        document.getElementById("userID").value = this.value;
        document.getElementById("deleteModalMessage").innerText = "你所选择删除的员工是：" + name + ",请确认是否继续执行。";
        $('#deleteConfirmModal').modal({
            show: true,
            backdrop: 'static'
        });
    });

    //确认删除人员
    $('.confirm-delete').click(function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ApplicationUser/Delete/' + $('#userID').val(),
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

    //重置密码
    $(document).on('click', '.passwordReset', function () {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ApplicationUser/PasswordReset/' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            toastr.success(result.message);
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {
        });
    })

    //启用
    $(document).on('click', '.enabled-user', function (e) {
        e.preventDefault();
        var $this = $(this);
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ApplicationUser/EnabledUser/' + $this.val(),
            beforeSend: function () { }
        }).done(function (result) {
            $this.attr('disabled', true);
            $this.nextAll('.disable-user').removeAttr('disabled');
            toastr.success(result.message)
        }).fail(function () {
            toastr.error("连接后台失败")
        }).always(function () {
        })
    });
    //禁用
    $(document).on('click', '.disable-user', function (e) {
        e.preventDefault();
        var $this = $(this);
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ApplicationUser/DisableUser/' + $this.val(),
            beforeSend: function () { }
        }).done(function (result) {
            $this.attr('disabled', true);
            $this.prevAll('.enabled-user').removeAttr('disabled');
            toastr.success(result.message)
        }).fail(function () {
            toastr.error("连接后台失败")
        }).always(function () {
        })
    })

    function shutCreateOrEdit() {
        $('.modal-backdrop').hide();
        $('#Create_Edit').hide();
    };

});