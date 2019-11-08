$(function () {
    var keywork = "";
    function List(probably) {
        keywork = $('#inputKeyWord').val();
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
            url: "../../HomeExhibition/List/?keywork=" + keywork + "&&probably=" + probably,
            data: jsonData,
            beforeSend: function () { }
        }).done(function (result) {
            $('#HomeExhibitionWorkPlace').html(result);
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


    //启用
    $(document).on('click', '.startUse', function (e) {
        e.preventDefault();
        var $this = $(this)
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: '../../HomeExhibition/StartUp?id=' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                $this.attr('disabled', true);
                $this.nextAll('.prohibit').removeAttr('disabled');
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message);
            }
        })
    });

    //禁止
    $(document).on('click', '.prohibit', function (e) {
        e.preventDefault();
        var $this = $(this)
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: '../../HomeExhibition/Disable?id=' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                $this.attr('disabled', true);
                $this.prevAll('.startUse').removeAttr('disabled');
                toastr.success(result.message);
            }
            else {
                toastr.error(result.message);
            }
        })
    });

    //删除
    $(document).on('click', '.delete-he', function (e) {
        e.preventDefault();
        var $this = $(this)
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: '../../HomeExhibition/Delete?id=' + $(this).val(),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                var tr = $this.parents('tr');
                console.log(tr);
                tr.remove();
                toastr.success(result.message);
            }
            else {
                toastr.error(result.message);
            }
        })
    });
});