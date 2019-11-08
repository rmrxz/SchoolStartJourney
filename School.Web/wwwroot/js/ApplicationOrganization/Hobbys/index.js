$(function () {
    var keywordValue
    // 爱好列表
    function gotoList() {
        keywordValue = $('#inputKeyWord').val();
        var listParaJson = nncqGetListParaJson();
        var jsonData = { "listPageParaJson": listParaJson };
        $.ajax({
            cache: false,
            type: "POST",
            async: true,
            url: "../../Hobbys/List/?keywork=" + keywordValue,
            data: jsonData,
            beforeSend: function () {
               
            }
        }).done(function (data) {
            document.getElementById("hobbysWorkPlace").innerHTML = data;
        }).fail(function (jqXHR, textStatus, errorThrown) {
            //console.error("调试错误:" + errorThrown);
        }).always(function () {
        });
    };

    //条件查询
    $(document).on('click', '.query-hobbys', function () {
        gotoList();
    })

    //添加爱好
    $(document).on('click', '.create-hobbys', function (e) {
        e.preventDefault();
        openCreateOrEdit();
    })

    //编辑爱好数据
    $(document).on('click', '.edit-hobbys', function (e) {
        e.preventDefault();
        openCreateOrEdit(this.value);
    });
    //添加或编辑爱好
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
            url: '../../Hobbys/CreateOrEdit/' + id,
            beforeSend: function () { }
        }).done(function (data) {
            document.getElementById('createOrEditArea').innerHTML = data;
            }).fail(function () {
                toastr.error("连接后台失败");
        }).always(function () {

        });
    };

    //更改图片
    $(document).on('click', '.edit-hobbysPhone', function (e) {
        e.preventDefault();
        $('#hobbyPhoneModal').modal({
            show: true,
            backdrop: 'static'
        });
        $('.modal-backdrop').show();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../Hobbys/hobbyPhone/' + this.value,
            beforeSend: function () { }
        }).done(function (data) {
            document.getElementById('hobbyPhoneArea').innerHTML = data;
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    });

    //触发input
    $(document).on('click', '.tn-hobby-phone', function () {
        $(".head-file").trigger("click");
    });
    //选择图片
    $(document).on('change', '.head-file', function (e) {
        var fileName = $(this).val();
        var suffixIndex = fileName.lastIndexOf(".");
        var suffix = fileName.substring(suffixIndex + 1).toUpperCase();
        if (suffix != "BMP" && suffix != "JPG" && suffix != "JPEG" && suffix != "PNG" && suffix != "GIF") {
            toastr.error("请上传图片（格式BMP、JPG、JPEG、PNG、GIF等）!");
            return;
        }
        var file = $('input[type=file]').get(0).files[0];
        readPath(file);
    });

    //替换图片路径
    function readPath(fil) {
        imaPath = $('.head-portrait img').attr("src");
        var reader = new FileReader();
        reader.readAsDataURL(fil);
        reader.onload = function () {
            $('.head-portrait img').attr("src", reader.result);
        };
    }

    //保存图片
    $(document).on('click', '.save-hobby-phone', function (e) {
        e.preventDefault();
        var file = $('.head-file').get(0).files[0];
        var data = new FormData();
        data.append(file.name, file);
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../Hobbys/SavehobbyPhone?id=' + $(this).attr('data_Id'),
            // 告诉jQuery不要去处理发送的数据
            processData: false,
            // 告诉jQuery不要去设置Content-Type请求头
            contentType: false,
            data: data,
            beforeSend: function () { }
        }).done(function (result) {
            $('#hobbyPhoneModal').modal('hide')
            gotoList('');
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    })


    // 打开删除爱好操作会话框
    $(document).on('click', '.delete-hobbys', function (e) {
        e.preventDefault();
        var name = $(this).attr("p_name");
        document.getElementById("hobbysID").value = this.value;
        document.getElementById("deleteModalMessage").innerText = "你所选择删除的数据是：" + name + ",请确认是否继续执行。";
        $('#deleteConfirmModal').modal({
            show: true,
            backdrop: 'static'
        });
    });

    //确认删除爱好
    $('.confirm-delete').click(function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../Hobbys/Delete/' + $('#hobbysID').val(),
            beforeSend: function () {
            }
        }).done(function (data) {
            if (data) {
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