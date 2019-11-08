$(function () {
    var keywordValue
    // 人员列表
    function gotoList() {
        keywordValue = $('#inputKeyWord').val();
        var listParaJson = nncqGetListParaJson();
        var jsonData = { "listPageParaJson": listParaJson };
        $.ajax({
            cache: false,
            type: "POST",
            async: true,
            url: "../../Person/List/?keywork=" + keywordValue,
            data: jsonData,
            beforeSend: function () {
                //document.getElementById("personWorkPlace").innerHTML = "<p style='margin-top:50px;' align='center'> <i class='fa fa-spinner fa-pulse fa-2x'></i></p>" +
                //    "<p style='margin-top:50px;' align='center'>  数据加载中，请稍候...</p>";
            }
        }).done(function (data) {
            document.getElementById("personWorkPlace").innerHTML = data;
        }).fail(function (jqXHR, textStatus, errorThrown) {
            //console.error("调试错误:" + errorThrown);
        }).always(function () {
        });
    };

    //条件查询
    $(document).on('click', '.query-person', function () {
        gotoList();
    })

    //添加人员
    $(document).on('click', '.create-person', function (e) {
        e.preventDefault();
        openCreateOrEdit();
    })

    //编辑人员数据
    $(document).on('click', '.edit-person', function (e) {
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
            url: '../../Person/CreateOrEdit/' + id,
            beforeSend: function () { }
        }).done(function (data) {
            document.getElementById('createOrEditArea').innerHTML = data;
        }).fail(function () {
            alert("连接后台失败");
        }).always(function () {

        });
    };


    // 新建或编辑人员信息
    function gotoCreateOrEdit(id) {
        // TBD
    };

    // 查看人员明细信息
    $(document).on('click', '.detail-person', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../Person/Detail/' + this.value,
            beforeSend: function () {
                //alert("开始访问");
            }
        }).done(function (data) {
            document.getElementById("personWorkPlace").innerHTML = data;
        }).fail(function () {
            alert("连接后台失败！");
        }).always(function () {
        });
    })

    // 打开删除人员操作会话框
    $(document).on('click', '.delete-person', function (e) {
        e.preventDefault();
        var name = $(this).attr("p_name");
        document.getElementById("personID").value = this.value;
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
            url: '../../Person/Delete/' + $('#personID').val(),
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
            alert("连接后台失败！");
        }).always(function () {
        });
    });

    //分页按钮
    $(document).on('click', '.page-index', function () {
        var pageIndex = $(this).attr("pageGtoup");
        $('#nncqPageIndex').val(pageIndex);
        gotoList();
    })


    // 处理分页器响应
    function gotoPage(pageIndex) {
        var pageIndex = $(this).attr("pageGtoup");
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

});